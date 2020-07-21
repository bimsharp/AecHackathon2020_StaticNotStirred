using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Views
{
    internal class BuildingLoadView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnChanged(string prop)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private IBuildingLoadModel _buildingLoadInputModel;
                
        public string ConstructionLiveLoadWeightTotal 
        {
            get => Helpers.Converters.ToString(_buildingLoadInputModel?.ConstructionLiveLoadWeightTotal, 3);
            set => _buildingLoadInputModel.ConstructionLiveLoadWeightTotal = Helpers.Converters.ToDouble(value);
        }

        public string LevelsAboveGroundCount
        {
            get => Helpers.Converters.ToString(_buildingLoadInputModel?.LevelsAboveGroundCount);
            set => _buildingLoadInputModel.LevelsAboveGroundCount = Helpers.Converters.ToInt(value);
        }

        public string LevelsBelowGroundCount
        {
            get => Helpers.Converters.ToString(_buildingLoadInputModel?.LevelsBelowGroundCount);
            set => _buildingLoadInputModel.LevelsBelowGroundCount = Helpers.Converters.ToInt(value);
        }

        public string FormWeightPerLinearFoot
        {
            get => Helpers.Converters.ToPLF(_buildingLoadInputModel?.FormWeightPerLinearFoot);
            set => _buildingLoadInputModel.FormWeightPerLinearFoot = Helpers.Converters.FromPLF(value);
        }

        public string StructuralBeamWeightPerLinearFoot
        {
            get => Helpers.Converters.ToPLF(_buildingLoadInputModel?.StructuralBeamWeightPerLinearFoot);
            set => _buildingLoadInputModel.StructuralBeamWeightPerLinearFoot = Helpers.Converters.FromPLF(value);
        }

        public string StructuralColumnWeightPerLinearFoot
        {
            get => Helpers.Converters.ToPLF(_buildingLoadInputModel?.StructuralColumnWeightPerLinearFoot);
            set => _buildingLoadInputModel.StructuralColumnWeightPerLinearFoot = Helpers.Converters.FromPLF(value);
        }

        public string StructuralWallWeightPerLinearFoot
        {
            get => Helpers.Converters.ToPLF(_buildingLoadInputModel?.StructuralWallWeightPerLinearFoot);
            set => _buildingLoadInputModel.StructuralWallWeightPerLinearFoot = Helpers.Converters.FromPLF(value);
        }

        public string AdditionalWeightPerLinearFoot
        {
            get => Helpers.Converters.ToPLF(_buildingLoadInputModel?.AdditionalWeightPerLinearFoot);
            set => _buildingLoadInputModel.AdditionalWeightPerLinearFoot = Helpers.Converters.FromPLF(value);
        }

        public ObservableCollection<LevelLoadView> LevelLoadViews { get; set; }

        public ObservableCollection<SquareLoadView> CurrentSquareLevelLoads { get; set; }

        public BuildingLoadView(IBuildingLoadModel buildingLoadInputModel)
        {
            _buildingLoadInputModel = buildingLoadInputModel;

            LevelLoadViews = new ObservableCollection<LevelLoadView>();
            CurrentSquareLevelLoads = new ObservableCollection<SquareLoadView>();
            foreach (ILevelLoadModel _levelLoadModel in _buildingLoadInputModel.LevelLoadModels) LevelLoadViews.Add(new LevelLoadView(_levelLoadModel));
        }

    }
}
