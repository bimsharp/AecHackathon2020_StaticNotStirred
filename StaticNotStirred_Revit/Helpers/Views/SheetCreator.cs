using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Helpers.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Views
{
    internal class SheetCreator
    {
        private Dictionary<string, FamilySymbol> _titleblockMaps;

        public SheetCreator(Document doc)
        {
            var _titleblocks = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .OfType<FamilySymbol>()
                .OrderBy(p => p.Name).ToList();

            _titleblockMaps = new Dictionary<string, FamilySymbol>();
            foreach (var _titleblock in _titleblocks)
            {
                string _key = _titleblock.FamilyName + ": " + _titleblock.Name;
                if (_titleblockMaps.ContainsKey(_key)) continue;

                _titleblockMaps.Add(_key, _titleblock);
            }
        }

        public ViewSheet CreateSheet(string titleblockName, string sheetName, string sheetNumber)
        {
            if (_titleblockMaps.ContainsKey(titleblockName) == false ||
                _titleblockMaps[titleblockName] == null ||
                _titleblockMaps[titleblockName].IsValidObject == false) return null;

            FamilySymbol _titleblockSymbol = _titleblockMaps[titleblockName];
            Document _doc = _titleblockSymbol.Document;

            ViewSheet _viewSheet = ViewSheet.Create(_doc, _titleblockSymbol.Id);
            if (string.IsNullOrWhiteSpace(sheetName) == false) _viewSheet.Name = sheetName;
            if (string.IsNullOrWhiteSpace(sheetNumber) == false) _viewSheet.SheetNumber = sheetNumber;

            return _viewSheet;
        }
    }
}
