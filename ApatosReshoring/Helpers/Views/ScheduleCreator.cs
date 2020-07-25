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

        public ViewSchedule CreateSchedule(bool isItemized, string scheduleName, BuiltInCategory category)
        {
            ViewSchedule _viewSchedule = ViewSchedule.CreateSchedule(_doc, new ElementId(category));
            if (string.IsNullOrWhiteSpace(scheduleName) == false) _viewSchedule.Name = scheduleName;

            _viewSchedule.Definition.IsItemized = isItemized;

            return _viewSchedule;
        }

        public static ViewSchedule CreateLayoutColumnSchedule(Document doc, string name, string suffix)
        {
            ScheduleCreator _scheduleCreator = new ScheduleCreator(doc);

            ViewSchedule _viewSchedule = _scheduleCreator.CreateSchedule(false, name + suffix, BuiltInCategory.OST_Columns);
            ScheduleField _pourNameField = _scheduleCreator.AppendField(_viewSchedule, "Pour Name");
            ScheduleField _count = _scheduleCreator.AppendField(_viewSchedule, "Count");
            ScheduleField _familyAndType = _scheduleCreator.AppendField(_viewSchedule, "Family and Type");
            ScheduleField _clearShoreHeight = _scheduleCreator.AppendField(_viewSchedule, "Clear Shore Height"); //Total Shore Length
            ScheduleField _maxHeight = _scheduleCreator.AppendField(_viewSchedule, "Max. Height");
            ScheduleField _minHeight = _scheduleCreator.AppendField(_viewSchedule, "Min. Height");
            ScheduleField _loadCapacity = _scheduleCreator.AppendField(_viewSchedule, "Load Capacity"); //Safe Working Load

            _scheduleCreator.AppendSortField(_viewSchedule, _pourNameField);
            _scheduleCreator.AppendSortField(_viewSchedule, _familyAndType);
            _scheduleCreator.AppendSortField(_viewSchedule, _clearShoreHeight);

            if (_pourNameField != null) _scheduleCreator.AppendFilter(_viewSchedule, _pourNameField, ScheduleFilterType.Contains, name);

            return _viewSchedule;
        }

        public static ViewSchedule CreatePourColumnSchedule(Document doc, string name, string suffix)
        {
            ScheduleCreator _scheduleCreator = new ScheduleCreator(doc);

            ViewSchedule _viewSchedule = _scheduleCreator.CreateSchedule(false, name + suffix, BuiltInCategory.OST_Columns);
            ScheduleField _pourNameField = _scheduleCreator.AppendField(_viewSchedule, "Pour Name");
            ScheduleField _count = _scheduleCreator.AppendField(_viewSchedule, "Count");
            ScheduleField _familyAndType = _scheduleCreator.AppendField(_viewSchedule, "Family and Type");
            ScheduleField _clearShoreHeight = _scheduleCreator.AppendField(_viewSchedule, "Clear Shore Height"); //Total Shore Length
            ScheduleField _maxHeight = _scheduleCreator.AppendField(_viewSchedule, "Max. Height");
            ScheduleField _minHeight = _scheduleCreator.AppendField(_viewSchedule, "Min. Height");
            ScheduleField _loadCapacity = _scheduleCreator.AppendField(_viewSchedule, "Load Capacity"); //Safe Working Load

            _scheduleCreator.AppendSortField(_viewSchedule, _pourNameField);
            _scheduleCreator.AppendSortField(_viewSchedule, _familyAndType);
            _scheduleCreator.AppendSortField(_viewSchedule, _clearShoreHeight);

            if (_pourNameField != null) _scheduleCreator.AppendFilter(_viewSchedule, _pourNameField, ScheduleFilterType.Equal, name);

            return _viewSchedule;
        }

        public static ViewSchedule CreateLayoutLoadSchedule(Document doc, string levelName, string suffix)
        {
            ScheduleCreator _scheduleCreator = new ScheduleCreator(doc);

            ViewSchedule _viewSchedule = _scheduleCreator.CreateSchedule(false, levelName + suffix, BuiltInCategory.OST_Levels);
            ScheduleField _nameField = _scheduleCreator.AppendField(_viewSchedule, "Name");
            ScheduleField _elevationField = _scheduleCreator.AppendField(_viewSchedule, "Elevation");
            ScheduleField _levelLoadCapacity = _scheduleCreator.AppendField(_viewSchedule, "Level Load Capacity");
            ScheduleField _loadDemand = _scheduleCreator.AppendField(_viewSchedule, "Load Demand");
            ScheduleField _liveLoadDemand = _scheduleCreator.AppendField(_viewSchedule, "Live Load Demand");
            ScheduleField _formworkDemand = _scheduleCreator.AppendField(_viewSchedule, "Formwork Demand");

            if (_elevationField != null) _scheduleCreator.AppendSortField(_viewSchedule, _elevationField);

            _scheduleCreator.AppendFilter(_viewSchedule, _nameField, ScheduleFilterType.Equal, levelName);

            return _viewSchedule;
        }

        public static ViewSchedule CreatePourLoadSchedule(Document doc, string levelName, string suffix)
        {
            ScheduleCreator _scheduleCreator = new ScheduleCreator(doc);

            ViewSchedule _viewSchedule = _scheduleCreator.CreateSchedule(false, levelName + suffix, BuiltInCategory.OST_Levels);
            ScheduleField _nameField = _scheduleCreator.AppendField(_viewSchedule, "Name");
            ScheduleField _elevationField = _scheduleCreator.AppendField(_viewSchedule, "Elevation");
            ScheduleField _levelLoadCapacity = _scheduleCreator.AppendField(_viewSchedule, "Level Load Capacity");
            ScheduleField _loadDemand = _scheduleCreator.AppendField(_viewSchedule, "Load Demand");
            ScheduleField _liveLoadDemand = _scheduleCreator.AppendField(_viewSchedule, "Live Load Demand");
            ScheduleField _formworkDemand = _scheduleCreator.AppendField(_viewSchedule, "Formwork Demand");

            if (_elevationField != null) _scheduleCreator.AppendSortField(_viewSchedule, _elevationField);

            _scheduleCreator.AppendFilter(_viewSchedule, _nameField, ScheduleFilterType.Equal, levelName);

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
            SchedulableField _schedulableField = null;
            if (fieldName.Equals("Elevation", StringComparison.InvariantCultureIgnoreCase))
            {
                // BuiltInParameter.LEVEL_ELEV
            }
            else
            {
                _schedulableField = _viewSchedule.Definition.GetSchedulableFields()
                    .FirstOrDefault(p => p.GetName(_doc).Equals(fieldName));
            }

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
