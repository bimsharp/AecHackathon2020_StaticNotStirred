using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal static class FamilyHelper
    {
        public static IFamilyDefinition GetOrLoadFamilyDefinition(Document doc, string filePathName)
        {
            var _familyDefinition = new EllisShore_LumberWithClampsFamilyDefinition(doc);
            if (_familyDefinition.Family == null)
            {
                if (doc.LoadFamily(filePathName, new ReplaceFamilyOptions(), out Family _family) == false) return null;
                _familyDefinition = new EllisShore_LumberWithClampsFamilyDefinition(doc);
            }
            return _familyDefinition;
        }
    }
}
