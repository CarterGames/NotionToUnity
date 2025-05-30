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

using System;
using CarterGames.Shared.NotionData;
using UnityEngine;
using CarterGames.Standalone.NotionData.ThirdParty;

namespace CarterGames.Standalone.NotionData.Editor
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
        public NotionRequestError(NdAsset asset, JSONNode errorJson)
        {
            this.asset = asset;

            try
            {
                errorCode = errorJson["errorCode"].AsInt;
            }
#pragma warning disable 0168
            catch (Exception e)
#pragma warning restore
            {
                errorCode = 0;
            }

            code = errorJson["code"];
            message = errorJson["message"];
        }
    }
}