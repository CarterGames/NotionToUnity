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

using CarterGames.Standalone.NotionData.Filters;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
	public static class FilterEditorNumber
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
				
				property.Fpr("option").Fpr("comparisonEnumIndex").intValue =
					(int) (NotionFilterIdComparison) EditorGUILayout.EnumPopup(
						(NotionFilterIdComparison) property.Fpr("option").Fpr("comparisonEnumIndex").intValue, GUILayout.Width(147.5f));
				GUILayout.Space(1f);
				EditorGUI.BeginChangeCheck();
				property.Fpr("option").Fpr("value").stringValue = EditorGUILayout.IntField(int.Parse(property.Fpr("option").Fpr("value").stringValue)).ToString();
			
				EditorGUILayout.EndHorizontal();
				if (EditorGUI.EndChangeCheck())
				{
					property.serializedObject.ApplyModifiedProperties();
					property.serializedObject.Update();
				}
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}