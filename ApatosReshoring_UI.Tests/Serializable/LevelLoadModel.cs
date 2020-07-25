using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Tests.Serializable
{
    [XmlInclude(typeof(LevelLoadModel))]
    public class LevelLoadModel
    { 
        public List<LoadModel> CapacityModels { get; set; }
        public List<LoadModel> DemandModels { get; set; }
        public List<LoadModel> ReshoreCapacityModels { get; set; }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Elevation { get; set; }
        public double TopOfSlabElevation { get; set; }
        public double ConcreteDepth { get; set; }
        public double Capacity { get; set; }
        public double Demand { get; set; }
        public double ReshoreDemand { get; set; }

        public LevelLoadModel()
        {
            CapacityModels = new List<LoadModel>();
            DemandModels = new List<LoadModel>();
            ReshoreCapacityModels = new List<LoadModel>();
        }
    }
}
