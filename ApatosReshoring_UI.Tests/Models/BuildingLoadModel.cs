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

namespace StaticNotStirred_UI.Tests.Models
{
    public class BuildingLoadModel : IBuildingLoadModel
    {
        public Guid Id { get; set; }

        public List<ILevelLoadModel> LevelLoadModels { get; set; }

        public double ConstructionLiveLoadTotalPounds { get; set; }
        public int LevelsAboveGroundCount { get; set; }
        public int LevelsBelowGroundCount { get; set; }
        public double FormWeightPerSquareFoot { get; set; }
        public double StructuralBeamWeightPerSquareFoot { get; set; }
        public double StructuralColumnWeightPerSquareFoot { get; set; }
        public double StructuralWallWeightPerSquareFoot { get; set; }
        public double AdditionalWeightPerSquareFoot { get; set; }
        public double ConstructionLiveLoadTotalPoundsPerSquareFoot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public BuildingLoadModel()
        {
            Id = Guid.NewGuid();
            LevelLoadModels = new List<ILevelLoadModel>();
        }
    }
}