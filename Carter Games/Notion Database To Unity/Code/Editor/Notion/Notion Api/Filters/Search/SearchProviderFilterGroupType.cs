/*
 * Copyright (c) 2024 Carter Games
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

using System.Collections.Generic;

namespace CarterGames.Standalone.NotionData.Editor
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