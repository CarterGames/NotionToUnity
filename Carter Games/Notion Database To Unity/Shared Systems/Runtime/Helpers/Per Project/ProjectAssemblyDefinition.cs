using System.Reflection;

namespace CarterGames.Assets.Shared.PerProject
{
    public static class ProjectAssemblyDefinition
    {
        public static Assembly[] ProjectEditorAssemblies
        {
            get
            {
                return new Assembly[]
                {
                    Assembly.Load("CarterGames.Standalone.NotionData.Editor"),
                    Assembly.Load("CarterGames.Standalone.NotionData.Runtime"),
                    Assembly.Load("CarterGames.Shared.Editor.NotionData"),
                    Assembly.Load("CarterGames.Shared.NotionData")
                };
            }
        }
        
        
        public static Assembly[] ProjectRuntimeAssemblies
        {
            get
            {
                return new Assembly[]
                {
                    Assembly.Load("CarterGames.Standalone.NotionData.Runtime"),
                    Assembly.Load("CarterGames.Standalone.NotionData.ThirdParty"),
                    Assembly.Load("CarterGames.Shared.NotionData")
                };
            }
        }
    }
}