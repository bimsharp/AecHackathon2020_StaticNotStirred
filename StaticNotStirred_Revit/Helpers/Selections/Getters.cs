using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Selections
{
    internal static class Getters
    {
        public static List<Element> GetScopeBoxes(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_VolumeOfInterest)
                .OrderBy(p => p.Name).ToList();
        }

        public static List<Level> GetLevels(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Level)).Cast<Level>()
                .OrderBy(p => p.Elevation).ToList();
        }

        public static List<FamilyInstance> GetColumnsByView(View view)
        {
            Document _doc = view.Document;
            return new FilteredElementCollector(_doc, view.Id)
                .OfCategory(BuiltInCategory.OST_Columns)
                .OfType<FamilyInstance>()
                .OrderBy(p => p.Name).ToList();
        }

        public static List<FamilySymbol> GetTitleblockSymbols(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .OfType<FamilySymbol>()
                .OrderBy(p => p.Name).ToList();
        }
    }
}
