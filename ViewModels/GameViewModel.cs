using Chess.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace chess_wpf_test.ViewModels
{
    public class GameViewModel : INotifyPropertyChanged
    {
        private Board _board = new();
        private int? _selectedX;
        private int? _selectedY;

        public ObservableCollection<SquareViewModel> Squares { get; }

        public Board GameBoard => _board;

        public GameViewModel()
        {
            Squares = new ObservableCollection<SquareViewModel>();
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    Squares.Add(new SquareViewModel(_board.GetFigureAt(i, j)));
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
                HighlightSquare(fromX, fromY, false);

                bool moved = _board.MakeMove(fromX, fromY, x, y);

                if (moved)
                {
                    UpdateSquare(fromX, fromY);
                    UpdateSquare(x, y);

                    if (_board.IsCheckmate())
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            if (Application.Current.MainWindow?.Content is Views.GameView gameView)
                            {
                                if (_board.CurrentPlayer == PieceColor.White)
                                {
                                    gameView.ShowGameOverMessage("Розовые победили!", "#D0F0C0", "#FF69B4");
                                }
                                else
                                {
                                    gameView.ShowGameOverMessage("Зелёные победили!", "#FADADD", "#008000");
                                }
                            }
                        });
                    }
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