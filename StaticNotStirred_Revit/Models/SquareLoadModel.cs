using Autodesk.Revit.DB;
using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace StaticNotStirred_Revit.Models
{
    public class SquareLoadModel : ISquareLoadModel
    {
        public Guid Id { get; set; }
        public string Source { get; set; }
        public string Name { get; set; }
        public double AmountPerSquareFoot { get; set; }
        public double MinX { get; set; }
        public double MinY { get; set; }
        public double MaxX { get; set; }
        public double MaxY { get; set; }

        internal double? TopElevation => PlanarFace?.Origin.Z;

        public SquareLoadModel()
        {
            Id = Guid.NewGuid();
            Curves = new List<Curve>();
        }

        #region Revit-specific

        internal List<Curve> Curves { get; set; }

        internal PlanarFace PlanarFace { get; set; }

        internal Element Element { get; set; }

        public Solid GetProjectedSolid(XYZ direction, double distance)
        {
            if (direction.IsAlmostEqualTo(PlanarFace.FaceNormal) == false &&
                direction.IsAlmostEqualTo(PlanarFace.FaceNormal.Negate()) == false) return null;

            CurveLoop _curveLoop = CurveLoop.Create(Curves);
            if (_curveLoop.IsOpen())
            {
                XYZ _closingEnd0 = _curveLoop.FirstOrDefault()?.GetEndPoint(0);
                XYZ _closingEnd1 = _curveLoop.LastOrDefault()?.GetEndPoint(1);
                if (_closingEnd0 != null && _closingEnd1 != null && _closingEnd0.IsAlmostEqualTo(_closingEnd1) == false)
                {
                    Line _line = Line.CreateBound(_closingEnd0, _closingEnd1);
                    _curveLoop.Append(_line);
                }
            }

            if (_curveLoop.IsOpen()) return null;

            Solid _solid = GeometryCreationUtilities.CreateExtrusionGeometry(new List<CurveLoop> { _curveLoop }, direction, Math.Abs(distance));

            return _solid;
        }

        #endregion
    }
}
