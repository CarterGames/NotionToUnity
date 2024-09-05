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
	/// A helper class to reference date time in a serialized fashion for use in notion data assets.
	/// </summary>
	[Serializable]
	public class SerializableDateTime
	{
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Fields
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		[SerializeField] private int year;
		[SerializeField] private int month;
		[SerializeField] private int day;
		[SerializeField] private int hour;
		[SerializeField] private int minute;
		[SerializeField] private int second;
		[SerializeField] private int millisecond;
		[SerializeField, HideInInspector] private DateTimeKind kind;

		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Properties
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// The year.
		/// </summary>
		public int Year => year;
		
		
		/// <summary>
		/// The month.
		/// </summary>
		public int Month => month;
		
		
		/// <summary>
		/// The day.
		/// </summary>
		public int Day => day;
		
		
		/// <summary>
		/// The hour.
		/// </summary>
		public int Hour => hour;
		
		
		/// <summary>
		/// The minute.
		/// </summary>
		public int Minute => minute;
		
		
		/// <summary>
		/// The seconds.
		/// </summary>
		public int Second => second;
		
		
		/// <summary>
		/// The millisecond.
		/// </summary>
		public int Millisecond => millisecond;
		
		
		/// <summary>
		/// The kind.
		/// </summary>
		public DateTimeKind Kind => kind;
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Constructors
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Makes a new SerializableDateTime class when called.
		/// </summary>
		/// <param name="dateTime">The date time to convert.</param>
		public SerializableDateTime(DateTime dateTime)
		{
			year = dateTime.Year;
			month = dateTime.Month;
			day = dateTime.Day;
			hour = dateTime.Hour;
			minute = dateTime.Minute;
			second = dateTime.Second;
			millisecond = dateTime.Millisecond;
			kind = dateTime.Kind;
		}
		
		
		/// <summary>
		/// Makes a new SerializableDateTime class when called.
		/// </summary>
		/// <param name="year">The year to set.</param>
		/// <param name="month">The month to set.</param>
		/// <param name="day">The day to set.</param>
		/// <param name="hour">The hour to set.</param>
		/// <param name="minute">The minute to set.</param>
		/// <param name="second">The second to set.</param>
		/// <param name="kind">The kind to set.</param>
		public SerializableDateTime(int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
		{
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			this.kind = kind;
		}
		
		
		/// <summary>
		/// Makes a new SerializableDateTime class when called.
		/// </summary>
		/// <param name="year">The year to set.</param>
		/// <param name="month">The month to set.</param>
		/// <param name="day">The day to set.</param>
		/// <param name="hour">The hour to set.</param>
		/// <param name="minute">The minute to set.</param>
		/// <param name="second">The second to set.</param>
		/// <param name="millisecond">The millisecond to set.</param>
		/// <param name="kind">The kind to set.</param>
		public SerializableDateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
		{
			this.year = year;
			this.month = month;
			this.day = day;
			this.hour = hour;
			this.minute = minute;
			this.second = second;
			this.millisecond = millisecond;
			this.kind = kind;
		}
		
		/* ─────────────────────────────────────────────────────────────────────────────────────────────────────────────
		|   Operators
		───────────────────────────────────────────────────────────────────────────────────────────────────────────── */
		
		/// <summary>
		/// Converts the SerializableDateTime to DateTime.
		/// </summary>
		/// <param name="serializableDateTime">The SerializableDateTime to read.</param>
		/// <returns>A date time class.</returns>
		public static implicit operator DateTime(SerializableDateTime serializableDateTime)
		{
			return new DateTime
			(
				serializableDateTime.year,
				serializableDateTime.month,
				serializableDateTime.day,
				serializableDateTime.hour,
				serializableDateTime.minute,
				serializableDateTime.second,
				serializableDateTime.millisecond,
				serializableDateTime.kind
			);
		}
	}
}