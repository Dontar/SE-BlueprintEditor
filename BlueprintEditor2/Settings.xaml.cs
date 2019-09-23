﻿using BlueprintEditor2.Resource;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BlueprintEditor2
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        static internal Settings LastWindow;
        readonly int[] Langs = new int[] {9, 1049}; 
        public Settings()
        {
            InitializeComponent();
            LastWindow = this;
            BlueprintFolderSetting.Text = MySettings.Current.BlueprintPatch;
            LangSelect.SelectedIndex = Array.IndexOf(Langs, MySettings.Current.LCID);
            MultiWindowCheckBox.IsChecked = MySettings.Current.MultiWindow;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            LastWindow = null;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MySettings.Serialize();
            SelectBlueprint.window.SetLock(true, 0);
            new Dialog(DialogPicture.question, Lang.Settings, Lang.PleaseRestartApp, (Dial) =>
            {
                if (Dial == DialоgResult.Yes)
                {
                    Process.Start(MyExtensions.AppFile);
                    Application.Current.Shutdown();
                }
                else
                {
                    SelectBlueprint.window.SetLock(false, null);
                    LastWindow = null;
                }
            }).Show();
        }

        private void MultiWindowCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            MySettings.Current.MultiWindow = MultiWindowCheckBox.IsChecked.Value;
        }

        private void LangSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MySettings.Current.LCID = Langs[LangSelect.SelectedIndex];
        }

        private void BlueprintFolderSetting_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Directory.Exists(BlueprintFolderSetting.Text))
            {
                MySettings.Current.BlueprintPatch = BlueprintFolderSetting.Text;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                dialog.SelectedPath = BlueprintFolderSetting.Text;
                dialog.ShowNewFolderButton = false;
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();
                if(result.Equals(System.Windows.Forms.DialogResult.OK)) BlueprintFolderSetting.Text = dialog.SelectedPath+"\\";
            }
        }
    }
}