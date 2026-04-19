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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CarterGames.Shared.NotionData
{
    /// <summary>
    /// Handles accessing the scriptable object data assets for this asset.
    /// </summary>
    public static class NotionDataAccessor
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
     
        private const string IndexPath = "[Notion Data] ND Asset Index";
        
        // A cache of all the assets found...
        private static NotionDataAssetIndex indexCache;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets all the assets from the asset.
        /// </summary>
        private static NotionDataAssetIndex Index
        {
            get
            {
                if (indexCache != null) return indexCache;
                indexCache = (NotionDataAssetIndex) Resources.Load(IndexPath, typeof(NotionDataAssetIndex));
                return indexCache;
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Gets the asset requested.
        /// </summary>
        /// <typeparam name="T">The asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static T GetAsset<T>() where T : NdAsset
        {
            if (Index.Lookup.ContainsKey(typeof(T).ToString()))
            {
                return (T)Index.Lookup[typeof(T).ToString()][0];
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets the asset requested.
        /// </summary>
        /// <typeparam name="T">The asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static T GetAsset<T>(string variantId) where T : NdAsset
        {
            if (Index.Lookup.ContainsKey(typeof(T).ToString()))
            {
                return (T)Index.Lookup[typeof(T).ToString()].FirstOrDefault(t => t.VariantId.Equals(variantId));
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets the asset requested.
        /// </summary>
        /// <typeparam name="T">The asset to get.</typeparam>
        /// <returns>The asset if it exists.</returns>
        public static List<T> GetAssets<T>() where T : NdAsset
        {
            if (Index.Lookup.ContainsKey(typeof(T).ToString()))
            {
                return Index.Lookup[typeof(T).ToString()].Cast<T>().ToList();
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets all the data assets stored in the index.
        /// </summary>
        /// <returns>All the assets stored.</returns>
        public static List<NdAsset> GetAllAssets()
        {
            var list = new List<NdAsset>();
            
            foreach (var entry in Index.Lookup.Values)
            {
                list.AddRange(entry);
            }

            return list;
        }
    }
}