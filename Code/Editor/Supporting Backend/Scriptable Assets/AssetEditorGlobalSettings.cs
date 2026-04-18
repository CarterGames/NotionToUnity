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
using CarterGames.Shared.NotionData;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
	[Serializable]
    public class AssetEditorGlobalSettings : EditorOnlyNdAsset
    {
	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Fields
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
	    
        [SerializeField] private NotionApiReleaseVersion apiReleaseVersion = NotionApiReleaseVersion.NotionApi20250903;
        [SerializeField] [Range(2, 25)] private int downloadTimeout = 10;
        [SerializeField] private string ignorePropertyPrefix = "#";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The Notion release API version to use.
        /// </summary>
        public NotionApiReleaseVersion NotionAPIReleaseVersion => apiReleaseVersion;


        /// <summary>
        /// The time required to wait until a download attempt will fail automatically.
        /// </summary>
        public int DownloadTimeout => downloadTimeout;
        
        
        /// <summary>
        /// Defines the prefix of properties in Notion the system should ignore.
        /// </summary>
        public string IgnorePropertyPrefix => ignorePropertyPrefix;
    }
}