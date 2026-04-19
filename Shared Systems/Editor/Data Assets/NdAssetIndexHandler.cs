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
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CarterGames.Shared.NotionData.Editor
{
    /// <summary>
    /// Handles the setup of the asset index for runtime references to scriptable objects used for the asset.
    /// </summary>
    public sealed class NdAssetIndexHandler : IPreprocessBuildWithReport, IAssetEditorInitialize
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static readonly string AssetFilter = $"t:{nameof(NdAsset)}";

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IAssetEditorInitialize Implementation
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Defines the order that this initializer run at.
        /// </summary>
        public int InitializeOrder => 1;

        
        /// <summary>
        /// Runs when the asset initialize flow is used.
        /// </summary>
        public void OnEditorInitialized()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
            
            UpdateIndex();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   IPreprocessBuildWithReport Implementation
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The order this script is processed in, in this case its the default.
        /// </summary>
        public int callbackOrder => 0;
        
        
        /// <summary>
        /// Runs before a build is executed.
        /// </summary>
        /// <param name="report">The report about the build (I don't need it, but its a param for the method).</param>
        public void OnPreprocessBuild(BuildReport report)
        {
            UpdateIndex();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Initializes the event subscription needed for this to work in editor.
        /// </summary>
        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            EditorApplication.update -= OnEditorUpdate;
            EditorApplication.update += OnEditorUpdate;
        }


        /// <summary>
        /// Runs when the editor has updated.
        /// </summary>
        private static void OnEditorUpdate()
        {
            // If the user is about to enter play-mode, update the index, otherwise leave it be. 
            if (!EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPlaying) return;
            UpdateIndex();
        }


        /// <summary>
        /// Updates the index with all the asset scriptable objects in the project.
        /// </summary>
        [MenuItem("Tools/Carter Games/Notion Data/Update Asset Index", priority = 101)]
        public static void UpdateIndex()
        {
            var foundAssets = new List<NdAsset>();
            var asset = AssetDatabase.FindAssets(AssetFilter, null);
            
            if (asset == null || asset.Length <= 0) return;

            foreach (var assetInstance in asset)
            {
                var assetPath = AssetDatabase.GUIDToAssetPath(assetInstance);
                var assetObj = (NdAsset) AssetDatabase.LoadAssetAtPath(assetPath, typeof(NdAsset));
                
                // Doesn't include editor only or the index itself.
                if (assetObj == null) continue;
                if (assetObj is NotionDataAssetIndex) continue;
                if (assetObj is EditorOnlyNdAsset) continue;
                foundAssets.Add((NdAsset) AssetDatabase.LoadAssetAtPath(assetPath, typeof(NdAsset)));
            }

            var indexProp = ScriptableRef.GetAssetDef<NotionDataAssetIndex>().ObjectRef;
            
            RemoveNullReferences(indexProp);
            UpdateIndexReferences(foundAssets ,indexProp);
            
            indexProp.ApplyModifiedProperties();
            indexProp.Update();
        }


        private static void RemoveNullReferences(SerializedObject indexProp)
        {
            for (var i = 0; i < indexProp.Fp("assets").Fpr("list").arraySize; i++)
            {
                var entry = indexProp.Fp("assets").Fpr("list").GetIndex(i);
                var jIndexAdjustment = 0;

                for (var j = 0; j < entry.Fpr("value").arraySize; j++)
                {
                    if (entry.Fpr("value").GetIndex(j - jIndexAdjustment).objectReferenceValue != null) continue;
                    entry.Fpr("value").DeleteIndex(j);
                    jIndexAdjustment++;
                }
            }
        }


        private static void UpdateIndexReferences(IReadOnlyList<NdAsset> foundAssets, SerializedObject indexProp)
        {
            indexProp.Fp("assets").Fpr("list").ClearArray();
            
            for (var i = 0; i < foundAssets.Count; i++)
            {
                for (var j = 0; j < indexProp.Fp("assets").Fpr("list").arraySize; j++)
                {
                    var entry = indexProp.Fp("assets").Fpr("list").GetIndex(j);
                    
                    if (entry.Fpr("key").stringValue.Equals(foundAssets[i].GetType().ToString()))
                    {
                        for (var k = 0; k < entry.Fpr("value").arraySize; k++)
                        {
                            if (entry.Fpr("value").GetIndex(k).objectReferenceValue == foundAssets[i]) goto AlreadyExists;
                        }
                        
                        entry.Fpr("value").InsertIndex(entry.Fpr("value").arraySize);
                        entry.Fpr("value").GetIndex(entry.Fpr("value").arraySize - 1).objectReferenceValue = foundAssets[i];
                        goto AlreadyExists;
                    }
                }
                
                indexProp.Fp("assets").Fpr("list").InsertIndex(indexProp.Fp("assets").Fpr("list").arraySize);
                indexProp.Fp("assets").Fpr("list").GetIndex(indexProp.Fp("assets").Fpr("list").arraySize - 1).Fpr("key").stringValue = foundAssets[i].GetType().ToString();
                
                if (indexProp.Fp("assets").Fpr("list").GetIndex(indexProp.Fp("assets").Fpr("list").arraySize - 1).Fpr("value").arraySize > 0)
                {
                    indexProp.Fp("assets").Fpr("list").GetIndex(indexProp.Fp("assets").Fpr("list").arraySize - 1)
                        .Fpr("value").ClearArray();
                }
                
                indexProp.Fp("assets").Fpr("list").GetIndex(indexProp.Fp("assets").Fpr("list").arraySize - 1).Fpr("value").InsertIndex(0);
                indexProp.Fp("assets").Fpr("list").GetIndex(indexProp.Fp("assets").Fpr("list").arraySize - 1)
                    .Fpr("value").GetIndex(0).objectReferenceValue = foundAssets[i];

                AlreadyExists: ;
            } 
        }
    }
}