namespace CarterGames.Standalone.NotionData
{
    public class NotionPropertyRollup : NotionProperty
    {
        protected override object InternalValue { get; set; }
        public override string JsonValue { get; protected set; }
    }
}