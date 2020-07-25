using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal static class FamilyHelpers
    {
        public static FamilyDefinition GetOrLoadFamilyDefinition(Document doc, string filePathName)
        {
            string _familyName = Path.GetFileNameWithoutExtension(filePathName);
            var _familyDefinition = new FamilyDefinition(doc, _familyName);
            if (_familyDefinition.Family == null)
            {
                if (doc.LoadFamily(filePathName, new ReplaceFamilyOptions(), out Family _family) == false) return null;
                _familyDefinition = new FamilyDefinition(doc, _familyName);
            }
            return _familyDefinition;
        }

        public static bool IsReshore(FamilyInstance familyInstance)
        {
            bool _result = familyInstance != null &&
                   familyInstance is FamilyInstance _familyInstance &&
                   familyInstance.Category?.Id.IntegerValue == (int)BuiltInCategory.OST_Columns &&
                   Names.SupportedFamilyNames.Contains(_familyInstance.Symbol.FamilyName);

            return _result;
        }

        //Revised From Jeremy Tammik
        internal static List<DirectShape> CreateDirectShapes(Document doc, IEnumerable<Solid> solids, ElementId _categoryId)
        {
            List<DirectShape> _directShapes = new List<DirectShape>();

            foreach (Solid _solid in solids)
            {
                if (_solid == null) continue;

                DirectShape _directShape = CreateDirectShape(doc, _solid, _categoryId);
                if (_directShapes != null) _directShapes.Add(_directShape);
            }
            return _directShapes;
        }

        internal static DirectShape CreateDirectShape(Document doc, Solid solid, ElementId _categoryId)
        {
            // Currently create direct shape 
            // replacement element in the original 
            // document – no API to properly transfer 
            // graphic styles to a new document.
            // A possible enhancement: make a copy 
            // of the current project and operate 
            // on the copy.

            DirectShape _directShape = DirectShape.CreateElement(doc, _categoryId);

            try
            {
                _directShape.SetShape(new List<GeometryObject> { solid });
            }
            catch (Autodesk.Revit.Exceptions.ArgumentException ex)
            {
            }
            return _directShape;
        }
    }
}
