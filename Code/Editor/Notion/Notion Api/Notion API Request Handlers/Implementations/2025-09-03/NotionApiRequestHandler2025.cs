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
using CarterGames.Shared.NotionData.Editor;
using Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CarterGames.NotionData.Editor
{
    public class NotionApiRequestHandler2025 : NotionApiRequestHandlerBase
    {
        public override NotionApiReleaseVersion ApiVersion => NotionApiReleaseVersion.NotionApi20250903;

        private string DatabaseUrl(string databaseId) => $"https://api.notion.com/v1/databases/{databaseId}";
        private string DataSourceUrl(string datasource) => $"https://api.notion.com/v1/data_sources/{datasource}/query";
        
        
        private static NotionRequestData LastRequestData { get; set; }
        private static List<string> LatestDataSources { get; set; }
        private static int SourceIdIndex { get; set; }

        private float CurrentStep { get; set; } = 0;
        private float Steps { get; set; } = 3;
        private float Progress01 => CurrentStep / Steps;


        private UnityWebRequest PrepareDatabaseRequest(NotionWebRequestData webRequestData)
        {
            UnityWebRequest request = UnityWebRequest.Get(webRequestData.Url);

            request.SetRequestHeader("Accept", "application/json");
            request.SetRequestHeader("Authorization", $"Bearer {webRequestData.ApiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", ApiVersion.ToVersionString());
            request.timeout = ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.DownloadTimeout;
            
            return request;
        }
        
        
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
            SourceIdIndex = 0;
            CurrentStep = 1;
            Steps = 3;
            
            WebRequestDatabase(data);
        }
        
        
        private void WebRequestDatabase(NotionRequestData data)
        {
            LastRequestData = data;
            
            if (!NotionSecretKeyValidator.IsKeyValid(LastRequestData.ApiKey))
            {
                if (EditorUtility.DisplayDialog("Notion Data", "Api key for database download is invalid.",
                        "Continue"))
                {
                    Debug.LogError(
                        "Notion API passed in is not valid, please double check it before sending another request.");
                }
                
                return;
            }
            
            var request = PrepareDatabaseRequest(new NotionWebRequestData(DatabaseUrl(LastRequestData.DatabaseId), LastRequestData.ApiKey, LastRequestData.Sorts, LastRequestData.Filter));
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            EditorUtility.DisplayProgressBar("Notion Data", "Retrieving Database Info", Progress01);
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    Debug.Log(request.downloadHandler.error);
                    EditorUtility.ClearProgressBar();
                    RequestError.Raise(new NotionRequestError(LastRequestData.RequestingAsset, JObject.Parse(request.downloadHandler.text)));
                    return;
                }
                
                OnDownloadInfoReceived(request.downloadHandler.text);
            };
        }
        
        
        private void WebRequestDataSource()
        {
            var request = PrepareInitialRequest(new NotionWebRequestData(DataSourceUrl(LatestDataSources[SourceIdIndex]), LastRequestData.ApiKey, LastRequestData.Sorts, LastRequestData.Filter));
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            EditorUtility.DisplayProgressBar("Notion Data", $"Downloading data from source {LatestDataSources[SourceIdIndex]}", Progress01);
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    EditorUtility.ClearProgressBar();
                    RequestError.Raise(new NotionRequestError(LastRequestData.RequestingAsset, JObject.Parse(request.downloadHandler.text)));
                    return;
                }

                OnDownloadReceived(request.downloadHandler.text);
            };
        }
        
        
        /// <summary>
        /// Runs the web request to get the Notion database requested.
        /// </summary>
        /// <param name="bodyData">The body to send with the API call.</param>
        private void WebRequestDataSourceRepeat(JObject bodyData)
        {
            var request = PrepareRepeatRequest(new NotionWebRequestData(DataSourceUrl(LatestDataSources[SourceIdIndex]), LastRequestData.ApiKey, bodyData, LastRequestData.Sorts, LastRequestData.Filter));
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            if (LastRequestData.ShowResponseDialogue)
            {
                EditorUtility.DisplayProgressBar("Notion Data", $"Downloading data from source {LatestDataSources[SourceIdIndex]}", Progress01);
            }
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    EditorUtility.ClearProgressBar();
                    RequestError.Raise(new NotionRequestError(LastRequestData.RequestingAsset, JObject.Parse(request.downloadHandler.text)));
                    return;
                }

                OnDownloadReceived(request.downloadHandler.text);
            };
        }
        
        
        private void OnDownloadInfoReceived(string data)
        {
            CurrentStep++;
            LatestDataSources = new List<string>();
            SourceIdIndex = 0;
            
            foreach (var entry in JObject.Parse(data)["data_sources"])
            {
                LatestDataSources.Add(entry["id"].Value<string>());
            }

            if (LatestDataSources.Count > 1)
            {
                Steps += LatestDataSources.Count - 1;
            }

            if (LatestDataSources.Count <= 0)
            {
                EditorUtility.ClearProgressBar();
                RequestError.Raise(new NotionRequestError(LastRequestData.RequestingAsset, new JObject()
                {
                    ["message"] = "No data sources found in the database."
                }));
                return;
            }
            
            WebRequestDataSource();
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
            
            LastRequestData.AppendResultData(resultData);

            // Does another call as there is more data to download still...
            if (json["has_more"].Value<bool>())
            {
                WebRequestDataSourceRepeat(new JObject()
                {
                    ["start_cursor"] = JObject.Parse(data)["next_cursor"]
                });
                
                return;
            }

            SourceIdIndex++;
            CurrentStep++;

            if (SourceIdIndex < LatestDataSources.Count)
            {
                WebRequestDataSource();
                return;
            }
            
            EditorUtility.DisplayProgressBar("Notion Data", "Parsing downloaded data", Progress01);
            DataReceived.Raise(LastRequestData.ResultData);
        }
    }
}