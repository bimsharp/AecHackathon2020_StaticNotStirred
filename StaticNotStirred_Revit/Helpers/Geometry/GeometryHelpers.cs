using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace StaticNotStirred_Revit.Helpers.Geometry
{
    internal static class GeometryHelpers
    {
        public static IEnumerable<GeometryObject> GetGeometryObjects(Element element, ViewDetailLevel viewDetailLevel)
        {
            List<GeometryObject> _geometry = new List<GeometryObject>();

            Options _options = new Options();

            _options.DetailLevel = viewDetailLevel;
            _options.ComputeReferences = true;
            _options.IncludeNonVisibleObjects = true;

            GeometryElement _geometryElement = element.get_Geometry(_options);
            if (_geometryElement == null) return _geometry;

            foreach (GeometryObject _geometryObject in _geometryElement)
            {
                if (_geometryObject is GeometryInstance _geometryInstance)
                {
                    foreach (GeometryObject _geometryInstanceObject in _geometryInstance.GetInstanceGeometry())
                    {
                        _geometry.Add(_geometryInstanceObject);
                    }
                }
                else
                {
                    _geometry.Add(_geometryObject);
                }
            }

            return _geometry;
        }


    }
}
