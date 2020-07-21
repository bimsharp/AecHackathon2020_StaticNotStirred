using StaticNotStirred_UI.Models;
using StaticNotStirred_UI.Views;
using System;
using System.Collections.Generic;
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

namespace StaticNotStirred_UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class BuildingLoadInputs : Window
    {
        internal BuildingLoadView View { get; set; }

        public BuildingLoadInputs(IBuildingLoadModel buildingLoadModel)
        {
            View = new BuildingLoadView(buildingLoadModel);
            InitializeComponent();
            DataContext = View;
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {

        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void export_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addLevel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeLevel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addSquareLoad_Click(object sender, RoutedEventArgs e)
        {

        }

        private void removeSquareLoad_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
