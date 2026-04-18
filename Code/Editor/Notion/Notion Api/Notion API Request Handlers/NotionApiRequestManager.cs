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