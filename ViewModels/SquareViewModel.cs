using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using Chess.Model;
using Figure = Chess.Model.Figure;

namespace chess_wpf_test.ViewModels
{
    // Класс одной клетки - для отрисовки
    public class SquareViewModel : INotifyPropertyChanged
    {
        private Figure? _figure;
        public Figure? Figure
        {
            get => _figure;
            set
            {
                if (_figure != value)
                {
                    _figure = value;
                    OnPropertyChanged(nameof(Figure));
                    OnPropertyChanged(nameof(DisplaySymbol));
                    OnPropertyChanged(nameof(SymbolBrush));
                    //OnPropertyChanged(nameof(BackgroundBrush));
                    OnPropertyChanged(nameof(HighlightBrush));
                }
            }
        }

        public string DisplaySymbol
        {
            get
            {
                if (_figure == null)
                {
                    return "";
                }
                return _figure.Symbol switch
                {
                    "K" => "♚",
                    "Q" => "♛",
                    "R" => "♜",
                    "B" => "♝",
                    "N" => "♞",
                    "P" => "♙",
                    _ => ""
                };
            }
        }


        private bool _isHighlighted;
        public bool isHighlighted
        {
            get => _isHighlighted;
            set {
                if (_isHighlighted != value)
                {
                    _isHighlighted = value;
                    OnPropertyChanged(nameof(isHighlighted));
                    OnPropertyChanged(nameof(HighlightBrush));
                }
            }
        }


        // кисть которой рисуется символ (противоположный цвет)
        public Brush SymbolBrush =>
            Figure != null ?
            (Figure.Color == PieceColor.White ? (Brush)(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#556b2f"))) : (Brush)(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DA3287"))))
            : Brushes.Transparent;

        // кисть которой рисуется фон (основной цвет фигуры)
        //public Brush BackgroundBrush =>
        //    Figure != null ?
        //    (Figure.Color == PieceColor.White ? Brushes.White: Brushes.Black) : Brushes.Transparent;

        // кисть подсветки

        //public Brush HighlightBrush =>
        //    Figure != null && isHighlighted ? Brushes.White : Brushes.Transparent;

        //public Brush HighlightBrush => Brushes.White;

        public Brush HighlightBrush =>
            isHighlighted ? Brushes.White : Brushes.Transparent;



        public SquareViewModel(Figure figure)
        {
            Figure = figure;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
