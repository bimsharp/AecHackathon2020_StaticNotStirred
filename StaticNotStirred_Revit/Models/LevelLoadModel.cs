using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Helpers;
using StaticNotStirred_Revit.Helpers.Geometry;
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
        public List<ISquareLoadModel> CapacityModels { get; set; }
        public List<ISquareLoadModel> DemandModels { get; set; }
        public List<ISquareLoadModel> ReshoreDemandModels { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Elevation { get; set; }
        public double TopOfSlabElevation { get; set; }
        public double ConcreteDepth { get; set; }
        public double Capacity { get; set; }
        public double Demand { get; set; }
        public double ReshoreDemand { get; set; }

        public LevelLoadModel()
        {
            Id = Guid.NewGuid();
            CapacityModels = new List<ISquareLoadModel>();
            DemandModels = new List<ISquareLoadModel>();
            ReshoreDemandModels = new List<ISquareLoadModel>();
            DemandProfileModels = new List<SquareLoadModel>();
        }

        #region Revit-specific

        internal Floor Floor { get; set; }

        internal Level Level { get; set; }

        internal List<SquareLoadModel> DemandProfileModels { get; set; }

        public static LevelLoadModel Create(Level level)
        {
            LevelLoadModel _floorModel = new LevelLoadModel
            {
                Level = level,
            };

            if (_floorModel != null && _floorModel.Level != null)
            {
                _floorModel.Elevation = level.Elevation;
                _floorModel.Name = level.Name;
            }

            return _floorModel;
        }

        public void addFloorLoadModel(Floor floor)
        {
            double? _floorThickness = floor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM)?.AsDouble();
            if (_floorThickness != null && _floorThickness.HasValue) ConcreteDepth = _floorThickness.Value;

            List<SquareLoadModel> _squareLoadModels = createFloorLoadModels(floor);
            DemandModels.AddRange(_squareLoadModels);
        }

        public void addFamilyInstanceLoadModel(FamilyInstance familyInstance)
        {
            List<SquareLoadModel> _squareLoadModels = createLoadModels(familyInstance);
            DemandModels.AddRange(_squareLoadModels);
        }

        public void addWallLoadModel(Wall wall)
        {
            List<SquareLoadModel> _squareLoadModels = createLoadModels(wall);
            DemandModels.AddRange(_squareLoadModels);
        }

        private List<SquareLoadModel> createFloorLoadModels(Floor element)
        {
            List<SquareLoadModel> _squareLoadModels = new List<SquareLoadModel>();

            //maximum parking ramp slope = 6.67%
            //https://www.rochestermn.gov/home/showdocument?id=18472#:~:text=Parking%20ramp%20slopes%20should%20not,International%20Building%20Code%20(IBC).
            List<Solid> _solids = GeometryHelpers.GetGeometryObjects(element, ViewDetailLevel.Fine).OfType<Solid>().ToList();
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

                    SquareLoadModel _loadModel = new SquareLoadModel
                    {
                        PlanarFace = _topFace,
                        Curves = _projectedCurves,
                        Element = element,
                    };

                    _squareLoadModels.Add(_loadModel);
                    this.DemandProfileModels.Add(_loadModel);
                    if (element is Floor &&
                        _loadModel.TopElevation.HasValue &&
                        this.TopOfSlabElevation < _loadModel.TopElevation)
                    {
                        this.TopOfSlabElevation = _loadModel.TopElevation.Value;
                    }
                }
            }

            return _squareLoadModels;
        }

        private List<SquareLoadModel> createLoadModels(Element element)
        {
            List<SquareLoadModel> _squareLoadModels = new List<SquareLoadModel>();

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

                SquareLoadModel _loadModel = new SquareLoadModel
                {
                    PlanarFace = _topFace,
                    Curves = _projectedCurves,
                    Element = element,
                };

                _squareLoadModels.Add(_loadModel);
                this.DemandProfileModels.Add(_loadModel);
                if (element is Floor &&
                    _loadModel.TopElevation.HasValue &&
                    this.TopOfSlabElevation < _loadModel.TopElevation)
                {
                    this.TopOfSlabElevation = _loadModel.TopElevation.Value;
                }
            }


            return _squareLoadModels;
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

        #endregion
    }
}
