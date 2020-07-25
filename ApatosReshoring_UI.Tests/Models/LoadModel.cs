using StaticNotStirred_UI.Enums;
using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_UI.Tests.Models
{
    public class LoadModel : ILoadModel
    {
        public Guid Id { get; set; }
        public LoadType LoadType { get; set; }
        public string Name { get; set; }
        public double DensityPoundsForcePerCubicFoot { get; set; }
        public double VolumeCubicFeet { get; set; }
        public double AreaSquareFeetXY { get; set; }
        public double HeightFeetZ { get; set; }
        public double ClearShoreHeightFeet { get; set; }
        public double OriginXFeet { get; set; }
        public double OriginYFeet { get; set; }
        public double TopElevationFeet { get; set; }
        public double PoundsForcePerSquareFoot { get; set; }

        public LoadModel()
        {
            Id = Guid.NewGuid();
        }
    }
}
