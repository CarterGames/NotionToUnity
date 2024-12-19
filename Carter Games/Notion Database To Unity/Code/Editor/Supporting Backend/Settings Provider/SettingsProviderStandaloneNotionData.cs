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

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
    public static class SettingsProviderStandaloneNotionData
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string Path = "Carter Games/Standalone/Notion Data";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Item
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [MenuItem("Tools/Carter Games/Standalone/Notion Data/Settings", priority = 0)]
        public static void OpenSettings()
        {
            SettingsService.OpenProjectSettings(Path);
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Settings Provider
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Handles the settings window in the engine.
        /// </summary>
        [SettingsProvider]
        public static SettingsProvider Provider()
        {
            var provider = new SettingsProvider(Path, SettingsScope.Project)
            {
                guiHandler = (searchContext) =>
                {
                    GUILayout.Space(7.5f);
                    
                    DrawVersionInfo();
                    GUILayout.Space(5f);
                    DrawAssetOptions();
                    DrawButtons();
                },
                
                keywords = new HashSet<string>(new[]
                {
                    "Notion", "Notion Data", "External Assets", "Tools", "Code", "Scriptable Object", "Carter Games", "Carter"
                })
            };
            
            return provider;
        }

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Draws the version info section on the settings provider.
        /// </summary>
        private static void DrawVersionInfo()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            EditorGUILayout.LabelField("Info", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();
            
            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.LabelField(new GUIContent(NotionMetaData.VersionNumber), new GUIContent(AssetInfo.VersionNumber));
            GUILayout.FlexibleSpace();
            VersionEditorGUI.DrawCheckForUpdatesButton();
            
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.LabelField(new GUIContent(NotionMetaData.ReleaseDate), new GUIContent(AssetInfo.ReleaseDate));

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }


        /// <summary>
        /// Draws the settings options for the asset.
        /// </summary>
        private static void DrawAssetOptions()
        {
            EditorGUILayout.BeginVertical("HelpBox");
            GUILayout.Space(1.5f);
            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            GeneralUtilEditor.DrawHorizontalGUILine();

            EditorGUILayout.PropertyField(ScriptableRef.GetAssetDef<DataAssetEditorGlobalSettings>().ObjectRef.Fp("apiVersion"), NotionMetaData.ApiVersion);
            EditorGUILayout.PropertyField(ScriptableRef.GetAssetDef<DataAssetEditorGlobalSettings>().ObjectRef.Fp("apiReleaseVersion"), NotionMetaData.ApiReleaseVersion);

            GUILayout.Space(1.5f);
            EditorGUILayout.EndVertical();
        }
        
        
        
        /// <summary>
        /// Draws the buttons section of the window.
        /// </summary>
        private static void DrawButtons()
        {
            GUILayout.Space(5f);
            
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("GitHub", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://github.com/CarterGames/NotionToUnity");
            }
            
            if (GUILayout.Button("Support Dev", GUILayout.Height(30), GUILayout.MinWidth(100)))
            {
                Application.OpenURL("https://carter.games/supportdev");
            }

            EditorGUILayout.EndHorizontal();
        }
    }
}