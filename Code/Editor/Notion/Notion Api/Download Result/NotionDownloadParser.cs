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

using System;
using System.Collections.Generic;
using System.Linq;
using CarterGames.Shared.NotionData.Editor;
using CarterGames.Shared.NotionData.Serializiation;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles the downloaded data from Notion for an asset to read and use.
    /// </summary>
    public static class NotionDownloadParser
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static Dictionary<string, NotionProperty> cacheDatabasePropertyParserLookup;

        private static readonly List<NotionProperty> Properties = new List<NotionProperty>()
        {
            { NotionPropertyCheckbox.Blank() },
            { NotionPropertyDate.Blank() },
            { NotionPropertyMultiSelect.Blank() },
            { NotionPropertyNumber.Blank() },
            { NotionPropertyRichText.Blank() },
            { NotionPropertySelect.Blank() },
            { NotionPropertyStatus.Blank() },
            { NotionPropertyTitle.Blank() },
            { NotionPropertyUrl.Blank() }
        };
            
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Dictionary<string, NotionProperty> DatabasePropertyParserLookup
        {
            get
            {
                if (cacheDatabasePropertyParserLookup is { } && cacheDatabasePropertyParserLookup.Count > 0) return
                    cacheDatabasePropertyParserLookup;

                cacheDatabasePropertyParserLookup = new Dictionary<string, NotionProperty>();

                foreach (var prop in Properties)
                {
                    cacheDatabasePropertyParserLookup.Add(prop.EditorOnly_PropertyIdentifier, prop);
                }

                return cacheDatabasePropertyParserLookup;
            }
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Handles parsing the data from the database into the query class for use.
        /// </summary>
        /// <param name="data">The JSON data to read.</param>
        /// <returns>The query result setup for use.</returns>
        public static NotionDatabaseQueryResult Parse(List<IDictionary<string, JToken>> data)
        {
            var result = new List<NotionDatabaseRow>();
            
            for (var i = 0; i < data.Count; i++)
            {
                result.Add(GetRowData(data[i]));
            }

            return new NotionDatabaseQueryResult(result);
        }


        /// <summary>
        /// Gets the data for a row of a notion database.
        /// </summary>
        /// <param name="data">The json to read.</param>
        /// <returns>The database row generated.</returns>
        private static NotionDatabaseRow GetRowData(IDictionary<string, JToken> data)
        {
            var keys = new List<string>();
            var values = new List<JToken>();
            var lookup = new SerializableDictionary<string, NotionProperty>();

            foreach (var entry in data)
            {
                var adjustedKey = entry.Key.Trim().ToLower().Replace(" ", string.Empty);

                if (adjustedKey.StartsWith(ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().ObjectRef
                        .Fp("ignorePropertyPrefix").stringValue))
                {
                    continue;
                }
                
                var actualKey = entry.Key;
                var notionType = entry.Value["type"].Value<string>();
                var valueForType = GetValueForType(notionType, entry.Value);
                var valueJson = valueForType;
                var downloadText = entry.Value.ToString();
                var propertyData = new NotionPropertyData(actualKey, valueForType, valueJson, downloadText);
                
                if (notionType == "rollup")
                {
                    if (entry.Value["rollup"]["array"].Count() <= 0) continue;
                    
                    notionType = entry.Value["rollup"]["array"][0]["type"].Value<string>();
                    downloadText = entry.Value["rollup"]["array"][0].ToString();
                    valueJson = GetValueForType(notionType, entry.Value["rollup"]["array"][0]);
                    valueForType = valueJson;
                    propertyData = new NotionPropertyData(actualKey, valueForType, valueJson, downloadText);
                }
                
                AddPropertyToLookup(lookup, adjustedKey, notionType, propertyData);
            }

            return new NotionDatabaseRow(lookup);
        }
        

        private static void AddPropertyToLookup(IDictionary<string, NotionProperty> lookup, string key, string notionType, NotionPropertyData propertyData)
        {
            switch (notionType)
            {
                case "title":
                    lookup.Add(key, NotionPropertyFactory.Title(propertyData));
                    break;
                case "rich_text":
                    lookup.Add(key, NotionPropertyFactory.RichText(propertyData));
                    break;
                case "number":
                    lookup.Add(key, NotionPropertyFactory.Number(propertyData));
                    break;
                case "checkbox":
                    lookup.Add(key, NotionPropertyFactory.Checkbox(propertyData));
                    break;
                case "select":
                    lookup.Add(key, NotionPropertyFactory.Select(propertyData));
                    break;
                case "status":
                    lookup.Add(key, NotionPropertyFactory.Status(propertyData));
                    break;
                case "date":
                    lookup.Add(key, NotionPropertyFactory.Date(propertyData));
                    break;
                case "multi_select":
                    lookup.Add(key, NotionPropertyFactory.MultiSelect(propertyData));
                    break;
                case "url":
                    lookup.Add(key, NotionPropertyFactory.Url(propertyData));
                    break;
                default:
                    Debug.LogWarning(
                        $"Unable to assign value: {key} as the Notion data type {notionType} is not supported.");
                    break;
            }
        }


        /// <summary>
        /// Gets the value of the notion property type entered.
        /// </summary>
        /// <param name="type">The type to read.</param>
        /// <param name="element">The element to get the value from.</param>
        /// <returns>The value found.</returns>
        private static string GetValueForType(string type, JToken element)
        {
            // Avoids issues where the type is not supported but the notion property type is (like a rollup showing a page etc).
            try
            {
                return DatabasePropertyParserLookup[type].EditorOnly_GetValueJsonFromContainer(element);
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                Debug.LogWarning("Not supported notion type");
                return string.Empty;
            }
        }
    }
}