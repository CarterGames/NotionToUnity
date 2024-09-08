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
using System.Reflection;
using UnityEngine;

namespace CarterGames.Standalone.NotionData
{   
    /// <summary>
    /// A base class for any Notion database data assets.
    /// </summary>
    /// <typeparam name="T">The type the data is storing.</typeparam>
    [Serializable]
    public abstract class NotionDataAsset<T> : DataAsset where T : new()
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#pragma warning disable
        [SerializeField, HideInInspector] private string linkToDatabase;     // Is used in editor space, so ignore the not used warning.
        [SerializeField, HideInInspector] private string databaseApiKey;     // Is used in editor space, so ignore the not used warning.
        [SerializeField, HideInInspector] private List<NotionSortProperty> sortProperties;
#pragma warning restore
        
        [SerializeField] private List<T> data;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The data stored on the asset.
        /// </summary>
        public List<T> Data => data;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Applies the data found to the asset.
        /// </summary>
        /// <param name="result">The resulting data downloaded to try and apply.</param>
        private void Apply(NotionDatabaseQueryResult result)
        {
            var list = new List<T>();


            foreach (var row in result.Rows)
            {
                var newEntry = new T();
                var newEntryFields = newEntry.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

                foreach (var field in newEntryFields)
                {
                    if (row.DataLookup.ContainsKey(field.Name.Trim().ToLower()))
                    {
                        var valueData = row.DataLookup[field.Name.Trim().ToLower()];
                        var fieldType = field.FieldType;
                        
                        if (fieldType.BaseType.FullName.Contains(typeof(NotionDataWrapper<>).Namespace + ".NotionDataWrapper"))
                        {
                            var instance = valueData.GetValueAs(fieldType);
                            field.SetValue(newEntry, instance);
                            
                            instance.GetType().BaseType.GetMethod("Assign", BindingFlags.NonPublic | BindingFlags.Instance)
                                ?.Invoke(instance, null);
                        }
                        else
                        {
                            field.SetValue(newEntry, valueData.GetValueAs(fieldType));
                        }
                    }
                }
                
                list.Add(newEntry);
            }
            
            data = list;
            PostDataDownloaded();
            
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
#endif
        }


        /// <summary>
        /// Override to run logic post download such as making edits to some data values or assigning others etc.
        /// </summary>
        protected virtual void PostDataDownloaded()
        { }
    }
}