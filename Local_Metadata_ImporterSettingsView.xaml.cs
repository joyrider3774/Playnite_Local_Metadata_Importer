using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System;
using System.Windows;
using Playnite.SDK;
using System.ComponentModel;
using System.Windows.Data;

namespace Local_Metadata_Importer_plugin
{

    public partial class Local_Metadata_ImporterSettingsView : UserControl
    {

        private readonly Local_Metadata_Importer_plugin plugin;

        public Local_Metadata_ImporterSettingsView()
        {
            InitializeComponent();
        }

        public Local_Metadata_ImporterSettingsView(Local_Metadata_Importer_plugin plugin)
        {
            this.plugin = plugin;
            InitializeComponent();
        }

        private void ButSelectIconFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string tmp = plugin.PlayniteApi.Dialogs.SelectFolder();
            if (tmp != string.Empty)
            {
                TbIconFolderName.Text = tmp;
                BindingExpression binding = TbIconFolderName.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource(); 
            }
        }

        private void ButSelectCoverFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string tmp = plugin.PlayniteApi.Dialogs.SelectFolder();
            if (tmp != string.Empty)
            {
                TbCoverFolderName.Text = tmp;
                BindingExpression binding = TbCoverFolderName.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void ButSelectBackgroundFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string tmp = plugin.PlayniteApi.Dialogs.SelectFolder();
            if (tmp != string.Empty)
            {
                TbBackgroundFolderName.Text = tmp;
                BindingExpression binding = TbBackgroundFolderName.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void ButSelectVideoFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string tmp = plugin.PlayniteApi.Dialogs.SelectFolder();
            if (tmp != string.Empty)
            {
                TbVideoFolderName.Text = tmp;
                BindingExpression binding = TbVideoFolderName.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }

        private void ButSelectLogoFolder_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string tmp = plugin.PlayniteApi.Dialogs.SelectFolder();
            if (tmp != string.Empty)
            {
                TbLogoFolderName.Text = tmp;
                BindingExpression binding = TbLogoFolderName.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
            }
        }      

        private void BtnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            int addedindex = PagesDataGrid.Items.Add(plugin.CreatePageObject(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty));
            PagesDataGrid.SelectedIndex = addedindex;
           // PagesDataGrid.ScrollIntoView(PagesDataGrid.SelectedItem);
            //to fix row nr
            PagesDataGrid.Items.Refresh();
            PagesDataGrid.ScrollIntoView(PagesDataGrid.SelectedItem);
        }

        private void BtnDel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (PagesDataGrid.SelectedItem != null)
            {
                int indexOfSelectedItem = PagesDataGrid.Items.IndexOf(PagesDataGrid.SelectedItem);
                PagesDataGrid.Items.Remove(PagesDataGrid.SelectedItem);
                if (PagesDataGrid.Items.Count > 0)
                {
                    if (PagesDataGrid.Items.Count > indexOfSelectedItem)
                    {
                        PagesDataGrid.SelectedItem = PagesDataGrid.Items[indexOfSelectedItem];
                    }
                    else
                    {
                        if (indexOfSelectedItem - 1 > -1)
                        {
                            PagesDataGrid.SelectedItem = PagesDataGrid.Items[indexOfSelectedItem - 1];
                        }
                    }

                }
            }
            //to fix row nr
            PagesDataGrid.Items.Refresh();
        }

        private void PagesDataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (PagesDataGrid.Items.Count > 0)
            {
                if (PagesDataGrid.SelectedItem == null)
                {
                    PagesDataGrid.SelectedItem = PagesDataGrid.Items[0];
                }
            }
        }

    }
}