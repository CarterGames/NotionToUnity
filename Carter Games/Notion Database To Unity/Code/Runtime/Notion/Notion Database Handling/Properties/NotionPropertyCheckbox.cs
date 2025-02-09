namespace CarterGames.Standalone.NotionData
{
    public sealed class NotionPropertyCheckbox : NotionProperty
    {
        protected override object InternalValue { get; set; }
        public bool Value => (bool) InternalValue;
        public override string JsonValue { get; protected set; }

        
        public NotionPropertyCheckbox(bool value, string jsonValue)
        {
            InternalValue = value;
            JsonValue = jsonValue;
        }
    }
}