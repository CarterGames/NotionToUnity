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

namespace CarterGames.Standalone.NotionData
{
    /// <summary>
    /// A helper class to create different Notion property types. Not intended for direct access.
    /// </summary>
    /// <remarks>Use NotionProperty.cs class to use these methods as intended.</remarks>
    public static class NotionPropertyFactory
    {
        public static NotionPropertyCheckbox Checkbox(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyCheckbox(bool.Parse(jsonValue), jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyDate Date(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyDate((SerializableDateTime) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyMultiSelect MultiSelect(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyMultiSelect((string[]) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertySelect Select(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertySelect((string) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyRichText RichText(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyRichText((string) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyTitle Title(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyTitle((string) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyStatus Status(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyStatus((string) value, jsonValue, rawDownloadText);
        }
        
        
        public static NotionPropertyNumber Number(object value, string jsonValue, string rawDownloadText)
        {
            return new NotionPropertyNumber(double.Parse(jsonValue), jsonValue, rawDownloadText);
        }
    }
}