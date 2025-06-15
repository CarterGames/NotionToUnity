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

using CarterGames.NotionData.Filters;
using CarterGames.NotionData.ThirdParty;

namespace CarterGames.NotionData
{
	public static class FilterJsonHelper
	{
		public static JSONObject ToJson(this NotionFilterOptionDef filterOptionDef)
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