/*
 * Notion Data (0.x)
 * Copyright (c) Carter Games
 *
 * This program is free software: you can redistribute it and/or modify it under the terms of the
 * GNU General Public License as published by the Free Software Foundation,
 * either version 3 of the License, or (at your option) any later version. 
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or
 * FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details. 
 *
 * You should have received a copy of the GNU General Public License along with this program.
 * If not, see <https://www.gnu.org/licenses/>. 
 */

using System.Reflection;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData
{
    /// <summary>
    /// An abstract class to handle data as if it were a notion property when downloaded.
    /// </summary>
    public abstract class NotionProperty
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#if UNITY_EDITOR
        /// <summary>
        /// The identifier for the property in json downloaded.
        /// </summary>
        public abstract string EditorOnly_PropertyIdentifier { get; }
#endif
        
        /// <summary>
        /// The name of the property in Notion without being forced to a lower string.
        /// </summary>
        public string PropertyName { get; protected set; }
        
        
        /// <summary>
        /// The typed-value, stored in an object so it can be generic without a type required.
        /// </summary>
        protected abstract object InternalValue { get; set; }
        
        
        /// <summary>
        /// The JSON value of the value this property holds.
        /// </summary>
        public abstract string JsonValue { get; protected set; }
        
        
        /// <summary>
        /// The raw download Json in-case it is needed.
        /// </summary>
        public abstract string DownloadText { get; protected set; }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Static Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Converts this data to a checkbox property.
        /// </summary>
        /// <returns>NotionPropertyCheckbox</returns>
        public NotionPropertyCheckbox CheckBox() => NotionPropertyFactory.Checkbox(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a date property.
        /// </summary>
        /// <returns>NotionPropertyDate</returns>
        public NotionPropertyDate Date() => NotionPropertyFactory.Date(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a multi-select property.
        /// </summary>
        /// <returns>NotionPropertyMultiSelect</returns>
        public NotionPropertyMultiSelect MultiSelect() => NotionPropertyFactory.MultiSelect(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a select property.
        /// </summary>
        /// <returns>NotionPropertySelect</returns>
        public NotionPropertySelect Select() => NotionPropertyFactory.Select(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a select property.
        /// </summary>
        /// <returns>NotionPropertyStatus</returns>
        public NotionPropertyStatus Status() => NotionPropertyFactory.Status(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a number property.
        /// </summary>
        /// <returns>NotionPropertyNumber</returns>
        public NotionPropertyNumber Number() => NotionPropertyFactory.Number(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a richtext property.
        /// </summary>
        /// <returns>NotionPropertyRichText</returns>
        public NotionPropertyRichText RichText() => NotionPropertyFactory.RichText(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        
        /// <summary>
        /// Converts this data to a title property.
        /// </summary>
        /// <returns>NotionPropertyTitle</returns>
        public NotionPropertyTitle Title() => NotionPropertyFactory.Title(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));

        
        /// <summary>
        /// Converts this data to a url property.
        /// </summary>
        /// <returns>NotionPropertyUrl</returns>
        public NotionPropertyUrl Url() => NotionPropertyFactory.Url(new NotionPropertyData(PropertyName, InternalValue, JsonValue, DownloadText));
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Helper Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Tries to convert the json value to the entered type.
        /// </summary>
        /// <param name="value">The converted value if successful.</param>
        /// <typeparam name="T">The type to try and convert to.</typeparam>
        /// <returns>If the conversion was successful.</returns>
        public bool TryConvertValueToType<T>(out T value)
        {
            value = default;
            
            if (typeof(T).BaseType.FullName.Contains(typeof(NotionDataWrapper<>).Namespace + ".NotionDataWrapper"))
            {
                if (NotionPropertyValueHandler.TryGetValueAsWrapper(this, typeof(T), out var valueObj))
                {
                    value = (T) valueObj;
                    return true;
                }
            }
            else
            {
                if (NotionPropertyValueHandler.TryGetValueAs(this, typeof(T), out var valueObj))
                {
                    value = (T) valueObj;
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Tries to convert the json value to the field entered.
        /// </summary>
        /// <param name="field">The field to apply to.</param>
        /// <param name="target">The target object the field is on.</param>
        /// <returns>If the conversion was successful.</returns>
        public bool TryConvertValueToFieldType(FieldInfo field, object target)
        {
            var fieldType = field.FieldType;
                        
            if (fieldType.BaseType.FullName.Contains(typeof(NotionDataWrapper<>).Namespace + ".NotionDataWrapper"))
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

#if UNITY_EDITOR
        public abstract string EditorOnly_GetValueJsonFromContainer(JToken json);
#endif
    }
}