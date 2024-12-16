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

using System;
using CarterGames.Standalone.NotionData.Filters;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
	public static class FilterEditorDate
	{
		private static SerializableDateTime dateTime = new SerializableDateTime(DateTime.MinValue);
		
		
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
					(int) (NotionFilterDateComparison) EditorGUILayout.EnumPopup(
						(NotionFilterDateComparison) property.Fpr("option").Fpr("comparisonEnumIndex").intValue, GUILayout.Width(147.5f));
				
				switch ((NotionFilterDateComparison) property.Fpr("option").Fpr("comparisonEnumIndex").intValue)
				{
					case NotionFilterDateComparison.IsEmpty:
					case NotionFilterDateComparison.IsNotEmpty:
					case NotionFilterDateComparison.NextMonth:
					case NotionFilterDateComparison.NextWeek:
					case NotionFilterDateComparison.NextYear:
					case NotionFilterDateComparison.PastMonth:
					case NotionFilterDateComparison.PastWeek:
					case NotionFilterDateComparison.PastYear:
					case NotionFilterDateComparison.ThisWeek:
						break;
					default:
						GUILayout.Space(1f);
						dateTime = JsonUtility.FromJson<SerializableDateTime>(property.Fpr("option").Fpr("value").stringValue);

						if (dateTime != null)
						{
							EditorGUI.BeginChangeCheck();
							
							EditorGUILayout.BeginVertical();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Year", GUILayout.Width(65f));
							var year = EditorGUILayout.IntField(GUIContent.none, dateTime.Year);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Month", GUILayout.Width(65f));
							var month = EditorGUILayout.IntField(GUIContent.none, dateTime.Month);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Day", GUILayout.Width(65f));
							var day = EditorGUILayout.IntField(GUIContent.none, dateTime.Day);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Hour", GUILayout.Width(65f));
							var hour = EditorGUILayout.IntField(GUIContent.none, dateTime.Hour);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Minute", GUILayout.Width(65f));
							var min = EditorGUILayout.IntField(GUIContent.none, dateTime.Minute);
							EditorGUILayout.EndHorizontal();
							
							EditorGUILayout.BeginHorizontal();
							EditorGUILayout.LabelField("Second", GUILayout.Width(65f));
							var seconds = EditorGUILayout.IntField(GUIContent.none, dateTime.Second);
							EditorGUILayout.EndHorizontal();

							EditorGUILayout.EndVertical();

							if (EditorGUI.EndChangeCheck())
							{
								dateTime = new SerializableDateTime(year, month, day, hour, min, seconds,
									DateTimeKind.Utc);
								property.Fpr("option").Fpr("value").stringValue = JsonUtility.ToJson(dateTime);
								property.serializedObject.ApplyModifiedProperties();
								property.serializedObject.Update();
							}
						}
						
						break;
				}
				
				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUILayout.EndVertical();
		}
	}
}