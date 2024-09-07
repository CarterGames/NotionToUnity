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
using CarterGames.Standalone.NotionData.Common;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
    /// <summary>
    /// Handles the downloaded data from Notion for an asset to read and use.
    /// </summary>
    public static class NotionDownloadParser
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static readonly Dictionary<string, INotionDatabasePropertyParser> DatabasePropertyParserLookup =
            new Dictionary<string, INotionDatabasePropertyParser>()
            {
                {"title", new NotionDatabasePropertyParserTitle()},
                {"rich_text", new NotionDatabasePropertyParserRichText()},
                {"date", new NotionDatabasePropertyParserDate()},
                {"checkbox", new NotionDatabasePropertyParserCheckbox()},
                {"select", new NotionDatabasePropertyParserSelect()},
                {"multi_select", new NotionDatabasePropertyParserMultiSelect()},
                {"number", new NotionDatabasePropertyParserNumber()}
            };
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles parsing the data from the database into the query class for use.
        /// </summary>
        /// <param name="data">The JSON data to read.</param>
        /// <returns>The query result setup for use.</returns>
        public static NotionDatabaseQueryResult Parse(List<KeyValuePair<string, JSONNode>> data)
        {
            var result = new List<NotionDatabaseRow>();

            for (var i = 0; i < data.Count; i++)
            {
                result.Add(GetRowData(data[i].Value["properties"]));
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
            var lookup = new SerializableDictionary<string, NotionDatabaseProperty>();


            foreach (var k in element.Keys)
            {
                keys.Add(k);
            }


            for (var i = 0; i < keys.Count; i++)
            {
                var propName = keys[i];
                var propType = element.AsObject[i]["type"].Value;
                var adjustedKey = keys[i].Trim().ToLower().Replace(" ", string.Empty);


                // Debug.Log(element.AsObject[i].ToString());
                
                
                switch (element.AsObject[i]["type"].Value)
                {
                    case "title":
                    case "rich_text":
                    case "number":
                    case "checkbox":
                    case "select":
                    case "date":
                    case "multi_select":
                        lookup.Add(adjustedKey, new NotionDatabaseProperty(propName, propType, GetValueForType(element.AsObject[i]["type"].Value, element.AsObject[i])));
                        break;
                    case "rollup":
                        lookup.Add(adjustedKey, new NotionDatabaseProperty(propName, propType, GetValueForType(element.AsObject[i]["rollup"]["array"][0]["type"].Value, element.AsObject[i]["rollup"]["array"][0])));
                        break;
                    default:
                        Debug.LogWarning($"Unable to assign value: {keys[i]} as the Notion data type {element.AsObject[i]["type"].Value} is not supported.");
                        break;
                }
            }

            return new NotionDatabaseRow(lookup);
        }


        /// <summary>
        /// Gets the value of the notion property type entered.
        /// </summary>
        /// <param name="type">The type to read.</param>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value found.</returns>
        private static string GetValueForType(string type, JSONNode element)
        {
            return DatabasePropertyParserLookup[type].GetValue(element);
        }
    }
}