using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace chess_wpf_test.Views
{
    public partial class GameView : UserControl
    {
        private readonly SolidColorBrush PinkBrush = new SolidColorBrush(Color.FromRgb(255, 181, 197));  // Розовый
        private readonly SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromRgb(46, 139, 87)); // Светло-зеленый

        public GameView()
        {
            InitializeComponent();
            InitializeChessBoard();
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
                        Background = (row + col) % 2 == 0
                            ? PinkBrush
                            : GreenBrush,
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(0.5)
                    };
                    ChessBoard.Children.Add(border);
                }
            }
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