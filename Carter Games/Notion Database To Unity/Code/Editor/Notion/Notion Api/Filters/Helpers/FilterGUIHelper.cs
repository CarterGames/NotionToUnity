using System;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor.Helpers
{
    public static class FilterGUIHelper
    {
	    private static int CurrentNestLevel(SerializedProperty target)
	    {
		    return EditorWindowFilterGUI.target.Fpr("list")
			    .GetIndex(EditorWindowFilterGUI.target.Fpr("list").arraySize - 1)
			    .Fpr("value")
			    .Fpr("nestLevel").intValue;
	    }
	    
	    
        public static void DrawAddFilterRuleButton(SerializedProperty target)
        {
	        if (!GUILayout.Button("+ Add filter rule", GUILayout.Width(125f), GUILayout.Height(22.5f))) return;
	        
	        var nestLevel = CurrentNestLevel(target);
				
	        SearchProviderFilterGroupType.GetProvider(nestLevel).SelectionMade.Clear();
	        SearchProviderFilterGroupType.GetProvider(nestLevel).SelectionMade.Add(OnSearchSelectionMade);
	        SearchProviderFilterGroupType.GetProvider(nestLevel).Open();

	        return;

	        void OnSearchSelectionMade(SearchTreeEntry searchTreeEntry)
	        {
		        SearchProviderFilterGroupType.GetProvider(nestLevel).SelectionMade.Remove(OnSearchSelectionMade);

		        if (int.Parse(searchTreeEntry.userData.ToString()) == 0)
		        {
			        // New rule to group
			        AddNewFilter(target);
		        }
		        else
		        {
			        // New group
			        AddNewGroup(target);
		        }
					
		        EditorWindowFilterGUI.target.serializedObject.ApplyModifiedProperties();
		        EditorWindowFilterGUI.target.serializedObject.Update();
	        }
        }


        private static void AddNewFilter(SerializedProperty target)
        {
	        target.Fpr("value").Fpr("filterOptions").InsertIndex(target.Fpr("value").Fpr("filterOptions").arraySize);
	        var newFilter = target.Fpr("value").Fpr("filterOptions")
		        .GetIndex(target.Fpr("value").Fpr("filterOptions").arraySize - 1);
	        newFilter.Fpr("typeName").stringValue = string.Empty;
	        newFilter.Fpr("option").Fpr("propertyName").stringValue = string.Empty;
	        newFilter.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;
	        newFilter.Fpr("option").Fpr("value").stringValue = string.Empty;
        }


        private static void AddNewGroup(SerializedProperty target)
        {
	        var filtersList = EditorWindowFilterGUI.target.Fpr("list");
	        
	        filtersList.InsertIndex(filtersList.arraySize);
	        filtersList.GetIndex(filtersList.arraySize - 1).Fpr("key").stringValue = Guid.NewGuid().ToString();
	        filtersList.GetIndex(filtersList.arraySize - 1).Fpr("value").Fpr("nestedId").stringValue = target.Fpr("key").stringValue;
	        filtersList.GetIndex(filtersList.arraySize - 1).Fpr("value").Fpr("nestLevel").intValue++;
	        filtersList.GetIndex(filtersList.arraySize - 1).Fpr("value").Fpr("filterOptions").ClearArray();

	        target.Fpr("value").Fpr("filterOptions").InsertIndex(target.Fpr("value").Fpr("filterOptions").arraySize);
	        
	        var newFilter = target.Fpr("value").Fpr("filterOptions").GetIndex(target.Fpr("value").Fpr("filterOptions").arraySize - 1);
	        newFilter.Fpr("typeName").stringValue = "Group";
	        newFilter.Fpr("option").Fpr("propertyName").stringValue = string.Empty;
	        newFilter.Fpr("option").Fpr("comparisonEnumIndex").intValue = 0;
	        newFilter.Fpr("option").Fpr("value").stringValue = filtersList.GetIndex(filtersList.arraySize - 1).Fpr("key").stringValue;
        }
    }
}