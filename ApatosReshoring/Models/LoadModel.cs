using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Helpers;
using StaticNotStirred_Revit.Helpers.Families;
using StaticNotStirred_UI.Enums;
using StaticNotStirred_UI.Helpers;
using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_Revit.Models
{
    public class LoadModel : ILoadModel
    {
        public Guid Id { get; set; }
        public LoadType LoadType { get; set; }
        public string Name { get; set; }
        public double PoundsForcePerSquareFoot { get; set; }
        public double DensityPoundsForcePerCubicFoot { get; set; }
        public double VolumeCubicFeet { get; set; }
        public double AreaSquareFeetXY { get; set; }
        public double HeightFeetZ { get; set; }
        public double ClearShoreHeightFeet { get; set; }
        public double OriginXFeet { get; set; }
        public double OriginYFeet { get; set; }        
        public double TopElevationFeet { get; set; }

        private LoadModel()
        {
            Id = Guid.NewGuid();
            Curves = new List<Curve>();
        }

        public static LoadModel Create()
        {
            LoadModel _loadModel = new LoadModel();
            
            return _loadModel;
        }

        public static LoadModel Create(string name, LoadType loadType, double poundsPerSquareFoot)
        {
            LoadModel _loadModel = new LoadModel
            { 
                Name = name,
                LoadType = loadType,
                PoundsForcePerSquareFoot = poundsPerSquareFoot,
            };

            return _loadModel;
        }

        #region Revit-specific

        internal List<Curve> Curves { get; set; }

        private PlanarFace _planarFace;
        internal PlanarFace PlanarFace
        {
            get => _planarFace;
            set
            {
                _planarFace = value;
                if (_planarFace == null) return;

                AreaSquareFeetXY = _planarFace.Area;

                OriginXFeet = _planarFace.Origin.X;

                OriginYFeet = _planarFace.Origin.Y;

                TopElevationFeet = _planarFace.Origin.Z;
            }
        }

        private Element _element;
        internal Element Element
        {
            get => _element;
            set
            {
                _element = value;
                if (_element == null) return;

                if (LoadType == LoadType.None || LoadType == LoadType.LiveLoad) return;

                ClearShoreHeightFeet = getClearShoreHeight(_element);

                DensityPoundsForcePerCubicFoot = getDensity(_element);

                HeightFeetZ = getHeight(_element);

                VolumeCubicFeet = getVolume(_element);
            }
        }

        internal Solid Solid { get; set; }
        internal DirectShape DirectShape { get; set; }

        private static double getClearShoreHeight(Element element)
        {
            Parameter _parameter = element.get_Parameter(Helpers.Parameters.Ids.ClearShoreHeightId);

            if (_parameter != null) return _parameter.AsDouble();
            else return 0.0;
        }

        private static double getDensity(Element element)
        {
            ElementType _elementType = element.Document.GetElement(element.GetTypeId()) as ElementType;

            Parameter _materialParameter = null;
            if (element is FamilyInstance && element.Category != null)
            {
                if (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns)
                {
                    _materialParameter = element.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
                }
                else if (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)
                {
                    _materialParameter = element.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
                }
                else if (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Columns) //reshores
                {
                    //no params as of 2020-07-20
                }
            }
            else if (element is Floor)
            {
                _materialParameter = _elementType?.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
            }
            else if (element is Wall)
            {
                _materialParameter = _elementType?.get_Parameter(BuiltInParameter.STRUCTURAL_MATERIAL_PARAM);
            }

            Material _material = _materialParameter == null
                ? null
                : element.Document.GetElement(_materialParameter.AsElementId()) as Material;

            PropertySetElement _propertySetElement = _material == null
                ? null
                : element.Document.GetElement(_material.StructuralAssetId) as PropertySetElement;

            StructuralAsset _structuralAsset = _propertySetElement == null
                ? null
                : _propertySetElement.GetStructuralAsset();

            double _density = _structuralAsset == null
                    ? 0.0
                    : _structuralAsset.Density; 

            return _density;
        }

        private static double getHeight(Element element)
        {
            Parameter _parameter = element.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM);

            if (_parameter != null) return _parameter.AsDouble();
            else return 0.0;
        }

        private static double getVolume(Element element)
        {
            Parameter _volumeParameter = null;
            if (element is Floor ||
                element is Wall ||
                    (element is FamilyInstance && element.Category != null &&
                        (element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns ||
                        element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming)))
            {
                _volumeParameter = element.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED);
            }

            double _volume = _volumeParameter == null
                    ? 0.0
                    : _volumeParameter.AsDouble();

            return _volume;
        }

        private Solid getProjectedSolid(XYZ direction, double distance)
        {
            Solid _solid = null;
            try
            {
                if (direction.IsAlmostEqualTo(PlanarFace.FaceNormal) == false &&
                direction.IsAlmostEqualTo(PlanarFace.FaceNormal.Negate()) == false) return null;

                CurveLoop _curveLoop = CurveLoop.Create(Curves);
                if (_curveLoop.IsOpen())
                {
                    XYZ _closingEnd0 = _curveLoop.FirstOrDefault()?.GetEndPoint(0);
                    XYZ _closingEnd1 = _curveLoop.LastOrDefault()?.GetEndPoint(1);
                    if (_closingEnd0 != null && _closingEnd1 != null && _closingEnd0.IsAlmostEqualTo(_closingEnd1) == false)
                    {

                        Line _line = Line.CreateBound(_closingEnd0, _closingEnd1);
                        _curveLoop.Append(_line);

                    }
                }

                if (_curveLoop.IsOpen()) return null;

                if (distance <= 0) distance = 0.5;
                _solid = GeometryCreationUtilities.CreateExtrusionGeometry(new List<CurveLoop> { _curveLoop }, direction, distance);
            }
            catch (Exception _ex) { }
            return _solid;
        }

        public void SetSolid(XYZ direction, double distance)
        {
            Solid = getProjectedSolid(direction, distance);
        }

        public static void SetSolids(IEnumerable<LoadModel> loadModels, Level currentLevel, Level topLevel, Level bottomLevel)
        {
            foreach (LoadModel _loadModel in loadModels)
            {
                try
                {
                    double _distanceToModelBottom = 0.0;
                    if (_loadModel.Element is Floor) _distanceToModelBottom = currentLevel.Elevation - bottomLevel.Elevation;
                    else _distanceToModelBottom = currentLevel.Elevation - bottomLevel.Elevation;

                    double _distanceToModelTop = _distanceToModelTop = topLevel.Elevation - currentLevel.Elevation;

                    switch (_loadModel.LoadType)
                    {
                        case LoadType.Capacity:
                        case LoadType.ReshoreDemand:
                            _loadModel.SetSolid(XYZ.BasisZ, _distanceToModelTop);
                            break;
                        case LoadType.Formwork:
                        case LoadType.Other:
                        case LoadType.Demand:
                        case LoadType.LiveLoad:
                            _loadModel.SetSolid(XYZ.BasisZ.Negate(), _distanceToModelBottom);
                            break;
                        case LoadType.None:
                        default:
                            break;
                    }
                }
                catch (Exception _ex)
                {
                    bool b = false;
                }
            }
        }

        public void SetDirectShapeWithParameters(Document _doc, ElementId _categoryId, string levelName)
        {
            if (Solid == null) return;

            DirectShape = FamilyHelpers.CreateDirectShape(_doc, Solid, _categoryId);
            if (DirectShape == null) return;

            switch (LoadType)
            {
                case LoadType.Capacity:
                    Parameter _loadCapacityParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.LoadCapacityParameterId);
                    if (_loadCapacityParameter != null && _loadCapacityParameter.IsReadOnly == false)
                    {
                        _loadCapacityParameter.Set(UnitUtils.ConvertToInternalUnits(PoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
                    }
                    break;

                case LoadType.Demand:
                case LoadType.Formwork:
                case LoadType.Other:
                    Parameter _loadDemandParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.LoadDemandParameterId);
                    if (_loadDemandParameter != null && _loadDemandParameter.IsReadOnly == false)
                    {
                        _loadDemandParameter.Set(UnitUtils.ConvertToInternalUnits(PoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
                    }
                    break;

                case LoadType.LiveLoad:
                    Parameter _liveLoadDemandParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.LiveLoadDemandParameterId);
                    if (_liveLoadDemandParameter != null && _liveLoadDemandParameter.IsReadOnly == false)
                    {
                        _liveLoadDemandParameter.Set(UnitUtils.ConvertToInternalUnits(PoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
                    }
                    break;

                case LoadType.ReshoreDemand:
                    Parameter _reshoreDemandParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.ReshoreDemandParameterId);
                    if (_reshoreDemandParameter != null && _reshoreDemandParameter.IsReadOnly == false)
                    {
                        _reshoreDemandParameter.Set(UnitUtils.ConvertToInternalUnits(PoundsForcePerSquareFoot, DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT));
                    }
                
                    break;
                case LoadType.None:
                default:
                    break;
            }

            Parameter _loadTypeParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.LoadTypeParameterId);
            if (_loadTypeParameter != null && _loadTypeParameter.IsReadOnly == false) _loadTypeParameter.Set(LoadType.GetDescription());

            Parameter _sourceElementIdParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.SourceElementIdId);
            if (_sourceElementIdParameter != null && _sourceElementIdParameter.IsReadOnly == false) _sourceElementIdParameter.Set(Element.Id.ToString());

            Parameter _sourceFamilyAndTypeParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.SourceName);
            if (_sourceFamilyAndTypeParameter != null && _sourceFamilyAndTypeParameter.IsReadOnly == false) _sourceFamilyAndTypeParameter.Set(Element.Name + " - " + levelName);

            Parameter _sourceCrossSectionalAreaIdParameter = DirectShape.get_Parameter(Helpers.Parameters.Ids.SourceCrossSectionalAreaId);
            if (_sourceCrossSectionalAreaIdParameter != null && _sourceCrossSectionalAreaIdParameter.IsReadOnly == false)
            {
                _sourceCrossSectionalAreaIdParameter.Set(UnitUtils.ConvertToInternalUnits(AreaSquareFeetXY, DisplayUnitType.DUT_SQUARE_FEET));
            }

        }

        #endregion
    }
}
