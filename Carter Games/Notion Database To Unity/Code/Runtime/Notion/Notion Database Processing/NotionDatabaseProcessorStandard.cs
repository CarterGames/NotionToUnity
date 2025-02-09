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
using System.Reflection;

namespace CarterGames.Standalone.NotionData
{
    /// <summary>
    /// The standard data parser from Notion to a NotionDataAsset. Matches the property name for parses the data to each entry.
    /// </summary>
    public class NotionDatabaseProcessorStandard : NotionDatabaseProcessor
    {
        public override List<T> Process<T>(NotionDatabaseQueryResult result)
        {
            var list = new List<T>();

            foreach (var row in result.Rows)
            {
                var newEntry = new T();
                var newEntryFields = newEntry.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in newEntryFields)
                {
                    // Tries to find the id matching the field name...
                    if (!row.DataLookup.ContainsKey(field.Name.Trim().ToLower())) continue;
                    
                    // Gets the property info assigned to that key...
                    var rowProperty = row.DataLookup[field.Name.Trim().ToLower()];
                    
                    // Tries to parse the data into the field type if possible...
                    rowProperty.TryConvertValueToFieldType(field, newEntry);
                }
                
                list.Add(newEntry);
            }
            
            return list;
        }
    }
}