using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal class FamilyDefinition
    {
        public string FamilyName { get; set; }
        
        private Family _family;
        public Family Family => _family;

        List<FamilySymbol> _familySymbols;
        public List<FamilySymbol> FamilySymbols => _familySymbols;

        public FamilyDefinition(Document doc, string familyName) : this(
            familyName,
            new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>(),
            new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>())
        {

        }

        public FamilyDefinition(string familyName, IEnumerable<Family> families, IEnumerable<FamilySymbol> familySymbols) : this(familyName)
        {
            _family = families.FirstOrDefault(p => p.Name == FamilyName);
            _familySymbols = _family == null
                ? new List<FamilySymbol>()
                : _family.GetFamilySymbolIds().Select(p => _family.Document.GetElement(p) as FamilySymbol).Where(p => p != null).ToList();
        }

        public FamilyDefinition(string familyName)
        {
            FamilyName = familyName;
        }

        public override bool Equals(object obj)
        {
            return obj is FamilyDefinition _definition &&
                   FamilyName == _definition.FamilyName;
        }

        public override int GetHashCode()
        {
            return 181542802 + EqualityComparer<string>.Default.GetHashCode(FamilyName);
        }
    }
}
