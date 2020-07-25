#region References
using StaticNotStirred_Revit.Helpers.Selections;
using StaticNotStirred_Revit.Helpers.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using StaticNotStirred_Revit.Helpers.Selections;
using StaticNotStirred_Revit.Helpers.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StaticNotStirred_Revit.Helpers.Annotations;
using StaticNotStirred_Revit.Helpers.Geometry;
using StaticNotStirred_Revit.Models;
using StaticNotStirred_Revit.Helpers.Families;
using StaticNotStirred_UI.Models;
using StaticNotStirred_UI.Enums;
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class CreateVisualizationSheetsCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return CreateVisualizationSheets(commandData.Application.ActiveUIDocument);
        }

        public static Result CreateVisualizationSheets(UIDocument uiDoc)
        {
            //ToDo: these objects would be better passed to/from a UI, or possibly stored somewhere as settings

            BuildingLoadModel _buildingLoadModel = new BuildingLoadModel(0, 0, 7.0, 7.0, 7.0, 7.0, 7.0);

            List<ILoadModel> _levelLiveLoadPerSquareFoots = new List<ILoadModel>
            {                
                LoadModel.Create("ROOF", LoadType.Capacity, 40),
                LoadModel.Create("Level 18", LoadType.Capacity, 24),
                LoadModel.Create("Level 17", LoadType.Capacity, 24),
                LoadModel.Create("Level 16", LoadType.Capacity, 24),
                LoadModel.Create("Level 15", LoadType.Capacity, 24),
                LoadModel.Create("Level 14", LoadType.Capacity, 24),
                LoadModel.Create("Level 13", LoadType.Capacity, 24),
                LoadModel.Create("Level 12", LoadType.Capacity, 24),
                LoadModel.Create("Level 11", LoadType.Capacity, 24),
                LoadModel.Create("Level 10", LoadType.Capacity, 24),
                LoadModel.Create("Level 09", LoadType.Capacity, 24),
                LoadModel.Create("Level 08", LoadType.Capacity, 24),
                LoadModel.Create("Level 07", LoadType.Capacity, 24),
                LoadModel.Create("Level 06", LoadType.Capacity, 24),
                LoadModel.Create("Level 05", LoadType.Capacity, 24),
                LoadModel.Create("Level 04", LoadType.Capacity, 24),
                LoadModel.Create("Level 03", LoadType.Capacity, 24),
                LoadModel.Create("Level 02", LoadType.Capacity, 100),
                LoadModel.Create("Level 01", LoadType.Capacity, 100),
                LoadModel.Create("P01", LoadType.Capacity, 40),
                LoadModel.Create("P02", LoadType.Capacity, 40),
                LoadModel.Create("P03", LoadType.Capacity, 40),
                LoadModel.Create("P04", LoadType.Capacity, 40),
                LoadModel.Create("P05", LoadType.Capacity, 40),

                LoadModel.Create("ROOF", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 18", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 17", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 16", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 15", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 14", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 13", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 12", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 11", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 10", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 09", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 08", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 07", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 06", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 05", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 04", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 03", LoadType.LiveLoad, 40),
                LoadModel.Create("Level 02", LoadType.LiveLoad, 100),
                LoadModel.Create("Level 01", LoadType.LiveLoad, 100),
                LoadModel.Create("P01", LoadType.LiveLoad, 45),
                LoadModel.Create("P02", LoadType.LiveLoad, 45),
                LoadModel.Create("P03", LoadType.LiveLoad, 45),
                LoadModel.Create("P04", LoadType.LiveLoad, 45),
                LoadModel.Create("P05", LoadType.LiveLoad, 45),

                LoadModel.Create("Level 18", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 17", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 16", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 15", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 14", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 13", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 12", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 11", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 10", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 09", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 08", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 07", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 06", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 05", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 04", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 03", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 02", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("Level 01", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("P01", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("P02", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("P03", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("P04", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                LoadModel.Create("P05", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
            };

            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Create Visualization View"))
            {
                _trans.Start();
                try
                {
                    _result = createVisualizationSheets(uiDoc, _buildingLoadModel, _levelLiveLoadPerSquareFoots);

                    _trans.Commit();
                }
                catch (Exception _ex)
                {
                    _trans.RollBack();
#if DEBUG
                    string _error = _ex.Message + Environment.NewLine + Environment.NewLine + _ex.StackTrace;
                    TaskDialog.Show("Error", _error);
#endif
                    return Result.Failed;
                }
            }

            return _result;
        }

        private static Result createVisualizationSheets(UIDocument uiDoc, BuildingLoadModel buildingLoadModel, IEnumerable<ILoadModel> levelLiveLoadPerSquareFoots)
        {
            //Setup
            Document _doc = uiDoc.Document;

            double _concreteDensityPoundsForcePerCubicFoot = 153.0;

            XYZ _capacityViewCoordinate = new XYZ(2.17649771769026, 0.766954561856788, 0);
            XYZ _combinedViewCoordinate = new XYZ(0.780872717690263, 0.766954561856788, 0);
            XYZ _demandViewCoordinate = new XYZ(0.780872717690263, 1.83481042377296, 0);
            
            var _levels = Getters.GetLevels(_doc).OrderByDescending(p => p.Elevation).ToList();

            var _floors = Getters.GetFloors(_doc).OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _structuralColumns = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_StructuralColumns).OfType<FamilyInstance>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _structuralFraming = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_StructuralFraming).OfType<FamilyInstance>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _walls = new FilteredElementCollector(_doc).OfClass(typeof(Wall)).OfType<Wall>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            
            BoundingBoxXYZ _modelExtents =
                GeometryHelpers.GetElementsBBox(
                    new FilteredElementCollector(_doc).WhereElementIsViewIndependent().WhereElementIsNotElementType().ToList());
            _modelExtents.Min = new XYZ(_modelExtents.Min.X, _modelExtents.Min.Y, _levels.LastOrDefault().Elevation - 1.0);

            Category _directShapeCategory = Category.GetCategory(_doc, BuiltInCategory.OST_GenericModel);

            Level _levelAbove = null;
            Level _bottomLevel = _levels.LastOrDefault();
            Level _topLevel = _levels.FirstOrDefault();

            //Begin to generate our VisualizationDeliverables - these are sheets with 3 plan-orientation isometric views.
            //  NOTE - isometric views were used so that semi-transparent color overrides can be overlayed over each other to represent demands vs capacities
            //  ToDo: it would be valuable to scale transparency by percentage of overall demand/capacity
            List<VisualizationDeliverable> _visualizationDeliverables = new List<VisualizationDeliverable>();
            foreach (Level _level in _levels)
            {
                if (_levelAbove == null) _levelAbove = _level;

                //Get the elements that are on our current Level
                List<Floor> _currentLevelFloors = _floors.Where(p => p.LevelId == _level.Id).ToList();
                List<FamilyInstance> _currentLevelStructuralColumns = new List<FamilyInstance>();
                List<FamilyInstance> _currentLevelStructuralFraming = new List<FamilyInstance>();
                List<Wall> _currentLevelWalls = new List<Wall>();

                BoundingBoxXYZ _levelBounds = new BoundingBoxXYZ
                {
                    Min = new XYZ(_modelExtents.Min.X, _modelExtents.Min.Y, _level.Elevation),
                    Max = new XYZ(_modelExtents.Max.X, _modelExtents.Max.Y, _levelAbove.Elevation)
                };

                BoundingBoxIsInsideFilter _withinLevelBoundsFilter = new BoundingBoxIsInsideFilter(new Outline(_levelBounds.Min, _levelBounds.Max));
                BoundingBoxIntersectsFilter _intersectsLevelBoundsFilter = new BoundingBoxIntersectsFilter(new Outline(_levelBounds.Min, _levelBounds.Max));

                if (_structuralColumns.Count > 0) _currentLevelStructuralColumns = new FilteredElementCollector(_doc, _structuralColumns.Select(p => p.Id).ToList()).WherePasses(_intersectsLevelBoundsFilter).OfType<FamilyInstance>().ToList();
                else _currentLevelStructuralColumns = new List<FamilyInstance>();

                if (_structuralFraming.Count > 0) _currentLevelStructuralFraming = new FilteredElementCollector(_doc, _structuralFraming.Select(p => p.Id).ToList()).WherePasses(_withinLevelBoundsFilter).OfType<FamilyInstance>().ToList();
                else _currentLevelStructuralFraming = new List<FamilyInstance>();

                if (_walls.Count > 0) _currentLevelWalls = new FilteredElementCollector(_doc, _walls.Select(p => p.Id).ToList()).WherePasses(_withinLevelBoundsFilter).OfType<Wall>().ToList();
                else _currentLevelWalls = new List<Wall>();

                //Generate LoadModels to populate a full LevelLoadModel
                LevelLoadModel _currentLevelLoadModel = LevelLoadModel.Create(_level);
                foreach (Floor _floor in _currentLevelFloors)
                {
                    //The "top" floor is where the initial Demand is determined, which is to be supported via reshores propagating down through the building
                    //ToDo: it would be valuable to be able to pick which Level to start from
                    if (_level.Id == _topLevel.Id)
                    {
                        Parameter _floorThicknessParameter = _floor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM);
                        double _floorThickness = _floorThicknessParameter == null
                            ? 0.0
                            : _floorThicknessParameter.AsDouble();

                        _currentLevelLoadModel.addFloorDemandLoadModel(_floor, _concreteDensityPoundsForcePerCubicFoot * _floorThickness);
                    }

                    //Add loads from other sources that are also distributed evenly along a level 
                    ILoadModel _floorCapacityLoadModel = levelLiveLoadPerSquareFoots.FirstOrDefault(p => p.Name == _level.Name);
                    if (_floorCapacityLoadModel == null) continue;

                    List<LoadModel> _floorLoadModels = _currentLevelLoadModel.addFloorCapacityLoadModel(_floor, _floorCapacityLoadModel.PoundsForcePerSquareFoot);                    
                    
                    foreach (ILoadModel _loadModel in levelLiveLoadPerSquareFoots.Where(p => p.Name.Equals(_level.Name)))
                    {
                        List<ILoadModel> _otherLoadModels = new List<ILoadModel>();
                        foreach (LoadModel _floorLoadModel in _floorLoadModels)
                        {
                            LoadModel _otherLoadModel = LoadModel.Create();
                            _otherLoadModel.Name = _loadModel.Name;
                            _otherLoadModel.LoadType = _loadModel.LoadType;
                            _otherLoadModel.PoundsForcePerSquareFoot = _loadModel.PoundsForcePerSquareFoot;
                            _otherLoadModel.AreaSquareFeetXY = _floorLoadModel.AreaSquareFeetXY;
                            _otherLoadModel.HeightFeetZ = _floorLoadModel.HeightFeetZ;
                            _otherLoadModel.OriginXFeet = _floorLoadModel.OriginXFeet;
                            _otherLoadModel.OriginYFeet = _floorLoadModel.OriginYFeet;

                            _otherLoadModel.Curves = _floorLoadModel.Curves;
                            _otherLoadModel.PlanarFace = _floorLoadModel.PlanarFace;
                            _otherLoadModel.Element = _floorLoadModel.Element;

                            _currentLevelLoadModel.addLoadModel(_otherLoadModel);
                        }
                    }
                }
                foreach (FamilyInstance _structuralColumn in _currentLevelStructuralColumns)
                {
                    _currentLevelLoadModel.addFamilyInstanceLoadModel(_structuralColumn, LoadType.Demand, buildingLoadModel.StructuralColumnWeightPerSquareFoot);
                    _currentLevelLoadModel.addFamilyInstanceLoadModel(_structuralColumn, LoadType.Capacity, buildingLoadModel.StructuralColumnWeightPerSquareFoot);
                }
                foreach (FamilyInstance _structuralFrame in _currentLevelStructuralFraming)
                {
                    _currentLevelLoadModel.addFamilyInstanceLoadModel(_structuralFrame, LoadType.Demand, buildingLoadModel.StructuralBeamWeightPerSquareFoot);
                }
                foreach (Wall _wall in _currentLevelWalls)
                {
                    _currentLevelLoadModel.addWallLoadModel(_wall, LoadType.Demand, buildingLoadModel.StructuralWallWeightPerSquareFoot);
                    _currentLevelLoadModel.addWallLoadModel(_wall, LoadType.Capacity, buildingLoadModel.StructuralWallWeightPerSquareFoot);
                }

                //Set the Solid elements that we will project through the building, to represent demands/capacities
                LoadModel.SetSolids(_currentLevelLoadModel.LoadModels.OfType<LoadModel>(), _level, _topLevel, _bottomLevel);

                VisualizationDeliverable _visualizationDeliverable = new VisualizationDeliverable(_currentLevelLoadModel)
                {
                    Floors = _currentLevelFloors,
                    StructuralColumns = _currentLevelStructuralColumns,
                    StructuralFraming = _currentLevelStructuralFraming,
                    Walls = _currentLevelWalls,
                };
                _visualizationDeliverables.Add(_visualizationDeliverable);

                _levelAbove = _level;
            }

            //Now that we've gathered all of our LoadModels, let's read the data about their actual demands/capacities
            buildingLoadModel.LevelLoadModels = _visualizationDeliverables.Select(p => p.LevelLoadModel as ILevelLoadModel).Where(p => p != null).ToList();
            
            buildingLoadModel.ReadLoads();

            foreach (LoadModel _loadModel in _visualizationDeliverables.Select(p => p.LevelLoadModel).SelectMany(p => p.LoadModels))
            {
                _loadModel.SetDirectShapeWithParameters(_doc, _directShapeCategory.Id, _loadModel.Name);
            }

            //Update Levels in the model with Load details
            foreach (LevelLoadModel _levelLoadModel in _visualizationDeliverables.Select(p => p.LevelLoadModel)) _levelLoadModel.SetLevelParameters();

            //Color our active View for the visualization
            colorActiveView(_doc, _visualizationDeliverables);

            //ToDo: something happened which broke colors being correctly applied to these sheets - will need to sort this out
            //createSheetsAndViews(_doc, _visualizationDeliverables, _modelExtents);
            //
            //colorViews(_doc, _visualizationDeliverables);
            //
            //createViewports(_doc, _visualizationDeliverables, _capacityViewCoordinate, _combinedViewCoordinate, _demandViewCoordinate);

            return Result.Succeeded;
        }

        private static void colorActiveView(Document doc, IEnumerable<VisualizationDeliverable> visualizationDeliverables)
        {
            OverrideGraphicSettings _capacityOgs = getCapacityOgs(doc);

            //Color Capacity DirectShapes
            IEnumerable<DirectShape> _capacityDirectShapes = getCapacityDirectShapes(visualizationDeliverables);
            foreach (DirectShape _capacityDirectShape in _capacityDirectShapes) if (_capacityDirectShape != null) doc.ActiveView.SetElementOverrides(_capacityDirectShape.Id, _capacityOgs);

            //Color Demand DirectShapes
            OverrideGraphicSettings _demandOgs = getDemandOgs(doc);
            IEnumerable<DirectShape> _demandDirectShapes = getDemandDirectShapes(visualizationDeliverables);
            foreach (DirectShape _demandDirectShape in _demandDirectShapes) if (_demandDirectShape != null) doc.ActiveView.SetElementOverrides(_demandDirectShape.Id, _demandOgs);
        }

        private static void colorViews(Document doc, IEnumerable<VisualizationDeliverable> visualizationDeliverables)
        {
            OverrideGraphicSettings _capacityOgs = getCapacityOgs(doc);
            OverrideGraphicSettings _demandOgs = getDemandOgs(doc);

            foreach (var _visualizationDeliverable in visualizationDeliverables)
            {
                IEnumerable<DirectShape> _capacityDirectShapes = getCapacityDirectShapes(_visualizationDeliverable).Where(p => p != null);
                IEnumerable<DirectShape> _demandDirectShapes = getDemandDirectShapes(_visualizationDeliverable).Where(p => p != null);

                if (_visualizationDeliverable.CapacityView != null)
                {
                    foreach (DirectShape _directShape in _capacityDirectShapes) _visualizationDeliverable.CapacityView.SetElementOverrides(_directShape.Id, _capacityOgs);
                }
                if (_visualizationDeliverable.CombinedView != null)
                {
                    foreach (DirectShape _directShape in _capacityDirectShapes) _visualizationDeliverable.CombinedView.SetElementOverrides(_directShape.Id, _capacityOgs);
                    foreach (DirectShape _directShape in _demandDirectShapes) _visualizationDeliverable.CombinedView.SetElementOverrides(_directShape.Id, _demandOgs);
                }
                if (_visualizationDeliverable.DemandView != null)
                {
                    foreach (DirectShape _directShape in _demandDirectShapes) _visualizationDeliverable.DemandView.SetElementOverrides(_directShape.Id, _demandOgs);
                }
            }
        }

        private static void createViewports(Document doc, IEnumerable<VisualizationDeliverable> visualizationDeliverables, XYZ capacityViewCoordinate, XYZ combinedViewCoordinate, XYZ demandViewCoordinate)
        {
            doc.Regenerate();

            foreach (var _visualizationDeliverable in visualizationDeliverables)
            {
                if (_visualizationDeliverable.CapacityView != null) Viewport.Create(doc, _visualizationDeliverable.Sheet.Id, _visualizationDeliverable.CapacityView.Id, capacityViewCoordinate);
                if (_visualizationDeliverable.CombinedView != null) Viewport.Create(doc, _visualizationDeliverable.Sheet.Id, _visualizationDeliverable.CombinedView.Id, combinedViewCoordinate);
                if (_visualizationDeliverable.DemandView != null) Viewport.Create(doc, _visualizationDeliverable.Sheet.Id, _visualizationDeliverable.DemandView.Id, demandViewCoordinate);
            }
        }
        
        private static void createSheetsAndViews(Document doc, IEnumerable<VisualizationDeliverable> visualizationDeliverables, BoundingBoxXYZ modelExtents)
        {
            //Setup
            string _titleblockName = "E1 30 x 42 Horizontal: E1 30x42 Horizontal";
            int _demandViewScale = 120;
            int _combinedViewScale = 120;
            int _capacityViewScale = 120;

            SheetCreator _sheetCreator = new SheetCreator(doc);

            OverrideGraphicSettings _modelElementsOgs = getModelOgs();

            Level _levelAbove = null;
            foreach (VisualizationDeliverable _visualizationDeliverable in visualizationDeliverables)
            {
                BoundingBoxXYZ _levelBounds = new BoundingBoxXYZ();
                _levelBounds.Min = new XYZ(modelExtents.Min.X, modelExtents.Min.Y, _visualizationDeliverable.Level.Elevation);

                if (_levelAbove == null)
                {
                    _levelBounds.Max = new XYZ(modelExtents.Max.X, modelExtents.Max.Y, modelExtents.Max.Z);
                    _levelAbove = _visualizationDeliverable.Level;
                }
                else
                {
                    _levelBounds.Max = new XYZ(modelExtents.Max.X, modelExtents.Max.Y, _levelAbove.Elevation);
                }

                //View Creation
                BoundedViewCreator _boundedViewCreator = new BoundedViewCreator(_visualizationDeliverable.Level, null, _levelBounds);

                string _viewName = _boundedViewCreator.GetViewName("Visualization", string.Empty);
                ViewSheet _viewSheet = _sheetCreator.CreateSheet(_titleblockName, _viewName, _viewName);

                _visualizationDeliverable.Sheet = _viewSheet;
                _visualizationDeliverable.DemandView = createView3D("Demand", _boundedViewCreator, _demandViewScale, _modelElementsOgs);
                _visualizationDeliverable.CombinedView = createView3D("Combined", _boundedViewCreator, _combinedViewScale, _modelElementsOgs);
                _visualizationDeliverable.CapacityView = createView3D("Capacity", _boundedViewCreator, _capacityViewScale, _modelElementsOgs);

                _levelAbove = _visualizationDeliverable.Level;
            }

        }

        private static View3D createView3D(string prefix, BoundedViewCreator boundedViewCreator, int demandViewScale, OverrideGraphicSettings modelElementsOgs)
        {
            string _fullViewName = boundedViewCreator.GetViewName(prefix, "FP");
            View3D _view3D = boundedViewCreator.CreateView3D(demandViewScale, _fullViewName);
            _view3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
            _view3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_SectionBox), true);
            _view3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Levels), true);
            _view3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Floors), true);
            _view3D.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_StructuralColumns), modelElementsOgs);
            _view3D.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_StructuralFraming), modelElementsOgs);
            _view3D.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_Walls), modelElementsOgs);
            _view3D.SetOrientation(new ViewOrientation3D(_view3D.Origin, new XYZ(0, 1, 0), new XYZ(0, 0, -1)));
            _view3D.AreAnalyticalModelCategoriesHidden = true;
            return _view3D;
        }

        private static OverrideGraphicSettings getCapacityOgs(Document doc)
        {
            FillPatternElement _solidFillPattern = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>()
                .FirstOrDefault(fpe => fpe.GetFillPattern().IsSolidFill);

            Color _capacityColor = new Color((byte)0, (byte)0, (byte)255);
            int _transparencyPercentForDemandCapacity = 96;

            OverrideGraphicSettings _capacityOgs = new OverrideGraphicSettings();

            _capacityOgs.SetSurfaceForegroundPatternColor(_capacityColor);
            _capacityOgs.SetCutForegroundPatternColor(_capacityColor);

            _capacityOgs.SetSurfaceForegroundPatternId(_solidFillPattern.Id);
            _capacityOgs.SetCutForegroundPatternId(_solidFillPattern.Id);
            _capacityOgs.SetSurfaceForegroundPatternVisible(true);
            _capacityOgs.SetCutForegroundPatternVisible(true);

            _capacityOgs.SetSurfaceTransparency(_transparencyPercentForDemandCapacity);

            return _capacityOgs;
        }

        private static OverrideGraphicSettings getDemandOgs(Document doc)
        {
            FillPatternElement _solidFillPattern = new FilteredElementCollector(doc)
                .OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>()
                .FirstOrDefault(fpe => fpe.GetFillPattern().IsSolidFill);

            Color _demandColor = new Color((byte)255, (byte)0, (byte)0);
            int _transparencyPercentForDemandCapacity = 96;

            OverrideGraphicSettings _capacityOgs = new OverrideGraphicSettings();

            _capacityOgs.SetSurfaceForegroundPatternColor(_demandColor);
            _capacityOgs.SetCutForegroundPatternColor(_demandColor);

            _capacityOgs.SetSurfaceForegroundPatternId(_solidFillPattern.Id);
            _capacityOgs.SetCutForegroundPatternId(_solidFillPattern.Id);
            _capacityOgs.SetSurfaceForegroundPatternVisible(true);
            _capacityOgs.SetCutForegroundPatternVisible(true);

            _capacityOgs.SetSurfaceTransparency(_transparencyPercentForDemandCapacity);

            return _capacityOgs;
        }

        private static OverrideGraphicSettings getModelOgs()
        {
            int _transparencyPercentForModelElements = 100;

            OverrideGraphicSettings _modelElementsOgs = new OverrideGraphicSettings();
            _modelElementsOgs.SetSurfaceTransparency(_transparencyPercentForModelElements);

            return _modelElementsOgs;
        }
        
        private static IEnumerable<DirectShape> getDemandDirectShapes(IEnumerable<VisualizationDeliverable> visualizationDeliverables)
        {
            return visualizationDeliverables.SelectMany(p => getDemandDirectShapes(p));
        }

        private static IEnumerable<DirectShape> getDemandDirectShapes(VisualizationDeliverable visualizationDeliverable)
        {
            return visualizationDeliverable.LevelLoadModel.LoadModels
                .Where(p =>
                    p.LoadType == LoadType.Demand ||
                    p.LoadType == LoadType.LiveLoad ||
                    p.LoadType == LoadType.Formwork ||
                    p.LoadType == LoadType.Other).OfType<LoadModel>()
                    .Select(p => p.DirectShape);
        }

        private static IEnumerable<DirectShape> getCapacityDirectShapes(IEnumerable<VisualizationDeliverable> visualizationDeliverables)
        {
            return visualizationDeliverables.SelectMany(p => getCapacityDirectShapes(p));
        }

        private static IEnumerable<DirectShape> getCapacityDirectShapes(VisualizationDeliverable visualizationDeliverable)
        {
            return visualizationDeliverable.LevelLoadModel.LoadModels
                .Where(p =>
                    p.LoadType == LoadType.Capacity ||
                    p.LoadType == LoadType.ReshoreDemand).OfType<LoadModel>()
                    .Select(p => p.DirectShape);
        }

    }
}
