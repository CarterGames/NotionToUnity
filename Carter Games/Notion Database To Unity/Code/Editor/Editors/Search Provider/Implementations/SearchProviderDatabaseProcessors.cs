using System.Collections.Generic;
using System.Linq;
using CarterGames.Shared.NotionData;

namespace CarterGames.NotionData.Editor
{
    public class SearchProviderDatabaseProcessors : SearchProvider<AssemblyClassDef>
    {
        private static SearchProviderDatabaseProcessors Instance;
        
        public override string ProviderTitle => "Select Database Processor";
        
        
        public override List<SearchGroup<AssemblyClassDef>> GetEntriesToDisplay()
        {
            NotionDatabaseProcessor ignore = null;
            
            if (ToExclude.Count > 0)
            {
                if (ToExclude.First().IsValid)
                {
                    ignore = ToExclude.First().GetDefinedType<NotionDatabaseProcessor>();
                }
            }
            
            var group = new List<SearchGroup<AssemblyClassDef>>();
            var entries = new List<SearchItem<AssemblyClassDef>>();
            var instances = AssemblyHelper.GetClassesOfType<NotionDatabaseProcessor>(false);
			
            foreach (var entry in instances)
            {
                if (ignore?.GetType() == entry.GetType()) continue;
                entries.Add(SearchItem<AssemblyClassDef>.Set(entry.GetType().Name, entry.GetType()));
            }
			
            group.Add(new SearchGroup<AssemblyClassDef>(entries));

            return group;
        }
        
        
        public static SearchProviderDatabaseProcessors GetProvider()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<SearchProviderDatabaseProcessors>();
            }

            return Instance;
        }
    }
}