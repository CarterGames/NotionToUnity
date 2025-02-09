namespace CarterGames.Standalone.NotionData
{
    public sealed class NotionPropertyDate : NotionProperty
    {
        protected override object InternalValue { get; set; }
        public SerializableDateTime Value => (SerializableDateTime) InternalValue;
        public override string JsonValue { get; protected set; }
        
        
        public NotionPropertyDate(SerializableDateTime value, string jsonValue)
        {
            InternalValue = value;
            JsonValue = jsonValue;
        }
    }
}