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
	/// A wrapper base class for converting a notion database property into a sprite by its name.
	/// </summary>
    [Serializable]
    public class NotionDataWrapperSprite : NotionDataWrapper<Sprite>
    {
	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Constructors
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

	    public NotionDataWrapperSprite(string id) : base(id) {}
        
	    /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
	    |   Operator
	    ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */

	    /// <summary>
	    /// Converts the wrapper to its implementation type.
	    /// </summary>
	    /// <param name="dataWrapper">The wrapper to convert.</param>
	    /// <returns>The value of the wrapper.</returns>
	    public static implicit operator Sprite(NotionDataWrapperSprite dataWrapper)
	    {
		    return dataWrapper.Value;
	    }


	    /// <summary>
	    /// Converts the type to thr wrapper.
	    /// </summary>
	    /// <param name="reference">The value to convert.</param>
	    /// <returns>The wrapper with the value.</returns>
	    public static implicit operator NotionDataWrapperSprite(Sprite reference)
	    {
		    return new NotionDataWrapperSprite(reference.name);
	    }
	    
	    
	    /// <summary>
	    /// Assigns the reference when called.
	    /// </summary>
	    protected override void Assign()
	    {
#if UNITY_EDITOR
		    if (!string.IsNullOrEmpty(id))
		    {
			    var asset = UnityEditor.AssetDatabase.FindAssets(id);
                
			    if (asset.Length > 0)
			    {
				    var path = UnityEditor.AssetDatabase.GUIDToAssetPath(asset[0]);
				    value = UnityEditor.AssetDatabase.LoadAssetAtPath<Sprite>(path);
			    }
			    else
			    {
				    Debug.LogWarning($"Unable to find a reference with the name {id}");
			    }
		    }
		    else
		    {
			    Debug.LogWarning("Unable to assign a reference, the id was empty.");
		    }
#endif
	    }
    }
}