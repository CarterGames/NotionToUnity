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

using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
	public static class FilterEditorCheckbox
	{
		public static void DrawFilterOption(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			EditorGUILayout.BeginVertical("Box");

			EditorGUILayout.BeginHorizontal();
			var label = $"{property.Fpr("option").Fpr("propertyName").stringValue} ({property.Fpr("typeName").stringValue})";
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
				GUI.backgroundColor = Color.white;
				if (GUILayout.Button(property.Fpr("option").Fpr("comparisonEnumIndex").intValue == 0 ? "is not" : "is", GUILayout.Width(147.5f)))
				{
					if (property.Fpr("option").Fpr("comparisonEnumIndex").intValue == 0)
					{
						property.Fpr("option").Fpr("comparisonEnumIndex").intValue = 1;
					}
					else
					{
						property.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;
					}
					
					property.serializedObject.ApplyModifiedProperties();
					property.serializedObject.Update();
				}
				GUI.backgroundColor = Color.white;
				
				GUILayout.Space(1.5f);
				
				EditorGUI.BeginChangeCheck();
				property.Fpr("option").Fpr("value").stringValue = EditorGUILayout.Toggle(bool.Parse(property.Fpr("option").Fpr("value").stringValue)) ? "true" : "false";
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties();
					property.serializedObject.Update();
				}
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}