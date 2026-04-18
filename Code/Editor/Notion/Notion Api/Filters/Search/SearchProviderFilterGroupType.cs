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

namespace CarterGames.NotionData.Editor
{
	public class SearchProviderFilterGroupType : SearchProvider<int>
	{
		private static SearchProviderFilterGroupType Instance;
		private static int NestLevel;
		public override string ProviderTitle => "Select Filter Type";
		
		
		public override List<SearchGroup<int>> GetEntriesToDisplay()
		{
			var list = new List<SearchGroup<int>>();
			var items = new List<SearchItem<int>>();
			var itemNames = new List<string>()
			{
				"+ Add filter rule"
			};

			if (NestLevel < 2)
			{
				itemNames.Add("+ Add filter group");
			}

			for (var i = 0; i < itemNames.Count; i++)
			{
				items.Add(SearchItem<int>.Set(itemNames[i], i));
			}

			list.Add(new SearchGroup<int>("", items));
			
			return list;
		}
		
		
		public static SearchProviderFilterGroupType GetProvider(int currentNestLevel)
		{
			NestLevel = currentNestLevel;
			
			if (Instance == null)
			{
				Instance = CreateInstance<SearchProviderFilterGroupType>();
			}

			return Instance;
		}
	}
}