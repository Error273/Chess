// ViewModels/MainViewModel.cs
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace chess_wpf_test.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object _currentViewModel;
        public object CurrentViewModel
        {
            get => _currentViewModel;
            set { _currentViewModel = value; OnPropertyChanged(); }
        }

        public MainViewModel()
        {
            // при старте — главное меню
            CurrentViewModel = new MainMenuViewModel(OnNewGame);
        }

        private void OnNewGame()
        {
            // переключаемся на пустой экран игры
            CurrentViewModel = new GameViewModel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
