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
using System.Linq;
using UnityEngine;

namespace CarterGames.NotionData
{
    /// <summary>
    /// A class that contains the result of a notion database query.
    /// </summary>
    [Serializable]
    public sealed class NotionDatabaseQueryResult
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Fields
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        [SerializeField] private List<NotionDatabaseRow> dataDownloaded;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// The rows downloaded from notion for use.
        /// </summary>
        public List<NotionDatabaseRow> Rows => dataDownloaded;
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        /// <summary>
        /// Makes a new instance of the class with the data downloaded.
        /// </summary>
        /// <param name="data">The data to apply.</param>
        public NotionDatabaseQueryResult(List<NotionDatabaseRow> data)
        {
            // Reverses the list as it is back to front when downloaded.
            dataDownloaded = data.AsEnumerable().ToList();
        }
    }
}