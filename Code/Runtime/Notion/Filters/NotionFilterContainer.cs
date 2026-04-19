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

using System;
using System.Linq;
using CarterGames.Shared.NotionData.Serializiation;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Filters
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
						if (string.IsNullOrWhiteSpace(entry.TypeName)) continue;
						total++;
					}
				}

				return total;
			}
		}


		public JObject ToFilterJson()
		{
			var json = new JObject();

			foreach (var group in filterGroups)
			{
				if (group.Value.IsNested) continue;

				var type = group.Value.IsAndCheck ? "and" : "or";
				json[type] = new JArray();

				foreach (var entry in group.Value.FilterOptions)
				{
					if (entry.TypeName == "Group")
					{
						var groupData = filterGroups.FirstOrDefault(t => t.Key == entry.Option.Value);
						var groupJson = new JObject();

						foreach (var groupEntry in groupData.Value.FilterOptions)
						{
							var groupType = groupData.Value.IsAndCheck ? "and" : "or";
							((JArray) groupJson[groupType]).Add(groupEntry.ToJson());
						}

						((JArray) json[type]).Add(groupJson);
						continue;
					}

					((JArray) json[type]).Add(entry.ToJson());
				}
			}

			return json;
		}
	}
}