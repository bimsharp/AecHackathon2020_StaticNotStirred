using StaticNotStirred_Revit.StructuralReshoring.Commands;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit
{
    public class App : IExternalApplication
    {        
        public Result OnStartup(UIControlledApplication application)
        {
            try
            {
                string _tabName = "Static Not Stirred";
                application.CreateRibbonTab(_tabName);

                string _panelName = "Structural Reshoring";
                RibbonPanel _ribbonPanel = application.CreateRibbonPanel(_tabName, _panelName);

                string _assemblyPath = System.Reflection.Assembly.GetExecutingAssembly()?.Location;

                // Place Temporay Shoring
                string _placeTemporaryShoringCmdButtonName = "Temporary\nShoring";
                PushButton _placeTemporaryShoringCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _placeTemporaryShoringCmdButtonName + "_id", 
                    _placeTemporaryShoringCmdButtonName, 
                    _assemblyPath, 
                    typeof(PlaceTemporayShoringCmd).FullName)) as PushButton;

                // Create Sectioned Views
                string _createSectionedViewsCmdButtonName = "Create\nViews";
                PushButton _createSectionedViewsCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _createSectionedViewsCmdButtonName + "_id",
                    _createSectionedViewsCmdButtonName,
                    _assemblyPath,
                    typeof(CreateDeliverableSheetsCmd).FullName)) as PushButton;

                // DataOutput
                string _dataOutputForCalcsCmdButtonName = "Data Output\nFor Calcs";
                PushButton _dataOutputForCalcsCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _dataOutputForCalcsCmdButtonName + "_id",
                    _dataOutputForCalcsCmdButtonName,
                    _assemblyPath,
                    typeof(DataOutputForCalcsCmd).FullName)) as PushButton;
            }
            catch (Exception _ex)
            {
#if DEBUG
                string _error = _ex.Message + Environment.NewLine + Environment.NewLine + _ex.StackTrace;
                TaskDialog.Show("Error", _error);
#endif
                return Result.Failed;
            }

            return Result.Succeeded;
        }

        

        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }
    }
}
