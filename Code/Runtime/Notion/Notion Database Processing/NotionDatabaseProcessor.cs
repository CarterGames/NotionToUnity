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
using System.Collections.Generic;

namespace CarterGames.NotionData
{
	/// <summary>
	/// Implement to alter the method at which data is parser to an asset from the data downloaded from Notion.
	/// </summary>
	/// <typeparam name="T">The type to parse to, normally the NotionDataAsset genetic passed argument type.</typeparam>
	[Serializable]
	public abstract class NotionDatabaseProcessor
	{
		public abstract List<object> Process<T>(NotionDatabaseQueryResult result) where T : new();
	}
}