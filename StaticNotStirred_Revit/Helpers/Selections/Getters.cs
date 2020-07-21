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

        public static List<Floor> GetFloors(Document doc)
        {
            return new FilteredElementCollector(doc)
                .OfClass(typeof(Floor)).Cast<Floor>()
                .OrderBy(p => p.get_BoundingBox(null)?.Max.Z).ToList();
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

        internal static List<Element> GetIntersectedElements(Document doc, IEnumerable<Element> elementsToCheck, Solid solid)
        {
            ElementFilter _filter = new ElementIntersectsSolidFilter(solid);

            List<Element> _elements = new FilteredElementCollector(doc, elementsToCheck.Select(p => p.Id).ToList())
                //.WhereElementIsNotElementType()
                //.WhereElementIsViewIndependent()
                .WherePasses(_filter).ToList();

            return _elements;
        }
    }
}
