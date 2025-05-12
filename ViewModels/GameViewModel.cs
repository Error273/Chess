// ViewModels/GameViewModel.cs
using Chess.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace chess_wpf_test.ViewModels
{
    public class GameViewModel: INotifyPropertyChanged
    {
        private Board _board = new();
        private int? _selectedX;
        private int? _selectedY;



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

        public void OnSquareClicked(int x, int y)
        {
            var figure = _board.GetFigureAt(x, y);

            
            if (_selectedX == null)
            {
                if (figure == null || figure.Color != _board.CurrentPlayer) return;
                _selectedX = x;
                _selectedY = y;
                HighlightSquare(x, y, true);
                return;
            }

            if (_selectedX != null)
            {
                int fromX = _selectedX.Value;
                int fromY = _selectedY.Value;
                bool moved = _board.MakeMove(fromX, fromY, x, y);
                HighlightSquare(fromX, fromY, false);
                if (moved)
                {
                    UpdateSquare(fromX, fromY);
                    UpdateSquare(x, y);
                }
                _selectedX = _selectedY = null;
            }
        }

        private void UpdateSquare(int x, int y)
        {
            int ind = x * 8 + y;
            Squares[ind].Figure = _board.GetFigureAt(x, y);
        }

        private void HighlightSquare(int x, int y, bool on)
        {
            int ind = x * 8 + y;
            Squares[ind].isHighlighted = on;
        }


        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
