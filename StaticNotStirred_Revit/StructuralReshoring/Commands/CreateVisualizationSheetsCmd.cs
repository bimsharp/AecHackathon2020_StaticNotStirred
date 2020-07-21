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
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Create Visualization Sheets"))
            {
                _trans.Start();
                try
                {
                    _result = createVisualizationSheets(uiDoc);

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

        private static Result createVisualizationSheets(UIDocument uiDoc)
        {
            Document _doc = uiDoc.Document;

            //Setup
            string _titleblockName = "E1 30 x 42 Horizontal: E1 30x42 Horizontal";

            XYZ _demandViewCoordinate = new XYZ(0.780872717690263, 1.83481042377296, 0);
            int _demandViewScale = 120;

            XYZ _combinedViewCoordinate = new XYZ(0.780872717690263, 0.766954561856788, 0);
            int _combinedViewScale = 120;

            XYZ _capacityViewCoordinate = new XYZ(2.17649771769026, 0.766954561856788, 0);
            int _capacityViewScale = 120;

            SheetCreator _sheetCreator = new SheetCreator(_doc);

            var _levels = Getters.GetLevels(_doc).OrderByDescending(p => p.Elevation).ToList();

            var _floors = Getters.GetFloors(_doc).OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _structuralColumns = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_StructuralColumns).OfType<FamilyInstance>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _structuralFraming = new FilteredElementCollector(_doc).OfCategory(BuiltInCategory.OST_StructuralFraming).OfType<FamilyInstance>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            var _walls = new FilteredElementCollector(_doc).OfClass(typeof(Wall)).OfType<Wall>().OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();

            var _visitedStructuralColumnIds = new HashSet<ElementId>();
            var _visitedStructuralFramingIds = new HashSet<ElementId>();
            var _visitedWallIds = new HashSet<ElementId>();

            BoundingBoxXYZ _modelExtents =
                GeometryHelpers.GetElementsBBox(
                    new FilteredElementCollector(_doc).WhereElementIsViewIndependent().WhereElementIsNotElementType().ToList());
            _modelExtents.Min = new XYZ(_modelExtents.Min.X, _modelExtents.Min.Y, _levels.LastOrDefault().Elevation - 1.0);

            FillPatternElement _solidFillPattern = new FilteredElementCollector(_doc)
                .OfClass(typeof(FillPatternElement)).Cast<FillPatternElement>()
                .FirstOrDefault(fpe => fpe.GetFillPattern().IsSolidFill);
            Color _demandColor = new Color((byte)255, (byte)0, (byte)0);
            Color _capacityColor = new Color((byte)0, (byte)0, (byte)255);
            int _transparencyPercent = 96;
            
            Category _demandCategory = Category.GetCategory(_doc, BuiltInCategory.OST_GenericModel); // _doc.Settings.Categories.NewSubcategory(Category.GetCategory(_doc, BuiltInCategory.OST_GenericModel), "Demand");
            Category _capacityCategory = Category.GetCategory(_doc, BuiltInCategory.OST_GenericModel); // _doc.Settings.Categories.NewSubcategory(Category.GetCategory(_doc, BuiltInCategory.OST_GenericModel), "Capacity");

            List<Solid> _demandSolids = new List<Solid>();

            Level _levelAbove = null;
            Level _bottomLevel = _levels.LastOrDefault();

            List<Tuple<ViewSheet, View, View, View>> _sheetsWithViews = new List<Tuple<ViewSheet, View, View, View>>();
            foreach (Level _level in _levels)
            {
                if (_levelAbove == null) _levelAbove = _level;

                List<Floor> _currentLevelFloors = _floors.Where(p => p.LevelId == _level.Id).ToList();
                List<FamilyInstance> _currentLevelStructuralColumns = new List<FamilyInstance>();
                List<FamilyInstance> _currentLevelStructuralFraming = new List<FamilyInstance>();
                List<Wall> _currentLevelWalls = new List<Wall>();

                BoundingBoxXYZ _levelBounds = new BoundingBoxXYZ();
                _levelBounds.Min = new XYZ(_modelExtents.Min.X, _modelExtents.Min.Y, _level.Elevation);
                _levelBounds.Max = new XYZ(_modelExtents.Max.X, _modelExtents.Max.Y, _levelAbove.Elevation);

                BoundingBoxIsInsideFilter _withinLevelBoundsFilter = new BoundingBoxIsInsideFilter(new Outline(_levelBounds.Min, _levelBounds.Max));
                BoundingBoxIntersectsFilter _intersectsLevelBoundsFilter = new BoundingBoxIntersectsFilter(new Outline(_levelBounds.Min, _levelBounds.Max));

                if (_structuralColumns.Count > 0)
                {
                    _currentLevelStructuralColumns =
                        new FilteredElementCollector(_doc, _structuralColumns.Select(p => p.Id).ToList())
                        .WherePasses(_intersectsLevelBoundsFilter)
                        .OfType<FamilyInstance>().ToList();
                }

                if (_structuralFraming.Count > 0)
                {
                    _currentLevelStructuralFraming =
                        new FilteredElementCollector(_doc, _structuralFraming.Select(p => p.Id).ToList())
                        .WherePasses(_withinLevelBoundsFilter)
                        .OfType<FamilyInstance>().ToList();
                }

                if (_walls.Count > 0)
                {
                    _currentLevelWalls =
                        new FilteredElementCollector(_doc, _walls.Select(p => p.Id).ToList())
                        .WherePasses(_intersectsLevelBoundsFilter)
                        .OfType<Wall>().ToList();
                }

                LevelLoadModel _currentLevelLoadModel = LevelLoadModel.Create(_level);
                foreach (Floor _floor in _currentLevelFloors) _currentLevelLoadModel.addFloorLoadModel(_floor);
                foreach (FamilyInstance _structuralColumn in _currentLevelStructuralColumns) _currentLevelLoadModel.addFamilyInstanceLoadModel(_structuralColumn);
                foreach (FamilyInstance _structuralFrame in _currentLevelStructuralFraming) _currentLevelLoadModel.addFamilyInstanceLoadModel(_structuralFrame);
                foreach (Wall _wall in _currentLevelWalls) _currentLevelLoadModel.addWallLoadModel(_wall);

                foreach (SquareLoadModel _demandProfileModel in _currentLevelLoadModel.DemandProfileModels)
                {
                    try
                    {
                        string _comments = _demandProfileModel.Element?.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS)?.AsString();

                        double _distanceToModelBottom = 0.0;
                        if (_demandProfileModel.Element is Floor) _distanceToModelBottom = _level.Elevation - _bottomLevel.Elevation;
                        else _distanceToModelBottom = _levelAbove.Elevation - _bottomLevel.Elevation;

                        Solid _projectedProfile = _demandProfileModel.GetProjectedSolid(XYZ.BasisZ.Negate(), _distanceToModelBottom);
                        if (_projectedProfile != null) _demandSolids.Add(_projectedProfile);
                    }
                    catch (Exception _ex)
                    {
                        bool b = false;
                    }
                }

                _levelAbove = _level;
            }

            List<DirectShape> _demandDirectShapes = FamilyHelpers.CreateDirectShapes(_doc, _demandSolids, _demandCategory.Id);
            HashSet<ElementId> _directShapeIds = new HashSet<ElementId>(_demandDirectShapes.Select(p => p.Id).Distinct());

            //ToDo: this is set differently in 2019 and earlier
            OverrideGraphicSettings _overrideGraphicSettings = new OverrideGraphicSettings();

            _overrideGraphicSettings.SetSurfaceForegroundPatternColor(_demandColor);
            _overrideGraphicSettings.SetCutForegroundPatternColor(_demandColor);

            _overrideGraphicSettings.SetSurfaceForegroundPatternId(_solidFillPattern.Id);
            _overrideGraphicSettings.SetCutForegroundPatternId(_solidFillPattern.Id);
            _overrideGraphicSettings.SetSurfaceForegroundPatternVisible(true);
            _overrideGraphicSettings.SetCutForegroundPatternVisible(true);

            _overrideGraphicSettings.SetSurfaceTransparency(_transparencyPercent);

            //foreach (DirectShape _directShape in _demandDirectShapes) _view.SetElementOverrides(_directShape.Id, _overrideGraphicSettings);

            _levelAbove = null;
            foreach (Level _level in _levels)
            {
                BoundingBoxXYZ _levelBounds = new BoundingBoxXYZ();
                _levelBounds.Min = new XYZ(_modelExtents.Min.X, _modelExtents.Min.Y, _level.Elevation);

                if (_levelAbove == null)
                {
                    _levelBounds.Max = new XYZ(_modelExtents.Max.X, _modelExtents.Max.Y, _modelExtents.Max.Z);
                    _levelAbove = _level;
                }
                else
                {
                    _levelBounds.Max = new XYZ(_modelExtents.Max.X, _modelExtents.Max.Y, _levelAbove.Elevation);
                }

                //View Creation
                BoundedViewCreator _boundedViewCreator = new BoundedViewCreator(_level, null, _levelBounds);

                string _viewName = _boundedViewCreator.GetViewName("Visualization", string.Empty);
                ViewSheet _viewSheet = _sheetCreator.CreateSheet(_titleblockName, _viewName, _viewName);

                string _demandViewName = _boundedViewCreator.GetViewName("Demand", "FP");
                View3D _demand3D = _boundedViewCreator.CreateView3D(_demandViewScale, _demandViewName);
                _demand3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                _demand3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_SectionBox), true);
                _demand3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Levels), true);
                _demand3D.SetOrientation(new ViewOrientation3D(_demand3D.Origin, new XYZ(0, 1, 0), new XYZ(0, 0, -1)));
                _demand3D.AreAnalyticalModelCategoriesHidden = true;

                string _combinedViewName = _boundedViewCreator.GetViewName("Combined", "FP");
                View3D _combined3D = _boundedViewCreator.CreateView3D(_combinedViewScale, _combinedViewName);
                _combined3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                _combined3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_SectionBox), true);
                _combined3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Levels), true);
                _combined3D.SetOrientation(new ViewOrientation3D(_combined3D.Origin, new XYZ(0, 1, 0), new XYZ(0, 0, -1)));
                _combined3D.AreAnalyticalModelCategoriesHidden = true;

                string _capacityViewName = _boundedViewCreator.GetViewName("Capacity", "FP");
                View3D _capacity3D = _boundedViewCreator.CreateView3D(_capacityViewScale, _capacityViewName);
                _capacity3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                _capacity3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_SectionBox), true);
                _capacity3D.SetCategoryHidden(new ElementId(BuiltInCategory.OST_Levels), true);
                _capacity3D.SetOrientation(new ViewOrientation3D(_capacity3D.Origin, new XYZ(0, 1, 0), new XYZ(0, 0, -1)));
                _capacity3D.AreAnalyticalModelCategoriesHidden = true;

                _sheetsWithViews.Add(new Tuple<ViewSheet, View, View, View>(_viewSheet, _demand3D, _combined3D, _capacity3D));

                _levelAbove = _level;
            }

            _doc.Regenerate();

            foreach (var _sheetWithView in _sheetsWithViews)
            {
                if (_sheetWithView.Item2 != null)
                {
                    List<Element> _demandPlanDirectShapes = new FilteredElementCollector(_doc, _sheetWithView.Item2.Id).OfCategory(BuiltInCategory.OST_GenericModel).ToList();
                    _demandPlanDirectShapes = _demandPlanDirectShapes.Where(p => _directShapeIds.Contains(p.Id)).ToList();
                    foreach (Element _directShape in _demandPlanDirectShapes) _sheetWithView.Item2.SetElementOverrides(_directShape.Id, _overrideGraphicSettings);

                    Viewport.Create(_doc, _sheetWithView.Item1.Id, _sheetWithView.Item2.Id, _demandViewCoordinate);
                }
                if (_sheetWithView.Item3 != null)
                {
                    List<DirectShape> _combinedPlanDirectShapes = new FilteredElementCollector(_doc, _sheetWithView.Item3.Id).OfClass(typeof(DirectShape)).OfType<DirectShape>().Where(p => _directShapeIds.Contains(p.Id)).ToList();
                    foreach (DirectShape _directShape in _combinedPlanDirectShapes) _sheetWithView.Item3.SetElementOverrides(_directShape.Id, _overrideGraphicSettings);

                    Viewport.Create(_doc, _sheetWithView.Item1.Id, _sheetWithView.Item3.Id, _combinedViewCoordinate);
                }
                if (_sheetWithView.Item4 != null)
                {
                    Viewport.Create(_doc, _sheetWithView.Item1.Id, _sheetWithView.Item4.Id, _capacityViewCoordinate);
                }
            }

            return Result.Succeeded;
        }

    }
}
