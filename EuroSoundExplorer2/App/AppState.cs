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
        }

        public AppConfig Configuration { get; private set; }
        public HashcodeParser HashTable { get; private set; }
        public LoadedProjectData LoadedData { get; private set; }
    }
}
