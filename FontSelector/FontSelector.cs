using System.IO;
using ColossalFramework.IO;
using ICities;

namespace FontSelector
{
    public class FontSelector : IUserMod
    {
        public string Name => "FontSelector";
        public string Description => "Select your own font from options";
        public static string ConfigFilePath => Path.Combine(DataLocation.applicationBase, ConfigFileName);
        public const string ConfigFileName = "FileSelector.json";

        public void OnSettingsUI(UIHelperBase helper)
        {
            var settings = new UserSettings();
            settings.Populate(helper, ConfigFilePath);
        }
    }
}