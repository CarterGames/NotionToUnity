using System;

namespace CarterGames.Standalone.NotionData
{
    public static class NotionPropertyFactory
    {
        public static NotionPropertyCheckbox Checkbox(object value, string jsonValue)
        {
            return new NotionPropertyCheckbox((bool) value, jsonValue);
        }
        
        
        public static NotionPropertyDate Date(object value, string jsonValue)
        {
            return new NotionPropertyDate((SerializableDateTime) value, jsonValue);
        }
        
        
        public static NotionPropertyMultiSelect MultiSelect(object value, string jsonValue)
        {
            return new NotionPropertyMultiSelect((string[]) value, jsonValue);
        }
        
        
        public static NotionPropertySelect Select(object value, string jsonValue)
        {
            return new NotionPropertySelect((string) value, jsonValue);
        }
        
        
        public static NotionPropertyRichText RichText(object value, string jsonValue)
        {
            return new NotionPropertyRichText((string) value, jsonValue);
        }
        
        
        public static NotionPropertyTitle Title(object value, string jsonValue)
        {
            return new NotionPropertyTitle((string) value, jsonValue);
        }
        
        
        public static NotionPropertyNumber Number(object value, string jsonValue)
        {
            return new NotionPropertyNumber((double) value, jsonValue);
        }
    }
}