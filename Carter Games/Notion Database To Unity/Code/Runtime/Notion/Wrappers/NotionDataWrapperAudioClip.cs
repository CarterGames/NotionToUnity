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
using UnityEngine;

namespace CarterGames.Standalone.NotionData
{
	/// <summary>
	/// A wrapper base class for converting a notion database property into an audio clip by its name.
	/// </summary>
    [Serializable]
    public class NotionDataWrapperAudioClip : NotionDataWrapper
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// The value stored in the wrapper.
        /// </summary>
        public AudioClip Value => (AudioClip) value;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        public NotionDataWrapperAudioClip(string id) : base(id)
        {
	        Assign<AudioClip>();
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Operator
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

        /// <summary>
        /// Converts the wrapper to its implementation type.
        /// </summary>
        /// <param name="dataWrapper">The wrapper to convert.</param>
        /// <returns>The value of the wrapper.</returns>
        public static implicit operator AudioClip(NotionDataWrapperAudioClip dataWrapper)
        {
	        return dataWrapper.Value;
        }


        /// <summary>
        /// Converts the type to thr wrapper.
        /// </summary>
        /// <param name="reference">The value to convert.</param>
        /// <returns>The wrapper with the value.</returns>
        public static implicit operator NotionDataWrapperAudioClip(AudioClip reference)
        {
	        return new NotionDataWrapperAudioClip(reference.name);
        }
    }
}