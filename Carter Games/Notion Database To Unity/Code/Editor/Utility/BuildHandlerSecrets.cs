﻿/*
 * Copyright (c) 2025 Carter Games
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