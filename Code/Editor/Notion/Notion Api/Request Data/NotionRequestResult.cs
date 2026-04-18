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
using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData.Editor
{
	/// <summary>
	/// A data class that holds the result of a request to the Notion API.
	/// </summary>
	public sealed class NotionRequestResult
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private readonly List<IDictionary<string, JToken>> data;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// The amount of data received.
		/// </summary>
		public int DataCount => data.Count;


		/// <summary>
		/// The json received stored per property name in a list.
		/// </summary>
		public List<IDictionary<string, JToken>> Data => data;
		
		
		/// <summary>
		/// Gets if the response was requested to be silenced or not.
		/// </summary>
		public bool SilentResponse { get; private set; }

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Constructors
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Makes a new response data class instance when called.
		/// </summary>
		/// <param name="data">The data to set.</param>
		/// <param name="silentResponse">Should the response be silenced in the editor?</param>
		public NotionRequestResult(List<IDictionary<string, JToken>> data, bool silentResponse)
		{
			this.data = data;
			SilentResponse = silentResponse;
		}
	}
}