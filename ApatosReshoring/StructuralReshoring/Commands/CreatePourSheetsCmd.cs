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
using ApatosReshoring_Revit.Helpers.Annotations;
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class CreatePourSheetsCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return CreatePourSheets(commandData.Application.ActiveUIDocument);
        }

        public static Result CreatePourSheets(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Create Pour Sheets"))
            {
                _trans.Start();
                try
                {
                    _result = createPourSheets(uiDoc);

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

        private static Result createPourSheets(UIDocument uiDoc)
        {

            //ToDo: these might be good as settings in a future UI
            string _titleblockName = "E1 30 x 42 Horizontal: E1 30x42 Horizontal";
            //string _elementName = "C-2"; // "EllisShore_LumberWithClamps";
            //
            //int _3dViewScale = 48;
            //XYZ _3dViewportCenter = new XYZ(2.21129429621462, 0.656294714474886, 0);
            //
            //int _sectionViewScale = 32;
            //XYZ _sectionViewportCenter = new XYZ(0.7791340191012, 1.72774616204266, 0);

            int _floorplanViewScale = 24;
            XYZ _floorplanViewportCenter = new XYZ(1.23473112570493, 1.15208223453682, 0);

            XYZ _columnScheduleBottomLeft = new XYZ(2.31721454780864, 2.37688649933288, 0);
            XYZ _levelScheduleBottomLeft = new XYZ(0.156336990263879, 2.37688649933288, 0);

            Document _doc = uiDoc.Document;

            //Get a BoundedView3DDefinition for each Level - Scope Box in the project
            var _levels = Getters.GetLevels(_doc);

            var _scopeBoxes = Getters.GetScopeBoxes(_doc);

            var _boundedViewCreators = new List<BoundedViewCreator>();
            double _extraExtents = 0.5;
            foreach (Level _level in _levels)
            {
                Level _levelAbove = _levels.FirstOrDefault(p => p.Elevation > _level.Elevation);
                if (_levelAbove == null) continue;

                foreach (Element _scopeBox in _scopeBoxes)
                {
                    BoundingBoxXYZ _scopeBoxBounds = _scopeBox.get_BoundingBox(null);
                    BoundingBoxXYZ _viewBounds = new BoundingBoxXYZ
                    {
                        Min = new XYZ(
                            _scopeBoxBounds.Min.X - _extraExtents, 
                            _scopeBoxBounds.Min.Y - _extraExtents, 
                            _level.Elevation - _extraExtents),
                        Max = new XYZ(
                            _scopeBoxBounds.Max.X + _extraExtents, 
                            _scopeBoxBounds.Max.Y + _extraExtents, 
                            _levelAbove.Elevation + _extraExtents)
                    };

                    _boundedViewCreators.Add(new BoundedViewCreator(_level, _scopeBox, _viewBounds));
                }
            }

            //Generate Views, set their Boundaries & Names, Adjust Visiblity Graphics
            OverrideGraphicSettings _70Transparent = new OverrideGraphicSettings();
            _70Transparent.SetSurfaceTransparency(70);
            
            List<Tuple<ElementId, string, List<View>>> _createdSheetIdViewIdSets = new List<Tuple<ElementId, string, List<View>>>();
            foreach (var _boundedViewCreator in _boundedViewCreators)
            {
                SheetCreator _sheetCreator = new SheetCreator(_doc);
                

                //Create Sheet
                string _pourName = _boundedViewCreator.GetViewName(string.Empty, string.Empty);
                ViewSheet _viewSheet = _sheetCreator.CreateSheet(_titleblockName, _pourName, _pourName);

                ////Create 3D View
                //View3D _3DView = _boundedViewCreator.CreateView3D(_3dViewScale);
                //_3DView.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                //_3DView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_Floors), _70Transparent);
                
                //Create Floorplan View
                ViewPlan _floorPlanView = _boundedViewCreator.CreateViewPlan(_floorplanViewScale);
                _floorPlanView.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                _floorPlanView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_Floors), _70Transparent);

                ////Create Section View
                //ViewSection _sectionView = _boundedViewCreator.CreateViewSection(_sectionViewScale);
                //_sectionView.SetCategoryHidden(new ElementId(BuiltInCategory.OST_VolumeOfInterest), true);
                //_sectionView.SetCategoryOverrides(new ElementId(BuiltInCategory.OST_Floors), _70Transparent);

                //Create Schedules
                ViewSchedule _columnSchedule = ScheduleCreator.CreatePourColumnSchedule(_doc, _pourName, " Reshores");
                ScheduleSheetInstance _columnScheduleSheetInstance = ScheduleSheetInstance.Create(_doc, _viewSheet.Id, _columnSchedule.Id, _columnScheduleBottomLeft);

                ViewSchedule _levelSchedule = ScheduleCreator.CreatePourLoadSchedule(_doc, _boundedViewCreator.Level?.Name, " - " + _boundedViewCreator.ScopeBox?.Name + " Loads");
                ScheduleSheetInstance _levelScheduleSheetInstance = ScheduleSheetInstance.Create(_doc, _viewSheet.Id, _levelSchedule.Id, _levelScheduleBottomLeft);

                _createdSheetIdViewIdSets.Add(new Tuple<ElementId, string, List<View>>(
                    _viewSheet.Id,
                    _pourName,
                    new List<View> { /*_3DView,*/ _floorPlanView, /*_sectionView*/ }
                  ));
            }

            _doc.Regenerate();

            FamilySymbol _tagSymbol = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_MultiCategoryTags)
                .OfClass(typeof(FamilySymbol)).OfType<FamilySymbol>()
                .FirstOrDefault(p => p.FamilyName.Contains("Mark"));

            foreach (var _createdSheetIdViewIdSet in _createdSheetIdViewIdSets)
            {
                foreach (View _view in _createdSheetIdViewIdSet.Item3)
                {
                    if (_view == null) continue;

                    //if (_view is View3D) Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _3dViewportCenter);
                    else if (_view is ViewPlan)
                    {
                        DimensionCreator.CreateDimensions(_view);
                        TagCreator.CreateTags(_view, _tagSymbol);
                        Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _floorplanViewportCenter);
                    }
                    //else if (_view is ViewSection) Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _sectionViewportCenter);
                }
            }

            return Result.Succeeded;
        }

    }
}
