using System;
using System.Reflection;
using CarterGames.Standalone.NotionData.Value_Conversion;
using UnityEngine;

namespace CarterGames.Standalone.NotionData
{
    public abstract class NotionProperty
    {
        protected abstract object InternalValue { get; set; }
        public abstract string JsonValue { get; protected set; }


        public NotionPropertyCheckbox CheckBox() => NotionPropertyFactory.Checkbox(InternalValue, JsonValue);
        public NotionPropertyDate Date() => NotionPropertyFactory.Date(InternalValue, JsonValue);
        public NotionPropertyMultiSelect MultiSelect() => NotionPropertyFactory.MultiSelect(InternalValue, JsonValue);
        public NotionPropertySelect Select() => NotionPropertyFactory.Select(InternalValue, JsonValue);
        public NotionPropertyNumber Number() => NotionPropertyFactory.Number(InternalValue, JsonValue);
        public NotionPropertyRichText RichText() => NotionPropertyFactory.RichText(InternalValue, JsonValue);
        public NotionPropertyTitle Title() => NotionPropertyFactory.Title(InternalValue, JsonValue);



        public bool TryConvertValueToFieldType(FieldInfo field, object target)
        {
            var fieldType = field.FieldType;
                        
            if (fieldType.BaseType.FullName.Contains(typeof(NotionDataWrapper).Namespace + ".NotionDataWrapper"))
            {
                if (NotionPropertyValueHandler.TryGetValueAsWrapper(this, fieldType, out var value))
                {
                    field.SetValue(target, value);
                    return true;
                }
            }
            else
            {
                if (NotionPropertyValueHandler.TryGetValueAs(this, fieldType, out var value))
                {
                    field.SetValue(target, value);
                    return true;
                }
            }

            return false;
        }
    }
}