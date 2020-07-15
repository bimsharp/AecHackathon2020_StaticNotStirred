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
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class CreateDeliverableSheetsCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return CreateDeliverableSheets(commandData.Application.ActiveUIDocument);
        }

        public static Result CreateDeliverableSheets(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Create Sectioned Views"))
            {
                _trans.Start();
                try
                {
                    _result = createDeliverableSheets(uiDoc);

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

        private static Result createDeliverableSheets(UIDocument uiDoc)
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

            int _floorplanViewScale = 20;
            XYZ _floorplanViewportCenter = new XYZ(1.13879414318027, 1.16675126090371, -0.025);

            XYZ _scheduleBottomLeft = new XYZ(1.9435380772204, 2.43263378194158, 0);

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
                ScheduleCreator _scheduleCreator = new ScheduleCreator(_doc);

                //Create Sheet
                string _pourName = _boundedViewCreator.GetViewName(string.Empty);
                ViewSheet _sheetView = _sheetCreator.CreateSheet(_titleblockName, _pourName, _pourName);

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

                //ToDo: get necessary parameters into shared parameters, so that they are visible in Project
                // //Create Schedule
                // ViewSchedule _viewSchedule = _scheduleCreator.CreateSchedule(false, _pourName + " Shoring");
                // ScheduleField _comments = _scheduleCreator.AppendField(_viewSchedule, "Comments"); //Pour Name
                // ScheduleField _count = _scheduleCreator.AppendField(_viewSchedule, "Count");
                // ScheduleField _familyAndType = _scheduleCreator.AppendField(_viewSchedule, "Family and Type");
                // //ScheduleField _material = _scheduleCreator.AppendField(_viewSchedule, "Material");
                // //ScheduleField _clampSpacing = _scheduleCreator.AppendField(_viewSchedule, "Clamp Spacing");
                // //ScheduleField _lowerShoreLength = _scheduleCreator.AppendField(_viewSchedule, "Lower Shore Length");
                // //ScheduleField _upperShoreLength = _scheduleCreator.AppendField(_viewSchedule, "Upper Shore Length");
                // ScheduleField _height = _scheduleCreator.AppendField(_viewSchedule, "Height");
                // ScheduleField _clearShoreHeight = _scheduleCreator.AppendField(_viewSchedule, "Clear Shore Height"); //Total Shore Length
                // //ScheduleField _shoreLength = _scheduleCreator.AppendField(_viewSchedule, "Shore Overlap");
                // //ScheduleField _lumberWidth = _scheduleCreator.AppendField(_viewSchedule, "Lumber Width");
                // //ScheduleField _lumberThickness = _scheduleCreator.AppendField(_viewSchedule, "Lumber Thickness");
                // ScheduleField _loadCapacity = _scheduleCreator.AppendField(_viewSchedule, "Load Capacity"); //Safe Working Load
                // 
                // _scheduleCreator.AppendSortField(_viewSchedule, _familyAndType);
                // //_scheduleCreator.AppendSortField(_viewSchedule, _lowerShoreLength);
                // //_scheduleCreator.AppendSortField(_viewSchedule, _upperShoreLength);
                // _scheduleCreator.AppendSortField(_viewSchedule, _clearShoreHeight);
                // 
                // _scheduleCreator.AppendFilter(_viewSchedule, _comments, ScheduleFilterType.Equal, _pourName);
                // 
                // ScheduleSheetInstance _scheduleSheetInstance = ScheduleSheetInstance.Create(_doc, _sheetView.Id, _viewSchedule.Id, _scheduleBottomLeft);

                _createdSheetIdViewIdSets.Add(new Tuple<ElementId, string, List<View>>(
                    _sheetView.Id,
                    _pourName,
                    new List<View> { /*_3DView,*/ _floorPlanView, /*_sectionView*/ }
                  ));
            }

            _doc.Regenerate();

            foreach (var _createdSheetIdViewIdSet in _createdSheetIdViewIdSets)
            {
                foreach (View _view in _createdSheetIdViewIdSet.Item3)
                {
                    if (_view == null) continue;

                    //if (_view is View3D) Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _3dViewportCenter);
                    else if (_view is ViewPlan)
                    {
                        DimensionCreator.CreateDimensions(_view, _createdSheetIdViewIdSet.Item2);
                        Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _floorplanViewportCenter);
                    }
                    //else if (_view is ViewSection) Viewport.Create(_doc, _createdSheetIdViewIdSet.Item1, _view.Id, _sectionViewportCenter);
                }
            }

            return Result.Succeeded;
        }

    }
}
