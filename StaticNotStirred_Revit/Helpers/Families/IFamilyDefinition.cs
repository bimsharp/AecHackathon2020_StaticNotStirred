using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal interface IFamilyDefinition
    {
        string FamilyName { get; }

        List<string> TypeNames { get; }

        void SetTypeNames();

        #region Revit-Specific

        Autodesk.Revit.DB.Family Family { get; }

        List<Autodesk.Revit.DB.FamilySymbol> FamilySymbols { get; }

        #endregion
    }
}
