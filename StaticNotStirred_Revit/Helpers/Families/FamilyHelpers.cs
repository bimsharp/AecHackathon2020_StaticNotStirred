using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Families
{
    internal static class FamilyHelpers
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

        internal static List<DirectShape> CreateDirectShapes(Document doc, IEnumerable<Solid> solids, ElementId _categoryId)
        {
            List<DirectShape> _directShapes = new List<DirectShape>();

            foreach (Solid _solid in solids)
            {
                // Currently create direct shape 
                // replacement element in the original 
                // document – no API to properly transfer 
                // graphic styles to a new document.
                // A possible enhancement: make a copy 
                // of the current project and operate 
                // on the copy.

                DirectShape _directShape = DirectShape.CreateElement(doc, _categoryId); //new ElementId(BuiltInCategory.OST_GenericModel)

                try
                {
                    _directShape.SetShape(new List<GeometryObject> { _solid });
                    _directShapes.Add(_directShape);
                }
                catch (Autodesk.Revit.Exceptions.ArgumentException ex)
                {
                }
            }
            return _directShapes;
        }
    }
}
