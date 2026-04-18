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

namespace CarterGames.NotionData
{
    public struct NotionPropertyData
    {
        public string propertyName;
        public object valueForType;
        public string jsonValue;
        public string downloadText;


        public NotionPropertyData(string propertyName, object valueForType, string jsonValue, string downloadText)
        {
            this.propertyName = propertyName;
            this.valueForType = valueForType;
            this.jsonValue = jsonValue;
            this.downloadText = downloadText;
        }
    }
}