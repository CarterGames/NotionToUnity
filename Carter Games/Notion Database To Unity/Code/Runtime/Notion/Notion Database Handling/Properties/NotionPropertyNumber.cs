namespace CarterGames.Standalone.NotionData
{
    public sealed class NotionPropertyNumber : NotionProperty
    {
        protected override object InternalValue { get; set; }
        public double Value => (double) InternalValue;
        public override string JsonValue { get; protected set; }
        
        
        public NotionPropertyNumber(double value, string jsonValue)
        {
            InternalValue = value;
            JsonValue = jsonValue;
        }
    }
}