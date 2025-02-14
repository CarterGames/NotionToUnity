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

using System.Collections.Generic;
using System.Linq;
using CarterGames.Standalone.NotionData.Common;
using CarterGames.Standalone.NotionData.Filters;

namespace CarterGames.Standalone.NotionData.Editor
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