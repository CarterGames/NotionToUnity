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
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;
using CarterGames.NotionData.ThirdParty;
using CarterGames.Shared.NotionData.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles accessing the Notion API for use in the data system for Notion.
    /// </summary>
    public static class NotionApiRequestHandler
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private static NotionRequestData requestData;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the data has been received from Notion.
        /// </summary>
        public static readonly Evt<NotionRequestResult> DataReceived = new Evt<NotionRequestResult>();
        
        
        /// <summary>
        /// Raises when there was an error when trying to receive data from Notion.
        /// </summary>
        public static readonly Evt<NotionRequestError> RequestError = new Evt<NotionRequestError>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Runs the web request to get the Notion database requested.
        /// </summary>
        /// <param name="data">The data to send.</param>
        public static void WebRequestPostWithAuth(NotionRequestData data)
        {
            requestData = data;
            
            if (!NotionSecretKeyValidator.IsKeyValid(requestData.ApiKey))
            {
                if (EditorUtility.DisplayDialog("Standalone Notion Data", "Api key for database download is invalid.",
                    "Continue"))
                {
                    Debug.LogError(
                        "Notion API passed in is not valid, please double check it before sending another request.");
                }
                
                return;
            }
            
            var request = PrepareRequest(requestData.Url, requestData.ApiKey, requestData.Sorts, requestData.Filter);
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            EditorUtility.DisplayProgressBar("Standalone Notion Data", "Downloading Data", 0f);
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    EditorUtility.ClearProgressBar();
                    RequestError.Raise(new NotionRequestError(requestData.RequestingAsset, JSON.Parse(request.downloadHandler.text)));
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
        public static void WebRequestPostWithAuth(NotionRequestData data, JSONObject bodyData)
        {
            var request = PrepareRequest(data.Url, data.ApiKey, bodyData, data.Sorts, data.Filter);
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            if (data.ShowResponseDialogue)
            {
                EditorUtility.DisplayProgressBar("Standalone Notion Data", "Downloading Data", .5f);
            }
            
            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    EditorUtility.ClearProgressBar();
                    RequestError.Raise(new NotionRequestError(data.RequestingAsset, JSONNode.Parse(request.downloadHandler.text)));
                    return;
                }

                OnDownloadReceived(request.downloadHandler.text);
            };
        }


        /// <summary>
        /// Prepares an API call with thr entered data.
        /// </summary>
        /// <param name="url">The url to use.</param>
        /// <param name="apiKey">The api key to use.</param>
        /// <param name="sorts">The sort properties to apply.</param>
        /// <param name="filters">The filter to apply.</param>
        /// <returns>A prepared UnityWebRequest.</returns>
        private static UnityWebRequest PrepareRequest(string url, string apiKey, NotionSortProperty[] sorts = null, NotionFilterContainer filters = null)
        {
            UnityWebRequest request;

            if (sorts == null && filters == null)
            {
                request = UnityWebRequest.Post(url, string.Empty);
            }
            else
            {
                var body = new JSONObject();

                if (sorts != null)
                {
                    if (sorts.Length > 0)
                    {
                        body["sorts"] = sorts.ToJsonArray();
                    }
                }
                
                if (filters != null)
                {
                    if (filters.TotalFilters > 0)
                    {
                        body["filter"] = filters.ToFilterJson();
                    }
                }

                request = UnityWebRequest.Put(url, body.ToString());
                request.method = "POST";
            }
            
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.NotionAPIReleaseVersion.ToVersionString());
            request.timeout = ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.DownloadTimeout;
            
            return request;
        }
        
        
        /// <summary>
        /// Prepares an API call with thr entered data.
        /// </summary>
        /// <param name="url">The url to use.</param>
        /// <param name="apiKey">The api key to use.</param>
        /// <param name="body">The body to use in the API call.</param>
        /// <param name="sorts">The sort properties to apply.</param>
        /// <param name="filter">The filter to apply.</param>
        /// <returns>A prepared UnityWebRequest.</returns>
        private static UnityWebRequest PrepareRequest(string url, string apiKey, JSONObject body, NotionSortProperty[] sorts = null, NotionFilterContainer filters = null)
        {
            if (sorts != null)
            {
                if (sorts.Length > 0)
                {
                    body["sorts"] = sorts.ToJsonArray();
                }
            }
                
            if (filters != null)
            {
                if (filters.TotalFilters > 0)
                {
                    body["filter"] = filters.ToFilterJson();
                }
            }
            
            var request = UnityWebRequest.Put(url, body.ToString());
            
            request.method = "POST";
            request.SetRequestHeader("Authorization", $"Bearer {apiKey}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.NotionAPIReleaseVersion.ToVersionString());
            request.timeout = ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>().AssetRef.DownloadTimeout;
            
            return request;
        }


        /// <summary>
        /// Runs on data received.
        /// </summary>
        /// <param name="data">The data received.</param>
        private static void OnDownloadReceived(string data)
        {
            var resultData = new List<KeyValuePair<string, JSONNode>>();
            
            foreach (var entry in JSONNode.Parse(data)["results"].AsArray)
            {
                resultData.Add(entry);
            }

            requestData.AppendResultData(resultData);

            // Does another call as there is more data to download still...
            if (JSONNode.Parse(data)["has_more"].AsBool)
            {
                WebRequestPostWithAuth(requestData, new JSONObject()
                {
                    ["start_cursor"] = JSONNode.Parse(data)["next_cursor"]
                });
                
                return;
            }
            
            EditorUtility.ClearProgressBar();
            Debug.Log("Completed Download Successfully.");
            DataReceived.Raise(requestData.ResultData);
        }
        

        /// <summary>
        /// Resets the request data stored locally when called.
        /// </summary>
        public static void ResetRequestData()
        {
            requestData = null;
        }
    }
}