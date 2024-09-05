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

using UnityEditor;
using CarterGames.Standalone.NotionData.ThirdParty;

namespace CarterGames.Standalone.NotionData.Editor
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
		private static JSONObject ToJsonObject(this NotionSortProperty sortProperty)
		{
			return new JSONObject()
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
		public static JSONArray ToJsonArray(this NotionSortProperty[] sortProperties)
		{
			var array = new JSONArray();

			foreach (var entry in sortProperties)
			{
				array.Add(entry.ToJsonObject());
			}

			return array;
		}
	}
}