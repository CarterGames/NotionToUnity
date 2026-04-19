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

using System.Collections.Generic;
using CarterGames.Shared.NotionData.Editor;
using UnityEditor;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    public static partial class NotionDataSettingsProvider
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private const string Path = "Carter Games/Notion Data";
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the settings asset in the project as a SerializedObject.
        /// </summary>
        private static SerializedObject SettingsAssetObject => ScriptableRef.GetAssetDef<NotionDataEditorSettings>().ObjectRef;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Menu Item
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [MenuItem("Tools/Carter Games/Notion Data/Settings", priority = 0)]
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
                    EditorGUI.indentLevel++;
                    EditorGUILayout.Space(10f);
                    
                    DrawInfo();
                    EditorGUILayout.Space(7.5f);
                    
                    EditorGUI.BeginChangeCheck();
                    
                    DrawNotionOptions();
                    EditorGUILayout.Space(7.5f);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        SettingsAssetObject.ApplyModifiedProperties();
                        SettingsAssetObject.Update();
                    }
                    
                    EditorGUILayout.BeginVertical();
                    GUILayout.Space(1.5f);
                    
                    EditorGUILayout.LabelField("Useful Links", EditorStyles.boldLabel);
                    GUILayout.Space(1.5f);
                    
                    DrawButtons();
                    
                    GUILayout.Space(2.5f);
                    EditorGUILayout.EndVertical();
                    
                    EditorGUI.indentLevel--;
                },
                
                keywords = new HashSet<string>(new[]
                {
                    "Notion", "Notion Data", "External Assets", "Tools", "Code", "Scriptable Object", "Carter Games", "Carter"
                })
            };
            
            return provider;
        }
    }
}