using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Views
{
    internal class BoundedViewCreator
    {
        public Level Level { get; set; }

        public Element ScopeBox { get; set; }

        public BoundingBoxXYZ Bounds { get; set; }

        private ViewFamilyType _3DViewFamilyType;
        private ViewFamilyType _floorPlanViewFamilyType;
        private ViewFamilyType _sectionViewFamilyType;

        public BoundedViewCreator(Level level, Element scopeBox, BoundingBoxXYZ bounds)
        {
            Level = level;
            ScopeBox = scopeBox;
            Bounds = bounds;

            Document _doc = Level?.Document;
            if (_doc != null)
            {
                List<ViewFamilyType> _viewFamilyTypes = new FilteredElementCollector(_doc)
                    .OfClass(typeof(ViewFamilyType)).Cast<ViewFamilyType>().ToList();

                _3DViewFamilyType = _viewFamilyTypes.FirstOrDefault(vft => vft.ViewFamily == ViewFamily.ThreeDimensional);
                _floorPlanViewFamilyType = _viewFamilyTypes.FirstOrDefault(vft => vft.ViewFamily == ViewFamily.FloorPlan);
                _sectionViewFamilyType = _viewFamilyTypes.FirstOrDefault(vft => vft.ViewFamily == ViewFamily.Section);
            }
        }

        public string GetViewName(string prefix)
        {
            return GetViewName(Level, ScopeBox, prefix);
        }

        public static string GetViewName(Level level, Element scopeBox, string prefix)
        {
            string _delimiter = " - ";

            string _viewName = string.Empty;
            if (string.IsNullOrWhiteSpace(prefix) == false) _viewName = prefix + _delimiter;

            List<string> _nameParts = new List<string>();
            if (level != null) _nameParts.Add(level.Name);
            if (scopeBox != null) _nameParts.Add(scopeBox.Name);

            _viewName += string.Join(_delimiter, _nameParts);

            return _viewName;
        }

        public View3D CreateView3D(int scale)
        {
            Document _doc = Level?.Document;
            if (_doc == null) return null;

            View3D _view3D = View3D.CreateIsometric(_doc, _3DViewFamilyType.Id);
            _view3D.SetSectionBox(Bounds);
            string _viewName = GetViewName("3D");
            if (string.IsNullOrWhiteSpace(_viewName) == false) _view3D.Name = _viewName;
            _view3D.Scale = scale;

            return _view3D;
        }

        public ViewSection CreateViewSection(int scale)
        {

            //ToDo: this... isn't right.
            throw new NotImplementedException();

            //Document _doc = Level?.Document;
            //if (_doc == null) return null;
            //
            ////double _xDelta = Bounds.Max.X - Bounds.Min.X;
            ////double _yDelta = Bounds.Max.Y - Bounds.Min.Y;
            ////double _zDelta = Bounds.Max.Z - Bounds.Min.Z;
            //
            //BoundingBoxXYZ _sectionBounds = new BoundingBoxXYZ
            //{
            //    Enabled = true,
            //    Min = new XYZ(Bounds.Min.X, Bounds.Min.Z, Bounds.Min.Y),// new XYZ(_xDelta / 2.0, _zDelta / 2.0, _yDelta / 2.0),
            //    Max = new XYZ(Bounds.Max.X, Bounds.Max.Z, Bounds.Max.Y), // new XYZ(-_xDelta / 2.0, -_zDelta / 2.0, -(_yDelta / 2.0)),
            //};
            //
            ////Plane _plane = Plane.CreateByNormalAndOrigin(XYZ.BasisY, Bounds.Min);
            //XYZ _origin = new XYZ(Bounds.Max.X, Bounds.Min.Y, Bounds.Max.Z);
            //// 0.5 * (_sectionBounds.Max + _sectionBounds.Min); // ProjectOnto(_plane, 0.5 * (_sectionBounds.Max + _sectionBounds.Min));
            //
            //Transform _sectionTransform = Transform.Identity;
            //_sectionTransform.Origin = _origin;
            //_sectionTransform.BasisX = XYZ.BasisX;
            //_sectionTransform.BasisY = XYZ.BasisZ.Negate();
            //_sectionTransform.BasisZ = XYZ.BasisY;
            //_sectionBounds.Transform = _sectionTransform;
            //
            //ViewSection _viewSection = ViewSection.CreateSection(_doc, _sectionViewFamilyType.Id, _sectionBounds);
            //
            ////_viewSection.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP)?.Set(ScopeBox.Id);
            //string _viewName = GetViewName("Section");
            //if (string.IsNullOrWhiteSpace(_viewName) == false) _viewSection.Name = _viewName;
            //_viewSection.Scale = scale;
            //
            //return _viewSection;
        }

        public ViewPlan CreateViewPlan(int scale)
        {
            Document _doc = Level?.Document;
            if (_doc == null) return null;

            ViewPlan _viewPlan = ViewPlan.Create(_doc, _floorPlanViewFamilyType.Id, Level.Id);
            _viewPlan.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP)?.Set(ScopeBox.Id);
            string _viewName = GetViewName("FloorPlan");
            if (string.IsNullOrWhiteSpace(_viewName) == false) _viewPlan.Name = _viewName;
            _viewPlan.Scale = scale;

            return _viewPlan;
        }

        //From Jeremy Tammik - The Building Coder - Util.cs
        public static XYZ ProjectOnto(Plane plane, XYZ p)
        {
            double _d = SignedDistanceTo(plane, p);

            XYZ _q = p - _d * plane.Normal;

            return _q;
        }

        //From Jeremy Tammik - The Building Coder - Util.cs
        public static UV ProjectInto(Plane plane, XYZ p)
        {
            XYZ _q = ProjectOnto(plane, p);
            XYZ _o = plane.Origin;
            XYZ _d = _q - _o;
            double _u = _d.DotProduct(plane.XVec);
            double _v = _d.DotProduct(plane.YVec);
            return new UV(_u, _v);
        }

        //From Jeremy Tammik - The Building Coder - Util.cs
        public static double SignedDistanceTo(Plane plane, XYZ p)
        {
            XYZ _v = p - plane.Origin;

            return plane.Normal.DotProduct(_v);
        }
    }
}
