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

using System;
using System.Linq;
using System.Text;
using CarterGames.Standalone.NotionData.Common;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Filters
{
	[Serializable]
	public class NotionFilterContainer
	{
		[SerializeField] private SerializableDictionary<string, NotionFilterGrouping> filterGroups;


		public int TotalFilters
		{
			get
			{
				var total = 0;
				
				foreach (var group in filterGroups)
				{
					foreach (var entry in group.Value.FilterOptions)
					{
						if (entry.TypeName == "Group") continue;
						total++;
					}
				}

				return total;
			}
		}
		

		public JSONObject ToFilterJson()
		{
			var json = new JSONObject();

			foreach (var group in filterGroups)
			{
				if (group.Value.IsNested) continue;

				var type = group.Value.IsAndCheck ? "and" : "or";
				json[type] = new JSONArray();

				foreach (var entry in group.Value.FilterOptions)
				{
					if (entry.TypeName == "Group")
					{
						var groupData = filterGroups.FirstOrDefault(t => t.Key == entry.Option.Value);
						var groupJson = new JSONObject();

						foreach (var groupEntry in groupData.Value.FilterOptions)
						{
							var groupType = groupData.Value.IsAndCheck ? "and" : "or";
							groupJson[groupType].Add(groupEntry.ToJson());
						}

						json[type].Add(groupJson);
						continue;
					}

					json[type].Add(entry.ToJson());
				}
			}

			return json;
		}
	}
}