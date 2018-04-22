﻿using System.Windows;
using CoreTweet;
using Twimager.Objects;
using Twimager.Resources;
using Twimager.Windows;

namespace Twimager
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App
    {
        private const string ConfigFile = "config.json";

        public Config Config { get; set; }
        public Tokens Twitter { get; set; }

        public static App GetCurrent()
        {
            return (App)Current;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Config = Config.Open(ConfigFile);

            if (Config.Credentials == null)
            {
                var window = new AuthWindow();
                window.ShowDialog();

                var result = window.Result;
                Config.Credentials = new Credentials
                {
                    AccessToken = result.AccessToken,
                    AccessTokenSecret = result.AccessTokenSecret
                };

                Config.Save();
            }

            Twitter = Tokens.Create(
                TwitterKeys.ConsumerKey,
                TwitterKeys.ConsumerSecret,
                Config.Credentials.AccessToken,
                Config.Credentials.AccessTokenSecret
            );

            new MainWindow().ShowDialog();
            Shutdown();
        }
    }
}
