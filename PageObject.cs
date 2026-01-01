using System.Collections.Generic;
using Newtonsoft.Json;
using Playnite.SDK.Plugins;

namespace Local_Metadata_Importer_plugin
{
    public class PageObject
    {
        public string Platform { get; set; }
        public string LogoFolderName { get; set; }
        public string IconFolderName { get; set; }
        public string BackgroundFolderName { get; set; }
        public string VideoFolderName { get; set; }
        public string CoverFolderName { get; set; }
        
        public PageObject(string aPlatform, string aLogoFolderName, string aIconFolderName,
            string aBackgroundFolderName, string aVideoFolderName, string aCoverFolderName)
        {
            Platform = aPlatform;
            LogoFolderName = aLogoFolderName;
            IconFolderName = aIconFolderName;
            BackgroundFolderName = aBackgroundFolderName;
            VideoFolderName = aVideoFolderName;
            CoverFolderName = aCoverFolderName;
        }
        
    }

}
