using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal static class Names
    {
        public static HashSet<string> SupportedFamilyNames = new HashSet<string>
        {
            "Reshoring Poles 6x6",
            "Reshoring Poles Ellis 4x4",
            "Reshoring Poles Titan-HV",
            "Reshoring Poles Titan-XL",
        };
    }
}
