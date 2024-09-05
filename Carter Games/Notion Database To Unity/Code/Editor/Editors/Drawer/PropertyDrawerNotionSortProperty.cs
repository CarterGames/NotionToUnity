using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor.Custom_Editors.Drawer
{
	[CustomPropertyDrawer(typeof(NotionSortProperty))]
	public class PropertyDrawerNotionSortProperty : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
            
			EditorGUI.BeginChangeCheck();

			var indent = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var left = new Rect(position.x, position.y, position.width - 120f, EditorGUIUtility.singleLineHeight);
			var right = new Rect(position.x + position.width - 17.5f, position.y, 120f, EditorGUIUtility.singleLineHeight);
			
			EditorGUI.PropertyField(left, property.Fpr("propertyName"), GUIContent.none);
			EditorGUI.PropertyField(right, property.Fpr("ascending"), GUIContent.none);

			if (EditorGUI.EndChangeCheck())
			{
				property.serializedObject.ApplyModifiedProperties();
			}
            
			EditorGUI.indentLevel = indent;
			EditorGUI.EndProperty();
		}


		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}
	}
}