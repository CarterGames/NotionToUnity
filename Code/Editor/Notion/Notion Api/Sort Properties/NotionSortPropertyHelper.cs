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

using Newtonsoft.Json.Linq;
using UnityEditor;

namespace CarterGames.NotionData.Editor
{
	/// <summary>
	/// A helper class to handle notion sort properties in the editor.
	/// </summary>
	public static class NotionSortPropertyHelper
	{
		/// <summary>
		/// Converts a serialized property into an array of notion sort properties.
		/// </summary>
		/// <param name="property">The property to read.</param>
		/// <returns>An array of notion sort properties.</returns>
		public static NotionSortProperty[] ToSortPropertyArray(this SerializedProperty property)
		{
			var array = new NotionSortProperty[property.arraySize];

			for (var i = 0; i < array.Length; i++)
			{
				var entry = property.GetIndex(i);
                    
				array[i] = new NotionSortProperty(
					entry.Fpr("propertyName").stringValue,
					entry.Fpr("ascending").boolValue
				);
			}

			return array;
		}
		
		
		/// <summary>
		/// Converts a notion sort property to valid Json for use in the Notion API calls.
		/// </summary>
		/// <param name="sortProperty">The property to convert.</param>
		/// <returns>The Json for the sort property.</returns>
		private static JObject ToJsonObject(this NotionSortProperty sortProperty)
		{
			return new JObject()
			{
				["property"] = sortProperty.PropertyName,
				["direction"] = sortProperty.SortAscending ? "ascending" : "descending"
			};
		}


		/// <summary>
		/// Converts a notion sort properties to valid Json array for use in the Notion API calls.
		/// </summary>
		/// <param name="sortProperties">The properties to convert.</param>
		/// <returns>The Json for the sort properties.</returns>
		public static JArray ToJsonArray(this NotionSortProperty[] sortProperties)
		{
			var array = new JArray();

			foreach (var entry in sortProperties)
			{
				array.Add(entry.ToJsonObject());
			}

			return array;
		}
	}
}