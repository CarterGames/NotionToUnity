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
using CarterGames.Standalone.NotionData.Filters;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
	public class EditorWindowFilterGUI : EditorWindow
	{
		private static string Id { get; set; }
		
		public static SerializedProperty target;

		
		private static Vector2 ScrollPos
		{
			get => SessionState.GetVector3(Id, Vector3.zero);
			set => SessionState.SetVector3(Id,value);
		}
		
		
		public static void OpenWindow(SerializedObject targetSerializedObject)
		{
			target = targetSerializedObject.Fp("filters").Fpr("filterGroups");
			Id = $"NotionData_FilterGUI_{targetSerializedObject.targetObject.GetInstanceID().ToString()}";
			var window = GetWindow<EditorWindowFilterGUI>(true, "Edit Filters");
			window.Show();
		}


		private void OnGUI()
		{
			if (EditorApplication.isCompiling || target == null)
			{
				target = new SerializedObject(Selection.activeObject).Fp("filters").Fpr("filterGroups");
				if (target == null) return;
			}
			
			EditorGUILayout.HelpBox("Edit the filters applied to the query when downloading here.", MessageType.Info);
			EditorGUILayout.Space(5f);

			ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);
			
			if (target.Fpr("list").arraySize <= 0)
			{
				if (GUILayout.Button("Add filtering"))
				{
					target.Fpr("list").InsertIndex(0);
					target.Fpr("list").GetIndex(0).Fpr("key").stringValue = Guid.NewGuid().ToString();
					target.serializedObject.ApplyModifiedProperties();
					target.serializedObject.Update();
				}
			}
			else
			{
				for (var i = 0; i < target.Fpr("list").arraySize; i++)
				{
					var entry = target.Fpr("list").GetIndex(i);

					if (entry.Fpr("value").Fpr("filterOptions").arraySize <= 0 && i == 0)
					{
						if (GUILayout.Button("Add Filter Option"))
						{
							SearchProviderFilterType.GetProvider().SelectionMade.Clear();
							SearchProviderFilterType.GetProvider().SelectionMade.Add(OnSearchSelectionMade);
							SearchProviderFilterType.GetProvider().Open();

							return;
							void OnSearchSelectionMade(SearchTreeEntry searchTreeEntry)
							{
								SearchProviderFilterType.GetProvider().SelectionMade.Remove(OnSearchSelectionMade);
								
								entry.Fpr("value").Fpr("filterOptions").InsertIndex(entry.Fpr("value").Fpr("filterOptions").arraySize);
								var newFilter = entry.Fpr("value").Fpr("filterOptions").GetIndex(entry.Fpr("value").Fpr("filterOptions").arraySize - 1);
								newFilter.Fpr("typeName").stringValue = ((NotionFilterOption)searchTreeEntry.userData).EditorTypeName;
								newFilter.Fpr("option").Fpr("propertyName").stringValue = string.Empty;
								newFilter.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;

								SetDefaultValueForType(newFilter.Fpr("option").Fpr("value"),
									newFilter.Fpr("typeName").stringValue);
								
								newFilter.serializedObject.ApplyModifiedProperties();
								newFilter.serializedObject.Update();
							}
						}
					}
					else
					{
						// Stops nested groups from displaying as normal groups.
						if (!string.IsNullOrEmpty(target.Fpr("list").GetIndex(i).Fpr("value").Fpr("nestedId").stringValue)) continue;
						
						EditorGUILayout.BeginVertical("HelpBox");
						EditorGUILayout.Space(1f);
						
						FilterGroupEditor.DrawGroup(entry);
						
						EditorGUILayout.Space(5f);
						
						if (GUILayout.Button("+ Add filter rule", GUILayout.Width(125f), GUILayout.Height(22.5f)))
						{
							SearchProviderFilterGroupType.GetProvider(target.Fpr("list").GetIndex(i).Fpr("value").Fpr("nestLevel").intValue).SelectionMade.Clear();
							SearchProviderFilterGroupType.GetProvider(target.Fpr("list").GetIndex(i).Fpr("value").Fpr("nestLevel").intValue).SelectionMade.Add(OnSearchSelectionMade);
							SearchProviderFilterGroupType.GetProvider(target.Fpr("list").GetIndex(i).Fpr("value").Fpr("nestLevel").intValue).Open();
							
							return;

							void OnSearchSelectionMade(SearchTreeEntry searchTreeEntry)
							{
								SearchProviderFilterGroupType.GetProvider(target.Fpr("list").GetIndex(i).Fpr("value").Fpr("nestLevel").intValue).SelectionMade.Remove(OnSearchSelectionMade);

								if (int.Parse(searchTreeEntry.userData.ToString()) == 0)
								{
									// New rule to group
									entry.Fpr("value").Fpr("filterOptions")
										.InsertIndex(entry.Fpr("value").Fpr("filterOptions").arraySize);
									var newFilter = entry.Fpr("value").Fpr("filterOptions")
										.GetIndex(entry.Fpr("value").Fpr("filterOptions").arraySize - 1);
									newFilter.Fpr("typeName").stringValue = string.Empty;
									newFilter.Fpr("option").Fpr("propertyName").stringValue = string.Empty;
									newFilter.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;
									newFilter.Fpr("option").Fpr("value").stringValue = string.Empty;
								}
								else
								{
									// New group
									target.Fpr("list").InsertIndex(target.Fpr("list").arraySize);
									target.Fpr("list").GetIndex(target.Fpr("list").arraySize - 1).Fpr("key").stringValue = Guid.NewGuid().ToString();
									target.Fpr("list").GetIndex(target.Fpr("list").arraySize - 1).Fpr("value")
										.Fpr("nestedId").stringValue = entry.Fpr("key").stringValue;
									target.Fpr("list").GetIndex(target.Fpr("list").arraySize - 1).Fpr("value")
										.Fpr("nestLevel").intValue++;
									target.Fpr("list").GetIndex(target.Fpr("list").arraySize - 1).Fpr("value")
										.Fpr("filterOptions").ClearArray();
									
									entry.Fpr("value").Fpr("filterOptions")
										.InsertIndex(entry.Fpr("value").Fpr("filterOptions").arraySize);
									var newFilter = entry.Fpr("value").Fpr("filterOptions")
										.GetIndex(entry.Fpr("value").Fpr("filterOptions").arraySize - 1);
									newFilter.Fpr("typeName").stringValue = "Group";
									newFilter.Fpr("option").Fpr("propertyName").stringValue = string.Empty;
									newFilter.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;
									newFilter.Fpr("option").Fpr("value").stringValue = target.Fpr("list").GetIndex(target.Fpr("list").arraySize - 1).Fpr("key").stringValue;
									
									target.serializedObject.ApplyModifiedProperties();
									target.serializedObject.Update();
								}
								


								target.serializedObject.ApplyModifiedProperties();
								target.serializedObject.Update();
							}
						}
						
						EditorGUILayout.Space(1f);
						EditorGUILayout.EndVertical();
					}
				}
			}
			
			EditorGUILayout.EndScrollView();
		}


		public static void SetDefaultValueForType(SerializedProperty property, string typeName)
		{
			switch (typeName)
			{
				case "CheckBox":
					property.stringValue = "false";
					break;
				case "Id":
				case "Number":
					property.stringValue = "0";
					break;
				case "Multi Select":
					property.stringValue = JsonUtility.ToJson(new NotionFilterMultiSelectList());
					break;
				case "Date":
					property.stringValue = JsonUtility.ToJson(new SerializableDateTime(0,0,0,0,0,0, DateTimeKind.Utc));
					break;
				default:
					property.stringValue = string.Empty;
					break;
			}
		}
	}
}