using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Views
{
    internal class LevelLoadView
    {
        private ILevelLoadModel _levelLoadInputModel;

        public string Name
        {
            get => _levelLoadInputModel == null ? string.Empty : _levelLoadInputModel.Name;
            set { if (_levelLoadInputModel != null) _levelLoadInputModel.Name = value; }
        }

        public string Elevation
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.Elevation);
            set => _levelLoadInputModel.Elevation = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string TopOfSlabElevation
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.TopOfSlabElevation);
            set => _levelLoadInputModel.TopOfSlabElevation = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string ConcreteDepth
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.ConcreteDepth);
            set => _levelLoadInputModel.ConcreteDepth = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string Capacity
        {
            get => Helpers.Converters.ToPSF(_levelLoadInputModel?.Capacity);
            set => _levelLoadInputModel.Capacity = Helpers.Converters.FromPSF(value);
        }

        public string Demand
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.Demand);
            set => _levelLoadInputModel.Demand = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string ReshoreDemand
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.ReshoreDemand);
            set => _levelLoadInputModel.ReshoreDemand = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public List<SquareLoadView> SquareLoadViews { get; set; }
        public LevelLoadView(ILevelLoadModel levelLoadInputModel)
        {
            _levelLoadInputModel = levelLoadInputModel;

            SquareLoadViews = new List<SquareLoadView>();
            foreach (ISquareLoadModel _squareLoadModel in _levelLoadInputModel.CapacityModels) SquareLoadViews.Add(new SquareLoadView(_squareLoadModel));
            foreach (ISquareLoadModel _squareLoadModel in _levelLoadInputModel.DemandModels) SquareLoadViews.Add(new SquareLoadView(_squareLoadModel));
            foreach (ISquareLoadModel _squareLoadModel in _levelLoadInputModel.ReshoreDemandModels) SquareLoadViews.Add(new SquareLoadView(_squareLoadModel));

        }
    }
}
