using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_Revit.Models
{
    public class BuildingLoadModel : IBuildingLoadModel
    {
        public Guid Id { get; set; }

        public List<ILevelLoadModel> LevelLoadModels { get; set; }

        public double ConstructionCapacityTotalPoundsPerSquareFoot { get; set; }
        public double ConstructionDemandTotalPoundsPerSquareFoot { get; set; }
        public double ConstructionLiveLoadTotalPoundsPerSquareFoot { get; set; }
        public double ConstructionReshoreCapacityTotalPoundsPerSquareFoot { get; set; }

        public int LevelsAboveGroundCount { get; set; }
        public int LevelsBelowGroundCount { get; set; }
        public double FormWeightPerSquareFoot { get; set; }
        public double StructuralBeamWeightPerSquareFoot { get; set; }
        public double StructuralColumnWeightPerSquareFoot { get; set; }
        public double StructuralWallWeightPerSquareFoot { get; set; }
        public double AdditionalWeightPerSquareFoot { get; set; }

        public BuildingLoadModel(
            int levelsAboveGroundCount,
            int levelsBelowGroundCount,
            double formWeightPerLinearFoot, 
            double structuralBeamWeightPerLinearFoot, 
            double structuralColumnWeightPerLinearFoot, 
            double structuralWallWeightPerLinearFoot, 
            double additionalWeightPerLinearFoot)
        {
            Id = Guid.NewGuid();
            LevelLoadModels = new List<ILevelLoadModel>();
            LevelsAboveGroundCount = levelsAboveGroundCount;
            LevelsBelowGroundCount = levelsBelowGroundCount;
            FormWeightPerSquareFoot = formWeightPerLinearFoot;
            StructuralBeamWeightPerSquareFoot = structuralBeamWeightPerLinearFoot;
            StructuralColumnWeightPerSquareFoot = structuralColumnWeightPerLinearFoot;
            StructuralWallWeightPerSquareFoot = structuralWallWeightPerLinearFoot;
            AdditionalWeightPerSquareFoot = additionalWeightPerLinearFoot;
        }

        public void ReadLoads()
        {
            //ToDo: this whole concept need to be re-considered - I didn't understand how units were used for these calculations, and was also getting early experience with UnitUtils
            ConstructionCapacityTotalPoundsPerSquareFoot = 0.0;
            ConstructionDemandTotalPoundsPerSquareFoot = 0.0;
            ConstructionLiveLoadTotalPoundsPerSquareFoot = 0.0;
            ConstructionReshoreCapacityTotalPoundsPerSquareFoot = 0.0;

            foreach (LevelLoadModel _levelLoadModel in LevelLoadModels.OfType<LevelLoadModel>())
            {
                _levelLoadModel.ReadLoads();

                ConstructionCapacityTotalPoundsPerSquareFoot += _levelLoadModel.CapacityPoundsForcePerSquareFoot;
                ConstructionDemandTotalPoundsPerSquareFoot += _levelLoadModel.DemandPoundsForcePerSquareFoot;
                ConstructionLiveLoadTotalPoundsPerSquareFoot += _levelLoadModel.LiveLoadDemandPerSquareFoot;
                ConstructionReshoreCapacityTotalPoundsPerSquareFoot += _levelLoadModel.ReshoreDemandPoundsForcePerSquareFoot;

                if (_levelLoadModel.ElevationFeet >= 0.0) LevelsAboveGroundCount++;
                else LevelsBelowGroundCount++;
            }
        }
    }
}