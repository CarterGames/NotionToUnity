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