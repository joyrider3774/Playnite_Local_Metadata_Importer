using Playnite.SDK;
using Playnite.SDK.Events;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace Local_Metadata_Importer_plugin
{
    public class Local_Metadata_Importer_plugin : GenericPlugin
    {
        public static string pluginFolder;

        private Local_Metadata_ImporterSettingsViewModel Settings { get; set; }
        public Local_Metadata_ImporterSettingsView SettingsView { get; private set; }

        public List<string> AvailablePlatforms { get; set; }

        private static readonly ILogger logger = LogManager.GetLogger();

        public override Guid Id { get; } = Guid.Parse("7fdd3f44-0209-49f3-a1f0-40297c22b8f2");

        public Local_Metadata_Importer_plugin(IPlayniteAPI api) : base(api)
        {
            pluginFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Settings = new Local_Metadata_ImporterSettingsViewModel(this);
            AvailablePlatforms = api.Database.Platforms.AsQueryable().Select(p => p.Name).ToList();
            Properties = new GenericPluginProperties
            {
                HasSettings = true
            };
        }


        public override ISettings GetSettings(bool firstRunSettings)
        {
            return Settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            SettingsView = new Local_Metadata_ImporterSettingsView(this);
            return SettingsView;
        }

        public void DoImport(List<Game> Games, bool ImportIcons, bool ImportCovers, bool ImportBackgrounds, bool ImportLogos, bool ImportVideos)
        {
            var progressOptions = new GlobalProgressOptions(String.Empty, true)
            {
                IsIndeterminate = false
            };
            _ = PlayniteApi.Dialogs.ActivateGlobalProgress((progressAction) =>
            {
                try
                {
                    progressAction.ProgressMaxValue = Games.Count;
                    int gameNr = 0;
                    foreach (Game game in Games)
                    {
                        bool iconfound = false;
                        bool coverfound = false;
                        bool videofound = false;
                        bool backgroundfound = false;
                        bool logofound = false;

                        gameNr++;
                        progressAction.CurrentProgressValue = gameNr;
                        progressAction.Text = "Importing for game " + progressAction.CurrentProgressValue.ToString() + " of " + progressAction.ProgressMaxValue.ToString() + ": " + game.Name;
                        if (progressAction.CancelToken.IsCancellationRequested)
                        {
                            break;
                        }
                        if (game.Platforms != null)
                        {
                            foreach (Platform platform in game.Platforms)
                            {
                                if ((iconfound || !ImportIcons) &&
                                    (coverfound || !ImportCovers) &&
                                    (backgroundfound || !ImportBackgrounds) &&
                                    (logofound || !ImportLogos) &&
                                    (videofound || !ImportVideos))
                                    break;

                                foreach (PageObject page in Settings.Settings.Pages)
                                {
                                    if ((iconfound || !ImportIcons) &&
                                        (coverfound || !ImportCovers) &&
                                        (backgroundfound || !ImportBackgrounds) &&
                                        (logofound || !ImportLogos) &&
                                        (videofound || !ImportVideos))
                                        break;

                                    if (page.Platform == platform.Name)
                                    {
                                        List<string> names = new List<string>();

                                        if (Settings.Settings.SearchGameNameFirst)
                                        {
                                            names.Add(Path.ChangeExtension(game.Name, ".png"));
                                            names.Add(Path.ChangeExtension(game.Name, ".gif"));
                                            names.Add(Path.ChangeExtension(game.Name, ".bmp"));
                                            names.Add(Path.ChangeExtension(game.Name, ".jpg"));
                                            names.Add(Path.ChangeExtension(game.Name, ".jpeg"));
                                            names.Add(Path.ChangeExtension(game.Name, ".ico"));
                                            foreach (GameRom rom in game.Roms)
                                            {
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".png"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".gif"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".bmp"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".jpg"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".jpeg"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".ico"));
                                            }
                                        }
                                        else
                                        {
                                            foreach (GameRom rom in game.Roms)
                                            {
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".png"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".gif"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".bmp"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".jpg"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".jpeg"));
                                                names.Add(Path.ChangeExtension(Path.GetFileName(rom.Path), ".ico"));
                                            }
                                            names.Add(Path.ChangeExtension(game.Name, ".png"));
                                            names.Add(Path.ChangeExtension(game.Name, ".gif"));
                                            names.Add(Path.ChangeExtension(game.Name, ".bmp"));
                                            names.Add(Path.ChangeExtension(game.Name, ".jpg"));
                                            names.Add(Path.ChangeExtension(game.Name, ".jpeg"));
                                            names.Add(Path.ChangeExtension(game.Name, ".ico"));
                                        }
                                        
                                        foreach (string name in names)
                                        {
                                            if (ImportIcons && !iconfound && page.IconFolderName != string.Empty && File.Exists(Path.Combine(page.IconFolderName, name)))
                                            {
                                                if (game.Icon != null)
                                                    PlayniteApi.Database.RemoveFile(game.Icon);
                                                game.Icon = PlayniteApi.Database.AddFile(Path.Combine(page.IconFolderName, name), game.Id);
                                                iconfound = true;
                                            }

                                            if (ImportCovers && !coverfound && page.CoverFolderName != string.Empty && File.Exists(Path.Combine(page.CoverFolderName, name)))
                                            {
                                                if (game.CoverImage != null)
                                                    PlayniteApi.Database.RemoveFile(game.CoverImage);
                                                game.CoverImage = PlayniteApi.Database.AddFile(Path.Combine(page.CoverFolderName, name), game.Id);
                                                coverfound = true;
                                            }

                                            if (ImportBackgrounds && !backgroundfound && page.BackgroundFolderName != string.Empty && File.Exists(Path.Combine(page.BackgroundFolderName, name)))
                                            {
                                                if (game.Icon != null)
                                                    PlayniteApi.Database.RemoveFile(game.BackgroundImage);
                                                game.BackgroundImage = PlayniteApi.Database.AddFile(Path.Combine(page.BackgroundFolderName, name), game.Id);
                                                backgroundfound = true;
                                            }

                                            if (ImportLogos && !logofound && page.LogoFolderName != string.Empty && File.Exists(Path.Combine(page.LogoFolderName, name)))
                                            {
                                                string logofilename = Path.Combine(PlayniteApi.Paths.ConfigurationPath, "ExtraMetadata", "games", game.Id.ToString(), "Logo.png");
                                                string newlogofilename = Path.Combine(page.LogoFolderName, name);
                                                //logos can be in use
                                                try
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(logofilename));
                                                    File.Copy(newlogofilename, logofilename, true);
                                                }
                                                catch { };
                                                logofound = true;
                                            }

                                            if (ImportVideos && !videofound && page.VideoFolderName != string.Empty && File.Exists(Path.Combine(page.VideoFolderName, Path.ChangeExtension(name, ".mp4"))))
                                            {
                                                string videofilename = Path.Combine(PlayniteApi.Paths.ConfigurationPath, "ExtraMetadata", "games", game.Id.ToString(), "VideoTrailer.mp4");
                                                string newvideofilename = Path.Combine(page.VideoFolderName, Path.ChangeExtension(name, ".mp4"));
                                                //videos can be in use
                                                try
                                                {
                                                    Directory.CreateDirectory(Path.GetDirectoryName(videofilename));
                                                    File.Copy(newvideofilename, videofilename, true);
                                                }
                                                catch { };
                                                videofound = true;
                                            }

                                            if ((iconfound || !ImportIcons) && 
                                                (coverfound || !ImportCovers) && 
                                                (backgroundfound ||ImportBackgrounds) && 
                                                (logofound || !ImportLogos) && 
                                                (videofound || !ImportVideos))
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                        if ((iconfound && ImportIcons) ||
                            (coverfound && ImportCovers) ||
                            (backgroundfound && ImportBackgrounds))
                            PlayniteApi.Database.Games.Update(game);

                    }
                }
                catch (Exception E)
                {
                    logger.Error(E, "Local Metadata Importer");
                }
            }, progressOptions);
        }

        public override IEnumerable<MainMenuItem> GetMainMenuItems(GetMainMenuItemsArgs args)
        {
            List<MainMenuItem> MainMenuItems = new List<MainMenuItem>
            {
                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|All",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), true, true, true, true, true);
                    }                        
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|All",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), true, true, true, true, true);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|All",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), true, true, true, true, true);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Icons",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), true, false, false, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Icons",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), true, false, false, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Icons",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), true, false, false, false, false);
                    }
                },


                 new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Covers",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), false, true, false, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Covers",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), false, true, false, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Covers",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), false, true, false, false, false);
                    }
                },


                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Backgrounds",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), false, false, true, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Backgrounds",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), false, false, true, false, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Backgrounds",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), false, false, true, false, false);
                    }
                },


                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Logos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), false, false, false, true, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Logos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), false, false, false, true, false);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Logos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), false, false, false, true, false);
                    }
                },



                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Videos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for all games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.Database.Games.ToList(), false, false, false, false, true);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Videos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for filtered games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.FilteredGames.ToList(), false, false, false, false, true);
                    }
                },

                new MainMenuItem{
                    MenuSection = "@Local Metadata Importer|Videos",
                    Icon = Path.Combine(pluginFolder, "icon.png"),
                    Description = "Import for selected games",
                    Action = (MainMenuItem) =>
                    {
                        DoImport(PlayniteApi.MainView.SelectedGames.ToList(), false, false, false, false, true);
                    }
                }
            };
            
            return MainMenuItems;
        }

        public PageObject CreatePageObject(string aPlatform, string aLogoFolderName, string aIconFolderName,
            string aBackgroundFolderName, string aVideoFolderName, string aCoverFolderName)
        {
            return new PageObject(aPlatform, aLogoFolderName, aIconFolderName, aBackgroundFolderName, aVideoFolderName, aCoverFolderName);
        }
    }
}