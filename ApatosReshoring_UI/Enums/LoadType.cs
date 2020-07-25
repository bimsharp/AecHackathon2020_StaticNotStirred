using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Enums
{
    public enum LoadType
    {
        [Description("None")] None = 0,
        [Description("Capacity")] Capacity,
        [Description("Demand")] Demand,
        [Description("Formwork")] Formwork,
        [Description("Live Load")] LiveLoad,
        [Description("Other")] Other,
        [Description("Reshore Demand")] ReshoreDemand,
    }
}
