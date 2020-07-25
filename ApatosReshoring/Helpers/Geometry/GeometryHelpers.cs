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

        public static BoundingBoxXYZ GetElementsBBox(IEnumerable<Element> elements)
        {
            List<BoundingBoxXYZ> _bBoxes = new List<BoundingBoxXYZ>();
            foreach (Element _element in elements)
            {
                if (_element.Category != null &&
                    (_element.Category.Id.IntegerValue == (int)BuiltInCategory.OST_Cameras))
                {
                    continue;
                }

                BoundingBoxXYZ _elementBBox = _element.get_BoundingBox(null);

                if (_elementBBox == null)
                {
                    continue;
                }

                _bBoxes.Add(_elementBBox);
            }

            //Minimums
            double _minX = 10000000.0;
            double _minY = 10000000.0;
            double _minZ = 10000000.0;

            //Maximums
            double _maxX = -10000000.0;
            double _maxY = -10000000.0;
            double _maxZ = -10000000.0;

            foreach (BoundingBoxXYZ _elementBBox in _bBoxes)
            {
                if (_elementBBox == null)
                {
                    continue;
                }

                //Minimums
                if (_elementBBox.Min.X < _minX)
                {
                    _minX = _elementBBox.Min.X;
                }

                if (_elementBBox.Min.Y < _minY)
                {
                    _minY = _elementBBox.Min.Y;
                }

                if (_elementBBox.Min.Z < _minZ)
                {
                    _minZ = _elementBBox.Min.Z;
                }

                //Maximums
                if (_elementBBox.Max.X > _maxX)
                {
                    _maxX = _elementBBox.Max.X;
                }

                if (_elementBBox.Max.Y > _maxY)
                {
                    _maxY = _elementBBox.Max.Y;
                }

                if (_elementBBox.Max.Z > _maxZ)
                {
                    _maxZ = _elementBBox.Max.Z;
                }
            }

            //Set Bounding Box Coordinates
            BoundingBoxXYZ _bBoxReturn = new BoundingBoxXYZ();
            _bBoxReturn.Min = new XYZ(_minX, _minY, _minZ);
            _bBoxReturn.Max = new XYZ(_maxX, _maxY, _maxZ);

            return _bBoxReturn;
        }
    }
}
