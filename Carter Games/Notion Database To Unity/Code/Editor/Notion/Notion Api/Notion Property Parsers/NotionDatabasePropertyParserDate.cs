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
using CarterGames.Standalone.NotionData.ThirdParty;
using UnityEngine;

namespace CarterGames.Standalone.NotionData.Editor
{
	public class NotionDatabasePropertyParserDate : INotionDatabasePropertyParser
	{
		public string PropertyIdentifier => "date";
		

		public string GetJsonValue(JSONNode json)
		{
			var entry = json["date"]["start"];
			DateTime dateTime;

			if (string.IsNullOrEmpty(entry)) return string.Empty;
			
			
			var dateString = entry.ToString().Split('T')[0].Replace("T", string.Empty).Replace("\"", string.Empty);
			var year = int.Parse(dateString.Split('-')[0]);
			var month = int.Parse(dateString.Split('-')[1]);
			var day = int.Parse(dateString.Split('-')[2]);
			
			
			if (entry.ToString().Contains("T"))
			{
				var timeString = entry.ToString().Split('T')[1].Replace("T", string.Empty).Replace("\"", string.Empty);
				var hour = int.Parse(timeString.Split(':')[0]);
				var minute = int.Parse(timeString.Split(':')[1]);
				var second = int.Parse(timeString.Split(':')[2].Substring(0, 2));
				var milliSecond = int.Parse(timeString.Split('.')[1].Substring(0, 3));
				
				dateTime = new DateTime(year, month, day, hour, minute, second, milliSecond);
			}
			else
			{
				dateTime = new DateTime(year, month, day);
			}


			return JsonUtility.ToJson(new SerializableDateTime(dateTime));
		}
	}
}