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
using CarterGames.Standalone.NotionData.Editor.Helpers;
using CarterGames.Standalone.NotionData.Filters;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
	public static class FilterEditorGroup
	{
		private static string TargetId;
		
		
		public static void DrawGroup(SerializedProperty property, string groupId)
		{
			TargetId = groupId;
			
			for (var i = 0; i < EditorWindowFilterGUI.target.Fpr("list").arraySize; i++)
			{
				if (EditorWindowFilterGUI.target.Fpr("list").GetIndex(i).Fpr("key").stringValue != groupId) continue;
				DrawGroupNested(EditorWindowFilterGUI.target.Fpr("list").GetIndex(i));
			}
		}


		private static void DrawGroupNested(SerializedProperty baseProp)
		{
			var data = baseProp.Fpr("value");
			
			EditorGUILayout.BeginVertical("HelpBox");

			if (data.Fpr("filterOptions").arraySize > 0)
			{
				for (var i = 0; i < data.Fpr("filterOptions").arraySize; i++)
				{
					DrawFilter(baseProp, data.Fpr("filterOptions").GetIndex(i), i);
				}

				EditorGUILayout.Space(5f);
			}
			else
			{
				GUI.backgroundColor = Color.red;
				
				if (GUILayout.Button("Delete filter group"))
				{
					for (var i = 0; i < EditorWindowFilterGUI.target.Fpr("list").arraySize; i++)
					{
						if (EditorWindowFilterGUI.target.Fpr("list").GetIndex(i).Fpr("key").stringValue == TargetId)
						{
							EditorWindowFilterGUI.target.Fpr("list").DeleteIndex(i);
							break;
						}

						for (var j = 0; j < EditorWindowFilterGUI.target.Fpr("list").GetIndex(i).Fpr("value").Fpr("filterOptions").arraySize; j++)
						{
							if (EditorWindowFilterGUI.target.Fpr("list").GetIndex(i).Fpr("value").Fpr("filterOptions")
								    .GetIndex(j).Fpr("option").Fpr("value").stringValue == TargetId)
							{
								EditorWindowFilterGUI.target.Fpr("list").GetIndex(i).Fpr("value").Fpr("filterOptions").DeleteIndex(j);
								break;
							}
						}
					}
					
					EditorWindowFilterGUI.target.serializedObject.ApplyModifiedProperties();
					EditorWindowFilterGUI.target.serializedObject.Update();
					return;
				}
				GUI.backgroundColor = Color.white;
			}

			FilterGUIHelper.DrawAddFilterRuleButton(baseProp);

			EditorGUILayout.EndVertical();
		}


		private static void DrawFilter(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			if (index == 0)
			{
				DrawFirst(baseProp, property, index);
			}
			else if (index == 1)
			{
				DrawSecond(baseProp, property, index);
			}
			else
			{
				DrawThirdOnwards(baseProp, property, index);
			}
		}


		private static void DrawFirst(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Where", GUILayout.Width(40f));

			DrawSingleFilter(baseProp, property, index);

			EditorGUILayout.EndHorizontal();
		}


		private static void DrawSecond(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			EditorGUILayout.BeginHorizontal();

			GUI.backgroundColor = Color.yellow;
			if (GUILayout.Button(baseProp.Fpr("value").Fpr("andCheck").boolValue ? "And" : "Or", GUILayout.Width(40f)))
			{
				baseProp.Fpr("value").Fpr("andCheck").boolValue = !baseProp.Fpr("value").Fpr("andCheck").boolValue;
				baseProp.serializedObject.ApplyModifiedProperties();
				baseProp.serializedObject.Update();
			}
			GUI.backgroundColor = Color.white;

			DrawSingleFilter(baseProp, property, index);
			
			EditorGUILayout.EndHorizontal();
		}
		
		
		private static void DrawThirdOnwards(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.LabelField(baseProp.Fpr("value").Fpr("andCheck").boolValue ? "And" : "Or",
				GUILayout.Width(40f));
			EditorGUI.EndDisabledGroup();

			DrawSingleFilter(baseProp, property, index);
			
			EditorGUILayout.EndHorizontal();
		}
		

		private static void DrawSingleFilter(SerializedProperty baseProp, SerializedProperty property, int index)
		{
			switch (property.Fpr("typeName").stringValue)
			{
				case "Rich text":
					FilterEditorRichText.DrawFilterOption(baseProp, property, index);
					break;
				case "CheckBox":
					FilterEditorCheckbox.DrawFilterOption(baseProp, property, index);
					break;
				case "Id":
					FilterEditorId.DrawFilterOption(baseProp, property, index);
					break;
				case "Select":
					FilterEditorSelect.DrawFilterOption(baseProp, property, index);
					break;
				case "Multi Select":
					FilterEditorMultiSelect.DrawFilterOption(baseProp, property, index);
					break;
				case "Number":
					FilterEditorNumber.DrawFilterOption(baseProp, property, index);
					break;
				case "Group":
					DrawGroup(baseProp, property.Fpr("option").Fpr("value").stringValue);
					break;
				case "Status":
					FilterEditorStatus.DrawFilterOption(baseProp, property, index);
					break;
				case "Date":
					FilterEditorDate.DrawFilterOption(baseProp, property, index);
					break;
				default:
					
					if (string.IsNullOrEmpty(property.Fpr("typeName").stringValue))
					{
						if (GUILayout.Button("Select filter type"))
						{
							SearchProviderFilterType.GetProvider().SelectionMade.Add(OnSearchSelectionMade);
							SearchProviderFilterType.GetProvider().Open();

							return;
							void OnSearchSelectionMade(SearchTreeEntry searchTreeEntry)
							{
								SearchProviderFilterType.GetProvider().SelectionMade.Remove(OnSearchSelectionMade);
								
								property.Fpr("typeName").stringValue = ((NotionFilterOption)searchTreeEntry.userData).EditorTypeName;
								EditorWindowFilterGUI.SetDefaultValueForType(property.Fpr("option").Fpr("value"), property.Fpr("typeName").stringValue);

								property.serializedObject.ApplyModifiedProperties();
								property.serializedObject.Update();
							}
						}
					}
					else
					{
						EditorGUILayout.PropertyField(property.Fpr("option").Fpr("propertyName"), GUIContent.none);
						EditorGUILayout.PropertyField(property.Fpr("option").Fpr("comparisonEnumIndex"), GUIContent.none);
						EditorGUILayout.PropertyField(property.Fpr("option").Fpr("value"), GUIContent.none);
					}

					break;
			}
		}
	}
}