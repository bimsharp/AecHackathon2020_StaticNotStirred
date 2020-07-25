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
using System.IO;
using StaticNotStirred_Revit.Models;
using StaticNotStirred_UI.Enums;
using StaticNotStirred_UI.Models;
using StaticNotStirred_Revit.Helpers.Parameters;
#endregion

namespace StaticNotStirred_Revit.StructuralReshoring.Commands
{
    [TransactionAttribute(TransactionMode.Manual)]
    [RegenerationAttribute(RegenerationOption.Manual)]
    public class PlaceTemporaryShoringCmd : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return PlaceTemporaryShoring(commandData.Application.ActiveUIDocument);
        }

        public static Result PlaceTemporaryShoring(UIDocument uiDoc)
        {
            Result _result = Result.Cancelled;
            using (Transaction _trans = new Transaction(uiDoc.Document, "Place Temporary Shoring"))
            {
                _trans.Start();
                try
                {
                    //ToDo: these objects would be better passed to/from a UI, or possibly stored somewhere as settings
                    BuildingLoadModel _buildingLoadModel = new BuildingLoadModel(0, 0, 7.0, 7.0, 7.0, 7.0, 7.0);

                    List<ILoadModel> _loadModels = new List<ILoadModel>
                    {
                        LoadModel.Create("Level 18", LoadType.Capacity, 24),
                        LoadModel.Create("Level 17", LoadType.Capacity, 24),
                        LoadModel.Create("Level 16", LoadType.Capacity, 24),
                        LoadModel.Create("Level 15", LoadType.Capacity, 24),
                        LoadModel.Create("Level 14", LoadType.Capacity, 24),
                        LoadModel.Create("Level 13", LoadType.Capacity, 24),
                        LoadModel.Create("Level 12", LoadType.Capacity, 24),
                        LoadModel.Create("Level 11", LoadType.Capacity, 24),
                        LoadModel.Create("Level 10", LoadType.Capacity, 24),
                        LoadModel.Create("Level 09", LoadType.Capacity, 24),
                        LoadModel.Create("Level 08", LoadType.Capacity, 24),
                        LoadModel.Create("Level 07", LoadType.Capacity, 24),
                        LoadModel.Create("Level 06", LoadType.Capacity, 24),
                        LoadModel.Create("Level 05", LoadType.Capacity, 24),
                        LoadModel.Create("Level 04", LoadType.Capacity, 24),
                        LoadModel.Create("Level 03", LoadType.Capacity, 24),
                        LoadModel.Create("Level 02", LoadType.Capacity, 100),
                        LoadModel.Create("Level 01", LoadType.Capacity, 100),
                        LoadModel.Create("P1", LoadType.Capacity, 40),
                        LoadModel.Create("P2", LoadType.Capacity, 40),
                        LoadModel.Create("P3", LoadType.Capacity, 40),
                        LoadModel.Create("P4", LoadType.Capacity, 40),
                        LoadModel.Create("P5", LoadType.Capacity, 40),

                        LoadModel.Create("ROOF", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 18", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 17", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 16", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 15", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 14", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 13", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 12", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 11", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 10", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 09", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 08", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 07", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 06", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 05", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 04", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 03", LoadType.LiveLoad, 40),
                        LoadModel.Create("Level 02", LoadType.LiveLoad, 100),
                        LoadModel.Create("Level 01", LoadType.LiveLoad, 100),
                        LoadModel.Create("P1", LoadType.LiveLoad, 45),
                        LoadModel.Create("P2", LoadType.LiveLoad, 45),
                        LoadModel.Create("P3", LoadType.LiveLoad, 45),
                        LoadModel.Create("P4", LoadType.LiveLoad, 45),
                        LoadModel.Create("P5", LoadType.LiveLoad, 45),

                        LoadModel.Create("Level 18", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 17", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 16", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 15", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 14", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 13", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 12", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 11", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 10", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 09", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 08", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 07", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 06", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 05", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 04", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 03", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 02", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("Level 01", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("P1", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("P2", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("P3", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("P4", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                        LoadModel.Create("P5", LoadType.Formwork, _buildingLoadModel.FormWeightPerSquareFoot),
                    };


                    _result = placeTemporaryShoring(uiDoc, _buildingLoadModel, _loadModels, @"C:\ProgramData\Autodesk\Revit\Addins\2021\ApatosReshoring\Reshoring Poles Ellis 4x4.rfa", "6-on-6");

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

        private static Result placeTemporaryShoring(UIDocument uiDoc, BuildingLoadModel buildingLoadModel, List<ILoadModel> loadModels, string reshoringFamilyPathName, string reshoringFamilySymbol)
        {
            //Setup
            Document _doc = uiDoc.Document;

            //ToDo: it would be valuable to let users pick which Family & Type they want to use - so they can rapidly understand differences in layout for different reshores
            Dictionary<string, FamilyDefinition> _familyMappings = new List<string>{
                @"C:\ProgramData\Autodesk\Revit\Addins\2021\ApatosReshoring\Reshoring Poles 6x6.rfa",
                @"C:\ProgramData\Autodesk\Revit\Addins\2021\ApatosReshoring\Reshoring Poles Ellis 4x4.rfa",
                @"C:\ProgramData\Autodesk\Revit\Addins\2021\ApatosReshoring\Reshoring Poles Titan-HV.rfa",
                @"C:\ProgramData\Autodesk\Revit\Addins\2021\ApatosReshoring\Reshoring Poles Titan-XL.rfa",
            }.ToDictionary(p => Path.GetFileNameWithoutExtension(p),
                           p => FamilyHelpers.GetOrLoadFamilyDefinition(_doc, p));

            string _familyName = Path.GetFileNameWithoutExtension(reshoringFamilyPathName);
            FamilySymbol _temporaryShoreSymbol = null;
            if (_familyMappings.ContainsKey(_familyName))
            {
                Family _family = _familyMappings[_familyName]?.Family;
                foreach (ElementId _symbolID in _family.GetFamilySymbolIds())
                {
                    FamilySymbol _familySymbol = _doc.GetElement(_symbolID) as FamilySymbol;
                    if (_familySymbol == null) continue;

                    if (_familySymbol.Name != reshoringFamilySymbol) continue;

                    _temporaryShoreSymbol = _familySymbol;
                    if (_temporaryShoreSymbol != null) break;
                }
            }

            if (_temporaryShoreSymbol == null) return Result.Failed;
            if (_temporaryShoreSymbol.IsActive == false) _temporaryShoreSymbol.Activate();

            var _levels = Getters.GetLevels(uiDoc.Document).OrderByDescending(p => p.Elevation).ToList();
            var _floors = Getters.GetFloors(_doc).OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
            
            Level _topLevel = _levels.FirstOrDefault();

            //Add Level Loads
            addLevelLoadModels(buildingLoadModel, _levels, _floors, _topLevel, loadModels);
            buildingLoadModel.ReadLoads();

            //Place Temporary Shores
            Dictionary<double, double> _clearShoreHeightCapacityMappings = getClearShoreHeightCapacityMappings(buildingLoadModel, _topLevel, _temporaryShoreSymbol);
            
            LevelLoadModel _levelLoadModelAbove = buildingLoadModel.LevelLoadModels.OfType<LevelLoadModel>().FirstOrDefault(p => p.Level.Id == _topLevel.Id);
            if (_levelLoadModelAbove == null) return Result.Failed;

            Dictionary<LevelLoadModel, double> _levelLoadModelXDistances = new Dictionary<LevelLoadModel, double>();
            Dictionary<LevelLoadModel, double> _levelLoadModelYDistances = new Dictionary<LevelLoadModel, double>();
            LevelLoadModel _lowestReshoredLevelLoadModel = null;
            double _levelAboveDemand = _levelLoadModelAbove.ReshoreDemandPoundsForcePerSquareFoot;
            foreach (LevelLoadModel _levelLoadModel in buildingLoadModel.LevelLoadModels)
            {
                if (_levelAboveDemand > 0.0)
                {
                    if (_levelLoadModel.Level.Id == _topLevel.Id) continue;

                    double _roundedClearShoreHeight = Math.Round(_levelLoadModel.ClearShoreHeight, 9);
                    if (_clearShoreHeightCapacityMappings.ContainsKey(_roundedClearShoreHeight) == false) continue;

                    double _loadCapacityPerShore = _clearShoreHeightCapacityMappings[_roundedClearShoreHeight];
                    double _minimumAreaPerShore = _loadCapacityPerShore / _levelAboveDemand;

                    double _allowedAreaSquareSideLength = Math.Sqrt(_minimumAreaPerShore);
                    double _roundedSideLength = Math.Floor(_allowedAreaSquareSideLength);

                    //safer to round both down & have a smaller area
                    _levelLoadModelXDistances.Add(_levelLoadModel, _roundedSideLength);
                    _levelLoadModelYDistances.Add(_levelLoadModel, _roundedSideLength);

                    //Adjust our Capacity & Demand, then determine if we can stop adding temportary reshoring
                    _levelLoadModel.ReshoreDemandPoundsForcePerSquareFoot = _levelAboveDemand;

                    _levelAboveDemand -= _levelLoadModel.CapacityPoundsForcePerSquareFoot;
                }
                else
                {
                    if (_lowestReshoredLevelLoadModel == null) _lowestReshoredLevelLoadModel = _levelLoadModelAbove;
                    _levelLoadModel.ReshoreDemandPoundsForcePerSquareFoot = 0.0;
                }

                _levelLoadModelAbove = _levelLoadModel;
            }

            //Update Level parameters
            foreach (LevelLoadModel _levelLoadModel in buildingLoadModel.LevelLoadModels) _levelLoadModel.SetLevelParameters();

            //Place temporary reshoring
            List<FamilyInstance> _temporaryShores = new List<FamilyInstance>();

            _levelLoadModelAbove = buildingLoadModel.LevelLoadModels.OfType<LevelLoadModel>().FirstOrDefault(p => p.Level.Id == _topLevel.Id); 
            foreach (LevelLoadModel _levelLoadModel in buildingLoadModel.LevelLoadModels.Where(p => p.ReshoreDemandPoundsForcePerSquareFoot > 0.0))
            {
                if (_levelLoadModel.Level.Id == _topLevel.Id) continue;

                if (_levelLoadModelXDistances.ContainsKey(_levelLoadModel) == false || _levelLoadModelYDistances.ContainsKey(_levelLoadModel) == false) break;

                double _sideXLength = _levelLoadModelXDistances[_levelLoadModel];
                double _sideYLength = _levelLoadModelYDistances[_levelLoadModel];

                //ToDo: add 1, subtract 1 from each side & check if their total area is closer to the calculated area than a square
                //ToDo: do this twice, and take the "middle" option

                List<Floor> _levelAboveFloors = _floors.Where(p => p.LevelId == _levelLoadModelAbove.Level.Id).ToList();

                List<FamilyInstance> _temporaryReshores = new List<FamilyInstance>();

                foreach (Floor _floor in _levelAboveFloors)
                {
                    BoundingBoxXYZ _currentFloorBounds = _floor.get_BoundingBox(null);

                    List<double> _xCoords = new List<double>();
                    for (double _x = 0.0; _x <= _currentFloorBounds.Max.X - _currentFloorBounds.Min.X; _x += _sideXLength) _xCoords.Add(_x);

                    List<double> _yCoords = new List<double>();
                    for (double _y = 0.0; _y <= _currentFloorBounds.Max.Y - _currentFloorBounds.Min.Y; _y += _sideYLength) _yCoords.Add(_y);

                    foreach (double _x in _xCoords)
                    {
                        foreach (double _y in _yCoords)
                        {
                            XYZ _insertionCoordinate = new XYZ(
                                _currentFloorBounds.Min.X + _x,
                                _currentFloorBounds.Min.Y + _y, 
                                _levelLoadModel.ElevationFeet);

                            FamilyInstance _temporaryShore = _doc.Create.NewFamilyInstance(_insertionCoordinate, _temporaryShoreSymbol, Autodesk.Revit.DB.Structure.StructuralType.Column);

                            Parameter _heightParam = _temporaryShore.LookupParameter("Height");
                            if (_heightParam != null && _heightParam.IsReadOnly == false) _heightParam.Set(_levelLoadModel.ClearShoreHeight);

                            _temporaryShores.Add(_temporaryShore);
                        }
                    }
                }
            }


            //Regenerate so that the latest locations are updated in the model for our newly placed reshores - the intersects filters below will fail without this
            _doc.Regenerate();

            var _scopeBoxes = Getters.GetScopeBoxes(_doc);
            Dictionary<ElementId, string> _shoreIdScopeBoxNames = new Dictionary<ElementId, string>();

            foreach (Element _scopeBox in _scopeBoxes)
            {
                List<Element> _columnsInScopeBox = Getters.GetInsideElements(_doc, _temporaryShores, _scopeBox.get_BoundingBox(null));

                foreach (Level _level in _levels)
                {
                    List<Element> _columnsInScopeBoxOnLevel = _columnsInScopeBox.Where(p => ((p.Location as LocationPoint)?.Point.Z - _level.Elevation) <= 1.0).ToList();

                    string _pourName = Helpers.Views.BoundedViewCreator.GetViewName(_level, _scopeBox, string.Empty, string.Empty);

                    foreach (Element _column in _columnsInScopeBoxOnLevel)
                    {
                        _column.get_Parameter(Ids.PourNameParameterId)?.Set(_pourName);
                    }
                }
            }

            return Result.Succeeded;
        }

        private static void addLevelLoadModels(BuildingLoadModel buildingLoadModel, IEnumerable<Level> levels, IEnumerable<Floor> floors, Level _topLevel, IEnumerable<ILoadModel> loadModels)
        {
            double _concreteDensityPoundsForcePerCubicFoot = 153.0;

            LevelLoadModel _levelLoadModelAbove = null;
            foreach (Level _level in levels)
            {
                Floor _levelFloor = floors.FirstOrDefault(p => p.LevelId == _level.Id);

                double _floorThickness = _levelFloor == null
                    ? 0.0
                    : _levelFloor.get_Parameter(BuiltInParameter.FLOOR_ATTR_THICKNESS_PARAM).AsDouble();

                BoundingBoxXYZ _levelFloorBounds = _levelFloor.get_BoundingBox(null);

                double _topOfSlabElevation = _levelFloorBounds == null
                    ? 0.0
                    : _levelFloorBounds.Max.Z;

                double _bottomOfSlabElevation = _levelFloorBounds == null
                    ? 0.0
                    : _levelFloorBounds.Min.Z;

                LevelLoadModel _levelLoadModel = new LevelLoadModel()
                {
                    Name = _level.Name,
                    Level = _level,
                    ElevationFeet = _level.Elevation,
                    ConcreteDepthFeet = _floorThickness,
                    TopOfSlabElevationFeet = _topOfSlabElevation,
                    BottomOfSlabElevationFeet = _bottomOfSlabElevation,
                    ClearShoreHeight = _levelLoadModelAbove == null
                        ? 0.0
                        : _levelLoadModelAbove.BottomOfSlabElevationFeet - _topOfSlabElevation,
                };

                _levelLoadModel.LoadModels.AddRange(loadModels.Where(p => p.Name == _levelLoadModel.Name));
                _levelLoadModel.addFloorDemandLoadModel(_levelFloor, _concreteDensityPoundsForcePerCubicFoot * _floorThickness);

                buildingLoadModel.LevelLoadModels.Add(_levelLoadModel);

                _levelLoadModelAbove = _levelLoadModel;
            }

        }

        private static List<FamilyInstance> placeShoresForClearShoreHeights(BuildingLoadModel buildingLoadModel, Level topLevel, IEnumerable<FamilySymbol> familySymbols)
        {
            Document _doc = topLevel.Document;

            List<FamilyInstance> _temporaryShores = new List<FamilyInstance>();
            foreach (LevelLoadModel _levelLoadModel in buildingLoadModel.LevelLoadModels)
            {
                if (_levelLoadModel.Level.Id == topLevel.Id) continue;

                foreach (FamilySymbol _familySymbol in familySymbols)
                {
                    FamilyInstance _temporaryShore = _doc.Create.NewFamilyInstance(new XYZ(0, 0, _levelLoadModel.Level.Elevation), _familySymbol, Autodesk.Revit.DB.Structure.StructuralType.Column);
                    _temporaryShore?.LookupParameter("Height")?.Set(_levelLoadModel.ClearShoreHeight);

                    _temporaryShores.Add(_temporaryShore);
                }
            }

            _doc.Regenerate();

            return _temporaryShores;
        }

        private static Dictionary<double, double> getClearShoreHeightCapacityMappings(BuildingLoadModel buildingLoadModel, Level topLevel, FamilySymbol familySymbol)
        {
            Document _doc = topLevel.Document;

            List<FamilyInstance> _temporaryShores = new List<FamilyInstance>();
            foreach (LevelLoadModel _levelLoadModel in buildingLoadModel.LevelLoadModels)
            {
                if (_levelLoadModel.Level.Id == topLevel.Id) continue;

                FamilyInstance _temporaryShore = _doc.Create.NewFamilyInstance(new XYZ(0, 0, _levelLoadModel.Level.Elevation), familySymbol, Autodesk.Revit.DB.Structure.StructuralType.Column);
                _temporaryShore?.LookupParameter("Height")?.Set(_levelLoadModel.ClearShoreHeight);

                _temporaryShores.Add(_temporaryShore);
            }

            _doc.Regenerate();

            Dictionary<double, double> _clearShoreHeightCapacityMappings = new Dictionary<double, double>();

            foreach (FamilyInstance _temporaryShore in _temporaryShores)
            {
                Parameter _clearShoreHeightParameter = _temporaryShore.get_Parameter(Ids.ClearShoreHeightId);
                double _clearShoreHeight = _clearShoreHeightParameter == null
                    ? 0.0
                    : Math.Round(_clearShoreHeightParameter.AsDouble(), 9);

                if (_clearShoreHeightCapacityMappings.ContainsKey(_clearShoreHeight)) continue;

                Parameter _loadCapacityParameter = _temporaryShore.get_Parameter(Ids.LoadCapacityParameterId);
                double _loadCapacity = _loadCapacityParameter == null
                    ? 0.0
                    : UnitUtils.ConvertFromInternalUnits(_loadCapacityParameter.AsDouble(), DisplayUnitType.DUT_POUNDS_FORCE_PER_SQUARE_FOOT);

                _clearShoreHeightCapacityMappings.Add(_clearShoreHeight, _loadCapacity);
            }

            _doc.Delete(_temporaryShores.Select(p => p.Id).ToList());

            return _clearShoreHeightCapacityMappings;
        }
    }
}
