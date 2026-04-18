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

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// A helper class to convert the notion release api versions to usable strings.
    /// </summary>
    public static class NotionApiVersionHelper
    {
        public static string ToVersionString(this NotionApiReleaseVersion apiReleaseVersion)
        {
            return apiReleaseVersion switch
            {
                NotionApiReleaseVersion.NotionApi20220628 => "2022-06-28",
                NotionApiReleaseVersion.NotionApi20250903 => "2025-09-03",
                _ => string.Empty
            };
        }
    }
}