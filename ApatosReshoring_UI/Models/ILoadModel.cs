using StaticNotStirred_UI.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Models
{
    public interface ILoadModel
    {
        Guid Id { get; set; }
        LoadType LoadType { get; set; }
        string Name { get; set; }
        double PoundsForcePerSquareFoot { get; set; }
        double DensityPoundsForcePerCubicFoot { get; set; }
        double VolumeCubicFeet { get; set; }
        double AreaSquareFeetXY { get; set; }
        double HeightFeetZ { get; set; }
        double ClearShoreHeightFeet { get; set; }
        double OriginXFeet { get; set; }
        double OriginYFeet { get; set; }
        double TopElevationFeet { get; set; }
    }
}