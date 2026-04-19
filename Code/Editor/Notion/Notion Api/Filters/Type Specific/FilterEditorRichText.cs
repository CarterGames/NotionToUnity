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

using CarterGames.NotionData.Filters;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
	public static class FilterEditorRichText
	{
		public static void DrawFilterOption(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			EditorGUILayout.BeginVertical("Box");

			EditorGUILayout.BeginHorizontal();
			var label = $"{property.Fpr("option").Fpr("propertyName").stringValue} ({TypeName(property)})";
			property.isExpanded = EditorGUILayout.Foldout(property.isExpanded, label);
			
			if (GUILayout.Button("-", GUILayout.Width(22.5f)))
			{
				baseProp.Fpr("value").Fpr("filterOptions").DeleteIndex(index);
				baseProp.serializedObject.ApplyModifiedProperties();
				baseProp.serializedObject.Update();
				return;
			}
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.Space(2f);
			
			if (property.isExpanded)
			{
				GeneralUtilEditor.DrawHorizontalGUILine();
				EditorGUILayout.PropertyField(property.Fpr("option").Fpr("isRollup"));
				GeneralUtilEditor.DrawHorizontalGUILine();
				EditorGUILayout.PropertyField(property.Fpr("option").Fpr("propertyName"));
				GUILayout.Space(1f);
				
				EditorGUILayout.BeginHorizontal();

				if (property.Fpr("option").Fpr("comparisonEnumIndex").intValue == 5 ||
				    property.Fpr("option").Fpr("comparisonEnumIndex").intValue == 6)
				{
					property.Fpr("option").Fpr("comparisonEnumIndex").intValue =
						(int) (NotionFilterRichTextComparison) EditorGUILayout.EnumPopup(
							(NotionFilterRichTextComparison) property.Fpr("option").Fpr("comparisonEnumIndex").intValue);
				}
				else
				{
					property.Fpr("option").Fpr("comparisonEnumIndex").intValue =
						(int) (NotionFilterRichTextComparison) EditorGUILayout.EnumPopup(
							(NotionFilterRichTextComparison) property.Fpr("option").Fpr("comparisonEnumIndex").intValue, GUILayout.Width(147.5f));
					GUILayout.Space(1f);
					EditorGUILayout.PropertyField(property.Fpr("option").Fpr("value"), GUIContent.none);
				}
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndVertical();
		}


		private static string TypeName(SerializedProperty property)
		{
			return property.Fpr("option").Fpr("isRollup").boolValue
				? $"Rollup of {property.Fpr("typeName").stringValue}"
				: property.Fpr("typeName").stringValue;
		}
	}
}