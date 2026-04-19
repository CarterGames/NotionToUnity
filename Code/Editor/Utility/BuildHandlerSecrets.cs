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

using CarterGames.Shared.NotionData;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles hiding secret keys/database url's in builds by removing them pre-build and re-applying them post build.
    /// </summary>
    public sealed class BuildHandlerSecrets : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public int callbackOrder { get; }
        
        
        public void OnPreprocessBuild(BuildReport report)
        {
            foreach (var asset in NotionDataAccessor.GetAllAssets())
            {
                var assetObject = new SerializedObject(asset);
            
                if (assetObject.Fp("databaseApiKey") == null) continue;
                
                EditorPrefs.SetString(assetObject.targetObject.GetInstanceID().ToString() + "database_url", assetObject.Fp("linkToDatabase").stringValue);
                EditorPrefs.SetString(assetObject.targetObject.GetInstanceID().ToString() + "secret_key", assetObject.Fp("databaseApiKey").stringValue);
                
                assetObject.Fp("linkToDatabase").stringValue = string.Empty;
                assetObject.Fp("databaseApiKey").stringValue = string.Empty;
                assetObject.ApplyModifiedProperties();
            }
        }
        
        
        public void OnPostprocessBuild(BuildReport report)
        {
            foreach (var asset in NotionDataAccessor.GetAllAssets())
            {
                var assetObject = new SerializedObject(asset);
            
                if (assetObject.Fp("databaseApiKey") == null) continue;
                
                if (EditorPrefs.HasKey(assetObject.targetObject.GetInstanceID().ToString() + "database_url"))
                {
                    assetObject.Fp("linkToDatabase").stringValue = EditorPrefs.GetString(assetObject.targetObject.GetInstanceID().ToString() + "database_url");
                }
                
                if (EditorPrefs.HasKey(assetObject.targetObject.GetInstanceID().ToString() + "secret_key"))
                {
                    assetObject.Fp("databaseApiKey").stringValue = EditorPrefs.GetString(assetObject.targetObject.GetInstanceID().ToString() + "secret_key");
                }
                
                assetObject.ApplyModifiedProperties();
                
                EditorPrefs.DeleteKey(assetObject.targetObject.GetInstanceID().ToString() + "database_url");
                EditorPrefs.DeleteKey(assetObject.targetObject.GetInstanceID().ToString() + "secret_key");
            }
        }
    }
}