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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;

namespace CarterGames.NotionData.Editor
{
	public class SearchProviderFilterType : SearchProvider<NotionFilterOption>
	{
		private static SearchProviderFilterType Instance;
		
		public override string ProviderTitle => "Select Notion Filter Type";
		
		
		public override List<SearchGroup<NotionFilterOption>> GetEntriesToDisplay()
		{
			var list = new List<SearchGroup<NotionFilterOption>>();
			var options = AssemblyHelper.GetClassesOfType<NotionFilterOption>().Where(t => !ToExclude.Contains(t) && t.EditorTypeName != "Group");
			var items = new List<SearchItem<NotionFilterOption>>();
			
			foreach (var entry in options)
			{
				items.Add(SearchItem<NotionFilterOption>.Set(entry.EditorTypeName, entry));
			}
			
			list.Add(new SearchGroup<NotionFilterOption>("", items));
			
			return list;
		}
		
		
		public static SearchProviderFilterType GetProvider()
		{
			if (Instance == null)
			{
				Instance = CreateInstance<SearchProviderFilterType>();
			}

			return Instance;
		}
	}
}