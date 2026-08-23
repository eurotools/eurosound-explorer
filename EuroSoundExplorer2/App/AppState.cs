using sb_explorer.Classes;

namespace sb_explorer
{
    public class AppState
    {
        public AppState()
        {
            Configuration = new AppConfig();
            HashTable = new HashcodeParser();
            LoadedData = new LoadedProjectData();
            ProjectProfiles = new ProjectProfileStore();
        }

        public AppConfig Configuration { get; private set; }
        public HashcodeParser HashTable { get; private set; }
        public LoadedProjectData LoadedData { get; private set; }
        public ProjectProfileStore ProjectProfiles { get; private set; }
    }
}
