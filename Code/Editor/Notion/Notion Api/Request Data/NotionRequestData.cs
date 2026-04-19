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
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CarterGames.NotionData.Editor
{
	/// <summary>
	/// A data class for the info of a request as it is being processed.
	/// </summary>
	public sealed class NotionRequestData
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private readonly NdAsset requestingAsset;
		private readonly string databaseId;
		private readonly string apiKey;
		private readonly NotionSortProperty[] sorts;
		private readonly NotionFilterContainer filter;
		private NotionRequestResult resultData;
		private readonly bool silentCall;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// The data asset the request is for.
		/// </summary>
		public NdAsset RequestingAsset => requestingAsset;


		public string DatabaseId => databaseId;
		
		
		/// <summary>
		/// The api key for the database to be accessed with.
		/// </summary>
		public string ApiKey => apiKey;
		
		
		/// <summary>
		/// The sorting to apply on requesting the data from the database.
		/// </summary>
		public NotionSortProperty[] Sorts => sorts;
		
		
		/// <summary>
		/// The filtering to apply on requesting the data from the database.
		/// </summary>
		public NotionFilterContainer Filter => filter;
		
		
		/// <summary>
		/// The result of the request.
		/// </summary>
		public NotionRequestResult ResultData => resultData;
		
		
		/// <summary>
		/// Should the dialogues show for this request?
		/// </summary>
		public bool ShowResponseDialogue => !silentCall;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Constructor
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Creates a new request data class instance when called.
		/// </summary>
		/// <param name="requestingAsset">The asset to use.</param>
		/// <param name="databaseId">The database id to get.</param>
		/// <param name="apiKey">The api key to get.</param>
		/// <param name="sorts">The sorting properties to apply.</param>
		/// <param name="silentResponse">Should the response from the request be hidden from the user? DEF = false</param>
		public NotionRequestData(NdAsset requestingAsset, string databaseId, string apiKey, NotionSortProperty[] sorts, NotionFilterContainer filter, bool silentResponse = false)
		{
			this.requestingAsset = requestingAsset;
			this.databaseId = databaseId;
			this.apiKey = apiKey;
			this.sorts = sorts;
			this.filter = filter;
			silentCall = silentResponse;
		}
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Methods
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

		/// <summary>
		/// Appends the data with more info when called (when over 100 entries etc).
		/// </summary>
		/// <param name="data">The data to add.</param>
		public void AppendResultData(List<IDictionary<string, JToken>> data)
		{
			if (resultData == null)
			{
				resultData = new NotionRequestResult(data, silentCall);
			}
			else
			{
				resultData.Data.AddRange(data);
			}
		}
	}
}