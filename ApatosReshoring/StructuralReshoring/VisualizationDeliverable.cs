using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.StructuralReshoring
{
    internal class VisualizationDeliverable
    {
        //Deliverables
        public View3D CapacityView { get; set; }
        public View3D CombinedView { get; set; }
        public View3D DemandView { get; set; }
        public View3D LevelBalanceView { get; set; }
        public ViewSheet Sheet { get; set; }

        //Support
        public Level Level => LevelLoadModel == null || LevelLoadModel.Level == null
            ? null
            : LevelLoadModel.Level;

        public LevelLoadModel LevelLoadModel { get; set; }

        //Model Elements
        public List<Floor> Floors { get; set; }
        public List<FamilyInstance> Reshores { get; set; }
        public List<FamilyInstance> StructuralColumns { get; set; }
        public List<FamilyInstance> StructuralFraming { get; set; }
        public List<Wall> Walls { get; set; }

        public VisualizationDeliverable(LevelLoadModel levelLoadModel) : base()
        {
            LevelLoadModel = levelLoadModel;
        }

        public VisualizationDeliverable()
        {
            Floors = new List<Floor>();
            Reshores = new List<FamilyInstance>();
            StructuralColumns = new List<FamilyInstance>();
            StructuralFraming = new List<FamilyInstance>();
            Walls = new List<Wall>();
        }
    }
}
