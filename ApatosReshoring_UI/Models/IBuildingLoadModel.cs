using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Models
{
    public interface IBuildingLoadModel
    {
        List<ILevelLoadModel> LevelLoadModels { get; set; }

        Guid Id { get; set; }

        double ConstructionLiveLoadTotalPoundsPerSquareFoot { get; set; }

        int LevelsAboveGroundCount { get; set; }

        int LevelsBelowGroundCount { get; set; }

        double FormWeightPerSquareFoot { get; set; }

        double StructuralBeamWeightPerSquareFoot { get; set; }

        double StructuralColumnWeightPerSquareFoot { get; set; }

        double StructuralWallWeightPerSquareFoot { get; set; }

        double AdditionalWeightPerSquareFoot { get; set; }


    }
}
