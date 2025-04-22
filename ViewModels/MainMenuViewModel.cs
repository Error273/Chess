// ViewModels/MainMenuViewModel.cs
using System;
using System.Windows;
using System.Windows.Input;

namespace chess_wpf_test.ViewModels
{
    public class MainMenuViewModel
    {
        public ICommand NewGameCommand { get; }
        public ICommand ExitCommand { get; }

        public MainMenuViewModel(Action onNewGame)
        {
            NewGameCommand = new RelayCommand(_ => onNewGame());
            ExitCommand = new RelayCommand(_ => Application.Current.Shutdown());
        }
    }
}
