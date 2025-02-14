/*
 * Copyright (c) 2025 Carter Games
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Reflection;

namespace CarterGames.Standalone.NotionData
{
    /// <summary>
    /// An abstract class to handle data as if it were a notion property when downloaded.
    /// </summary>
    public abstract class NotionProperty
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
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
        public NotionPropertyCheckbox CheckBox() => NotionPropertyFactory.Checkbox(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a date property.
        /// </summary>
        /// <returns>NotionPropertyDate</returns>
        public NotionPropertyDate Date() => NotionPropertyFactory.Date(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a multi-select property.
        /// </summary>
        /// <returns>NotionPropertyMultiSelect</returns>
        public NotionPropertyMultiSelect MultiSelect() => NotionPropertyFactory.MultiSelect(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a select property.
        /// </summary>
        /// <returns>NotionPropertySelect</returns>
        public NotionPropertySelect Select() => NotionPropertyFactory.Select(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a select property.
        /// </summary>
        /// <returns>NotionPropertyStatus</returns>
        public NotionPropertyStatus Status() => NotionPropertyFactory.Status(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a number property.
        /// </summary>
        /// <returns>NotionPropertyNumber</returns>
        public NotionPropertyNumber Number() => NotionPropertyFactory.Number(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a richtext property.
        /// </summary>
        /// <returns>NotionPropertyRichText</returns>
        public NotionPropertyRichText RichText() => NotionPropertyFactory.RichText(InternalValue, JsonValue, DownloadText);
        
        
        /// <summary>
        /// Converts this data to a title property.
        /// </summary>
        /// <returns>NotionPropertyTitle</returns>
        public NotionPropertyTitle Title() => NotionPropertyFactory.Title(InternalValue, JsonValue, DownloadText);

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
    }
}