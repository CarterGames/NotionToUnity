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


using System.Collections.Generic;
using CarterGames.Shared.NotionData.Editor;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CarterGames.NotionData.Editor
{
    public class NotionApiRequestHandler2022 : NotionApiRequestHandlerBase
    {
        private static NotionRequestData requestData;
        
        public override NotionApiReleaseVersion ApiVersion => NotionApiReleaseVersion.NotionApi20220628;


        private string DatabaseUrl(string databaseId) => $"https://api.notion.com/v1/databases/{databaseId}/query";
        
        
        private float CurrentStep { get; set; } = 0;
        private float Steps { get; set; } = 2;
        private float Progress01 => CurrentStep / Steps;
        
        
        private UnityWebRequest PrepareInitialRequest(NotionWebRequestData webRequestData)
        {
            UnityWebRequest request;

            if (webRequestData.Sorting == null && webRequestData.Filters == null)
            {
                request = UnityWebRequest.Post(webRequestData.Url, string.Empty);
            }
            else
            {
                var body = new JObject();

                if (webRequestData.Sorting != null)
                {
                    if (webRequestData.Sorting.Length > 0)
                    {
                        body["sorts"] = webRequestData.Sorting.ToJsonArray();
                    }
                }
                
                if (webRequestData.Filters != null)
                {
                    if (webRequestData.Filters.TotalFilters > 0)
                    {
                        body["filter"] = webRequestData.Filters.ToFilterJson();
                    }
                }

                request = UnityWebRequest.Put(webRequestData.Url, body.ToString());
                request.method = "POST";
            }
            
            request.SetRequestHeader("Authorization", $"Bearer {webRequestData.ApiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", ApiVersion.ToVersionString());
            request.timeout = ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.DownloadTimeout;
            
            return request;
        }


        private UnityWebRequest PrepareRepeatRequest(NotionWebRequestData webRequestData)
        {
            if (webRequestData.Sorting != null)
            {
                if (webRequestData.Sorting .Length > 0)
                {
                    webRequestData.Body["sorts"] = webRequestData.Sorting .ToJsonArray();
                }
            }
                
            if (webRequestData.Filters != null)
            {
                if (webRequestData.Filters.TotalFilters > 0)
                {
                    webRequestData.Body["filter"] = webRequestData.Filters.ToFilterJson();
                }
            }
            
            var request = UnityWebRequest.Put(webRequestData.Url, webRequestData.Body.ToString());
            
            request.method = "POST";
            request.SetRequestHeader("Authorization", $"Bearer {webRequestData.ApiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", ApiVersion.ToVersionString());
            request.timeout = ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.DownloadTimeout;
            
            return request;
        }
        
        
        public override void StartDownload(NotionRequestData data)
        {
            CurrentStep = 1;
            Steps = 2;
            WebRequestPostWithAuth(data);
        }


        private void WebRequestPostWithAuth(NotionRequestData data)
        {
            requestData = data;
            
            if (!NotionSecretKeyValidator.IsKeyValid(requestData.ApiKey))
            {
                if (EditorUtility.DisplayDialog("Notion Data", "Api key for database download is invalid.",
                        "Continue"))
                {
                    Debug.LogError(
                        "Notion API passed in is not valid, please double check it before sending another request.");
                }
                
                return;
            }
            
            var request = PrepareInitialRequest(new NotionWebRequestData(DatabaseUrl(requestData.DatabaseId), requestData.ApiKey, requestData.Sorts, requestData.Filter));
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            if (DownloadAllHandler.IsDownloadingAllAssets)
            {
                DownloadAllHandler.SetProgressMessage($"Downloading data from database {requestData.DatabaseId} for {requestData.RequestingAsset.name}.");
            }
            else
            {
                EditorUtility.DisplayProgressBar("Notion Data", $"Downloading data from database {requestData.DatabaseId}.", Progress01);
            }
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    if (!DownloadAllHandler.IsDownloadingAllAssets)
                    {
                        EditorUtility.ClearProgressBar();
                    }
                    
                    RequestError.Raise(new NotionRequestError(requestData.RequestingAsset, JObject.Parse(request.downloadHandler.text)));
                    return;
                }

                OnDownloadReceived(request.downloadHandler.text);
            };
        }
        
        
        /// <summary>
        /// Runs the web request to get the Notion database requested.
        /// </summary>
        /// <param name="data">The data for the request.</param>
        /// <param name="bodyData">The body to send with the API call.</param>
        private void WebRequestPostWithAuth(NotionRequestData data, JObject bodyData)
        {
            var request = PrepareRepeatRequest(new NotionWebRequestData(DatabaseUrl(data.DatabaseId), data.ApiKey, bodyData, data.Sorts, data.Filter));
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            if (data.ShowResponseDialogue)
            {
                if (DownloadAllHandler.IsDownloadingAllAssets)
                {
                    DownloadAllHandler.SetProgressMessage($"Downloading data from database {requestData.DatabaseId} for {requestData.RequestingAsset.name}.");
                }
                else
                {
                    EditorUtility.DisplayProgressBar("Notion Data", $"Downloading data from database {requestData.DatabaseId}", Progress01);
                }
            }
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    if (!DownloadAllHandler.IsDownloadingAllAssets)
                    {
                        EditorUtility.ClearProgressBar();
                    }
                    
                    RequestError.Raise(new NotionRequestError(data.RequestingAsset, JObject.Parse(request.downloadHandler.text)));
                    return;
                }

                OnDownloadReceived(request.downloadHandler.text);
            };
        }
        

        private void OnDownloadReceived(string data)
        {
            var resultData = new List<IDictionary<string, JToken>>();
            var json = JObject.Parse(data);
            var resultsArray = JArray.FromObject(json["results"]);
            
            foreach (var entry in resultsArray)
            {
                resultData.Add(entry["properties"].Value<IDictionary<string, JToken>>());
            }
            
            requestData.AppendResultData(resultData);
            
            // Does another call as there is more data to download still...
            if (json["has_more"].Value<bool>())
            {
                WebRequestPostWithAuth(requestData, new JObject()
                {
                    ["start_cursor"] = json["next_cursor"]
                });
                
                return;
            }
            
            CurrentStep++;
            
            if (DownloadAllHandler.IsDownloadingAllAssets)
            {
                DownloadAllHandler.SetProgressMessage($"Parsing downloaded data to {requestData.RequestingAsset.name}.");
            }
            else
            {
                EditorUtility.DisplayProgressBar("Notion Data", "Parsing downloaded data.", Progress01);
            }
            
            DataReceived.Raise(requestData.ResultData);
        }
    }
}