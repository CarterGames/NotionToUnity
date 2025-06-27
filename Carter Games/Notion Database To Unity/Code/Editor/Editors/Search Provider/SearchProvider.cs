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
using CarterGames.Shared.NotionData;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    // Implement to make a search provider for something....
    // You still have to have a way to open it, but it will show the values entered.
    public abstract class SearchProvider<T> : ScriptableObject, ISearchWindowProvider
    {
        public readonly Evt<SearchTreeEntry> SelectionMade = new Evt<SearchTreeEntry>();
        
        
        public abstract string ProviderTitle { get; }
        public abstract List<SearchGroup<T>> GetEntriesToDisplay();
        public List<T> ToExclude { get; set; } = new List<T>();
        

        public void Open()
        {
            ToExclude.Clear();
            
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), this);
        }
        
        
        public void Open(T currentValue)
        {
            ToExclude.Clear();
            ToExclude.Add(currentValue);
            
            SearchWindow.Open(new SearchWindowContext(GUIUtility.GUIToScreenPoint(Event.current.mousePosition)), this);
        }
        
        
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var searchList = new List<SearchTreeEntry>();
            
            searchList.Add(new SearchTreeGroupEntry(new GUIContent(ProviderTitle), 0));
            searchList.AddRange(AdditionalEntries);

            return searchList;
        }
        
        
        private List<SearchTreeEntry> AdditionalEntries
        {
            get
            {
                var list = new List<SearchTreeEntry>();

                foreach (var entries in GetEntriesToDisplay())
                {
                    if (!entries.IsValidGroup)
                    {
                        foreach (var value in entries.Values)
                        {
                            if (ToExclude.Contains(value.Value)) continue;
                            list.Add(SearchHelper.CreateEntry(value.Key, 1, value.Value));
                        }
                    }
                    else
                    {
                        list.Add(SearchHelper.CreateGroup(entries.Key, 1));
                        
                        foreach (var value in entries.Values)
                        {
                            if (ToExclude.Contains(value.Value)) continue;
                            list.Add(SearchHelper.CreateEntry(value.Key, 2, value.Value));
                        }
                    }
                }

                return list;
            }
        }
        
        
        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            if (searchTreeEntry == null) return false;
            SelectionMade.Raise(searchTreeEntry);
            return true;
        }
    }
}