using StaticNotStirred_UI.Enums;
using StaticNotStirred_UI.Helpers;
using StaticNotStirred_UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_UI.Views
{
    internal class LoadView
    {
        private ILoadModel _loadModel;

        public string Source
        {
            get => _loadModel == null || _loadModel.LoadType == LoadType.None 
                ? string.Empty 
                : _loadModel.LoadType.GetDescription();
            set
            {
                if (_loadModel == null) return;
                if (Enum.TryParse(value, out LoadType _loadType))
                {
                    _loadModel.LoadType = _loadType;
                }
                else
                {
                    _loadModel.LoadType = LoadType.None;
                }
            }
        }

        public string Name
        {
            get => _loadModel == null ? string.Empty : _loadModel.Name;
            set { if (_loadModel != null) _loadModel.Name = value; }
        }

        public string AmountPerSquareFoot
        {
            get => Helpers.Converters.ToPSF(_loadModel?.PoundsForcePerSquareFoot);
            set => _loadModel.PoundsForcePerSquareFoot = Helpers.Converters.FromPSF(value);
        }


        //public double HeightFeetZ { get; set; }

        //public string MinX
        //{
        //    get => Helpers.Converters.ToString(_squareLoadModel?.MinX, 9);
        //    set { if (_squareLoadModel != null) _squareLoadModel.MinX = Helpers.Converters.ToDouble(value); }
        //}
        //
        //public string MinY
        //{
        //    get => Helpers.Converters.ToString(_squareLoadModel?.MinY, 9);
        //    set { if (_squareLoadModel != null) _squareLoadModel.MinY = Helpers.Converters.ToDouble(value); }
        //}
        //
        //public string MaxX
        //{
        //    get => Helpers.Converters.ToString(_squareLoadModel?.MaxX, 9);
        //    set { if (_squareLoadModel != null) _squareLoadModel.MaxX = Helpers.Converters.ToDouble(value); }
        //}
        //
        //public string MaxY
        //{
        //    get => Helpers.Converters.ToString(_squareLoadModel?.MaxY, 9);
        //    set { if (_squareLoadModel != null) _squareLoadModel.MaxY = Helpers.Converters.ToDouble(value); }
        //}

        public LoadView(ILoadModel loadModel)
        {
            _loadModel = loadModel;
        }
    }
}
