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
    public class CreateReshoringLayoutSheetsCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return CreateReshoringLayoutSheets(commandData.Application.ActiveUIDocument);
        }

        public static Result CreateReshoringLayoutSheets(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Create Reshoring Layout Sheets"))
            {
                _trans.Start();
                try
                {
                    _result = createReshoringLayoutSheets(uiDoc);

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

        private static Result createReshoringLayoutSheets(UIDocument uiDoc)
        {
            Document _doc = uiDoc.Document;

            string _titleblockName = "E1 30 x 42 Horizontal: E1 30x42 Horizontal";
            XYZ _columnScheduleBottomLeft = new XYZ(2.3354130772204, 2.39482128194158, 0);
            XYZ _viewCenter = new XYZ(1.45453036348288, 1.18116967618813, 0.871414246948733);
            XYZ _levelScheduleBottomLeft = new XYZ(0.156336990263879, 2.37688649933288, 0);

            var _levels = Getters.GetLevels(_doc);

            Level _levelAbove = null;
            Level _topLevel = _levels.FirstOrDefault();
            Level _bottomLevel = _levels.LastOrDefault();

            List<Tuple<ViewSheet, View>> _sheetsWithViews = new List<Tuple<ViewSheet, View>>();
            

            foreach (Level _level in _levels)
            {
                if (_levelAbove == null)
                {
                    _levelAbove = _level;
                }

                BoundedViewCreator _boundedViewCreator = new BoundedViewCreator(_level, null, null);
                SheetCreator _sheetCreator = new SheetCreator(_doc);
                string _viewName = _boundedViewCreator.GetViewName(string.Empty, "FP");
                ViewSheet _viewSheet = _sheetCreator.CreateSheet(_titleblockName, _viewName, _viewName);
                ViewPlan _viewPlan = _boundedViewCreator.CreateViewPlan(80);
                _sheetsWithViews.Add(new Tuple<ViewSheet, View>(_viewSheet, _viewPlan));

                ViewSchedule _columnSchedule = ScheduleCreator.CreateLayoutColumnSchedule(_doc, _level.Name, " Reshores");
                ScheduleSheetInstance _columnScheduleSheetInstance = ScheduleSheetInstance.Create(_doc, _viewSheet.Id, _columnSchedule.Id, _columnScheduleBottomLeft);

                ViewSchedule _levelSchedule = ScheduleCreator.CreateLayoutLoadSchedule(_doc, _level.Name, " Loads");
                ScheduleSheetInstance _levelScheduleSheetInstance = ScheduleSheetInstance.Create(_doc, _viewSheet.Id, _levelSchedule.Id, _levelScheduleBottomLeft);

                _levelAbove = _level;
            }

            _doc.Regenerate();

            FamilySymbol _tagSymbol = new FilteredElementCollector(_doc)
                .OfCategory(BuiltInCategory.OST_MultiCategoryTags)
                .OfClass(typeof(FamilySymbol)).OfType<FamilySymbol>()
                .FirstOrDefault(p => p.FamilyName.Contains("Mark"));


            foreach (var _sheetWithView in _sheetsWithViews)
            {
                DimensionCreator.CreateDimensions(_sheetWithView.Item2);
                TagCreator.CreateTags(_sheetWithView.Item2, _tagSymbol);
                Viewport.Create(_doc, _sheetWithView.Item1.Id, _sheetWithView.Item2.Id, _viewCenter);
            }

            return Result.Succeeded;
        }

    }
}
