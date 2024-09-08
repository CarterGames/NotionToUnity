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
using System.Linq;
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
                if (TryParseAsList(fieldType, out result))
                {
                    return result;
                }

                try
                {
                    return JsonUtility.FromJson<NotionArrayWrapper<string>>(propertyValue).list.ToArray();
                }
#pragma warning disable
                catch (Exception e)
                {
                    return null;
                }
#pragma warning restore
            }

            if (fieldType.ToString().Contains("System.Collections.Generic.List"))
            {
                if (TryParseAsList(fieldType, out result))
                {
                    return result;
                }

                try
                {
                    return JsonUtility.FromJson<NotionArrayWrapper<string>>(propertyValue).list;
                }
#pragma warning disable
                catch (Exception e)
                {
                    return null;
                }
#pragma warning restore
            }
            
            if (TryParseAsPrimitive(fieldType, out result))
            {
                return result;
            }

            if (fieldType.IsEnum)
            {
                if (fieldType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0)
                {
                    var combined = string.Empty;
                    var elements = JSON.Parse(propertyValue).AsArray;
                    
                    for (var index = 0; index < elements.Count; index++)
                    {
                        combined += elements[index].Value;
                        
                        if (index == elements.Count - 1) continue;
                        combined += ",";
                    }
                    
                    return Enum.Parse(fieldType, combined);
                }
                else
                {
                    return Enum.Parse(fieldType, propertyValue.Replace(" ", ""));
                }
            }

            if (TryParseWrapper(fieldType, out result))
            {
                return result;
            }

            if (fieldType.IsClass)
            {
                return JsonUtility.FromJson(propertyValue, fieldType);
            }

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


        private bool TryParseAsList(Type fieldType, out object result)
        {
            result = null;

            JSONArray toRead;

            try
            {
                toRead = JSON.Parse(propertyValue).AsArray;
            }
#pragma warning disable
            catch (Exception e)
#pragma warning restore
            {
                var builder = new StringBuilder();
                builder.Append("[");

                for (var i = 0; i < propertyValue.Split(',').Length; i++)
                {
                    builder.Append($"\"{propertyValue.Split(',')[i].Trim()}\"");
                    
                    if (i == propertyValue.Split(',').Length - 1) continue;
                    builder.Append(",");
                }
                
                builder.Append("]");
                toRead = JSON.Parse(builder.ToString()).AsArray;
            }


            switch (fieldType.Name)
            {
                case { } x when x.Contains("Int"):
                    
                    var parsedIntArray = new int[toRead.Count];

                    for (var i = 0; i < toRead.Count; i++)
                    {
                        parsedIntArray[i] = int.Parse(toRead[i].Value);
                    }

                    result = parsedIntArray;
                    break;
                case { } x when x.Contains("Boolean"):
                    
                    var parsedBoolArray = new bool[toRead.Count];

                    for (var i = 0; i < toRead.Count; i++)
                    {
                        parsedBoolArray[i] = bool.Parse(toRead[i].Value);
                    }

                    result = parsedBoolArray;
                    break;
                case { } x when x.Contains("Single"):
                    
                    var parsedFloatArray = new float[toRead.Count];

                    for (var i = 0; i < toRead.Count; i++)
                    {
                        parsedFloatArray[i] = float.Parse(toRead[i].Value);
                    }

                    result = parsedFloatArray;
                    break;
                case { } x when x.Contains("Double"):
                    
                    var parsedDoubleArray = new double[toRead.Count];

                    for (var i = 0; i < toRead.Count; i++)
                    {
                        parsedDoubleArray[i] = double.Parse(toRead[i].Value);
                    }

                    result = parsedDoubleArray;
                    break;
                case { } x when x.Contains("String"):
                    
                    var parsedStringArray = new string[toRead.Count];

                    for (var i = 0; i < toRead.Count; i++)
                    {
                        parsedStringArray[i] = toRead[i].Value;
                    }

                    result = parsedStringArray;
                    break;
                default:
                    break;
            }
            
            return result != null;
        }
    }
}