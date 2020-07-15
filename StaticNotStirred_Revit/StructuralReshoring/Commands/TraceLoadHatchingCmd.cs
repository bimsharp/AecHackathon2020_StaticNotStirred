#region References
using StaticNotStirred_Revit.Helpers.Selections;
using StaticNotStirred_Revit.Helpers.Views;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class TraceLoadHatchingCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return TraceLoadHatching(commandData.Application.ActiveUIDocument);
        }

        public static Result TraceLoadHatching(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Trace Load Hatching"))
            {
                _trans.Start();
                try
                {
                    _result = traceLoadHatching(uiDoc);

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

        private static Result traceLoadHatching(UIDocument uiDoc)
        {
            Document _doc = uiDoc.Document;

            //Get a BoundedView3DDefinition for each Level - Scope Box in the project
            var _levels = new FilteredElementCollector(_doc)
                .OfClass(typeof(Level)).Cast<Level>()
                .OrderBy(p => p.Elevation).ToList();

            

            return Result.Succeeded;
        }

    }
}
