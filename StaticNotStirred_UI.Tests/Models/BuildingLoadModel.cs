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
    [XmlInclude(typeof(BuildingLoadModel))]
    public class BuildingLoadModel : IBuildingLoadModel
    {
        public Guid Id { get; set; }

        public List<ILevelLoadModel> LevelLoadModels { get; set; }

        public double ConstructionLiveLoadWeightTotal { get; set; }
        public int LevelsAboveGroundCount { get; set; }
        public int LevelsBelowGroundCount { get; set; }
        public double FormWeightPerLinearFoot { get; set; }
        public double StructuralBeamWeightPerLinearFoot { get; set; }
        public double StructuralColumnWeightPerLinearFoot { get; set; }
        public double StructuralWallWeightPerLinearFoot { get; set; }
        public double AdditionalWeightPerLinearFoot { get; set; }

        public BuildingLoadModel()
        {
            Id = Guid.NewGuid();
            LevelLoadModels = new List<ILevelLoadModel>();
        }
    }
}