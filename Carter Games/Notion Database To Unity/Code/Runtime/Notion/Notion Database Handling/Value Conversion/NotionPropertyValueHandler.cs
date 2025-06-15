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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.NotionData
{
    /// <summary>
    /// Handles converting Json to a usable type.
    /// </summary>
    public static class NotionPropertyValueHandler
    {
        /// <summary>
        /// Gets the value in this property its type if possible. 
        /// </summary>
        /// <param name="property">The notion property to read from.</param>
        /// <param name="fieldType">The field type to match.</param>
        /// <param name="value">The object value assigned.</param>
        /// <returns>If the parsing was successful.</returns>
        public static bool TryGetValueAs(NotionProperty property, Type fieldType, out object value)
        {
            Debug.Log(fieldType);
            Debug.Log(fieldType.IsArray);
 
            
            if (fieldType.IsArray)
            {
                if (TryParseAsArray(property, fieldType, out value)) return true;
                
                if (fieldType.IsEnum)
                {
                    if (TryParseEnumOrEnumFlags(property, fieldType, out value)) return true;
                }
            }

            if (fieldType.IsGenericList())
            {
                if (TryParseAsList(property, fieldType, out value)) return true;
                
                if (fieldType.IsEnum)
                {
                    if (TryParseEnumOrEnumFlags(property, fieldType, out value)) return true;
                }
            }
            
            
            if (TryParseAsPrimitive(property, fieldType, out value)) return true;
            
            
            if (fieldType.IsEnum)
            {
                if (TryParseEnumOrEnumFlags(property, fieldType, out value)) return true;
            }
            
            
            if (TryParseWrapper(property, fieldType, out value)) return true;
            
            Debug.Log(property.PropertyName);
            
            if (fieldType.IsClass)
            {
                value = JsonUtility.FromJson(property.JsonValue, fieldType);
                return value != null;
            }

            Debug.LogError("Couldn't parse data");
            return false;
        }
        
        
        /// <summary>
        /// Gets the value in this property its type if possible (Only as wrapper). 
        /// </summary>
        /// <param name="property">The notion property to read from.</param>
        /// <param name="fieldType">The field type to match.</param>
        /// <param name="value">The object value assigned.</param>
        /// <returns>If the parsing was successful.</returns>
        public static bool TryGetValueAsWrapper(NotionProperty property, Type fieldType, out object value)
        {
            if (TryParseWrapper(property, fieldType, out value)) return true;
            
            Debug.LogError("Couldn't parse data");
            return false;
        }

        
        private static bool TryParseAsPrimitive(NotionProperty property, Type fieldType, out object result)
        {
            result = null;
            
            try
            {
                switch (fieldType.Name)
                {
                    case { } x when x.Contains("Int"):
                        result = (int) property.Number().Value;
                        break;
                    case { } x when x.Contains("Boolean"):
                        result = bool.Parse(property.JsonValue);
                        break;
                    case { } x when x.Contains("Single"):
                        result = (float) property.Number().Value;
                        break;
                    case { } x when x.Contains("Double"):
                        result = property.Number().Value;
                        break;
                    case { } x when x.Contains("String"):
                        result = property.RichText().Value;
                        break;
                }

                return result != null;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                return result != null;
            }
        }
        
        
        private static bool TryParseWrapper(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

            try
            {
                if (fieldType.BaseType.FullName.Contains(typeof(NotionDataWrapper<>).Namespace + ".NotionDataWrapper"))
                {
                    var allWrapperTypes = AssemblyHelper.GetClassesNamesOfType(typeof(NotionDataWrapper<>), false);

                    foreach (var wrapperType in allWrapperTypes)
                    {
                        if (wrapperType != fieldType) continue;

                        var constructor = wrapperType.GetConstructor(new[] {typeof(string)});
                        result = constructor.Invoke(new object[] {property.RichText().Value});
                    }
                }

                return result != null;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                return false;
            }
        }


        private static JSONArray GetPropertyValueAsCollection(NotionProperty property)
        {
            try
            {
                var value = property.MultiSelect().Value;

                // If the value is an collection of more than 1 element.
                if (value.Length > 1)
                {
                    try
                    {
                        // Tries to parse as an array normally.
                        return JSON.Parse(property.JsonValue).AsArray;
                    }
#pragma warning disable 0168
                    catch (Exception e)
#pragma warning restore
                    {
                        // Bracket the data in the [] so it can be parsed correctly.
                        var builder = new StringBuilder();
                        builder.Append("[");

                        // Parse each entry correctly into the collection.
                        for (var i = 0; i < property.JsonValue.Split(',').Length; i++)
                        {
                            builder.Append($"\"{property.JsonValue.Split(',')[i].Trim()}\"");

                            if (i == property.JsonValue.Split(',').Length - 1) continue;
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
                    if (property.JsonValue.Contains("[") && property.JsonValue.Contains("]"))
                    {
                        return JSON.Parse(property.JsonValue).AsArray;
                    }
                    //
                    // Bracket the data in the [] so it can be parsed correctly.
                    //
                    else
                    {
                        var builder = new StringBuilder();

                        builder.Append("[");
                        builder.Append($"\"{property.JsonValue.Split(',')[0].Trim()}\"");
                        builder.Append("]");

                        return JSON.Parse(builder.ToString()).AsArray;
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                // If the value is already bracketed when downloading, just use that.
                //
                if (property.JsonValue.Contains("[") && property.JsonValue.Contains("]"))
                {
                    return JSON.Parse(property.JsonValue).AsArray;
                }
                
                //
                // Bracket the data in the [] so it can be parsed correctly.
                //
                else
                {
                    var builder = new StringBuilder();

                    builder.Append("[");
                    builder.Append($"\"{property.JsonValue.Split(',')[0].Trim()}\"");
                    builder.Append("]");

                    return JSON.Parse(builder.ToString()).AsArray;
                }
            }
        }


        private static bool TryParseAsArray(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

            try
            {
                var data = GetPropertyValueAsCollection(property);
                
                if (fieldType.GetElementType().IsEnum)
                {
                    var parsedStringArray = new string[data.Count];

                    for (var i = 0; i < data.Count; i++)
                    {
                        parsedStringArray[i] = data[i].Value;
                    }

                    result = parsedStringArray.Select(t => (int) Enum.Parse(fieldType.GetElementType(), t)).ToArray();
                    return true;
                }
                else
                {
                    switch (fieldType.GetElementType()?.Name)
                    {
                        case { } x when x.Equals("Int"):

                            var parsedIntArray = new int[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedIntArray[i] = int.Parse(data[i].Value);
                            }

                            result = parsedIntArray;
                            break;
                        case { } x when x.Equals("Boolean"):

                            var parsedBoolArray = new bool[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedBoolArray[i] = bool.Parse(data[i].Value);
                            }

                            result = parsedBoolArray;
                            break;
                        case { } x when x.Equals("Single"):

                            var parsedFloatArray = new float[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedFloatArray[i] = float.Parse(data[i].Value);
                            }

                            result = parsedFloatArray;
                            break;
                        case { } x when x.Equals("Double"):

                            var parsedDoubleArray = new double[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedDoubleArray[i] = double.Parse(data[i].Value);
                            }

                            result = parsedDoubleArray;
                            break;
                        case { } x when x.Equals("String"):

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
                }

                return result != null;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                return false;
            }
        }


        private static bool TryParseAsList(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

            try
            {
                var data = GetPropertyValueAsCollection(property);
                var typeName = fieldType.GenericTypeArguments.Length > 0
                    ? fieldType.GenericTypeArguments[0]
                    : fieldType;


                switch (typeName.Name)
                {
                    case { } x when x.Contains("Int"):

                        var parsedIntList = new List<int>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedIntList.Add(int.Parse(data[i].Value));
                        }

                        result = parsedIntList;
                        break;
                    case { } x when x.Contains("Boolean"):

                        var parsedBoolList = new List<bool>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedBoolList.Add(bool.Parse(data[i].Value));
                        }

                        result = parsedBoolList;
                        break;
                    case { } x when x.Contains("Single"):

                        var parsedFloatList = new List<float>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedFloatList.Add(float.Parse(data[i].Value));
                        }

                        result = parsedFloatList;
                        break;
                    case { } x when x.Contains("Double"):

                        var parsedDoubleList = new List<double>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedDoubleList.Add(double.Parse(data[i].Value));
                        }

                        result = parsedDoubleList;
                        break;
                    case { } x when x.Contains("String"):

                        var parsedStringList = new List<string>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedStringList.Add(data[i].Value);
                        }

                        result = parsedStringList;
                        break;
                    default:
                        break;
                }

                return result != null;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                return false;
            }
        }


        private static bool TryParseEnumOrEnumFlags(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

            try
            {
                if (fieldType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0 && JSON.Parse(property.JsonValue).IsArray)
                {
                    var combined = string.Empty;
                    var elements = JSON.Parse(property.JsonValue).AsArray;

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
                        result = Enum.Parse(fieldType, property.JsonValue.Replace(" ", ""));
                        return result != null;
                    }
#pragma warning disable 0168
                    catch (Exception e)
#pragma warning restore
                    {
                        result = fieldType.IsValueType ? Activator.CreateInstance(fieldType) : null;
                        return result != null;
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                return false;
            }
        }
        
        
        private static bool IsGenericList(this Type o)
        {
            return (o.IsGenericType && (o.GetGenericTypeDefinition() == typeof(List<>)));
        }
    }
}