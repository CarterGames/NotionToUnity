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

using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData
{
    /// <summary>
    /// A number type Notion property container.
    /// </summary>
    public sealed class NotionPropertyNumber : NotionProperty
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#if UNITY_EDITOR
        /// <summary>
        /// The identifier for the property in json downloaded.
        /// </summary>
        public override string EditorOnly_PropertyIdentifier => "number";
#endif


        /// <summary>
        /// The typed-value, stored in an object so it can be generic without a type required.
        /// </summary>
        protected override object InternalValue { get; set; }
        
        
        /// <summary>
        /// The JSON value of the value this property holds.
        /// </summary>
        public override string JsonValue { get; protected set; }
        
        
        /// <summary>
        /// The raw download Json in-case it is needed.
        /// </summary>
        public override string DownloadText { get; protected set; }


        /// <summary>
        /// The value cast to the type the property is in C#
        /// </summary>
        public double Value => (double) InternalValue;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static NotionPropertyNumber Blank() => new NotionPropertyNumber();
        
        private NotionPropertyNumber() { }
        
        public NotionPropertyNumber(NotionPropertyData data)
        {
            PropertyName = data.propertyName;
            InternalValue = double.Parse(data.jsonValue);
            JsonValue = data.jsonValue;
            DownloadText = data.downloadText;
        }
        
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Methods
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#if UNITY_EDITOR
        /// <summary>
        /// Gets the value from the json for this type.
        /// </summary>
        /// <param name="json">The json to read.</param>
        /// <returns>string (Json)</returns>
        public override string EditorOnly_GetValueJsonFromContainer(JToken json)
        {
            return json["number"].Value<string>();
        }
#endif
    }
}