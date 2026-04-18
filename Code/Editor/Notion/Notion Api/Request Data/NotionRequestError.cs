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
using CarterGames.Shared.NotionData;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace CarterGames.NotionData.Editor
{
    /// <summary>
    /// Handles a wrapper class for notion errors with the asset the error's on.
    /// </summary>
    [Serializable]
    public sealed class NotionRequestError
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private NdAsset asset;
        [SerializeField] private int errorCode;
        [SerializeField] private string code;
        [SerializeField] private string message;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The asset related to the error.
        /// </summary>
        public NdAsset Asset => asset;
        
        
        /// <summary>
        /// The HTTP error code response to the request.
        /// </summary>
        public int ErrorCode => errorCode;
        
        
        /// <summary>
        /// The code for the specific error the user ran into.
        /// </summary>
        public string Error => code;
        
        
        /// <summary>
        /// The message Notion sent back in relation to the error, usually gives decent context to the issue.
        /// </summary>
        public string Message => message;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new error class instance when called.
        /// </summary>
        /// <param name="asset">The related asset.</param>
        /// <param name="errorJson">The json to read for the error message.</param>
        public NotionRequestError(NdAsset asset, JObject errorJson)
        {
            this.asset = asset;

            try
            {
                errorCode = errorJson["errorCode"].Value<int>();
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                errorCode = 0;
            }

            code = errorJson["code"].Value<string>();
            message = errorJson["message"].Value<string>();
        }
    }
}