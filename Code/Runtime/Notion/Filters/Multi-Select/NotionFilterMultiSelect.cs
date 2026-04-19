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
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Filters
{
	[Serializable]
	public class NotionFilterMultiSelect : NotionFilterOption
	{
		private static readonly Dictionary<NotionFilerMultiSelectComparison, string> FilterStringLookup =
			new Dictionary<NotionFilerMultiSelectComparison, string>()
			{
				{ NotionFilerMultiSelectComparison.Contains, "contains" },
				{ NotionFilerMultiSelectComparison.DoesNotContain, "does_not_contain" },
				{ NotionFilerMultiSelectComparison.IsEmpty, "is_empty" },
				{ NotionFilerMultiSelectComparison.IsNotEmpty, "is_not_empty" },
			};
		
		
		private NotionFilerMultiSelectComparison Comparison => (NotionFilerMultiSelectComparison) comparisonEnumIndex;
		
		public override string EditorTypeName => "Multi Select";
		
		
		public NotionFilterMultiSelect() {}
		public NotionFilterMultiSelect(NotionFilterOption filterOption)
		{
			propertyName = filterOption.PropertyName;
			value = filterOption.Value;
			comparisonEnumIndex = filterOption.ComparisonEnumIndex;
			isRollup = filterOption.IsRollup;
		}
		
		
		public override JObject ToJson()
		{
			var data = new JObject();

			if (!isRollup)
			{
				data["property"] = propertyName;
			}

			if (Comparison != NotionFilerMultiSelectComparison.IsEmpty &&
			    Comparison != NotionFilerMultiSelectComparison.IsNotEmpty)
			{
				data["multi_select"][FilterStringLookup[Comparison]] = JsonUtility.ToJson(value);
			}
			else
			{
				data["multi_select"][FilterStringLookup[Comparison]] = true;
			}
			
			return data;
		}
	}
}