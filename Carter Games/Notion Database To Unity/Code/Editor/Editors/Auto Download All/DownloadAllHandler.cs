/*
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CarterGames.Shared.NotionData;
using CarterGames.Shared.NotionData.Editor;
using CarterGames.NotionData.Filters;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles an editor window for downloading all data assets at once.
    /// </summary>
    public sealed class DownloadAllHandler : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static bool haltOnError = true;
        private static List<NdAsset> toProcess;
        private static int TotalToProcessed = 0;
        private static int TotalProcessed = 0;
        private static bool hasErrorOnDownload;
        private static List<NotionRequestError> silencedErrors;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static bool IsDownloadingAllAssets { get; private set; }
        
        public static float AllDownloadProgress01 => TotalProcessed / TotalToProcessed;
        
        private static NdAsset TargetAsset { get; set; }
        private static SerializedObject TargetAssetObject { get; set; }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Item
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [MenuItem("Tools/Carter Games/Notion Data/Update All Notion Data", priority = 21)]
        private static void DownloadAll()
        {
            haltOnError = true;
            TotalToProcessed = 0;
            TotalProcessed = 0;
            hasErrorOnDownload = false;
            silencedErrors = new List<NotionRequestError>();
            
            NdAssetIndexHandler.UpdateIndex();
            
            if (HasOpenInstances<DownloadAllHandler>())
            {
                FocusWindowIfItsOpen(typeof(DownloadAllHandler));
            }
            else
            {
                var window = GetWindow<DownloadAllHandler>(true, "Download All Notion Data");
                window.minSize = new Vector2(400, 150);
                window.maxSize = new Vector2(400, 150);
            }
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Unity Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private void OnGUI()
        {
            GUILayout.Space(7.5f);
            
            EditorGUILayout.HelpBox("Using this tool will auto download all Notion data assets found in the project with their current settings.", MessageType.Info);
            
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();
            
            haltOnError = EditorGUILayout.Toggle(new GUIContent("Halt download on error:"), haltOnError);
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            
            GUILayout.Space(5f);

            GUI.backgroundColor = Color.green;
            
            if (GUILayout.Button("Update All Notion Data Assets", GUILayout.Height(25f)))
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    EditorUtility.DisplayDialog("Download All Notion Data", "You cannot download data while offline.",
                        "Continue");
                    return;
                }

                toProcess = NotionDataAccessor.GetAllAssets().Where(t => t.GetType() != typeof(EditorOnlyNdAsset))
                    .ToList();
                
                TotalToProcessed = toProcess.Count;
                TotalProcessed = 0;
                hasErrorOnDownload = false;
                silencedErrors ??= new List<NotionRequestError>();
                silencedErrors.Clear();
                TargetAsset = null;

                IsDownloadingAllAssets = true;
                
                ProcessNextAsset();
            }
            
            GUI.backgroundColor = Color.white;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static void ProcessNextAsset()
        {
            if (TotalProcessed == TotalToProcessed)
            {
                OnAllDownloadCompleted();
                return;
            }
            
            try
            {
                TargetAsset = toProcess[TotalProcessed];
                TargetAssetObject = new SerializedObject(TargetAsset);

                if (TargetAssetObject.Fp("databaseApiKey") == null)
                {
                    TotalProcessed++;
                    ProcessNextAsset();
                    Debug.LogWarning($"No api key assigned to asset {TargetAssetObject.targetObject.name}, skipping it.");
                    return;
                }
                    
                var databaseId = TargetAssetObject.Fp("linkToDatabase").stringValue.Split('/').Last().Split('?').First();
                
                EditorUtility.DisplayProgressBar("Download All Notion Data", $"Downloading {TargetAsset.name}", AllDownloadProgress01);
                
                NotionFilterContainer filters = null;
                
                if (TargetAssetObject.GetType().BaseType!
                        .GetField("filters", BindingFlags.NonPublic | BindingFlags.Instance) != null)
                {
                    filters = (NotionFilterContainer) TargetAssetObject.GetType().BaseType!
                        .GetField("filters", BindingFlags.NonPublic | BindingFlags.Instance)
                        !.GetValue(TargetAssetObject.targetObject);
                }
                
                
                var requestData = new NotionRequestData(TargetAsset, databaseId, TargetAssetObject.Fp("databaseApiKey").stringValue, TargetAssetObject.Fp("sortProperties").ToSortPropertyArray(), filters, true);
                
                NotionApiRequestManager.RunRequest(requestData, OnAssetDownloadComplete, OnAssetDownloadComplete);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                EditorUtility.ClearProgressBar();
                IsDownloadingAllAssets = false;
                throw;
            }
        }
        

        private static void OnAssetDownloadComplete(NotionRequestResult result)
        {
            NotionDatabaseQueryResult queryResult;
            
            try
            {
                queryResult = NotionDownloadParser.Parse(result.Data);
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
                TargetAsset.GetType().BaseType.GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.Invoke(TargetAssetObject.targetObject, new object[] { queryResult });
                
                EditorUtility.ClearProgressBar();
                
                if (!result.SilentResponse)
                {
                    EditorUtility.DisplayDialog("Notion Data Download", "Download completed & parsed successfully", "Continue");
                }
            }
            catch (Exception e)
            {
                if (!haltOnError)
                {
                    hasErrorOnDownload = true;
                    silencedErrors.Add(new NotionRequestError(TargetAsset, new JObject()
                    {
                        ["message"] = $"Applying values from parsed data failed. See message below for more information: {e}"
                    }));
                }
                else
                {
                    Debug.LogError("Applying values from parsed data failed. See message below for more information:");
                    Debug.LogError(e);
                
                    EditorUtility.ClearProgressBar();
                
                    if (!result.SilentResponse)
                    {
                        EditorUtility.DisplayDialog("Notion Data Download", "Download completed, but failed to parse all properties. See console for more information.", "Continue");
                    }
                }
            }

            TargetAssetObject.ApplyModifiedProperties();
            TargetAssetObject.Update();
            
            TotalProcessed++;
            ProcessNextAsset();
        }
        
        
        private static void OnAssetDownloadComplete(NotionRequestError error)
        {
            TotalProcessed++;
            
            if (!haltOnError)
            {
                hasErrorOnDownload = true;
                silencedErrors.Add(error);
                
                TargetAssetObject.ApplyModifiedProperties();
                TargetAssetObject.Update();
                
                ProcessNextAsset();
                return;
            }
            
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Download All Notion Data", $"Download failed due to an error:\n{error.Asset.name}\n{error.Error}\n{error.Message}", "Continue");
            
            TargetAssetObject.ApplyModifiedProperties();
            TargetAssetObject.Update();
        }


        private static void OnAllDownloadCompleted()
        {
            EditorUtility.ClearProgressBar();
            IsDownloadingAllAssets = false;
            
            if (hasErrorOnDownload)
            {
                if (EditorUtility.DisplayDialog("Download All Notion Data",
                        "Download completed with errors.\nSee console for errors.", "Continue"))
                {
                    foreach (var error in silencedErrors)
                    {
                        Debug.LogError($"Failed to download an asset: {error.Asset.name} | {error.Error} | {error.Message}", error.Asset);
                    }
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Download All Notion Data", "Download completed.", "Continue");
            }
        }


        public static void SetProgressMessage(string message)
        {
            EditorUtility.DisplayProgressBar("Download All Notion Data", message, AllDownloadProgress01);
        }
    }
}