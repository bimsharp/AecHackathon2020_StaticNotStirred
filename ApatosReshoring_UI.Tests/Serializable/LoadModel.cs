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

namespace StaticNotStirred_UI.Tests.Serializable
{
    [XmlInclude(typeof(LoadModel))]
    public class LoadModel
    {
        public Guid Id { get; set; }
        public LoadType LoadType { get; set; }
        public string Name { get; set; }
        public double AmountPerSquareFoot { get; set; }
        public double DensityPoundsForcePerCubicFeet { get; set; }
        public double VolumeCubicFeet { get; set; }
        public double AreaSquareFeet { get; set; }
        public double ClearShoreHeightFeet { get; set; }
        public double OriginX { get; set; }
        public double OriginY { get; set; }
        public double TopElevation { get; set; }

        public LoadModel()
        {

        }
    }
}
