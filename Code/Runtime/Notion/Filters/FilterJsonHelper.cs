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

using CarterGames.NotionData.Filters;
using Newtonsoft.Json.Linq;

namespace CarterGames.NotionData
{
	public static class FilterJsonHelper
	{
		public static JObject ToJson(this NotionFilterOptionDef filterOptionDef)
		{
			return filterOptionDef.TypeName switch
			{
				"CheckBox" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterCheckBox(filterOptionDef.Option))
					: new NotionFilterCheckBox(filterOptionDef.Option).ToJson(),
				
				"Date" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterDate(filterOptionDef.Option))
					: new NotionFilterDate(filterOptionDef.Option).ToJson(),
				
				"Id" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterId(filterOptionDef.Option))
					: new NotionFilterId(filterOptionDef.Option).ToJson(),
				
				"Multi Select" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterMultiSelect(filterOptionDef.Option))
					: new NotionFilterMultiSelect(filterOptionDef.Option).ToJson(),

				"Number" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterNumber(filterOptionDef.Option))
					: new NotionFilterNumber(filterOptionDef.Option).ToJson(),

				"Rich text" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterRichText(filterOptionDef.Option))
					: new NotionFilterRichText(filterOptionDef.Option).ToJson(),

				"Select" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterSelect(filterOptionDef.Option))
					: new NotionFilterSelect(filterOptionDef.Option).ToJson(),
				
				"Status" => filterOptionDef.Option.IsRollup
					? NotionFilterRollup.ToRollupJson(new NotionFilterStatus(filterOptionDef.Option))
					: new NotionFilterStatus(filterOptionDef.Option).ToJson(),
				_ => null
			};
		}
	}
}