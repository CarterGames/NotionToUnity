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