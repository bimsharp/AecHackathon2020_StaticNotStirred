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

        double ConstructionLiveLoadWeightTotal { get; set; }

        int LevelsAboveGroundCount { get; set; }

        int LevelsBelowGroundCount { get; set; }

        double FormWeightPerLinearFoot { get; set; }

        double StructuralBeamWeightPerLinearFoot { get; set; }

        double StructuralColumnWeightPerLinearFoot { get; set; }

        double StructuralWallWeightPerLinearFoot { get; set; }

        double AdditionalWeightPerLinearFoot { get; set; }


    }
}
