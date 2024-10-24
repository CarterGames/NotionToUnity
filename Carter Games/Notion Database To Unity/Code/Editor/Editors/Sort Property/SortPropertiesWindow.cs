using System;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
    public class SortPropertiesWindow : EditorWindow
    {
        private static SerializedObject Target { get; set; }
        
        
        public static void OpenWindow(SerializedObject target)
        {
            Target = target;
            
            if (HasOpenInstances<SortPropertiesWindow>())
            {
                FocusWindowIfItsOpen<SortPropertiesWindow>();
            }
            else
            {
                GetWindow<SortPropertiesWindow>(true, "Edit Sort Properties");
            }
        }


        private void OnGUI()
        {
            if (Target == null) return;

            EditorGUILayout.HelpBox("Edit the sort properties for this Notion data asset below", MessageType.Info);
            
            EditorGUILayout.Space(5f);

            var entry = Target.Fp("sortProperties");
            
            EditorGUILayout.PropertyField(Target.Fp("sortProperties"));
        }
    }
}