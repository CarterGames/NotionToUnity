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
using UnityEngine;

namespace CarterGames.NotionData
{
	/// <summary>
	/// A class to define a sort property.
	/// </summary>
	[Serializable]
	public class NotionSortProperty
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		[SerializeField] private string propertyName;
		[SerializeField] private bool ascending;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// The name of the property to sort.
		/// </summary>
		public string PropertyName => propertyName;
		
		
		/// <summary>
		/// Should it sort ascending.
		/// </summary>
		public bool SortAscending => ascending;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Constructors
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Makes a new sort property when called.
		/// </summary>
		/// <param name="property">The property name to sort by.</param>
		/// <param name="sortAscending">Should it sort ascending.</param>
		public NotionSortProperty(string property, bool sortAscending)
		{
			propertyName = property;
			ascending = sortAscending;
		}
	}
}