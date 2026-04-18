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
using UnityEditor;

namespace CarterGames.Shared.NotionData.Editor
{
	/// <summary>
	/// Handles the creation and referencing of the asset index file.
	/// </summary>
	public sealed class ScriptableAssetDefNdAssetIndex : IScriptableAssetDef<NotionDataAssetIndex>
	{
		// IScriptableAssetDef Implementation
		/* ────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		private static NotionDataAssetIndex cache;
		private static SerializedObject objCache;

		public Type AssetType => typeof(NotionDataAssetIndex);
		public string DataAssetFileName => $"[{NdAssetVersionData.AssetName}] ND Asset Index.asset";
		public string DataAssetFilter => $"t:{typeof(NotionDataAssetIndex).FullName} name={DataAssetFileName}";
		public string DataAssetPath => $"{ScriptableRef.FullPathResources}{DataAssetFileName}";

		public NotionDataAssetIndex AssetRef => ScriptableRef.GetOrCreateAsset(this, ref cache);
		public SerializedObject ObjectRef => ScriptableRef.GetOrCreateAssetObject(this, ref objCache);
		
		public void TryCreate()
		{
			ScriptableRef.GetOrCreateAsset(this, ref cache);
		}

		public void OnCreated() { }
	}
}