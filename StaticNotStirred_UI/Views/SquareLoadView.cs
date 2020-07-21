using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Views
{
    internal class SquareLoadView
    {
        private ISquareLoadModel _squareLoadModel;

        public string Source
        {
            get => _squareLoadModel == null ? string.Empty : _squareLoadModel.Source;
            set { if (_squareLoadModel != null) _squareLoadModel.Source = value; }
        }

        public string Name
        {
            get => _squareLoadModel == null ? string.Empty : _squareLoadModel.Name;
            set { if (_squareLoadModel != null) _squareLoadModel.Name = value; }
        }

        public string AmountPerSquareFoot
        {
            get => Helpers.Converters.ToPSF(_squareLoadModel?.AmountPerSquareFoot);
            set => _squareLoadModel.AmountPerSquareFoot = Helpers.Converters.FromPSF(value);
        }

        public string MinX
        {
            get => Helpers.Converters.ToString(_squareLoadModel?.MinX, 9);
            set { if (_squareLoadModel != null) _squareLoadModel.MinX = Helpers.Converters.ToDouble(value); }
        }

        public string MinY
        {
            get => Helpers.Converters.ToString(_squareLoadModel?.MinY, 9);
            set { if (_squareLoadModel != null) _squareLoadModel.MinY = Helpers.Converters.ToDouble(value); }
        }

        public string MaxX
        {
            get => Helpers.Converters.ToString(_squareLoadModel?.MaxX, 9);
            set { if (_squareLoadModel != null) _squareLoadModel.MaxX = Helpers.Converters.ToDouble(value); }
        }

        public string MaxY
        {
            get => Helpers.Converters.ToString(_squareLoadModel?.MaxY, 9);
            set { if (_squareLoadModel != null) _squareLoadModel.MaxY = Helpers.Converters.ToDouble(value); }
        }

        public SquareLoadView(ISquareLoadModel zoneLoadInputModel)
        {
            _squareLoadModel = zoneLoadInputModel;
        }
    }
}
