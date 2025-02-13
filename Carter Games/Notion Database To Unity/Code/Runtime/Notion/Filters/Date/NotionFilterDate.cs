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

using System;
using System.Collections.Generic;
using System.Globalization;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Filters
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
		
		
		public override JSONObject ToJson()
		{
			var data = new JSONObject();

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