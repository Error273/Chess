using System;
using System.Windows;
using System.Windows.Media;

namespace chess_wpf_test
{
    public partial class App : Application
    {
        public static MediaPlayer BackgroundPlayer = new MediaPlayer();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitBackgroundMusic();
        }

        private void InitBackgroundMusic()
        {
            string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "background.mp3");

            BackgroundPlayer.Open(new Uri(filePath, UriKind.Absolute));
            BackgroundPlayer.Volume = 0.3;
            BackgroundPlayer.MediaEnded += (s, ev) =>
            {
                BackgroundPlayer.Position = TimeSpan.Zero;
                BackgroundPlayer.Play();
            };
            BackgroundPlayer.Play();
        }
    }
}