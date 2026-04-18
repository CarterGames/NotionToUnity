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

using CarterGames.NotionData.Filters;
using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData.Editor
{
    public class NotionWebRequestData
    {
        public string Url;
        public string ApiKey;
        public NotionSortProperty[] Sorting;
        public NotionFilterContainer Filters;
        public JObject Body;


        public NotionWebRequestData(string url, string apiKey, NotionSortProperty[] sorting, NotionFilterContainer filters)
        {
            Url = url;
            ApiKey = apiKey;
            Sorting = sorting;
            Filters = filters;
            Body = null;
        }
        
        
        public NotionWebRequestData(string url, string apiKey, JObject body, NotionSortProperty[] sorting, NotionFilterContainer filters)
        {
            Url = url;
            ApiKey = apiKey;
            Sorting = sorting;
            Filters = filters;
            Body = body;
        }
    }
}