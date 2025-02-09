namespace CarterGames.Standalone.NotionData
{
    public sealed class NotionPropertyRichText : NotionProperty
    {
        protected override object InternalValue { get; set; }
        public string Value => (string) InternalValue;
        public override string JsonValue { get; protected set; }
        
        
        public NotionPropertyRichText(string value, string jsonValue)
        {
            InternalValue = value;
            JsonValue = jsonValue;
        }
    }
}