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
    /// <summary>
    /// An enum of any supported API versions
    /// </summary>
    public enum NotionApiReleaseVersion
    {
        [InspectorName("2025-09-03 (Latest)")] NotionApi20250903 = 1,
        [InspectorName("2022-06-28")] NotionApi20220628 = 0,
    }
}