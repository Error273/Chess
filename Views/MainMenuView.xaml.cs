﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using WpfAnimatedGif;

namespace chess_wpf_test.Views
{
    public partial class MainMenuView : UserControl
    {
        private DispatcherTimer _timer;

        public MainMenuView()
        {
            InitializeComponent();
            Debug.WriteLine("MainMenuView initialized");

            NewGameBtn.Click += NewGameBtn_Click;
            ExitBtn.Click += ExitBtn_Click;

            Loaded += OnViewLoaded;
        }

        private void OnViewLoaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("View loaded - loading gif images...");
            LoadGif(HappyCatImage, "Assets/happy_cat.gif");
            LoadGif(SadCatImage, "Assets/sad_cat.gif");
        }

        private void LoadGif(Image targetImage, string relativePath)
        {
            try
            {
                var uri = new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute);
                var image = new BitmapImage(uri);
                ImageBehavior.SetAnimatedSource(targetImage, image);
                RenderOptions.SetBitmapScalingMode(targetImage, BitmapScalingMode.HighQuality);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading gif: {ex.Message}");
            }
        }

        private void ShowCat(Image image)
        {
            image.Visibility = Visibility.Visible;

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(0.4)
            };
            image.BeginAnimation(OpacityProperty, fadeIn);
        }

        private void NewGameBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("New Game button clicked");

            _timer?.Stop();
            ShowCat(HappyCatImage);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(2) };
            _timer.Tick += (s, args) =>
            {
                _timer.Stop();
                NavigateToGameView();
            };
            _timer.Start();
        }

        private void NavigateToGameView()
        {
            try
            {
                var gameView = new GameView();
                var parentWindow = Window.GetWindow(this);
                if (parentWindow != null)
                {
                    parentWindow.Content = gameView;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error navigating to GameView: {ex.Message}");
            }
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Exit button clicked");
            _timer?.Stop();

            HappyCatImage.BeginAnimation(OpacityProperty, null);
            SadCatImage.BeginAnimation(OpacityProperty, null);
            HappyCatImage.Visibility = Visibility.Collapsed;
            ShowCat(SadCatImage);

            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
            _timer.Tick += (s, args) =>
            {
                _timer.Stop();
                Debug.WriteLine("Exiting application...");
                Application.Current.Shutdown();
            };
            _timer.Start();
        }

        private void ClearGifResources()
        {
            HappyCatImage.Source = null;
            SadCatImage.Source = null;
        }
    }
}