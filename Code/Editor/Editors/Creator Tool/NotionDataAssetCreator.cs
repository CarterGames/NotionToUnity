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

using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Creates notion data assets for the user.
    /// </summary>
    public sealed class NotionDataAssetCreator : EditorWindow
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private string dataAssetName;
        private bool makeDataAssetWhenGenerated;
        private string lastSavePath;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Items
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a menu item for the creator to open.
        /// </summary>
        [MenuItem("Tools/Carter Games/Notion Data/Notion Data Asset Creator", priority = 20)]
        private static void ShowWindow()
        {
            var window = GetWindow<NotionDataAssetCreator>(true);
            window.titleContent = new GUIContent("Notion Data Asset Creator");
            window.minSize = new Vector2(450, 180);
            window.maxSize = new Vector2(450, 180);
            window.Show();
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws the GUI.
        /// </summary>
        private void OnGUI()
        {
            GUILayout.Space(4f);

            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("Notion Data Asset Name", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();
            dataAssetName = EditorGUILayout.TextField(dataAssetName);

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();

            GUILayout.Space(4f);

            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);

            EditorGUILayout.LabelField("Will create:", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();
            EditorGUILayout.LabelField(new GUIContent("DataAsset: "),
                new GUIContent("NotionDataAsset" + dataAssetName + ".cs"));
            EditorGUILayout.LabelField(new GUIContent("Data: "), new GUIContent("NotionData" + dataAssetName + ".cs"));

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();

            GUILayout.Space(4f);

            GUI.backgroundColor = string.IsNullOrEmpty(dataAssetName) ? Color.white : Color.green;
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(dataAssetName));
            
            if (GUILayout.Button("Create", GUILayout.Height(25f)))
            {
                CreateDataFile();
                CreateAssetFile(out string path);
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                dataAssetName = string.Empty;

                if (EditorUtility.DisplayDialog("Notion Data Asset Creator",
                        "Would you like to open the newly created files for editing?", "Yes", "No"))
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
                }
            }
            
            EditorGUI.EndDisabledGroup();
        }


        /// <summary>
        /// Creates the asset file for the notion data class the user is making.
        /// </summary>
        /// <param name="filePath">The path to create at.</param>
        private void CreateAssetFile(out string filePath)
        {
            filePath = EditorUtility.SaveFilePanelInProject("Save New Notion Data Asset Class",
                $"NotionDataAsset{dataAssetName}", "cs", "", lastSavePath);
            
            var script = AssetDatabase.FindAssets($"t:Script {nameof(NotionDataAssetCreator)}")[0];
            var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);
            pathToTextFile = pathToTextFile.Replace("NotionDataAssetCreator.cs", "NotionDataAssetTemplate.txt");
            
            TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile);
            template = new TextAsset(template.text);
            var replace = template.text.Replace("%DataAssetName%", "NotionDataAsset" + dataAssetName);
            replace = replace.Replace("%DataTypeName%", "NotionData" + dataAssetName);

            File.WriteAllText(filePath, replace);
            EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
        }


        /// <summary>
        /// Creates the data file for the notion data class the user is making.
        /// </summary>
        private void CreateDataFile()
        {
            lastSavePath = EditorUtility.SaveFilePanelInProject("Save New Notion Data Class",
                $"NotionData{dataAssetName}", "cs", "");
            
            var script = AssetDatabase.FindAssets($"t:Script {nameof(NotionDataAssetCreator)}")[0];
            var pathToTextFile = AssetDatabase.GUIDToAssetPath(script);
            pathToTextFile = pathToTextFile.Replace("NotionDataAssetCreator.cs", "NotionDataTemplate.txt");
            
            TextAsset template = AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile);
            template = new TextAsset(template.text);
            var replace = template.text.Replace("%DataTypeName%", "NotionData" + dataAssetName);

            File.WriteAllText(lastSavePath, replace);
            EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
        }
    }
}