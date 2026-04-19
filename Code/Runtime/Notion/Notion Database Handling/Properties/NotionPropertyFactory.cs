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

namespace CarterGames.NotionData
{
    /// <summary>
    /// A helper class to create different Notion property types. Not intended for direct access.
    /// </summary>
    /// <remarks>Use NotionProperty.cs class to use these methods as intended.</remarks>
    public static class NotionPropertyFactory
    {
        public static NotionPropertyCheckbox Checkbox(NotionPropertyData data)
        {
            return new NotionPropertyCheckbox(data);
        }
        
        
        public static NotionPropertyDate Date(NotionPropertyData data)
        {
            return new NotionPropertyDate(data);
        }
        
        
        public static NotionPropertyMultiSelect MultiSelect(NotionPropertyData data)
        {
            return new NotionPropertyMultiSelect(data);
        }
        
        
        public static NotionPropertySelect Select(NotionPropertyData data)
        {
            return new NotionPropertySelect(data);
        }
        
        
        public static NotionPropertyRichText RichText(NotionPropertyData data)
        {
            return new NotionPropertyRichText(data);
        }
        
        
        public static NotionPropertyTitle Title(NotionPropertyData data)
        {
            return new NotionPropertyTitle(data);
        }
        
        
        public static NotionPropertyStatus Status(NotionPropertyData data)
        {
            return new NotionPropertyStatus(data);
        }
        
        
        public static NotionPropertyNumber Number(NotionPropertyData data)
        {
            return new NotionPropertyNumber(data);
        }

        
        public static NotionPropertyUrl Url(NotionPropertyData data)
        {
            return new NotionPropertyUrl(data);
        }
    }
}