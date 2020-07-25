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

namespace StaticNotStirred_UI.Tests.Models
{
    public class LevelLoadModel : ILevelLoadModel
    {
        public List<ILoadModel> LoadModels { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double ElevationFeet { get; set; }
        public double TopOfSlabElevationFeet { get; set; }
        public double ConcreteDepthFeet { get; set; }
        public double CapacityPoundsForcePerSquareFoot { get; set; }
        public double DemandPoundsForcePerSquareFoot { get; set; }
        public double ReshoreDemandPoundsForcePerSquareFoot { get; set; }
        public double BottomOfSlabElevationFeet { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public double FormworkDemandPoundsForcePerSquareFoot { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public LevelLoadModel()
        {
            Id = Guid.NewGuid();
            LoadModels = new List<ILoadModel>();
        }

        public void addLoadModel(ILoadModel loadModel)
        {
            LoadModels.Add(loadModel);
        }
    }
}
