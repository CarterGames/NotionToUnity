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
using System.Globalization;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Filters
{
	[Serializable]
	public class NotionFilterDate : NotionFilterOption
	{
		private static readonly Dictionary<NotionFilterDateComparison, string> FilterStringLookup =
			new Dictionary<NotionFilterDateComparison, string>()
			{
				{ NotionFilterDateComparison.After, "after" },
				{ NotionFilterDateComparison.Before, "before" },
				{ NotionFilterDateComparison.Equals, "equals" },
				{ NotionFilterDateComparison.IsEmpty, "is_empty" },
				{ NotionFilterDateComparison.IsNotEmpty, "is_not_empty" },
				{ NotionFilterDateComparison.NextMonth, "next_month" },
				{ NotionFilterDateComparison.NextWeek, "next_week" },
				{ NotionFilterDateComparison.NextYear, "next_year" },
				{ NotionFilterDateComparison.OnOrAfter, "on_or_after" },
				{ NotionFilterDateComparison.OnOrBefore, "on_or_before" },
				{ NotionFilterDateComparison.PastMonth, "past_month" },
				{ NotionFilterDateComparison.PastWeek, "past_week" },
				{ NotionFilterDateComparison.PastYear, "past_year" },
				{ NotionFilterDateComparison.ThisWeek, "this_week" },
			};
		
		
		private NotionFilterDateComparison Comparison => (NotionFilterDateComparison) comparisonEnumIndex;
		
		public override string EditorTypeName => "Date";
		
		
		public NotionFilterDate() {}
		public NotionFilterDate(NotionFilterOption filterOption)
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

			// No reason for default as its impossible to hit.
#pragma warning disable
			switch (Comparison)
#pragma warning restore
			{
				case NotionFilterDateComparison.IsEmpty:
				case NotionFilterDateComparison.IsNotEmpty:
					
					data["date"][FilterStringLookup[Comparison]] = true;
					break;
				case NotionFilterDateComparison.NextMonth:
				case NotionFilterDateComparison.NextWeek:
				case NotionFilterDateComparison.NextYear:
				case NotionFilterDateComparison.PastMonth:
				case NotionFilterDateComparison.PastWeek:
				case NotionFilterDateComparison.PastYear:
				case NotionFilterDateComparison.ThisWeek:

					data["date"][FilterStringLookup[Comparison]] = "{}";
					break;
				case NotionFilterDateComparison.OnOrAfter:
				case NotionFilterDateComparison.OnOrBefore:
				case NotionFilterDateComparison.After:
				case NotionFilterDateComparison.Before:
				case NotionFilterDateComparison.Equals:

					data["date"][FilterStringLookup[Comparison]] = JsonUtility.FromJson<DateTime>(value).ToString("o", CultureInfo.InvariantCulture);
					break;
			}
			
			return data;
		}
	}
}