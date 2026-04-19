using CarterGames.Shared.NotionData.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    public static partial class NotionDataSettingsProvider
    {
        private static readonly GUIContent ApiReleaseVersion = new GUIContent("API release version:", "The API release version to use in Notion API calls.");
        private static readonly GUIContent DownloadTimeout = new GUIContent("Download Timeout:", "The duration until a download attempt will auto-fail.");
        private static readonly GUIContent IgnorePrefix = new GUIContent("Property ignore prefix:", "Defines the prefix to check against to ignore some notion properties.");
        
        
        /// <summary>
        /// Draws the Notion settings for the asset.
        /// </summary>
        private static void DrawNotionOptions()
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(1.5f);
            EditorGUILayout.LabelField("Notion Settings", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.PropertyField(ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef.Fp("apiReleaseVersion"), ApiReleaseVersion);
            EditorGUILayout.PropertyField(ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef.Fp("downloadTimeout"), DownloadTimeout);
            EditorGUILayout.PropertyField(ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef.Fp("ignorePropertyPrefix"), IgnorePrefix);

            if (EditorGUI.EndChangeCheck())
            {
                ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef.ApplyModifiedProperties();
                ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef.Update();
            }
            
            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
    }
}