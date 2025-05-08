using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;


namespace chess_wpf_test.Views
{
    public partial class GameView : UserControl
    {
        private readonly SolidColorBrush PinkBrush = new SolidColorBrush(Color.FromRgb(255, 181, 197));
        private readonly SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromRgb(46, 139, 87));

        public GameView()
        {
            InitializeComponent();
            InitializeChessBoard();
            InitializePieces();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBoardSize();
        }

        private void InitializeChessBoard()
        {
            for (int row = 0; row < 8; row++)
            {
                for (int col = 0; col < 8; col++)
                {
                    var border = new Border
                    {
                        Background = (row + col) % 2 == 0 ? PinkBrush : GreenBrush,
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(0.5)
                    };

                    ChessBoard.Children.Add(border);
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);
                }
            }
        }

        private void InitializePieces()
        {
            // Черные фигуры
            AddPiece(0, 0, "rook_b");
            AddPiece(0, 1, "knight_b");
            AddPiece(0, 2, "bishop_b");
            AddPiece(0, 3, "queen_b");
            AddPiece(0, 4, "king_b");
            AddPiece(0, 5, "bishop_b");
            AddPiece(0, 6, "knight_b");
            AddPiece(0, 7, "rook_b");

            for (int col = 0; col < 8; col++)
            {
                AddPiece(1, col, "pawn_b");
            }

            // Белые фигуры
            AddPiece(7, 0, "rook_w");
            AddPiece(7, 1, "knight_w");
            AddPiece(7, 2, "bishop_w");
            AddPiece(7, 3, "queen_w");
            AddPiece(7, 4, "king_w");
            AddPiece(7, 5, "bishop_w");
            AddPiece(7, 6, "knight_w");
            AddPiece(7, 7, "rook_w");

            for (int col = 0; col < 8; col++)
            {
                AddPiece(6, col, "pawn_w");
            }
        }

        private void AddPiece(int row, int col, string pieceType)
        {
            var cell = ChessBoard.Children
                .OfType<Border>()
                .FirstOrDefault(b => Grid.GetRow(b) == row && Grid.GetColumn(b) == col);

            if (cell != null)
            {
                var pieceRectangle = new Rectangle
                {
                    Fill = GetPieceColor(pieceType),
                    Width = 35,
                    Height = 35,
                    RadiusX = 5,
                    RadiusY = 5,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };

                var pieceText = new TextBlock
                {
                    Text = GetPieceSymbol(pieceType),
                    Foreground = pieceType.EndsWith("_w") ? Brushes.Black : Brushes.White,
                    FontWeight = FontWeights.Bold,
                    FontSize = 14,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };

                var container = new Grid();
                container.Children.Add(pieceRectangle);
                container.Children.Add(pieceText);

                cell.Child = container;
            }
        }

        private Brush GetPieceColor(string pieceType)
        {
            bool isWhite = pieceType.EndsWith("_w");
            string piece = pieceType.Split('_')[0];

            return piece switch
            {
                "king" => isWhite ? Brushes.White : Brushes.Black,
                "queen" => isWhite ? Brushes.White : Brushes.Black,
                "rook" => isWhite ? Brushes.White : Brushes.Black,
                "bishop" => isWhite ? Brushes.White : Brushes.Black,
                "knight" => isWhite ? Brushes.White : Brushes.Black,
                "pawn" => isWhite ? Brushes.White : Brushes.Black,
                _ => Brushes.Black
            };
        }

        private string GetPieceSymbol(string pieceType)
        {
            string piece = pieceType.Split('_')[0];

            return piece switch
            {
                "king" => "♔",
                "queen" => "♕",
                "rook" => "♖",
                "bishop" => "♗",
                "knight" => "♘",
                "pawn" => "♙",
                _ => ""
            };
        }

        private void UpdateBoardSize()
        {
            double availableSize = Math.Min(ActualWidth, ActualHeight) - 40;
            double scale = availableSize / 400;
            BoardViewbox.Width = 400 * scale;
            BoardViewbox.Height = 400 * scale;
        }


    }
}