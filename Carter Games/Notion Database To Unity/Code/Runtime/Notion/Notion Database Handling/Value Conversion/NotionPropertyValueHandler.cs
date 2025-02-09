using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using CarterGames.Standalone.NotionData.Common;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Value_Conversion
{
    public static class NotionPropertyValueHandler
    {
        /// <summary>
        /// Gets the value in this property its type if possible. 
        /// </summary>
        /// <param name="fieldType">The field type to match.</param>
        /// <returns>The resulting entry.</returns>
        public static bool TryGetValueAs(NotionProperty property, Type fieldType, out object value)
        {
            if (fieldType.IsArray)
            {
                if (TryParseAsArray(property, fieldType, out value)) return true;
                if (TryParseAsList(property, fieldType, out value)) return true;
                
                if (fieldType.IsEnum)
                {
                    if (TryParseEnumOrEnumFlags(property, fieldType, out value)) return true;
                }
            }


            if (fieldType.ToString().Contains("System.Collections.Generic.List"))
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

            Debug.LogError("Couldn't parse data");
            return false;
        }
        
        
        public static bool TryGetValueAsWrapper(NotionProperty property, Type fieldType, out object value)
        {
            if (TryParseWrapper(property, fieldType, out value)) return true;
            
            Debug.LogError("Couldn't parse data");
            return false;
        }


        /// <summary>
        /// Tries to parse the data to the matching field type.
        /// </summary>
        /// <param name="fieldType">The field type to check.</param>
        /// <param name="result">The resulting parsed data.</param>
        /// <returns>If it was successful.</returns>
        private static bool TryParseAsPrimitive(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

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
        
        
        private static bool TryParseWrapper(NotionProperty property, Type fieldType, out object result)
        {
            result = null;

            if (fieldType.BaseType.FullName.Contains(typeof(NotionDataWrapper).Namespace + ".NotionDataWrapper"))
            {
                var allWrapperTypes = AssemblyHelper.GetClassesNamesOfType<NotionDataWrapper>(false);

                foreach (var wrapperType in allWrapperTypes)
                {
                    if (wrapperType != fieldType) continue;

                    var constructor = wrapperType.GetConstructor(new[] {typeof(string)});
                    result = constructor.Invoke(new object[] {property.RichText().Value});
                }
            }

            return result != null;
        }


        private static JSONArray GetPropertyValueAsCollection(NotionProperty property)
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
#pragma warning disable
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
        

        private static bool TryParseAsArray(NotionProperty property, Type fieldType, out object result)
        {
            var data = GetPropertyValueAsCollection(property);
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
        
        
        private static bool TryParseAsList(NotionProperty property, Type fieldType, out object result)
        {
            var data = GetPropertyValueAsCollection(property);
            var typeName = fieldType.GenericTypeArguments.Length > 0 ? fieldType.GenericTypeArguments[0] : fieldType;
            result = null;
            
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


        private static bool TryParseEnumOrEnumFlags(NotionProperty property, Type fieldType, out object result)
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