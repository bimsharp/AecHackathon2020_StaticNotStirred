#region References
using StaticNotStirred_Revit.Helpers.Families;
using StaticNotStirred_Revit.Helpers.Selections;
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
    public class PlaceTemporaryShoringCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return PlaceTemporayShoring(commandData.Application.ActiveUIDocument);
        }

        public static Result PlaceTemporayShoring(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Place Scope Boxes"))
            {
                _trans.Start();
                try
                {
                    _result = placeTemporayShoring(uiDoc);

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

        private static Result placeTemporayShoring(UIDocument uiDoc)
        {
            Document _doc = uiDoc.Document;

            string _familyPathName = @"C:\$\AEC Hackathon 2020\AecHackathon2020_StructuralReshoring\Resources\2019 Families\EllisShore_LumberWithClamps.rfa";
            var _familyDefinition = FamilyHelpers.GetOrLoadFamilyDefinition(_doc, _familyPathName);

            


            //scale - 1" = 30'
            //hatches are typically standardized - governed by building code

            //Question: what determines clamp spacing?  https://ellismanufacturing.com/pages/column-clamp-spacing-calculator
            //Question: how much "extra" lumber is needed, in addition to clamp spacing? 3.5"
            //Question: what is the logic behind placement/pour schedule order?  By Level?  Lowest Level to Highest Level? Heaviest Total Vertical Load to Lightest?
            //Question: what numbers are required for equipment selection?
            //Question: what building code info is needed?
            //Question: what room data is needed?
            //Question: what area data is needed?
            //Question: is there a "set" level count that we need concerned with? for example, is it "only the next 5 levels above" or is it "all levels above"?  depends - it will be a varying X levels

            //Get: CAD, RVT, PDF "real" files
            //Get: CAD - read hatch patterns & key to understand
            //Get: families from Jason

            //ToDo: read input: level heights for all levels above - this will likely be a set number, based on the number of levels expected to be above the current level, while this level is drying 
            //ToDo: read input: level static load (optional: Foundations, Snow Load, Seismic Load, etc)
            //ToDo: read input: level live load
            //ToDo: read input: floor density / weight per sqft / thickness
            //ToDo: read input: building code
            //ToDo: read input: room data
            //ToDo: read input: area data

            //ToDo: generate output: temporary shoring insertion points
            //ToDo: generate output: temporary shoring families placed; with parameters set
            //      Total Shore Height
            //      Lower Shore Member Length
            //      Clamp Spacing
            //      Type Name

            //ToDo: generate output: calculation numerical inputs & outputs (for equipment selection)
            //ToDo: generate output: placement schedule

            return Result.Succeeded;
        }

    }
}
