using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApatosReshoring_Revit.Helpers.Annotations
{
    internal class TagCreator
    {
        //ToDo: genericize this, so that it allows common settings for all tags, then unique settings as needed per tag
        public static List<IndependentTag> CreateTags(View view, FamilySymbol tagSymbol)
        {
            if (tagSymbol == null || tagSymbol.Category == null) return null;

            Document _doc = view.Document;

            List<FamilyInstance> _reshores = new FilteredElementCollector(_doc).OfClass(typeof(FamilyInstance)).OfType<FamilyInstance>().ToList();

            TagMode _tagMode = tagSymbol.Category.Id.IntegerValue == (int)BuiltInCategory.OST_MultiCategoryTags
                ? TagMode.TM_ADDBY_MULTICATEGORY
                : TagMode.TM_ADDBY_CATEGORY;

            List<IndependentTag> _tags = new List<IndependentTag>();
            foreach (FamilyInstance _reshore in _reshores)
            {
                BoundingBoxXYZ _boundingBoxXYZ = _reshore.get_BoundingBox(null);

                XYZ _tagHeadCoordinate = (_boundingBoxXYZ.Min + _boundingBoxXYZ.Max) / 2.0;

                Reference _reference = new Reference(_reshore);

                IndependentTag _tag = IndependentTag.Create(_doc, view.Id, _reference, true, _tagMode, TagOrientation.Horizontal, _tagHeadCoordinate);
                _tags.Add(_tag);
            }
            return _tags;
        }
    }
}
