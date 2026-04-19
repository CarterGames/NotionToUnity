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
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Filters
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
		
		

		public virtual JObject ToJson()
		{
			return null;
		}
	}
}