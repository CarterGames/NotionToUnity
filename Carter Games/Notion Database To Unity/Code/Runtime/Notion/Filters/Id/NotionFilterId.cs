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
using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData.Filters
{
	[Serializable]
	public class NotionFilterId : NotionFilterOption
	{
		private static readonly Dictionary<NotionFilterIdComparison, string> FilterStringLookup =
			new Dictionary<NotionFilterIdComparison, string>()
			{
				{ NotionFilterIdComparison.DoesNotEqual, "does_not_equal" },
				{ NotionFilterIdComparison.Equals, "equals" },
				{ NotionFilterIdComparison.GreaterThan, "greater_than" },
				{ NotionFilterIdComparison.GreaterThanOrEqualTo, "greater_than_or_equal_to" },
				{ NotionFilterIdComparison.LessThan, "less_than" },
				{ NotionFilterIdComparison.LessThanOrEqualTo, "less_than_or_equal_to" },
			};
		
		
		private NotionFilterIdComparison Comparison => (NotionFilterIdComparison) comparisonEnumIndex;
		
		public override string EditorTypeName => "Id";
		
		public NotionFilterId() {}
		public NotionFilterId(NotionFilterOption filterOption)
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
			
			data["unique_id"][FilterStringLookup[Comparison]] = true;
			
			return data;
		}
	}
}