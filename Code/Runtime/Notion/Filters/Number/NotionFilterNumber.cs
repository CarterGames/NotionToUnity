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

namespace CarterGames.NotionData.Filters
{
	[Serializable]
	public class NotionFilterNumber : NotionFilterOption
	{
		private static readonly Dictionary<NotionFilterNumberComparison, string> FilterStringLookup =
			new Dictionary<NotionFilterNumberComparison, string>()
			{
				{ NotionFilterNumberComparison.DoesNotEqual, "does_not_equal" },
				{ NotionFilterNumberComparison.Equals, "equals" },
				{ NotionFilterNumberComparison.GreaterThan, "greater_than" },
				{ NotionFilterNumberComparison.GreaterThanOrEqualTo, "greater_than_or_equal_to" },
				{ NotionFilterNumberComparison.IsEmpty, "is_empty" },
				{ NotionFilterNumberComparison.IsNotEmpty, "is_not_empty" },
				{ NotionFilterNumberComparison.LessThan, "less_than" },
				{ NotionFilterNumberComparison.LessThanOrEqualTo, "less_than_or_equal_to" },
			};
		
		
		private NotionFilterNumberComparison Comparison => (NotionFilterNumberComparison) comparisonEnumIndex;
		public override string EditorTypeName => "Number";
		
		
		public NotionFilterNumber() {}
		public NotionFilterNumber(NotionFilterOption filterOption)
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

			if (Comparison != NotionFilterNumberComparison.IsEmpty ||
			    Comparison != NotionFilterNumberComparison.IsNotEmpty)
			{
				data["number"][FilterStringLookup[Comparison]] = value.ToString();
			}
			else
			{
				data["number"][FilterStringLookup[Comparison]] = true;
			}
			
			return data;
		}
	}
}