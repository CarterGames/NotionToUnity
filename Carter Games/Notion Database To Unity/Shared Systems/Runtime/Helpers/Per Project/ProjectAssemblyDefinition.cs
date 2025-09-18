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
 * FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Reflection;

namespace CarterGames.Shared.NotionData
{
    /// <summary>
    /// Handles getting all the assembles that are internal to the asset.
    /// </summary>
    public static class ProjectAssemblyDefinition
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        private static Assembly[] cacheProjectEditorAssemblies = Array.Empty<Assembly>();
        private static Assembly[] cacheProjectRuntimeAssemblies = Array.Empty<Assembly>();
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The editor space assemblies
        /// </summary>
        public static Assembly[] ProjectEditorAssemblies
        {
            get
            {
                if (cacheProjectEditorAssemblies.Length > 0) return cacheProjectEditorAssemblies;
                
                cacheProjectEditorAssemblies =  new Assembly[]
                {
                    Assembly.Load("CarterGames.NotionData.Editor"),
                    Assembly.Load("CarterGames.NotionData.Runtime"),
                    Assembly.Load("CarterGames.Shared.NotionData.Editor"),
                    Assembly.Load("CarterGames.Shared.NotionData")
                };
                
                return cacheProjectEditorAssemblies;
            }
        }


        /// <summary>
        /// The runtime space assemblies
        /// </summary>
        public static Assembly[] ProjectRuntimeAssemblies
        {
            get
            {
                if (cacheProjectRuntimeAssemblies.Length > 0) return cacheProjectRuntimeAssemblies;

                cacheProjectRuntimeAssemblies = new Assembly[]
                {
                    Assembly.Load("CarterGames.NotionData.Runtime"),
                    Assembly.Load("CarterGames.Shared.NotionData"),
                };

                return cacheProjectRuntimeAssemblies;
            }
        }
    }
}