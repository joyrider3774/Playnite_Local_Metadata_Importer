using Newtonsoft.Json;
using Playnite.SDK;
using Playnite.SDK.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.IO;
using System;
using Playnite.SDK.Data;
using Local_Metadata_Importer_plugin;

namespace Local_Metadata_Importer_plugin
{
    public class Local_Metadata_ImporterSettings
    {
        public bool SearchGameNameFirst { get; set; }        
        public List<PageObject> Pages { get; set; } = new List<PageObject>();
    }

    public class Local_Metadata_ImporterSettingsViewModel : ObservableObject, ISettings
    { 
        private readonly Local_Metadata_Importer_plugin plugin;
        private Local_Metadata_ImporterSettings editingClone { get; set; }

        private Local_Metadata_ImporterSettings settings;
        public Local_Metadata_ImporterSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                OnPropertyChanged();
            }
        }      

        // Playnite serializes settings object to a JSON object and saves it as text file.
        // If you want to exclude some property from being saved then use `JsonIgnore` ignore attribute.
        //  [JsonIgnore]
        // public bool OptionThatWontBeSaved { get; set; } = false;

        public Local_Metadata_ImporterSettingsViewModel(Local_Metadata_Importer_plugin plugin)
        {
            // Injecting your plugin instance is required for Save/Load method because Playnite saves data to a location based on what plugin requested the operation.
            this.plugin = plugin;            

            // Load saved settings.
            var savedSettings = plugin.LoadPluginSettings<Local_Metadata_ImporterSettings>();

            // LoadPluginSettings returns null if not saved data is available.
            if (savedSettings != null)
            {
                Settings = savedSettings;               
            }
            else
            {
                Settings = new Local_Metadata_ImporterSettings();
            }
        }

        public void BeginEdit()
        {
            // Code executed when settings view is opened and user starts editing values.
            editingClone = Serialization.GetClone(Settings);

            foreach (PageObject page in Settings.Pages)
            {
                plugin.SettingsView.PagesDataGrid.Items.Add(page);
            }

           
            plugin.SettingsView.PlatformCombobox.Items.Dispatcher.Invoke(() =>
            {               
                foreach (var platform in plugin.PlayniteApi.Database.Platforms.AsQueryable().OrderBy(o => (o != null) ? o.Name : null).Concat(new List<Platform> { null }))
                {
                    string platformName = platform != null ? platform.Name : string.Empty;
                    plugin.SettingsView.PlatformCombobox.Items.Add(platformName);                    
                }
            });
        }

        public void CancelEdit()
        {
            // Code executed when user decides to cancel any changes made since BeginEdit was called.
            // This method should revert any changes made to Option1 and Option2.
            Settings = editingClone;
        }

        public void EndEdit()
        {
            // Code executed when user decides to confirm changes made since BeginEdit was called.
            // This method should save settings made to Option1 and Option2.

            if (plugin.SettingsView.ChkGameNameInsteadRomName.IsChecked == true)
                Settings.SearchGameNameFirst = true;
            else
                Settings.SearchGameNameFirst = false;


            Settings.Pages.Clear();
            foreach (PageObject page in plugin.SettingsView.PagesDataGrid.Items)
            {
                Settings.Pages.Add(page);
            }

            plugin.SavePluginSettings(Settings);
        }

        public bool VerifySettings(out List<string> errors)
        {
            // Code execute when user decides to confirm changes made since BeginEdit was called.
            // Executed before EndEdit is called and EndEdit is not called if false is returned.
            // List of errors is presented to user if verification fails.

            bool returnvalue = true;
            errors = new List<string>();

            return returnvalue;
        }        
    }
}