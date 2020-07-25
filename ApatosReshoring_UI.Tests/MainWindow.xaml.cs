using StaticNotStirred_UI.Tests.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StaticNotStirred_UI.Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void doStuff_Click(object sender, RoutedEventArgs e)
        {

            //ToDo: update this CSPROJ to accomodate change in shape of UI objects


            BuildingLoadModel _buildingLoadModel = new BuildingLoadModel
            {
                ConstructionLiveLoadTotalPounds = 1.5,
                LevelsAboveGroundCount = 2,
                LevelsBelowGroundCount = 3,
                FormWeightPerSquareFoot = 4,
                StructuralBeamWeightPerSquareFoot = 5.5,
                StructuralColumnWeightPerSquareFoot = 6.5,
                StructuralWallWeightPerSquareFoot = 7.5,
                AdditionalWeightPerSquareFoot = 8.5,
            };

            LevelLoadModel _levelLoadModel = new LevelLoadModel
            {
                Name = "My Level 1",
                ElevationFeet = 9.5,
                TopOfSlabElevationFeet = 10.5,
                ConcreteDepthFeet = 11.5,
                CapacityPoundsForcePerSquareFoot = 12.5,
                DemandPoundsForcePerSquareFoot = 13.5,
                ReshoreDemandPoundsForcePerSquareFoot = 14.5,
            };
            _buildingLoadModel.LevelLoadModels.Add(_levelLoadModel);

            LoadModel _capacity = new LoadModel
            {
                LoadType =  Enums.LoadType.Capacity,
                Name = "My Capacity 1",
                //AmountPerSquareFoot = 15.5,
                //MinX = 16.123456789,
                //MinY = 17.123456789,
                //MaxX = 18.123456789,
                //MaxY = 19.123456789,
            };

            LoadModel _demand = new LoadModel
            {
                LoadType = Enums.LoadType.Demand,
                Name = "My Demand 1",
                //AmountPerSquareFoot = 20.5,
                //MinX = 21.123456789,
                //MinY = 22.123456789,
                //MaxX = 23.123456789,
                //MaxY = 24.123456789,
            };

            LoadModel _liveLoad = new LoadModel
            {
                LoadType = Enums.LoadType.ReshoreDemand,
                Name = "My Reshore Demand 1",
                //AmountPerSquareFoot = 25.5,
                //MinX = 26.123456789,
                //MinY = 27.123456789,
                //MaxX = 28.123456789,
                //MaxY = 29.123456789,
            };

            LoadModel _reshoreCapacity = new LoadModel
            {
                LoadType = Enums.LoadType.ReshoreDemand,
                Name = "My Reshore Demand 1",
                //AmountPerSquareFoot = 30.5,
                //MinX = 31.123456789,
                //MinY = 32.123456789,
                //MaxX = 33.123456789,
                //MaxY = 34.123456789,
            };

            //_levelLoadModel.CapacityModels.Add(_capacity);
            //_levelLoadModel.DemandModels.Add(_demand);
            //_levelLoadModel.LiveLoadModels.Add(_liveLoad);
            //_levelLoadModel.ReshoreDemandModels.Add(_reshoreCapacity);

            //string _filePathName = @"C:\$\AEC Hackathon 2020 (models)\BuildingLoadModel.xml";
            ////Serializable.BuildingLoadModel.SerializeToXml(_buildingLoadModel, _filePathName);
            //Helpers.Extension.ToXML(_buildingLoadModel);
            //Process.Start(System.IO.Path.GetDirectoryName(_filePathName));
            BuildingLoadInputs _buildingLoadInputs = new BuildingLoadInputs(_buildingLoadModel);
            _buildingLoadInputs.ShowDialog();
            if (_buildingLoadInputs.DialogResult != true) return;


        }
    }
}
