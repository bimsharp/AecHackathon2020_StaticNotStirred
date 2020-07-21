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
                    typeof(PlaceTemporaryShoringCmd).FullName)) as PushButton;

                // Create Pour Sheets
                string _createPourSheetsCmdButtonName = "Create\nPour Sheets";
                PushButton _createPourSheetsCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _createPourSheetsCmdButtonName + "_id",
                    _createPourSheetsCmdButtonName,
                    _assemblyPath,
                    typeof(CreatePourSheetsCmd).FullName)) as PushButton;

                // Create Floorplate Sheets
                string _createFloorplateSheetsCmdButtonName = "Create\nFloorplate\nSheets";
                PushButton _createFloorplateSheetsCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _createFloorplateSheetsCmdButtonName + "_id",
                    _createFloorplateSheetsCmdButtonName,
                    _assemblyPath,
                    typeof(CreateFloorplateSheetsCmd).FullName)) as PushButton;

                // Create Visualization Sheets
                string _createVisualizationSheetsCmdButtonName = "Create\nVisualization\nSheets";
                PushButton _createVisualizationSheetsCmdButton = _ribbonPanel.AddItem(new PushButtonData(
                    _createVisualizationSheetsCmdButtonName + "_id",
                    _createVisualizationSheetsCmdButtonName,
                    _assemblyPath,
                    typeof(CreateVisualizationSheetsCmd).FullName)) as PushButton;

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
