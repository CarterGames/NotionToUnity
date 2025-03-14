﻿/*
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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Common
{
    /// <summary>
    /// A helper class for assembly related logic.
    /// </summary>
    public static class AssemblyHelper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Gets the number of classes of the requested type in the project.
        /// </summary>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>The total in the project.</returns>
        public static int CountClassesOfType<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                
            return assemblies.SelectMany(x => x.GetTypes())
                .Count(x => x.IsClass && typeof(T).IsAssignableFrom(x));
        }
        
        
        /// <summary>
        /// Gets the number of classes of the requested type in the project.
        /// </summary>
        /// <param name="assemblies">The assemblies to check through.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>The total in the project.</returns>
        public static int CountClassesOfType<T>(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes())
                .Count(x => x.IsClass && typeof(T).IsAssignableFrom(x));
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<T> GetClassesOfType<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName)
                .Select(type => (T)Activator.CreateInstance(type));
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<Type> GetClassesNamesOfType<T>()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName);
        }
        
        
        public static IEnumerable<Type> GetClassesNamesOfBaseType(Type baseType)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            
            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && x.BaseType is {IsConstructedGenericType: true} && x.FullName != baseType.FullName)
                .Where(t => baseType == t.BaseType.GetGenericTypeDefinition());
        }
        
        
        /// <summary>
        /// Gets all the classes of the entered type in the project.
        /// </summary>
        /// <param name="assemblies">The assemblies to check through.</param>
        /// <typeparam name="T">The type to find.</typeparam>
        /// <returns>All the implementations of the entered class.</returns>
        public static IEnumerable<T> GetClassesOfType<T>(params Assembly[] assemblies)
        {
            return assemblies.SelectMany(x => x.GetTypes())
                .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x) && x.FullName != typeof(T).FullName)
                .Select(type => (T)Activator.CreateInstance(type));
        }
    }
}