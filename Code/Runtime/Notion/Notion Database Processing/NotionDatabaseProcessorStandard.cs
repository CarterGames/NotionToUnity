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
using System.Reflection;

namespace CarterGames.NotionData
{
    /// <summary>
    /// The standard data parser from Notion to a NotionDataAsset. Matches the property name for parses the data to each entry.
    /// </summary>
    [Serializable]
    public sealed class NotionDatabaseProcessorStandard : NotionDatabaseProcessor
    {
        public override List<object> Process<T>(NotionDatabaseQueryResult result)
        {
            var list = new List<object>();

            foreach (var row in result.Rows)
            {
                // Make a new instance of the generic type.
                var newEntry = new T();
                
                // Gets the fields on the type to convert & write to.
                var newEntryFields = newEntry.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in newEntryFields)
                {
                    // Tries to find the id matching the field name.
                    // If none found, it just skips it.
                    if (!row.DataLookup.ContainsKey(field.Name.Trim().ToLower())) continue;
                    
                    // Gets the property info assigned to that key.
                    var rowProperty = row.DataLookup[field.Name.Trim().ToLower()];
                    
                    // Tries to parse the data into the field type if possible.
                    rowProperty.TryConvertValueToFieldType(field, newEntry);
                }
                
                list.Add(newEntry);
            }
            
            return list;
        }
    }
}