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
using System.IO;
using System.Linq;
using CarterGames.Shared.NotionData.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles finding assets in the project in editor space and creating/referencing/caching them for use.
    /// </summary>
    public static class FileEditorUtil
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Getter Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets a script file in the asset.
        /// </summary>
        /// <param name="assetPath">The path to the script.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetScriptInAsset<T>(string assetPath, params string[] pathContains)
        {
            string path = string.Empty;
                
            foreach (var scriptFound in AssetDatabase.FindAssets($"t:Script {nameof(T)}"))
            {
                path = AssetDatabase.GUIDToAssetPath(scriptFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(scriptFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets a asset file in the asset.
        /// </summary>
        /// <param name="filter">The filter to check.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetAssetInstance<T>(string filter, params string[] pathContains)
        {
            string path = string.Empty;
            
            foreach (var assetFound in AssetDatabase.FindAssets(filter, null))
            {
                path = AssetDatabase.GUIDToAssetPath(assetFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(assetFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Gets a asset file in the asset.
        /// </summary>
        /// <param name="filter">The filter to check.</param>
        /// <param name="assetPath">The path to check.</param>
        /// <param name="pathContains">Parts of a string the path should contain.</param>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The found file as an object if found successfully.</returns>
        public static object GetAssetInstance<T>(string filter, string assetPath, params string[] pathContains)
        {
            if (AssetDatabase.AssetPathToGUID(assetPath).Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath(assetPath, typeof(T));
            }
            
            string path = string.Empty;
            
            foreach (var assetFound in AssetDatabase.FindAssets(filter, null))
            {
                path = AssetDatabase.GUIDToAssetPath(assetFound);

                foreach (var containCheck in pathContains)
                {
                    if (!path.Contains(containCheck)) goto Loop;
                }
                
                path = AssetDatabase.GUIDToAssetPath(assetFound);
                return AssetDatabase.LoadAssetAtPath(path, typeof(T));
                Loop: ;
            }

            return null;
        }
        
        
        /// <summary>
        /// Does the traditional get or assign cache method but with the new get asset instance variant.
        /// </summary>
        /// <param name="cache">The cache to update.</param>
        /// <param name="filter">The filter to search for.</param>
        /// <param name="pathContains">The strings to match in the path for the asset.</param>
        /// <typeparam name="T">The type to get.</typeparam>
        /// <returns>The updated cache.</returns>
        public static T GetOrAssignCache<T>(ref T cache, string filter, params string[] pathContains)
        {
            if (cache != null) return cache;
            cache = (T) GetAssetInstance<T>(filter, pathContains);
            return cache;
        }
        

        /// <summary>
        /// Creates a scriptable object or assigns the cache to an existing instance if one is found.
        /// </summary>
        /// <param name="cache">The cache to check.</param>
        /// <param name="filter">The filter to use.</param>
        /// <param name="path">The path to create the asset to if not found.</param>
        /// <param name="pathContains">Any string that should be in the path to make sure its the right asset.</param>
        /// <typeparam name="T">The type to check for.</typeparam>
        /// <returns>The found or created asset.</returns>
        public static T CreateSoGetOrAssignAssetCache<T>(ref T cache, string filter, string path, params string[] pathContains) where T : ScriptableObject
        {
            if (cache != null) return cache;

            cache = (T)GetAssetInstance<T>(filter, path, pathContains);

            if (cache == null)
            {
                cache = CreateScriptableObject<T>(path);
            }
            
            NdAssetIndexHandler.UpdateIndex();

            return cache;
        }
        
        
        /// <summary>
        /// Creates, gets or assigns a serialized object reference.
        /// </summary>
        /// <param name="cache">The cache to assign to.</param>
        /// <param name="reference">The reference to set from.,</param>
        /// <typeparam name="T">The type to reference.</typeparam>
        /// <returns>The updated cache.</returns>
        public static SerializedObject CreateGetOrAssignSerializedObjectCache<T>(ref SerializedObject cache, T reference)
        {
            if (cache != null && cache.targetObject != null) return cache;
            cache = new SerializedObject(reference as Object);
            return cache;
        }


        /// <summary>
        /// Creates a scriptable object of the type entered when called.
        /// </summary>
        /// <param name="path">The path to create the new asset at.</param>
        /// <typeparam name="T">The type to make.</typeparam>
        /// <returns>The newly created asset.</returns>
        private static T CreateScriptableObject<T>(string path) where T : ScriptableObject
        {
            var instance = ScriptableObject.CreateInstance(typeof(T));

            CreateToDirectory(path);

            AssetDatabase.CreateAsset(instance, path);
            AssetDatabase.Refresh();

            return (T)instance;
        }
        
        
        /// <summary>
        /// Creates all the folders to a path if they don't exist already.
        /// </summary>
        /// <param name="path">The path to create to.</param>
        public static void CreateToDirectory(string path)
        {
            var currentPath = string.Empty;
            var split = path.Split('/');

            for (var i = 0; i < path.Split('/').Length; i++)
            {
                var element = path.Split('/')[i];
                currentPath += element + "/";

                if (i.Equals(split.Length - 1))continue;
                if (Directory.Exists(currentPath))continue;
                
                Directory.CreateDirectory(currentPath);
            }
        }
        
        
        /// <summary>
        /// Deletes a directory and any assets within when called.
        /// </summary>
        /// <param name="path">The path to delete.</param>
        public static void DeleteDirectoryAndContents(string path)
        {
            foreach (var file in Directory.GetFiles(path).ToList())
            {
                AssetDatabase.DeleteAsset(file);
            }

            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}