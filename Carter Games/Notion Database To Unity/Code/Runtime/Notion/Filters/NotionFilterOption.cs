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

using System;
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Filters
{
	[Serializable]
	public class NotionFilterOption
	{
		[SerializeField] protected string propertyName;
		[SerializeField] protected string value;
		[SerializeField] protected int comparisonEnumIndex;
		[SerializeField] protected bool isRollup;
		[SerializeField] protected int rollupComparisonEnumIndex;


		public string PropertyName => propertyName;
		public string Value => value;
		public int ComparisonEnumIndex => comparisonEnumIndex;
		public bool IsRollup => isRollup;
		public int RollupComparisonEnumIndex => rollupComparisonEnumIndex;
		public virtual string EditorTypeName { get; }
		
		

		public virtual JSONObject ToJson()
		{
			return null;
		}
	}
}