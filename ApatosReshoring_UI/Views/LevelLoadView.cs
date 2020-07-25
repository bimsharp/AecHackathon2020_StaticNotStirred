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
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.ElevationFeet);
            set => _levelLoadInputModel.ElevationFeet = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string TopOfSlabElevation
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.TopOfSlabElevationFeet);
            set => _levelLoadInputModel.TopOfSlabElevationFeet = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string ConcreteDepth
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.ConcreteDepthFeet);
            set => _levelLoadInputModel.ConcreteDepthFeet = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string Capacity
        {
            get => Helpers.Converters.ToPSF(_levelLoadInputModel?.CapacityPoundsForcePerSquareFoot);
            set => _levelLoadInputModel.CapacityPoundsForcePerSquareFoot = Helpers.Converters.FromPSF(value);
        }

        public string Demand
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.DemandPoundsForcePerSquareFoot);
            set => _levelLoadInputModel.DemandPoundsForcePerSquareFoot = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public string ReshoreDemand
        {
            get => Helpers.Converters.DecimalFeetToFeetInches_32ndInch(_levelLoadInputModel?.ReshoreDemandPoundsForcePerSquareFoot);
            set => _levelLoadInputModel.ReshoreDemandPoundsForcePerSquareFoot = Helpers.Converters.FeetInchesToDecimalFeet(value);
        }

        public List<LoadView> LoadViews { get; set; }
        public LevelLoadView(ILevelLoadModel levelLoadInputModel)
        {
            _levelLoadInputModel = levelLoadInputModel;
            LoadViews = _levelLoadInputModel.LoadModels.Select(p => new LoadView(p)).Where(p => p != null).ToList();
        }
    }
}
