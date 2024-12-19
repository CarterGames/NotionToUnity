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

using UnityEditor;

namespace CarterGames.Standalone.NotionData.Editor
{
    public sealed class SortPropertiesWindow : EditorWindow
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
            if (Target == null)
            {
                Target = new SerializedObject(Selection.activeObject);
                if (Target == null) return;
            }

            EditorGUILayout.HelpBox("Edit the sort properties for this Notion data asset below", MessageType.Info);
            
            EditorGUILayout.Space(5f);

            var entry = Target.Fp("sortProperties");
            
            EditorGUILayout.PropertyField(Target.Fp("sortProperties"));
        }
    }
}