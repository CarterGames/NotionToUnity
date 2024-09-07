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

using System.IO;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
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
        [MenuItem("Tools/Carter Games/Standalone/Notion Data/Asset Creator", priority = 20)]
        private static void ShowWindow()
        {
            var window = GetWindow<NotionDataAssetCreator>();
            window.titleContent = new GUIContent("Notion Data Asset Creator");
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

            EditorGUILayout.LabelField("Data Asset Name", EditorStyles.boldLabel);
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

            if (GUILayout.Button("Create"))
            {
                CreateDataFile();
                CreateAssetFile(out string path);
                
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
                
                EditorUtility.RequestScriptReload();

                if (EditorUtility.DisplayDialog("Notion Data Asset Creator",
                        "Would you like to open the newly created files for editing?", "Yes", "No"))
                {
                    AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
                }
            }
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
            replace = replace.Replace("%DataTypeName%", "Data" + dataAssetName);

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
            var replace = template.text.Replace("%DataTypeName%", "Data" + dataAssetName);

            File.WriteAllText(lastSavePath, replace);
            EditorUtility.SetDirty(AssetDatabase.LoadAssetAtPath<TextAsset>(pathToTextFile));
        }
    }
}