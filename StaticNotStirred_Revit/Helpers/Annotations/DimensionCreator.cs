using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Annotations
{
    internal class DimensionCreator
    {
        Line _line;
        List<Reference> _references;


        public DimensionCreator(Line line, IEnumerable<Reference> references)
        {
            _line = line.Clone() as Line;
            _references = references.ToList();
        }

        public void AddReference(Reference reference)
        {
            if (_references == null) _references = new List<Reference>();

            _references.Add(reference);
        }

        public Dimension CreatePlanDimension(ViewPlan viewPlan)
        {
            if (viewPlan == null) return null;
            if (_line == null || _references == null || _references.Count < 2) return null;

            Document _doc = viewPlan.Document;

            ReferenceArray _referenceArray = new ReferenceArray();
            foreach (Reference _reference in _references)
            {
                _referenceArray.Append(_reference);
            }

            Dimension _dimension = _doc.Create.NewDimension(viewPlan, _line, _referenceArray);
            return _dimension;
        }

        public static List<Dimension> CreateDimensions(View view, string pourName)
        {
            List<Dimension> _dimensions = new List<Dimension>();

            Document _doc = view.Document;
            try
            {
                var _columns = Selections.Getters.GetColumnsByView(view);

                List<List<FamilyInstance>> _columnMatrix = new List<List<FamilyInstance>>();

                var _columnsSortedByY = _columns.OrderByDescending(p => ((LocationPoint)p.Location).Point.Y).ToList();

                List<FamilyInstance> _currentColumnOfColumns = new List<FamilyInstance>();
                double? _currentY = null;
                foreach (FamilyInstance _column in _columnsSortedByY)
                {
                    double _y = ((LocationPoint)_column.Location).Point.Y;
                    if (_currentY == null) _currentY = _y;

                    if (Math.Abs(_currentY.Value - _y) <= _doc.Application.ShortCurveTolerance)
                    {
                        //same column, add it to the list
                        _currentColumnOfColumns.Add(_column);
                    }
                    else
                    {
                        _columnMatrix.Add(_currentColumnOfColumns.OrderBy(p => ((LocationPoint)p.Location).Point.X).ToList());
                        _currentColumnOfColumns = new List<FamilyInstance>();
                        _currentY = null;

                        _currentColumnOfColumns.Add(_column);
                    }
                }
                if (_currentColumnOfColumns != null) _columnMatrix.Add(_currentColumnOfColumns.OrderBy(p => ((LocationPoint)p.Location).Point.X).ToList());

                List<Reference> _rowReferences = new List<Reference>();
                foreach (FamilyInstance _cell in _columnMatrix[0])
                {
                    FamilySymbol _familySymbol = _doc.GetElement(_cell.GetTypeId()) as FamilySymbol;

                    //Get References from string representations
                    string _referenceRepresentation1 = _cell.UniqueId + ":0:INSTANCE:" + _familySymbol.UniqueId + ":1:SURFACE";
                    string _referenceRepresentation4 = _cell.UniqueId + ":0:INSTANCE:" + _familySymbol.UniqueId + ":4:SURFACE";

                    Reference _surface1Reference = Reference.ParseFromStableRepresentation(_doc, _referenceRepresentation1);
                    Reference _surface4Reference = Reference.ParseFromStableRepresentation(_doc, _referenceRepresentation4);

                    if (_surface1Reference == null || _surface4Reference == null) continue;

                    //Get GeometryObjects from References

                    // //Note - this will get a common Face of Column, but the preferred dimensions are to center-of-column.
                    //PlanarFace _planarFace = GeometryUtils.GetGeometryObjects(_cell)
                    //    .OfType<Solid>().SelectMany(p => p.Faces.OfType<PlanarFace>())
                    //    .Where(p => p.FaceNormal.IsAlmostEqualTo(XYZ.BasisX))
                    //    .OrderByDescending(p => p.Origin.Y)
                    //    .FirstOrDefault();
                    //
                    //string _newReferenceId = _cell.UniqueId + ":0:INSTANCE:" + _planarFace.Reference.ConvertToStableRepresentation(_doc);
                    //Reference _newReference = Reference.ParseFromStableRepresentation(_doc, _newReferenceId);
                    //
                    //_rowReferences.Add(_newReference);
                    _rowReferences.Add(_surface1Reference);
                }

                if (_rowReferences.Count >= 2)
                {
                    try
                    {
                        ReferenceArray _referenceArray = new ReferenceArray();
                        foreach (Reference _reference in _rowReferences) _referenceArray.Append(_reference);
                        Line _line = Line.CreateUnbound((_columnMatrix[0][0].Location as LocationPoint).Point, XYZ.BasisX);
                        Dimension _dimension = _doc.Create.NewDimension(view, _line, _referenceArray);
                        _dimensions.Add(_dimension);
                    }
                    catch (Exception _ex) { }
                }

                List<Reference> _columnReferences = new List<Reference>();
                foreach (FamilyInstance _cell in _columnMatrix.Select(p => p.FirstOrDefault()))
                {
                    FamilySymbol _familySymbol = _doc.GetElement(_cell.GetTypeId()) as FamilySymbol;

                    //Get References from string representations
                    string _referenceRepresentation1 = _cell.UniqueId + ":0:INSTANCE:" + _familySymbol.UniqueId + ":1:SURFACE";
                    string _referenceRepresentation4 = _cell.UniqueId + ":0:INSTANCE:" + _familySymbol.UniqueId + ":4:SURFACE";

                    Reference _surface1Reference = Reference.ParseFromStableRepresentation(_doc, _referenceRepresentation1);
                    Reference _surface4Reference = Reference.ParseFromStableRepresentation(_doc, _referenceRepresentation4);

                    if (_surface1Reference == null || _surface4Reference == null) continue;

                    _columnReferences.Add(_surface4Reference);
                }

                if (_columnReferences.Count >= 2)
                {
                    try
                    {
                        ReferenceArray _referenceArray = new ReferenceArray();
                        foreach (Reference _reference in _columnReferences) _referenceArray.Append(_reference);
                        Line _line = Line.CreateUnbound((_columnMatrix[0][0].Location as LocationPoint).Point, XYZ.BasisY);
                        Dimension _dimension = _doc.Create.NewDimension(view, _line, _referenceArray);
                        _dimensions.Add(_dimension);
                    }
                    catch (Exception _ex) { }
                }
            }
            catch (Exception _ex)
            {
            }

            return _dimensions;
        }
    }
}
