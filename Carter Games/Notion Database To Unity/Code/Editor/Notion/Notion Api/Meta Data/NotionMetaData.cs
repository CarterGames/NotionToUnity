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
        
        
        public static readonly GUIContent ApiVersion =
            new GUIContent(
                "API version:",
                "The major API version to use in Notion API calls.");
        
        
        public static readonly GUIContent ApiReleaseVersion =
            new GUIContent(
                "API release version:",
                "The API release version to use in Notion API calls.");
        
        
        public static readonly GUIContent ApiKey =
            new GUIContent(
                "API key:",
                "The API key to use with the download.");
        
        
        public static readonly GUIContent DatabaseLink =
            new GUIContent(
                "Link to database:",
                "The link to the page with the database on that you want to download.");

        
        public static readonly GUIContent DownloadTimeout =             
            new GUIContent(
            "Download Timeout:",
            "The duration until a download attempt will auto-fail.");
    }
}