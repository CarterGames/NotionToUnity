/*
 * Copyright (c) 2024 Carter Games
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

using System;
using System.Collections.Generic;
using System.Text;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData
{
    /// <summary>
    /// A class that contains a notion property from a notion database row.
    /// </summary>
    [Serializable]
    public sealed class NotionDatabaseProperty
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private string propertyName;
        [SerializeField] private string propertyType;
        [SerializeField] private string propertyValue;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public string JsonValue => propertyValue;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new property with the entered details.
        /// </summary>
        /// <param name="propName">The nam of the property.</param>
        /// <param name="propertyType">The type the property is in notion.</param>
        /// <param name="propValue">The value of the property in notion.</param>
        public NotionDatabaseProperty(string propName, string propertyType, string propValue)
        {
            propertyName = propName;
            this.propertyType = propertyType;
            propertyValue = propValue;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the value in this property its type if possible. 
        /// </summary>
        /// <param name="fieldType">The field type to match.</param>
        /// <returns>The resulting entry.</returns>
        public object GetValueAs(Type fieldType)
        {
            object result;
            
            if (propertyValue == null)
            {
                return null;
            }
            
            
            if (fieldType.IsArray)
            {
                if (TryParseAsArray(fieldType, out result)) return result;
                if (TryParseAsList(fieldType, out result)) return result;
                
                if (fieldType.IsEnum)
                {
                    if (TryParseEnumOrEnumFlags(fieldType, out result)) return result;
                }
            }
            
            
            if (TryParseAsPrimitive(fieldType, out result)) return result;
            
            
            if (fieldType.IsEnum)
            {
                if (TryParseEnumOrEnumFlags(fieldType, out result)) return result;
            }
            
            
            if (TryParseWrapper(fieldType, out result)) return result;
            if (fieldType.IsClass) return JsonUtility.FromJson(propertyValue, fieldType);
            

            Debug.LogError("Couldn't parse data");
            return null;
        }


        /// <summary>
        /// Tries to parse the data to the matching field type.
        /// </summary>
        /// <param name="fieldType">The field type to check.</param>
        /// <param name="result">The resulting parsed data.</param>
        /// <returns>If it was successful.</returns>
        private bool TryParseAsPrimitive(Type fieldType, out object result)
        {
            result = null;

            switch (fieldType.Name)
            {
                case { } x when x.Contains("Int"):
                    result = int.Parse(propertyValue);
                    break;
                case { } x when x.Contains("Boolean"):
                    result = bool.Parse(propertyValue);
                    break;
                case { } x when x.Contains("Single"):
                    result = float.Parse(propertyValue);
                    break;
                case { } x when x.Contains("Double"):
                    result = double.Parse(propertyValue);
                    break;
                case { } x when x.Contains("String"):
                    result = propertyValue;
                    break;
            }

            return result != null;
        }
        
        
        private bool TryParseWrapper(Type fieldType, out object result)
        {
            result = null;
            
            if (fieldType == typeof(NotionDataWrapperSprite))
            {
                result = new NotionDataWrapperSprite(propertyValue);
            }
            else if (fieldType == typeof(NotionDataWrapperPrefab))
            {
                result = new NotionDataWrapperPrefab(propertyValue);
            }
            else if (fieldType == typeof(NotionDataWrapperAudioClip))
            {
                result = new NotionDataWrapperAudioClip(propertyValue);
            }

            return result != null;
        }


        private JSONArray GetPropertyValueAsCollection()
        {
            // If the value is an collection of more than 1 element.
            if (propertyValue.Split(',').Length > 1)
            {
                try
                {
                    // Tries to parse as an array normally.
                    return JSON.Parse(propertyValue).AsArray;
                }
#pragma warning disable
                catch (Exception e)
#pragma warning restore
                {
                    // Bracket the data in the [] so it can be parsed correctly.
                    var builder = new StringBuilder();
                    builder.Append("[");

                    // Parse each entry correctly into the collection.
                    for (var i = 0; i < propertyValue.Split(',').Length; i++)
                    {
                        builder.Append($"\"{propertyValue.Split(',')[i].Trim()}\"");

                        if (i == propertyValue.Split(',').Length - 1) continue;
                        builder.Append(",");
                    }

                    builder.Append("]");

                    return JSON.Parse(builder.ToString()).AsArray;
                }
            }
            // Only a single element collection.
            else
            {
                // If the value is already bracketed when downloading, just use that.
                //
                if (propertyValue.Contains("[") && propertyValue.Contains("]"))
                {
                    return JSON.Parse(propertyValue).AsArray;
                }
                //
                // Bracket the data in the [] so it can be parsed correctly.
                //
                else
                {
                    var builder = new StringBuilder();
                    
                    builder.Append("[");
                    builder.Append($"\"{propertyValue.Split(',')[0].Trim()}\"");
                    builder.Append("]");

                    return JSON.Parse(builder.ToString()).AsArray;
                }
            }
        }
        

        private bool TryParseAsArray(Type fieldType, out object result)
        {
            var data = GetPropertyValueAsCollection();
            result = null;
            
            switch (fieldType.Name)
            {
                case { } x when x.Contains("Int"):
                    
                    var parsedIntArray = new int[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedIntArray[i] = int.Parse(data[i].Value);
                    }

                    result = parsedIntArray;
                    break;
                case { } x when x.Contains("Boolean"):
                    
                    var parsedBoolArray = new bool[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedBoolArray[i] = bool.Parse(data[i].Value);
                    }

                    result = parsedBoolArray;
                    break;
                case { } x when x.Contains("Single"):
                    
                    var parsedFloatArray = new float[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedFloatArray[i] = float.Parse(data[i].Value);
                    }

                    result = parsedFloatArray;
                    break;
                case { } x when x.Contains("Double"):
                    
                    var parsedDoubleArray = new double[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedDoubleArray[i] = double.Parse(data[i].Value);
                    }

                    result = parsedDoubleArray;
                    break;
                case { } x when x.Contains("String"):
                    
                    var parsedStringArray = new string[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedStringArray[i] = data[i].Value;
                    }

                    result = parsedStringArray;
                    break;
                default:
                    break;
            }
            
            return result != null;
        }
        
        
        private bool TryParseAsList(Type fieldType, out object result)
        {
            var data = GetPropertyValueAsCollection();
            var typeName = fieldType.GenericTypeArguments.Length > 0 ? fieldType.GenericTypeArguments[0] : fieldType;
            result = null;
            
            switch (typeName.Name)
            {
                case { } x when x.Contains("Int"):
                    
                    var parsedIntList = new List<int>();

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedIntList[i] = int.Parse(data[i].Value);
                    }

                    result = parsedIntList;
                    break;
                case { } x when x.Contains("Boolean"):
                    
                    var parsedBoolList = new List<bool>();

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedBoolList[i] = bool.Parse(data[i].Value);
                    }

                    result = parsedBoolList;
                    break;
                case { } x when x.Contains("Single"):
                    
                    var parsedFloatList = new List<float>();

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedFloatList[i] = float.Parse(data[i].Value);
                    }

                    result = parsedFloatList;
                    break;
                case { } x when x.Contains("Double"):
                    
                    var parsedDoubleList = new List<double>();

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedDoubleList[i] = double.Parse(data[i].Value);
                    }

                    result = parsedDoubleList;
                    break;
                case { } x when x.Contains("String"):

                    var parsedStringList = new List<string>();

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedStringList[i] = data[i].Value;
                    }

                    result = parsedStringList;
                    break;
                default:
                    break;
            }
            
            return result != null;
        }


        private bool TryParseEnumOrEnumFlags(Type fieldType, out object result)
        {
            if (fieldType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0 && JSON.Parse(propertyValue).IsArray)
            {
                var combined = string.Empty;
                var elements = JSON.Parse(propertyValue).AsArray;

                if (elements.Count <= 1)
                {
                    result = Enum.Parse(fieldType, elements[0].Value.Replace(" ", ""));
                    return result != null;
                }
                    
                for (var index = 0; index < elements.Count; index++)
                {
                    combined += elements[index].Value;
                        
                    if (index == elements.Count - 1) continue;
                    combined += ",";
                }
                    
                result = Enum.Parse(fieldType, combined);
                return result != null;
            }
            else
            {
                try
                {
                    result = Enum.Parse(fieldType, propertyValue.Replace(" ", ""));
                    return result != null;
                }
#pragma warning disable
                catch (Exception e)
#pragma warning restore
                {
                    result = fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;
                    return result != null;
                }
            }
        }
    }
}