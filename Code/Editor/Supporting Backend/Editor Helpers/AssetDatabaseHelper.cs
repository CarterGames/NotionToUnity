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

using UnityEditor;

namespace CarterGames.NotionData.Editor
{
    public static class AssetDatabaseHelper
    {
        /// <summary>
        /// Gets if asset database can find an asset at the defined path.
        /// </summary>
        /// <param name="path">The path top find.</param>
        /// <typeparam name="T">The type to try and get.</typeparam>
        /// <returns>If the asset exists in the asset database.</returns>
        public static bool FileIsInProject<T>(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            return AssetDatabase.LoadAssetAtPath(path, typeof(T)) != null;
        }
    }
}