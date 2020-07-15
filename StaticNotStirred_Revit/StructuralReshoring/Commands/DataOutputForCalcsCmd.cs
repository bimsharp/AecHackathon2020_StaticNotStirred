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
using StaticNotStirred_Revit.Helpers;
#endregion


namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    class DataOutputForCalcsCmd : IExternalCommand
    {
        //Point Loads
        //Scheduling
        //Power App and BlueBeam can both use manual input of XML
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return DataOutputForCalcs(commandData.Application.ActiveUIDocument);
        }

        public static Result DataOutputForCalcs(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Trace Load Hatching"))
            {
                _trans.Start();
                try
                {
                    _result = dataOutputForCalcs(uiDoc);

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

        private static Result dataOutputForCalcs(UIDocument uiDoc)
        {
            Document _doc = uiDoc.Document;

            CalculationOutputs _calculationOutputs = new CalculationOutputs
            {
                ConcreteDensity = 1.5,                 //Pcf
                ConcreteDensityUnits = "PCF",
                ConstructionLiveLoad = 2.5,            //Psf
                ConstructionLiveLoadUnits = "PSF",
                FormworkWeight = 3.5,                  //Psf
                FormworkWeightUnits = "PSF",
                BeamFormwork = 4.5,                    //Psf
                BeamFormworkUnits = "PSF",
                BeamFormworkAdditionalLength = 5.5,    //Psf
                BeamFormworkAdditionalLengthUnits = "PSF",
                WallFormwork = 6.5,                    //Psf
                WallFormworkUnits = "PSF",
                DaysToClosurePourPourBack = 7.5,       //days
                DaysToClosurePourPourBackUnits = "DAYS",
                DaysToFormworkInstall = 8.5,           //days
                DaysToFormworkInstallUnits = "DAYS",

                ClearShoreHeight = 9.5,                //ft
                ClearShoreHeightUnits = "FT",
            };
            _calculationOutputs.SerializeToXml(@"C:\$\AEC Hackathon 2020\StaticNotStirred_Revit\Resources\2019 Model\" + "report.xml");

            return Result.Succeeded;
        }

    }
}
