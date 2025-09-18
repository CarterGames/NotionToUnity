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
using Newtonsoft.Json.Linq;
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
            
            
            if (fieldType.IsClass)
            {
                value = JsonUtility.FromJson(property.JsonValue, fieldType);
                return value != null;
            }

            Debug.LogError($"Couldn't parse data for {property.PropertyName} - {fieldType}");
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


        private static JArray GetPropertyValueAsCollection(NotionProperty property)
        {
            try
            {
                var value = property.MultiSelect().JsonValue;

                // If the value is an collection of more than 1 element.
                if (value.Length > 1)
                {
                    try
                    {
                        // Tries to parse as an array normally.
                        return JArray.Parse(property.JsonValue);
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

                        return JArray.Parse(builder.ToString());
                    }
                }
                // Only a single element collection.
                else
                {
                    // If the value is already bracketed when downloading, just use that.
                    //
                    var charArray = property.JsonValue.ToCharArray();
                
                    if (charArray[0].Equals('[') && charArray[charArray.Length - 1].Equals(']'))
                    {
                        return JArray.Parse(property.JsonValue);
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

                        return JArray.Parse(builder.ToString());
                    }
                }
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                // If the value is already bracketed when downloading, just use that.
                //
                var charArray = property.JsonValue.ToCharArray();
                
                if (charArray[0].Equals('[') && charArray[charArray.Length - 1].Equals(']'))
                {
                    var raw = property.JsonValue.Replace("[", string.Empty).Replace("]", string.Empty);
                    var collection = new JArray();

                    foreach (var entry in raw.Split(','))
                    {
                        collection.Add(entry.Trim());
                    }

                    return collection;
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

                    return JArray.Parse(builder.ToString());
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
                        parsedStringArray[i] = data[i].Value<string>();
                    }

                    result = parsedStringArray.Select(t => (int)Enum.Parse(fieldType.GetElementType(), t)).ToArray();
                    return true;
                }
                else
                {
                    switch (fieldType.GetElementType().Name)
                    {
                        case { } x when x.Contains("Int"):

                            var parsedIntArray = new int[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedIntArray[i] = int.Parse(data[i].Value<string>());
                            }

                            result = parsedIntArray;
                            break;
                        case { } x when x.Contains("Boolean"):

                            var parsedBoolArray = new bool[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedBoolArray[i] = bool.Parse(data[i].Value<string>());
                            }

                            result = parsedBoolArray;
                            break;
                        case { } x when x.Contains("Single"):

                            var parsedFloatArray = new float[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedFloatArray[i] = float.Parse(data[i].Value<string>());
                            }

                            result = parsedFloatArray;
                            break;
                        case { } x when x.Contains("Double"):

                            var parsedDoubleArray = new double[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedDoubleArray[i] = double.Parse(data[i].Value<string>());
                            }

                            result = parsedDoubleArray;
                            break;
                        case { } x when x.Contains("String"):

                            var parsedStringArray = new string[data.Count];

                            for (var i = 0; i < data.Count; i++)
                            {
                                parsedStringArray[i] = data[i].ToString();
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
                            parsedIntList.Add(int.Parse(data[i].Value<string>()));
                        }

                        result = parsedIntList;
                        break;
                    case { } x when x.Contains("Boolean"):

                        var parsedBoolList = new List<bool>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedBoolList.Add(bool.Parse(data[i].Value<string>()));
                        }

                        result = parsedBoolList;
                        break;
                    case { } x when x.Contains("Single"):

                        var parsedFloatList = new List<float>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedFloatList.Add(float.Parse(data[i].Value<string>()));
                        }

                        result = parsedFloatList;
                        break;
                    case { } x when x.Contains("Double"):

                        var parsedDoubleList = new List<double>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedDoubleList.Add(double.Parse(data[i].Value<string>()));
                        }

                        result = parsedDoubleList;
                        break;
                    case { } x when x.Contains("String"):

                        var parsedStringList = new List<string>();

                        for (var i = 0; i < data.Count; i++)
                        {
                            parsedStringList.Add(data[i].Value<string>());
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
                if (fieldType.GetCustomAttributes(typeof(FlagsAttribute), true).Length > 0 && property.JsonValue.Contains("["))
                {
                    var combined = string.Empty;
                    var elements = JArray.Parse(property.JsonValue);

                    if (elements.Count <= 1)
                    {
                        result = Enum.Parse(fieldType, elements[0].Value<string>().Replace(" ", ""));
                        return result != null;
                    }

                    for (var index = 0; index < elements.Count; index++)
                    {
                        combined += elements[index].Value<string>();

                        if (index == elements.Count - 1) continue;
                        combined += ",";
                    }

                    result = Enum.Parse(fieldType, combined);
                    return result != null;
                }
                else
                {
                    if (GetPropertyValueAsCollection(property).Count > 0)
                    {
                        var combined = string.Empty;
                        var data = GetPropertyValueAsCollection(property);
                        
                        for (var index = 0; index < data.Count; index++)
                        {
                            combined += data[index].Value<string>();

                            if (index == data.Count - 1) continue;
                            combined += ",";
                        }

                        result = Enum.Parse(fieldType, combined);
                    }
                    else
                    {
                        result = Enum.Parse(fieldType, property.JsonValue.Replace(" ", ""));
                        return result != null;
                    }
                    
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
