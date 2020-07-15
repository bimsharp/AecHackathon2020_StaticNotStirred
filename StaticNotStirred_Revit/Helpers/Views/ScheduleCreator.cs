using Autodesk.Revit.DB;
using StaticNotStirred_Revit.Helpers.Selections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticNotStirred_Revit.Helpers.Views
{
    internal class ScheduleCreator
    {
        private Document _doc;
        
        public ScheduleCreator(Document doc)
        {
            _doc = doc;
        }

        public ViewSchedule CreateSchedule(bool isItemized, string scheduleName)
        {
            ViewSchedule _viewSchedule = ViewSchedule.CreateSchedule(_doc, new ElementId(BuiltInCategory.OST_Columns));
            if (string.IsNullOrWhiteSpace(scheduleName) == false) _viewSchedule.Name = scheduleName;

            _viewSchedule.Definition.IsItemized = isItemized;

            return _viewSchedule;
        }

        internal ScheduleFilter AppendFilter(ViewSchedule _viewSchedule, ScheduleField field, ScheduleFilterType filterType, object value)
        {
            ScheduleFilter _filter;
            if (value is int _valueInt)
                _filter = new ScheduleFilter(field.FieldId, filterType, _valueInt);
            else if (value is double _valueDouble)
                _filter = new ScheduleFilter(field.FieldId, filterType, _valueDouble);
            else if (value is string _valueString)
                _filter = new ScheduleFilter(field.FieldId, filterType, _valueString);
            else if (value is ElementId _valueId)
                _filter = new ScheduleFilter(field.FieldId, filterType, _valueId);
            else if (value == null)
                _filter = new ScheduleFilter(field.FieldId, filterType);
            else return null;

            _viewSchedule.Definition.AddFilter(_filter);
            return _filter;
        }

        internal ScheduleField AppendField(ViewSchedule _viewSchedule, string fieldName)
        {
            SchedulableField _schedulableField = _viewSchedule.Definition.GetSchedulableFields()
                .FirstOrDefault(p => p.GetName(_doc).Equals(fieldName));

            if (_schedulableField == null) return null;

            ScheduleField _scheduleField = _viewSchedule.Definition.AddField(_schedulableField);
            return _scheduleField;
        }

        internal ScheduleSortGroupField AppendSortField(ViewSchedule _viewSchedule, ScheduleField field)
        {
            ScheduleSortGroupField _sortGroupField = new ScheduleSortGroupField(field.FieldId);

            _viewSchedule.Definition.AddSortGroupField(_sortGroupField);
            return _sortGroupField;
        }

        internal ScheduleSortGroupField AppendGroupField(ViewSchedule _viewSchedule, ScheduleField field)
        {
            ScheduleSortGroupField _sortGroupField = new ScheduleSortGroupField(field.FieldId);
            _sortGroupField.ShowHeader = false;
            _sortGroupField.SortOrder = ScheduleSortOrder.Descending;
            _sortGroupField.ShowBlankLine = false;

            _viewSchedule.Definition.AddSortGroupField(_sortGroupField);
            return _sortGroupField;
        }
    }
}
