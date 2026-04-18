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
using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData.Filters
{
	[Serializable]
	public class NotionFilterCheckBox : NotionFilterOption
	{
		private const string EqualsString = "equals";
		private const string NotEqualsString = "does_not_equal";


		private string CheckString => comparisonEnumIndex.Equals(0) ? NotEqualsString : EqualsString;
		
		
		public override string EditorTypeName => "CheckBox";
		
		
		public NotionFilterCheckBox() {}

		public NotionFilterCheckBox(NotionFilterOption filterOption)
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
			
			data["checkbox"][CheckString] = value.ToLower();
			
			return data;
		}
	}
}