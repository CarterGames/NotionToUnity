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

using System.Collections.Generic;
using NotionToUnity.Common;
using UnityEngine;

namespace NotionToUnity.Editor
{
    /// <summary>
    /// Handles the downloaded data from Notion for an asset to read and use.
    /// </summary>
    public static class NotionDownloadParser
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles parsing the data from the database into the query class for use.
        /// </summary>
        /// <param name="data">The JSON data to read.</param>
        /// <returns>The query result setup for use.</returns>
        public static NotionDatabaseQueryResult Parse(string data)
        {
            var result = new List<NotionDatabaseRow>();
                    
            for (var i = 0; i < JSONNode.Parse(data)["results"].AsArray.Count; i++)
            {
                result.Add(GetRowData(JSONNode.Parse(data)["results"][i]["properties"]));
            }

            return new NotionDatabaseQueryResult(result);
        }


        /// <summary>
        /// Gets the data for a row of a notion database.
        /// </summary>
        /// <param name="element">The json to read.</param>
        /// <returns>The database row generated.</returns>
        private static NotionDatabaseRow GetRowData(JSONNode element)
        {
            var keys = new List<string>();
            var lookup = new SerializableDictionary<string, NotionProperty>();
                
            
            foreach (var k in element.Keys)
            {
                keys.Add(k);
            }
            
            
            for (var i = 0; i < keys.Count; i++)
            {
                var propName = keys[i];
                var propType = element.AsObject[i]["type"].Value;
                var adjustedKey = keys[i].Trim().ToLower().Replace(" ", string.Empty);
                
                switch (element.AsObject[i]["type"].Value)
                {
                    case "title":
                        lookup.Add(adjustedKey, new NotionProperty(propName, propType, element.AsObject[i]["title"][0]["text"]["content"].Value));
                        break;
                    case "rich_text":
                        lookup.Add(adjustedKey, new NotionProperty(propName, propType, element.AsObject[i]["rich_text"][0]["text"]["content"].Value));
                        break;
                    case "number":
                        lookup.Add(adjustedKey, new NotionProperty(propName, propType, element.AsObject[i]["number"].Value));
                        break;
                    case "checkbox":
                        lookup.Add(adjustedKey, new NotionProperty(propName, propType, element.AsObject[i]["checkbox"].Value));
                        break;
                    case "select":

                        if (element.AsObject[i]["select"]["name"] == null)
                        {
                            lookup.Add(adjustedKey, new NotionProperty(propName, propType, null));
                        }
                        else
                        {
                            lookup.Add(adjustedKey, new NotionProperty(propName, propType, element.AsObject[i]["select"]["name"].Value));
                        }
                 
                        break;
                    case "multi_select":

                        var elements = new NotionArrayWrapper<string>
                        {
                            list = new List<string>()
                        };

                        for (var j = 0; j < element.AsObject[i]["multi_select"].Count; j++)
                        {
                            elements.list.Add(element.AsObject[i]["multi_select"][j]["name"].Value);
                        }
                        
                        lookup.Add(adjustedKey, new NotionProperty(propName, propType, JsonUtility.ToJson(elements)));
                        break;
                    default:
                        Debug.LogError($"Unable to assign value: {keys[i]} as the Notion data type was not found.");
                        break;
                }
            }
            
                
            return new NotionDatabaseRow(lookup);
        }
    }
}