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
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections.Generic;
using CarterGames.Shared.NotionData;
using CarterGames.NotionData.Filters;
using CarterGames.NotionData.ThirdParty;

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
		
		
		/// <summary>
		/// The url for the call.
		/// </summary>
		public string Url => $"https://api.notion.com/{NotionDataAccessor.GetAsset<AssetEditorGlobalSettings>().NotionApiVersion.ToString()}/databases/{databaseId}/query";
		
		
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
		public void AppendResultData(List<KeyValuePair<string, JSONNode>> data)
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