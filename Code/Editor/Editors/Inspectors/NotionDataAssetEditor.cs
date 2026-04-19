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
using System.Linq;
using System.Reflection;
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;
using CarterGames.Shared.NotionData.Editor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    [CustomEditor(typeof(NotionDataAsset<>), true)]
    public sealed class NotionDataAssetEditor : UnityEditor.Editor
    {
        private static readonly GUIContent ApiKey = new GUIContent("API key:", "The API key to use with the download.");
        private static readonly GUIContent DatabaseLink = new GUIContent("Link to database:", "The link to the page with the database on that you want to download.");
        
        
        public override void OnInspectorGUI()
        {
            EditorGUILayout.Space(5f);
            
            RenderNotionSettings();
            
            EditorGUILayout.Space(1.5f);
            GeneralUtilEditor.DrawHorizontalGUILine();

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.Fp("m_Script"));
            EditorGUI.EndDisabledGroup();

            DrawPropertiesExcluding(serializedObject, "sortProperties", "filters", "m_Script", "processor");
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
        

        private void RenderNotionSettings()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            EditorGUILayout.Space(1.5f);
            
            EditorGUILayout.LabelField("Notion Database Settings", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();
            
            EditorGUILayout.PropertyField(serializedObject.Fp("linkToDatabase"), DatabaseLink);
            EditorGUILayout.PropertyField(serializedObject.Fp("databaseApiKey"), ApiKey);

            if (string.IsNullOrEmpty(serializedObject.Fp("processor").Fpr("type").stringValue))
            {
                GUI.backgroundColor = Color.green;
                
                if (GUILayout.Button("Select Processor"))
                {
                    SearchProviderDatabaseProcessors.GetProvider().SelectionMade.Remove(OnSelectionMade);
                    SearchProviderDatabaseProcessors.GetProvider().SelectionMade.Add(OnSelectionMade);
                    
                    SearchProviderDatabaseProcessors.GetProvider().Open();
                }
                
                GUI.backgroundColor = Color.white;
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Processor:", serializedObject.Fp("processor").Fpr("type").stringValue.Split('.').Last().Replace("NotionDatabaseProcessor", string.Empty));
                EditorGUI.EndDisabledGroup();
                
                GUI.backgroundColor = Color.yellow;
                
                if (GUILayout.Button("Edit", GUILayout.Width(45)))
                {
                    SearchProviderDatabaseProcessors.GetProvider().SelectionMade.Remove(OnSelectionMade);
                    SearchProviderDatabaseProcessors.GetProvider().SelectionMade.Add(OnSelectionMade);
                    
                    SearchProviderDatabaseProcessors.GetProvider().Open();
                }
                
                GUI.backgroundColor = Color.red;

                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    serializedObject.Fp("processor").Fpr("assembly").stringValue = string.Empty;
                    serializedObject.Fp("processor").Fpr("type").stringValue = string.Empty;

                    serializedObject.ApplyModifiedProperties();
                    serializedObject.Update();
                }
                                
                GUI.backgroundColor = Color.white;
                
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();

            
            var filterTotal = ((NotionFilterContainer) target.GetType().BaseType!
                .GetField("filters", BindingFlags.NonPublic | BindingFlags.Instance)
                !.GetValue(serializedObject.targetObject)).TotalFilters;
            
            if (GUILayout.Button($"Filters ({filterTotal})"))
            {
                EditorWindowFilterGUI.OpenWindow(serializedObject);
            }
            
            if (GUILayout.Button($"Sorting ({serializedObject.Fp("sortProperties").arraySize})"))
            {
                SortPropertiesWindow.OpenWindow(serializedObject);
            }
            
            EditorGUILayout.EndHorizontal();
            
            EditorGUI.BeginDisabledGroup(
                !NotionSecretKeyValidator.IsKeyValid(serializedObject.Fp("databaseApiKey").stringValue) ||
                string.IsNullOrEmpty(serializedObject.Fp("linkToDatabase").stringValue) ||
                string.IsNullOrEmpty(serializedObject.Fp("processor").Fpr("type").stringValue));


            GUILayout.Space(1.5f);
            
            GUI.backgroundColor = Color.green;
            
            if (GUILayout.Button("Download Data", GUILayout.Height(22.5f)))
            {
                // Add to index if not present...
                if (!ScriptableRef.GetAssetDef<NotionDataAssetIndex>().AssetRef.Lookup[serializedObject.targetObject.GetType().ToString()].Contains((NdAsset) serializedObject.targetObject))
                {
                    NdAssetIndexHandler.UpdateIndex();
                }
                
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    EditorUtility.DisplayDialog("Notion Data", "You cannot download data while offline.",
                        "Continue");
                    goto EndMarker;
                }
                
                // Do download stuff...
                var databaseId = serializedObject.Fp("linkToDatabase").stringValue.Split('/').Last().Split('?').First();
                
                var sorts = serializedObject.Fp("sortProperties").ToSortPropertyArray();
                var filters = (NotionFilterContainer) target.GetType().BaseType!
                    .GetField("filters", BindingFlags.NonPublic | BindingFlags.Instance)
                    !.GetValue(serializedObject.targetObject);
                
                var requestData = new NotionRequestData((NdAsset) serializedObject.targetObject, databaseId, serializedObject.Fp("databaseApiKey").stringValue, sorts, filters);

                NotionApiRequestManager.RunRequest(requestData, OnDataReceived, OnErrorReceived);
                EditorGUILayout.BeginVertical();
            }
            
            GUI.backgroundColor = Color.white;

            EndMarker: ;
            
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            return;

            void OnSelectionMade(SearchTreeEntry entry)
            {
                serializedObject.Fp("processor").Fpr("assembly").stringValue = ((AssemblyClassDef)entry.userData).Assembly;
                serializedObject.Fp("processor").Fpr("type").stringValue = ((AssemblyClassDef)entry.userData).Type;

                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
            }
        }


        private void OnDataReceived(NotionRequestResult data)
        {
            NotionDatabaseQueryResult queryResult;
            
            try
            {
                queryResult = NotionDownloadParser.Parse(data.Data);
            }
            catch (Exception e)
            {
                Debug.LogError("Parsing of downloaded data failed. See message below for more information:");
                Debug.LogError(e);
                EditorUtility.ClearProgressBar();
                throw;
            }
            
            try
            {
                target.GetType().BaseType.GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(serializedObject.targetObject, new object[] { queryResult });
                
                EditorUtility.ClearProgressBar();
                
                if (!data.SilentResponse)
                {
                    EditorUtility.DisplayDialog("Notion Data Download", "Download completed & parsed successfully", "Continue");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Applying values from parsed data failed. See message below for more information:");
                Debug.LogError(e);
                
                EditorUtility.ClearProgressBar();
                
                if (!data.SilentResponse)
                {
                    EditorUtility.DisplayDialog("Notion Data Download", "Download completed, but failed to parse all properties. See console for more information.", "Continue");
                }
            }

            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }


        private void OnErrorReceived(NotionRequestError error)
        {
            EditorUtility.DisplayDialog("Notion Data Download", $"Download failed ({error.Error}):\n{error.Message}", "Continue");
            
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }
    }
}