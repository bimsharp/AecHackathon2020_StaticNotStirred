using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Selections
{
    internal class ScopeBoxSelectionFilter : ISelectionFilter
    {
        public bool AllowElement(Element elem)
        {
            bool _allowed = elem.Category != null && elem.Category.Id.IntegerValue == (int)BuiltInCategory.OST_VolumeOfInterest;
            return _allowed;
        }

        public bool AllowReference(Reference reference, XYZ position)
        {
            return true;
        }
    }
}
