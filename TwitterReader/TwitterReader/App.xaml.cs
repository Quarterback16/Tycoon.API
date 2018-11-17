using System;
using System.IO;
using System.Windows;
using TwitterAccess;

namespace TwitterReader
{
	public partial class App : Application
    {
        public static TwitterCredentials TwitterCreds { get; private set; }
        public static MainViewModel MainViewModel { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            TwitterCreds = GetSavedTwitterCredentials();
            if (TwitterCreds == null)
            {
                Shutdown();
                return;
            }

            MainViewModel = new MainViewModel();
            var mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private TwitterCredentials GetSavedTwitterCredentials()
        {            
            TwitterCredentials twitterCreds = null;
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"TwitterCredentials.json");
            if (!File.Exists(jsonFilePath))
            {
                MessageBox.Show($"Cannot find Twitter credential file: {jsonFilePath}", "Error");
            }
            else
            {
                try
                {
                    twitterCreds = JsonHelper.DeserializeFromFile<TwitterCredentials>(jsonFilePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
            return twitterCreds;
        }
    }
}