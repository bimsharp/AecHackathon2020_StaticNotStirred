using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal class EllisShore_LumberWithClampsFamilyDefinition : IFamilyDefinition
    {
        public string FamilyName => "EllisShore_LumberWithClamps";

        List<string> _typeNames;
        public List<string> TypeNames
        {
            get
            {
                SetTypeNames();
                return _typeNames;
            }
        }

        private Family _family;
        public Family Family => _family;

        List<FamilySymbol> _familySymbols;
        public List<FamilySymbol> FamilySymbols => _familySymbols;

        public EllisShore_LumberWithClampsFamilyDefinition(Document doc) : this(
            new FilteredElementCollector(doc).OfClass(typeof(Family)).Cast<Family>(),
            new FilteredElementCollector(doc).OfClass(typeof(FamilySymbol)).Cast<FamilySymbol>()) { }

        public EllisShore_LumberWithClampsFamilyDefinition(IEnumerable<Family> families, IEnumerable<FamilySymbol> familySymbols) : this()
        {
            _family = families.FirstOrDefault(p => p.Name == FamilyName);
            _familySymbols = familySymbols.Where(p => p.FamilyName == FamilyName && TypeNames.Contains(p.Name)).ToList();
        }

        public EllisShore_LumberWithClampsFamilyDefinition() { }

        public void SetTypeNames()
        {
            if (TypeNames == null || TypeNames.Count == 0)
            {
                _typeNames = new List<string>();
                foreach (Enum _enum in Enum.GetValues(typeof(EllisShore_LumberWithClampsFamilyType)))
                {
                    if (_enum.ToString() == "None") continue;

                    string _description = _enum.GetDescription();
                    _typeNames.Add(_enum.GetDescription());
                }
            }
        }

        public override bool Equals(object obj)
        {
            return obj is EllisShore_LumberWithClampsFamilyDefinition _definition &&
                   FamilyName == _definition.FamilyName;
        }

        public override int GetHashCode()
        {
            return 181542802 + EqualityComparer<string>.Default.GetHashCode(FamilyName);
        }
    }
}
