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
        private Figure _figure;
        public Figure Figure
        {
            get => _figure;
            set
            {
                if (_figure != value)
                {
                    _figure = value;
                    OnPropertyChanged(nameof(Figure));
                    OnPropertyChanged(nameof(DisplaySymbol));
                }
            }
        }

        public string DisplaySymbol
        {
            get
            {
                return _figure.Symbol switch
                {
                    "K" => "♔",
                    "Q" => "♕",
                    "R" => "♖",
                    "B" => "♗",
                    "N" => "♘",
                    "P" => "♙",
                    _ => ""
                };
            }
        }

        // кисть которой рисуется символ (противоположный цвет)
        public Brush SymbolBrush =>
            Figure != null ?
            (Figure.Color == PieceColor.White ? Brushes.Black : Brushes.White) : Brushes.Transparent;
        // кисть которой рисуется фон (основной цвет фигуры)
        public Brush HighlightBrush =>
            Figure != null ?
            (Figure.Color == PieceColor.White ? Brushes.White: Brushes.Black) : Brushes.Transparent;



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
