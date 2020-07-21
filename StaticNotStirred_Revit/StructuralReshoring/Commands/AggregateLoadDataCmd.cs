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
using StaticNotStirred_Revit.Helpers.Geometry;
using StaticNotStirred_Revit.Helpers;
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class AggregateLoadDataCmd : IExternalCommand
    {
        //Safe Assumptions per existing reshoring calculator
        //https://www.concrete.org/store/productdetail.aspx?ItemID=SP4AUTOR&Format=EXCEL&Language=English&Units=US_Units
        //  All previously cast slabs are identical and have equaal stiffness
        //  Ground-level or other grade base suport is rigid
        //  Shores and Reshores are spaced closely enough to treat their reactions as a distributed load
        //  Shores and reshores are infinitely stiff relative to the slabs
        //  Reshores are installed snug-tight without initially carrying any load
        //  No Super Heavy Point Loads
        //  No Cantilevered Slabs

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

            var _levels = Getters.GetLevels(_doc).OrderByDescending(p => p.Elevation).ToList();

            var _floors = Getters.GetFloors(_doc);

            Level _levelAbove = null;
            foreach (Level _level in _levels)
            {
                //if (_levelAbove == null)
                //{
                //    _levelAbove = _level;
                //    continue;
                //}
                //
                ////Get Floors on the current Level
                ////ToDo: Get Load Zones on the current Level
                ////ToDo: get Wall Loads on the current level
                ////ToDo: get Beam Loads on the current Level (needed? since weight of current level isn't applied to the current level's beams)
                //List<Floor> _currentLevelFloors = _floors.Where(p =>
                //    p.get_BoundingBox(null)?.Max.Z >= _level.Elevation &&
                //    p.get_BoundingBox(null)?.Max.Z < _levelAbove.Elevation).ToList();
                //
                //if (_currentLevelFloors.Count == 0) continue;
                //
                ////Get Floors for the Levels above the current Level
                ////ToDo: Get Load Zones above the current Level
                ////ToDo: get Wall Loads above the current Level
                ////ToDo: get Beam Loads above the current Level
                //List<Floor> _levelAboveFloors = _floors.Where(p =>
                //p.get_BoundingBox(null)?.Max.Z >= _levelAbove.Elevation).ToList();
                //
                //if (_levelAboveFloors.Count == 0) continue;
                //
                //List<FloorModel> _floorModels = _currentLevelFloors.Select(p => FloorModel.Create(p)).Where(p => p != null).ToList();
                //
                //foreach (FloorModel _floorModel in _floorModels)
                //{
                //    foreach (FloorProfileModel _floorProfileModel in _floorModel.FloorProfileModels)
                //    {
                //        Solid _projectedProfile = _floorProfileModel.GetProjectedSolid(XYZ.BasisZ, 1000.0);
                //
                //        List<Element> _intersectedFloorsAbove = Getters.GetIntersectedElements(_doc, _levelAboveFloors, _projectedProfile);
                //    }
                //}
            }

            return Result.Succeeded;
        }



    }
}
