using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Helpers;
using StaticNotStirred_Revit.Helpers.Families;
using StaticNotStirred_Revit.Helpers.Geometry;
using StaticNotStirred_UI.Enums;
using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_Revit.Models
{
    public class LevelLoadModel : ILevelLoadModel
    {
        public List<ILoadModel> LoadModels { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double ElevationFeet { get; set; }
        public double TopOfSlabElevationFeet { get; set; }
        public double BottomOfSlabElevationFeet { get; set; }
        public double ClearShoreHeight { get; set; }
        public double ConcreteDepthFeet { get; set; }
        public double CapacityPoundsForcePerSquareFoot { get; set; }
        public double DemandPoundsForcePerSquareFoot { get; set; }
        public double FormworkDemandPoundsForcePerSquareFoot { get; set; }
        public double LiveLoadDemandPerSquareFoot { get; set; }
        public double ReshoreDemandPoundsForcePerSquareFoot { get; set; }

        public LevelLoadModel()
        {
            Id = Guid.NewGuid();
            LoadModels = new List<ILoadModel>();
        }

        #region Revit-specific

        internal Level Level { get; set; }

        public static LevelLoadModel Create(Level level)
        {
            LevelLoadModel _floorModel = new LevelLoadModel
            {
                Level = level,
            };

            if (_floorModel != null && _floorModel.Level != null)
            {
                _floorModel.ElevationFeet = level.Elevation;
                _floorModel.Name = level.Name;
            }

            return _floorModel;
        }

        public void ReadLoads()
        {
            CapacityPoundsForcePerSquareFoot = readLoads(LoadType.Capacity);
            DemandPoundsForcePerSquareFoot = readLoads(LoadType.Demand) + readLoads(LoadType.Other);
            FormworkDemandPoundsForcePerSquareFoot = readLoads(LoadType.Formwork);
            LiveLoadDemandPerSquareFoot = readLoads(LoadType.LiveLoad);
            ReshoreDemandPoundsForcePerSquareFoot =
                DemandPoundsForcePerSquareFoot
                + FormworkDemandPoundsForcePerSquareFoot
                + LiveLoadDemandPerSquareFoot
                + ReshoreDemandPoundsForcePerSquareFoot
                - CapacityPoundsForcePerSquareFoot;
        }

        private double readLoads(LoadType loadType)
        {
            double _load = 0.0;

            switch (loadType)
            {
                case LoadType.Capacity:
                    List<LoadModel> _capacityLoadModels = LoadModels.OfType<LoadModel>().Where(p => p.LoadType == LoadType.Capacity && FamilyHelpers.IsReshore(p.Element as FamilyInstance) == false).ToList();
                    _load = _capacityLoadModels.Sum(p => p.PoundsForcePerSquareFoot);
                    break;

                case LoadType.Demand:
                case LoadType.Formwork:
                case LoadType.LiveLoad:
                case LoadType.Other:
                    List<LoadModel> _demandLoadModels = LoadModels.OfType<LoadModel>().Where(p => p.LoadType == loadType).ToList();
                    _load = _demandLoadModels.Sum(p => p.PoundsForcePerSquareFoot);
                    break;

                case LoadType.ReshoreDemand:
                    List<LoadModel> _reshoreDemandLoadModels = LoadModels.OfType<LoadModel>().Where(p => p.LoadType == LoadType.ReshoreDemand && FamilyHelpers.IsReshore(p.Element as FamilyInstance)).ToList();
                    _load = _reshoreDemandLoadModels.Sum(p => p.PoundsForcePerSquareFoot);
                    break;

                case LoadType.None:
                default:
                    break;
            }

            return _load;
        }

        public List<LoadModel> addFloorCapacityLoadModel(Floor floor, double poundsForcePerSquareFoot)
        {
            double? _floorThickness = floor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM)?.AsDouble();
            if (_floorThickness != null && _floorThickness.HasValue &&  ConcreteDepthFeet < _floorThickness.Value) ConcreteDepthFeet = _floorThickness.Value;

            List<LoadModel> _loadModels = createFloorLoadModels(floor, LoadType.Capacity);
            foreach (LoadModel _loadModel in _loadModels) _loadModel.PoundsForcePerSquareFoot = poundsForcePerSquareFoot;

            LoadModels.AddRange(_loadModels);
            return _loadModels;
        }

        public List<LoadModel> addFloorDemandLoadModel(Floor floor, double poundsForcePerSquareFoot)
        {
            double? _floorThickness = floor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM)?.AsDouble();
            if (_floorThickness != null && _floorThickness.HasValue && _floorThickness.Value < ConcreteDepthFeet) ConcreteDepthFeet = _floorThickness.Value;

            List<LoadModel> _loadModels = createFloorLoadModels(floor, LoadType.Demand);
            foreach (LoadModel _loadModel in _loadModels) _loadModel.PoundsForcePerSquareFoot = poundsForcePerSquareFoot;
            
            LoadModels.AddRange(_loadModels);
            return _loadModels;
        }

        public List<LoadModel> addFamilyInstanceLoadModel(FamilyInstance familyInstance, LoadType loadType, double poundsForcePerSquareFoot)
        {
            List<LoadModel> _loadModels = createLoadModels(familyInstance, loadType);
            foreach (LoadModel _loadModel in _loadModels) _loadModel.PoundsForcePerSquareFoot = poundsForcePerSquareFoot;

            LoadModels.AddRange(_loadModels);
            return _loadModels;
        }

        public void addLoadModel(ILoadModel loadModel)
        {
            LoadModels.Add(loadModel);
        }

        public List<LoadModel> addWallLoadModel(Wall wall, LoadType loadType, double poundsForcePerSquareFoot)
        {
            List<LoadModel> _loadModels = createLoadModels(wall, loadType);
            foreach (LoadModel _loadModel in _loadModels) _loadModel.PoundsForcePerSquareFoot = poundsForcePerSquareFoot;

            LoadModels.AddRange(_loadModels);
            return _loadModels;
        }

        private List<LoadModel> createFloorLoadModels(Floor floor, LoadType loadType)
        {
            List<LoadModel> _squareLoadModels = new List<LoadModel>();

            //maximum parking ramp slope = 6.67%
            //https://www.rochestermn.gov/home/showdocument?id=18472#:~:text=Parking%20ramp%20slopes%20should%20not,International%20Building%20Code%20(IBC).
            List<Solid> _solids = GeometryHelpers.GetGeometryObjects(floor, ViewDetailLevel.Fine).OfType<Solid>().ToList();
            List<PlanarFace> _topFaces = _solids.SelectMany(p => p.Faces.OfType<PlanarFace>()).Where(p =>
                p.FaceNormal.Z > 0.0 &&
                (p.FaceNormal.Z / Math.Sqrt(Math.Pow(p.FaceNormal.X, 2.0) + Math.Pow(p.FaceNormal.Y, 2.0))) > 0.0667).ToList();

            //Populate  FloorProfileModel for our FloorModel
            foreach (PlanarFace _topFace in _topFaces)
            {
                foreach (EdgeArray _edgeArray in _topFace.EdgeLoops.OfType<EdgeArray>())
                {
                    List<Curve> _curves = _edgeArray.OfType<Edge>().Select(p => p.AsCurve()).ToList();
                    Plane _topFaceXYPlane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, _topFace.Origin);

                    //Project curves onto a flat plane with Normal = XYZ.BasizZ
                    List<Curve> _projectedCurves = new List<Curve>();
                    foreach (Curve _curve in _curves)
                    {
                        XYZ _projectedEnd0 = _topFaceXYPlane.ProjectOnto(_curve.GetEndPoint(0));
                        XYZ _projectedEnd1 = _topFaceXYPlane.ProjectOnto(_curve.GetEndPoint(1));

                        if (_projectedEnd0.DistanceTo(_projectedEnd1) <= 1E-09) continue;

                        if (_curve is Line _line)
                        {
                            Line _projectedLine = Line.CreateBound(_projectedEnd0, _projectedEnd1);
                            _projectedCurves.Add(_projectedLine);
                        }
                        else if (_curve is Arc _arc)
                        {
                            XYZ _projectedPointOnArc = _topFaceXYPlane.ProjectOnto(_arc.Evaluate(0.5, true));
                            Arc _projectedArc = Arc.Create(_projectedEnd0, _projectedEnd1, _projectedPointOnArc);
                            _projectedCurves.Add(_projectedArc);
                        }
                    }

                    bool _sorted = SortCurvesContiguous(_projectedCurves);

                    LoadModel _loadModel = LoadModel.Create();
                    _loadModel.LoadType = loadType;
                    _loadModel.PlanarFace = _topFace;
                    _loadModel.Curves = _projectedCurves;
                    _loadModel.Element = floor;

                    _squareLoadModels.Add(_loadModel);

                    if (floor is Floor)
                    {
                        if (TopOfSlabElevationFeet < _loadModel.TopElevationFeet) TopOfSlabElevationFeet = _loadModel.TopElevationFeet;
                    }
                }
            }

            return _squareLoadModels;
        }

        private List<LoadModel> createLoadModels(Element element, LoadType loadType)
        {
            List<LoadModel> _squareLoadModels = new List<LoadModel>();

            //maximum parking ramp slope = 6.67%
            //https://www.rochestermn.gov/home/showdocument?id=18472#:~:text=Parking%20ramp%20slopes%20should%20not,International%20Building%20Code%20(IBC).
            List<Solid> _solids = GeometryHelpers.GetGeometryObjects(element, ViewDetailLevel.Fine).OfType<Solid>().ToList();
            List<PlanarFace> _topFaces = _solids.SelectMany(p => p.Faces.OfType<PlanarFace>()).Where(p =>
                p.FaceNormal.Z > 0.0 &&
                (p.FaceNormal.Z / Math.Sqrt(Math.Pow(p.FaceNormal.X, 2.0) + Math.Pow(p.FaceNormal.Y, 2.0))) > 0.0667).ToList();

            //Populate  FloorProfileModel for our FloorModel
            Plane _levelPlane = Plane.CreateByNormalAndOrigin(XYZ.BasisZ, new XYZ(0, 0, Level.Elevation));
            PlanarFace _topFace = _topFaces.OrderBy(p => p.Origin.DistanceTo(_levelPlane.ProjectOnto(p.Origin))).FirstOrDefault();
            if (_topFace == null) return _squareLoadModels;

            foreach (EdgeArray _edgeArray in _topFace.EdgeLoops.OfType<EdgeArray>())
            {
                List<Curve> _curves = _edgeArray.OfType<Edge>().Select(p => p.AsCurve()).ToList();

                //Project curves onto a flat plane with Normal = XYZ.BasizZ
                List<Curve> _projectedCurves = new List<Curve>();
                foreach (Curve _curve in _curves)
                {
                    XYZ _projectedEnd0 = _levelPlane.ProjectOnto(_curve.GetEndPoint(0));
                    XYZ _projectedEnd1 = _levelPlane.ProjectOnto(_curve.GetEndPoint(1));

                    if (_projectedEnd0.DistanceTo(_projectedEnd1) <= 1E-09) continue;

                    if (_curve is Line _line)
                    {
                        Line _projectedLine = Line.CreateBound(_projectedEnd0, _projectedEnd1);
                        _projectedCurves.Add(_projectedLine);
                    }
                    else if (_curve is Arc _arc)
                    {
                        XYZ _projectedPointOnArc = _levelPlane.ProjectOnto(_arc.Evaluate(0.5, true));
                        Arc _projectedArc = Arc.Create(_projectedEnd0, _projectedEnd1, _projectedPointOnArc);
                        _projectedCurves.Add(_projectedArc);
                    }
                }

                bool _sorted = SortCurvesContiguous(_projectedCurves);

                LoadModel _loadModel = LoadModel.Create();
                _loadModel.LoadType = loadType;
                _loadModel.PlanarFace = _topFace;
                _loadModel.Curves = _projectedCurves;
                _loadModel.Element = element;

                _squareLoadModels.Add(_loadModel);
            }

            return _squareLoadModels;
        }

        public void SetLevelParameters()
        {
            if (Level == null) return;

            //ILoadModel _loadModel = LoadModels.FirstOrDefault(p => );
            //if (_loadModel == null) return;

            Parameter _loadCapacityParameter = Level.get_Parameter(Helpers.Parameters.Ids.LevelLoadCapacityParameterId);
            if (_loadCapacityParameter != null && _loadCapacityParameter.IsReadOnly == false)
            {
                _loadCapacityParameter.Set(UnitUtils.ConvertToInternalUnits(CapacityPoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            }

            Parameter _loadDemandParameter = Level.get_Parameter(Helpers.Parameters.Ids.LoadDemandParameterId);
            if (_loadDemandParameter != null && _loadDemandParameter.IsReadOnly == false)
            {
                _loadDemandParameter.Set(UnitUtils.ConvertToInternalUnits(DemandPoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            }

            Parameter _formworkDemandParameter = Level.get_Parameter(Helpers.Parameters.Ids.FormworkDemandParameterId);
            if (_formworkDemandParameter != null && _formworkDemandParameter.IsReadOnly == false)
            {
                _formworkDemandParameter.Set(UnitUtils.ConvertToInternalUnits(FormworkDemandPoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            }

            Parameter _liveLoadDemandParameter = Level.get_Parameter(Helpers.Parameters.Ids.LiveLoadDemandParameterId);
            if (_liveLoadDemandParameter != null && _liveLoadDemandParameter.IsReadOnly == false)
            {
                _liveLoadDemandParameter.Set(UnitUtils.ConvertToInternalUnits(LiveLoadDemandPerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            }

            Parameter _reshoreDemandParameter = Level.get_Parameter(Helpers.Parameters.Ids.ReshoreDemandParameterId);
            if (_reshoreDemandParameter != null && _reshoreDemandParameter.IsReadOnly == false)
            {
                _reshoreDemandParameter.Set(UnitUtils.ConvertToInternalUnits(ReshoreDemandPoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
            }
        }


        //From Jeremy Tammik - https://thebuildingcoder.typepad.com/blog/2013/03/sort-and-orient-curves-to-form-a-contiguous-loop.html
        public static bool SortCurvesContiguous(IList<Curve> curves)
        {
            double _inch = 1.0 / 12.0;
            double _sixteenth = _inch / 16.0;

            int n = curves.Count;

            // Walk through each curve (after the first) 
            // to match up the curves in order

            for (int i = 0; i < n; ++i)
            {
                Curve curve = curves[i];
                XYZ endPoint = curve.GetEndPoint(1);

                XYZ p;

                // Find curve with start point = end point

                bool found = (i + 1 >= n);

                for (int j = i + 1; j < n; ++j)
                {
                    p = curves[j].GetEndPoint(0);

                    // If there is a match end->start, 
                    // this is the next curve

                    if (_sixteenth > p.DistanceTo(endPoint))
                    {
                        if (i + 1 != j)
                        {
                            Curve tmp = curves[i + 1];
                            curves[i + 1] = curves[j];
                            curves[j] = tmp;
                        }
                        found = true;
                        break;
                    }

                    p = curves[j].GetEndPoint(1);

                    // If there is a match end->end, 
                    // reverse the next curve

                    if (_sixteenth > p.DistanceTo(endPoint))
                    {
                        if (i + 1 == j)
                        {
                            Curve _reversedCurve = CreateReversedCurve(curves[j]);
                            if (_reversedCurve != null) curves[i + 1] = _reversedCurve;
                            else curves[i + 1] = curves[j];
                        }
                        else
                        {
                            Curve tmp = curves[i + 1];

                            Curve _reversedCurve = CreateReversedCurve(curves[j]);
                            if (_reversedCurve != null) curves[i + 1] = _reversedCurve;
                            else curves[i + 1] = curves[j];

                            curves[j] = tmp;
                        }
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return false;
                }
            }

            return true;
        }

        //From Jeremy Tammik - https://thebuildingcoder.typepad.com/blog/2013/03/sort-and-orient-curves-to-form-a-contiguous-loop.html
        static Curve CreateReversedCurve(Curve orig)
        {
            if (orig is Line == false && orig is Arc == false)
            {
                return null;
            }

            if (orig is Line)
            {
                return Line.CreateBound(
                  orig.GetEndPoint(1),
                  orig.GetEndPoint(0));
            }
            else if (orig is Arc)
            {
                return Arc.Create(orig.GetEndPoint(1),
                  orig.GetEndPoint(0),
                  orig.Evaluate(0.5, true));
            }
            else
            {
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            return obj is LevelLoadModel model &&
                   Id.Equals(model.Id);
        }

        public override int GetHashCode()
        {
            return 2108858624 + Id.GetHashCode();
        }

        #endregion
    }
}
