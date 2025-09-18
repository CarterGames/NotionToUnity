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

using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData
{
    /// <summary>
    /// A title type Notion property container.
    /// </summary>
    public sealed class NotionPropertyUrl : NotionProperty
    {
        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Properties
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
#if UNITY_EDITOR
        /// <summary>
        /// The identifier for the property in json downloaded.
        /// </summary>
        public override string EditorOnly_PropertyIdentifier => "url";
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
        public string Value => (string) InternalValue;

        /* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
        |   Constructors
        ───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
        
        public static NotionPropertyUrl Blank() => new NotionPropertyUrl();
        
        private NotionPropertyUrl() { }
        
        public NotionPropertyUrl(NotionPropertyData data)
        {
            PropertyName = data.propertyName;
            InternalValue = data.valueForType.ToString();
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
            return json["url"].Value<string>();
        }
#endif
    }
}