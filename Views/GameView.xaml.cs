using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Linq;
using chess_wpf_test.ViewModels;
using Chess.Model;


namespace chess_wpf_test.Views
{
    public partial class GameView : UserControl
    {
        
        private readonly SolidColorBrush PinkBrush = new SolidColorBrush(Color.FromRgb(255, 181, 197));
        private readonly SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromRgb(46, 139, 87));

        public GameView()
        {
            InitializeComponent();
            this.DataContext = new GameViewModel();
            InitializeChessBoard();
        }

        
        private void InitializeChessBoard()
        {
            var vm = DataContext as GameViewModel;
            if (vm == null) return;


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


                    var cellGrid = new Grid();
                    int vmIndex = row * 8 + col; // индекс фигуры в одномерном массиве Squares

                    var textBlock = new TextBlock 
                    {
                        FontSize = 32,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    };

                    var pieceRectangle = new Rectangle
                    {
                        Width = 35,
                        Height = 35,
                        RadiusX = 5,
                        RadiusY = 5,
                    };

                    // биндинг на символ фигуры
                    var textBinding = new System.Windows.Data.Binding($"Squares[{vmIndex}].DisplaySymbol")
                    {
                        Source = vm,
                        Mode = System.Windows.Data.BindingMode.OneWay
                    };
                    // биндинг на цвет символа фигуры
                    var symbolColorBinding = new System.Windows.Data.Binding($"Squares[{vmIndex}].SymbolBrush")
                    {
                        Source = vm,
                        Mode = System.Windows.Data.BindingMode.OneWay
                    };
                    // биндинг на цвет прямоугольника под фигурой
                    var highlightColorBinding = new System.Windows.Data.Binding($"Squares[{vmIndex}].HighlightBrush")
                    {
                        Source = vm,
                        Mode = System.Windows.Data.BindingMode.OneWay
                    };

                    textBlock.SetBinding(TextBlock.TextProperty, textBinding);
                    textBlock.SetBinding(TextBlock.ForegroundProperty, symbolColorBinding);
                    pieceRectangle.SetBinding(Rectangle.FillProperty, highlightColorBinding);

                    cellGrid.Children.Add( pieceRectangle );
                    cellGrid.Children.Add( textBlock );
                    border.Child = cellGrid;
                    ChessBoard.Children.Add(border);

                    // транспонирование, потому что wpf дурак
                    Grid.SetRow(border, col);
                    Grid.SetColumn(border, row);
                }
            }
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateBoardSize();
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