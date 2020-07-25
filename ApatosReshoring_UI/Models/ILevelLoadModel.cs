using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Models
{
    public interface ILevelLoadModel
    {
        List<ILoadModel> LoadModels { get; set; }
        Guid Id { get; set; }
        string Name { get; set; }
        double ElevationFeet { get; set; }
        double TopOfSlabElevationFeet { get; set; }
        double BottomOfSlabElevationFeet { get; set; }
        double ConcreteDepthFeet { get; set; }
        double CapacityPoundsForcePerSquareFoot { get; set; }
        double DemandPoundsForcePerSquareFoot { get; set; }
        double FormworkDemandPoundsForcePerSquareFoot { get; set; }
        double ReshoreDemandPoundsForcePerSquareFoot { get; set; }

        void addLoadModel(ILoadModel loadModel);
    }
}
