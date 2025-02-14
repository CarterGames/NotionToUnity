namespace CarterGames.Standalone.NotionData.Editor
{
    public sealed class ScriptableObjectInitialize : IAssetEditorReload
    {
        public void OnEditorReloaded()
        {
            ScriptableRef.TryCreateAssets();
        }
    }
}