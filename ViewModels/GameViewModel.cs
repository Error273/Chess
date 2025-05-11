// ViewModels/GameViewModel.cs
using Chess.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace chess_wpf_test.ViewModels
{
    public class GameViewModel: INotifyPropertyChanged
    {
        private Board _board = new();
        // одномерный массив клеток - нужен для того чтобы нормально работала привязка
        public ObservableCollection<SquareViewModel> Squares { get;}

        public Board GameBoard
        {
            get => _board;
        }

        public GameViewModel()
        {
            // заполняем squares фигурами из Board
            Squares = new ObservableCollection<SquareViewModel>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Squares.Add(new SquareViewModel(_board.GetFigureAt(i, j)));
                }
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
