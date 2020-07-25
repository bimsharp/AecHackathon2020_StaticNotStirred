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

        public string GetViewName(string prefix, string suffix)
        {
            return GetViewName(Level, ScopeBox, prefix, suffix);
        }

        public static string GetViewName(Level level, Element scopeBox, string prefix, string suffix)
        {
            string _delimiter = " - ";

            List<string> _nameParts = new List<string>();

            if (string.IsNullOrWhiteSpace(prefix) == false) _nameParts.Add(prefix);
            if (level != null) _nameParts.Add(getCleanedNameWithNumbers(level.Name));
            if (scopeBox != null) _nameParts.Add(getCleanedNameWithNumbers(scopeBox.Name));
            if (string.IsNullOrWhiteSpace(suffix) == false) _nameParts.Add(suffix);

            string _viewName = string.Join(_delimiter, _nameParts);
            return _viewName;
        }

        private static string getCleanedNameWithNumbers(string name)
        {
            int _padding = 2;

            char? _lastChar = name.LastOrDefault();
            if (_lastChar == null ||
                _lastChar.HasValue == false ||
                char.IsDigit(_lastChar.Value) == false) return name;

            string _numericChars = string.Empty;
            string _nonNumericChars = string.Empty;
            bool _secondPeriodFound = false;
            foreach (char _char in name.Reverse())
            {
                if (_char == '.' || char.IsDigit(_char) && _secondPeriodFound == false) _numericChars += _char;
                else if (_char == '.' && _numericChars.Contains('.'))
                {
                    _secondPeriodFound = true;
                    _nonNumericChars += _char;
                }
                else _nonNumericChars += _char;
            }
            _numericChars = new string(_numericChars.Reverse().ToArray());
            _nonNumericChars = new string(_nonNumericChars.Reverse().ToArray());

            if (_numericChars.Contains('.'))
            {
                if (double.TryParse(_numericChars, out double _doubleSheetNumber))
                {
                    return _nonNumericChars + _doubleSheetNumber.ToString("D" + _padding);
                }
                else return name;

            }
            else if (_numericChars.Length > 0)
            {
                if (int.TryParse(_numericChars, out int _intSheetNumber))
                {
                    return _nonNumericChars + _intSheetNumber.ToString("D" + _padding);
                }
                else return name;
            }
            else return name;
        }

        public View3D CreateView3D(int scale)
        {
            string _viewName = GetViewName(string.Empty, "3D");

            return CreateView3D(scale, _viewName);
        }

        public View3D CreateView3D(int scale, string viewName)
        {
            Document _doc = Level?.Document;
            if (_doc == null) return null;

            View3D _view3D = View3D.CreateIsometric(_doc, _3DViewFamilyType.Id);
            if (Bounds != null) _view3D.SetSectionBox(Bounds);

            if (string.IsNullOrWhiteSpace(viewName) == false) _view3D.Name = viewName;
            _view3D.Scale = scale;

            return _view3D;
        }

        public ViewSection CreateViewSection(int scale)
        {

            //ToDo: this... isn't right.
            //ToDo: account for case where Bounds/Scope Box is not set
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
            string _viewName = GetViewName(string.Empty, "FP");
            return CreateViewPlan(scale, _viewName);
        }

        public ViewPlan CreateViewPlan(int scale, string viewName)
        {
            Document _doc = Level?.Document;
            if (_doc == null) return null;

            ViewPlan _viewPlan = ViewPlan.Create(_doc, _floorPlanViewFamilyType.Id, Level.Id);
            if (ScopeBox != null)
            {
                _viewPlan.get_Parameter(BuiltInParameter.VIEWER_VOLUME_OF_INTEREST_CROP)?.Set(ScopeBox.Id);
            }
            else if (ScopeBox == null && Bounds != null)
            {
                _viewPlan.CropBoxActive = true;
                _viewPlan.CropBox = Bounds;
            }

            if (string.IsNullOrWhiteSpace(viewName) == false) _viewPlan.Name = viewName;
            _viewPlan.Scale = scale;

            return _viewPlan;
        }
    }
}
