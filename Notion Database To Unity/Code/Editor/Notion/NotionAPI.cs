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

using System;
using NotionToUnity.Common;
using UnityEngine;
using UnityEngine.Networking;

namespace NotionToUnity.Editor
{
    /// <summary>
    /// Handles accessing the Notion API for use in the data system for Notion.
    /// </summary>
    [Serializable]
    public static class NotionAPI
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        private const string RootUrl = "https://api.notion.com";
        private const string Version = "v1";
        private const string DatabaseElement = "databases";
        private const string APIKeyPrefix = "secret_";
        private const int APIKeySuffixLength = 43;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Events
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Raises when the data has been received from Notion.
        /// </summary>
        public static Evt<string> DataReceived = new Evt<string>();
        
        
        /// <summary>
        /// Raises when there was an error when trying to receive data from Notion.
        /// </summary>
        public static Evt RequestError = new Evt();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the url to download the database from.
        /// </summary>
        /// <param name="databaseId">The id of the database to download.</param>
        /// <returns>The complete Url for use.</returns>
        private static string Url(string databaseId)
        {
            return $"{RootUrl}/{Version}/{DatabaseElement}/{databaseId}/query";
        }

        
        /// <summary>
        /// Runs the web request to get the Notion database requested.
        /// </summary>
        /// <param name="databaseId">The id of the database to get.</param>
        /// <param name="apiKey">The api key to use for the download.</param>
        public static void WebRequestPostWithAuth(string databaseId, string apiKey = "___")
        {
            var apiKeyToUse = IsValidApiKey(apiKey);
            
            var request = UnityWebRequest.Post(Url(databaseId), string.Empty);
            request.SetRequestHeader("Authorization", $"Bearer {apiKeyToUse}");
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Notion-Version", "2022-06-28");
            
            AsyncOperation asyncOperation = request.SendWebRequest();

            asyncOperation.completed += (a) =>
            {
                if (!string.IsNullOrEmpty(request.error))
                {
                    RequestError.Raise();
                    return;
                }
                
                DataReceived.Raise(request.downloadHandler.text);
            };
        }

        
        /// <summary>
        /// Returns if the api key entered is in the valid format for notion or not.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns>If the key is valid or not.</returns>
        public static bool IsValidApiKey(string key)
        {
            return 
                !string.IsNullOrEmpty(key) &&
                key.Contains(APIKeyPrefix) && 
                key.Substring(0, key.Length - APIKeyPrefix.Length).Length.Equals(APIKeySuffixLength);
        }
    }
}