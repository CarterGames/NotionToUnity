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
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;
using UnityEngine;

namespace CarterGames.NotionData
{   
    /// <summary>
    /// A base class for any Notion database data assets.
    /// </summary>
    /// <typeparam name="T">The type the data is storing.</typeparam>
    [Serializable]
    public abstract class NotionDataAsset<T> : NdAsset where T : new()
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#pragma warning disable
        [SerializeField, HideInInspector] private string linkToDatabase;
        [SerializeField, HideInInspector] private string databaseApiKey;
        [SerializeField] private NotionFilterContainer filters;
        [SerializeField] private List<NotionSortProperty> sortProperties;
        [SerializeField] protected AssemblyClassDef processor = typeof(NotionDatabaseProcessorStandard);
#pragma warning restore
        
        [SerializeField] private string variantId;
        [SerializeField] private List<T> data;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The id to define this asset.
        /// </summary>
        public override string VariantId => string.IsNullOrEmpty(variantId) ? name : variantId;
        
        
        /// <summary>
        /// The data stored on the asset.
        /// </summary>
        public List<T> Data => data;

        
#if UNITY_EDITOR
        /// <summary>
        /// Defines the parser used to apply the data to the asset from Notion.
        /// </summary>
        protected virtual NotionDatabaseProcessor DatabaseProcessor => processor.GetDefinedType<NotionDatabaseProcessor>();
#endif
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Applies the data found to the asset.
        /// </summary>
        /// <param name="result">The resulting data downloaded to try and apply.</param>
        private void Apply(NotionDatabaseQueryResult result)
        {
#if UNITY_EDITOR
            data = DatabaseProcessor.Process<T>(result).Cast<T>().ToList();
            PostDataDownloaded();
            

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