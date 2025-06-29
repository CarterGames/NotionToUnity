﻿/*
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
using CarterGames.NotionData.ThirdParty;
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
		
		
		public override JSONObject ToJson()
		{
			var data = new JSONObject();

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