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
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
    /// <summary>
    /// Handles an editor window for downloading all data assets at once.
    /// </summary>
    public sealed class DownloadAllHandler : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static bool haltOnDownload = true;
        private static List<DataAsset> toProcess;
        private static int TotalToProcessed = 0;
        private static int TotalProcessed = 0;
        private static bool hasErrorOnDownload;
        private static List<NotionRequestError> silencedErrors;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Item
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        [MenuItem("Tools/Carter Games/Standalone/Notion Data/Update Data", priority = 21)]
        private static void DownloadAll()
        {
            haltOnDownload = true;
            TotalToProcessed = 0;
            TotalProcessed = 0;
            hasErrorOnDownload = false;
            silencedErrors = new List<NotionRequestError>();
            
            DataAssetIndexHandler.UpdateIndex();
            
            if (HasOpenInstances<DownloadAllHandler>())
            {
                FocusWindowIfItsOpen(typeof(DownloadAllHandler));
            }
            else
            {
                var window = CreateWindow<DownloadAllHandler>("Download Notion Data");
                window.maxSize = new Vector2(400, 400);
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
            
            haltOnDownload = EditorGUILayout.Toggle(new GUIContent("Halt download on error:"), haltOnDownload);
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
            
            
            GUILayout.Space(5f);

            GUI.backgroundColor = Color.green;
            
            if (GUILayout.Button("Update All Notion Data Assets"))
            {
                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    EditorUtility.DisplayDialog("Standalone Notion Data", "You cannot download data while offline.",
                        "Continue");
                    return;
                }
                
                toProcess = DataAccess.GetAllAssets();
                TotalToProcessed = toProcess.Count;
                TotalProcessed = 0;
                hasErrorOnDownload = false;
                silencedErrors ??= new List<NotionRequestError>();
                silencedErrors.Clear();
                
                ProcessNextAsset();
            }
            
            GUI.backgroundColor = Color.white;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static void ProcessNextAsset()
        {
            if (toProcess.Count <= 0)
            {
                OnAllDownloadCompleted();
                return;
            }
            
            var asset = toProcess.First();
            toProcess.RemoveAt(0);
            
            var assetObject = new SerializedObject(asset);

            if (assetObject.Fp("databaseApiKey") == null)
            {
                ProcessNextAsset();
                return;
            }
            
            TotalProcessed++;
                    
            var databaseId = assetObject.Fp("linkToDatabase").stringValue.Split('/').Last().Split('?').First();
                
            NotionApiRequestHandler.DataReceived.Remove(OnAssetDownloadComplete);
            NotionApiRequestHandler.DataReceived.Add(OnAssetDownloadComplete);
                
            NotionApiRequestHandler.RequestError.Remove(OnAssetDownloadComplete);
            NotionApiRequestHandler.RequestError.Add(OnAssetDownloadComplete);
            
            EditorUtility.DisplayProgressBar("Standalone Notion Data", $"Downloading {asset.name}", (float) TotalProcessed / TotalToProcessed);
            
            NotionApiRequestHandler.ResetRequestData();
            
            var requestData = new NotionRequestData(asset, databaseId, assetObject.Fp("databaseApiKey").stringValue, assetObject.Fp("sortProperties").ToSortPropertyArray(), true);
            NotionApiRequestHandler.WebRequestPostWithAuth(requestData);
        }
        

        private static void OnAssetDownloadComplete(NotionRequestResult result)
        {
            ProcessNextAsset();
        }
        
        
        private static void OnAssetDownloadComplete(NotionRequestError error)
        {
            if (!haltOnDownload)
            {
                hasErrorOnDownload = true;
                silencedErrors.Add(error);
                ProcessNextAsset();
                return;
            }
            
            EditorUtility.ClearProgressBar();
            EditorUtility.DisplayDialog("Standalone Notion Data", $"Download failed due to an error:\n{error.Asset.name}\n{error.Error}\n{error.Message}", "Continue");
        }


        private static void OnAllDownloadCompleted()
        {
            EditorUtility.ClearProgressBar();
            
            if (hasErrorOnDownload)
            {
                if (EditorUtility.DisplayDialog("Standalone Notion Data",
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
                EditorUtility.DisplayDialog("Standalone Notion Data", "Download completed.", "Continue");
            }
        }
    }
}