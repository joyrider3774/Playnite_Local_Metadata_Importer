using Playnite.SDK;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Resources;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace HtmlExporterPlugin
{
    static class Constants
    {
        public const string AppName = "Local Metadata Importer";

        private static IResourceProvider resources = new ResourceProvider();
        
        public static string MenuAllGamesText = resources.GetString("LOC_LOCALMETADATAIMPORTER_MenuAllGames");
        public static string MenuFilteredGamesText = resources.GetString("LOC_LOCALMETADATAIMPORTER_MenuFilteredGames");
        public static string MenuSelectedGamesText =  resources.GetString("LOC_LOCALMETADATAIMPORTER_MenuSelectedGames");

        public static string ImportAllText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportAll");
        public static string ImportIconsText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportIcons");
        public static string ImportCoversText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportCovers");
        public static string ImportBackgroundsText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportBackgrounds");
        public static string ImportLogosText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportLogos");
        public static string ImportVideosText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportVideos");


        public static string ImportingForGameText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportingForGame");
        public static string ImportingForGameOfText = resources.GetString("LOC_LOCALMETADATAIMPORTER_ImportingForGameOf");

    }
}
