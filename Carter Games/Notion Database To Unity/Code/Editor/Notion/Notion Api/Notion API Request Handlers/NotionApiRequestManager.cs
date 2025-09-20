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
using CarterGames.Shared.NotionData.Editor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    public static class NotionApiRequestManager
    {
        private static Dictionary<NotionApiReleaseVersion, NotionApiRequestHandlerBase> HandlersLookup = new Dictionary<NotionApiReleaseVersion, NotionApiRequestHandlerBase>()
        {
            { NotionApiReleaseVersion.NotionApi20220628, new NotionApiRequestHandler2022() },
            { NotionApiReleaseVersion.NotionApi20250903, new NotionApiRequestHandler2025() }
        };


        private static NotionApiReleaseVersion TargetVersion => ScriptableRef.GetAssetDef<AssetEditorGlobalSettings>()
            .AssetRef.NotionAPIReleaseVersion;


        public static void RunRequest(NotionRequestData requestData, Action<NotionRequestResult> onDataReceived,
            Action<NotionRequestError> onError)
        {
            var handler = HandlersLookup[TargetVersion];

            handler.DataReceived.RemoveAnonymous("success");
            handler.RequestError.RemoveAnonymous("error");
            
            handler.DataReceived.AddAnonymous("success", (r) => onDataReceived?.Invoke(r));
            handler.RequestError.AddAnonymous("error", (e) => onError?.Invoke(e));
            
            handler.StartDownload(requestData);
        }
    }
}