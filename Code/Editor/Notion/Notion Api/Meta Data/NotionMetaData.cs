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

using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    public static class NotionMetaData
    {
        public static readonly GUIContent VersionNumber =
            new GUIContent(
                "Version:",
                "The version number for the standalone asset.");
        
        
        public static readonly GUIContent ReleaseDate =
            new GUIContent(
                "Release date (Y/M/D):",
                "The release date of the standalone asset. In Year/Month/Day order.");
        
        

        
        
        public static readonly GUIContent ApiKey =
            new GUIContent(
                "API key:",
                "The API key to use with the download.");
        
        
        public static readonly GUIContent DatabaseLink =
            new GUIContent(
                "Link to database:",
                "The link to the page with the database on that you want to download.");

        

    }
}