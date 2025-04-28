using System;

namespace Chess.Model
{
    public enum PieceColor
    {
        White,
        Black
    }

    public abstract class Figure
    {
        // Координаты на доске (0..7)
        public int X { get; protected set; }
        public int Y { get; protected set; }

        // Цвет фигуры
        public PieceColor Color { get; }

        // Конструктор
        protected Figure(int x, int y, PieceColor color)
        {
            X = x;
            Y = y;
            Color = color;
        }

        // Метод перемещения
        public virtual void MoveTo(int newX, int newY)
        {
            X = newX;
            Y = newY;
        }

        // Метод для проверки: может ли фигура пойти в указанную клетку?
        public abstract bool CanMoveTo(int newX, int newY, Figure?[,] board);

        // Хелпер: инвертировать направление (для чёрных фигур)
        protected int Direction()
        {
            return Color == PieceColor.White ? 1 : -1;
        }

        // Общий метод для проверки выхода за границы поля
        protected bool IsInsideBoard(int x, int y)
        {
            return x >= 0 && x < 8 && y >= 0 && y < 8;
        }
        public virtual char Symbol => 'F'; // Буква по умолчанию для абстрактной фигуры

        // Метод для копирования фигуры (если захочется делать симуляции)
        public abstract Figure Clone();
    }
}

namespace Chess.Model
{
    public class Pawn : Figure
    {
        public bool hasMoved;

        public Pawn(int x, int y, PieceColor color) : base(x, y, color)
        {
            hasMoved = false;
        }
        public override char Symbol => 'P'; // Pawn
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int direction = Direction();
            int dx = newX - X;
            int dy = newY - Y;

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Простое движение вперёд на 1
            if (dx == 0 && dy == direction && board[newX, newY] == null)
            {
                return true;
            }

            // Движение вперёд на 2 клетки с начальной позиции
            if (dx == 0 && dy == 2 * direction && !hasMoved)
            {
                // Проверка, что обе клетки свободны
                int intermediateY = Y + direction;
                if (board[X, intermediateY] == null && board[newX, newY] == null)
                    return true;
            }

            // Атака по диагонали
            if (Math.Abs(dx) == 1 && dy == direction)
            {
                Figure? target = board[newX, newY];
                if (target != null && target.Color != Color)
                    return true;
            }

            return false;
        }

        public override void MoveTo(int newX, int newY)
        {
            base.MoveTo(newX, newY);
            hasMoved = true;
        }

        public override Figure Clone()
        {
            Pawn copy = new Pawn(X, Y, Color);
            copy.hasMoved = this.hasMoved;
            return copy;
        }
    }
}

namespace Chess.Model
{
    public class Rook : Figure
    {
        public Rook(int x, int y, PieceColor color) : base(x, y, color)
        {
        }
        public override char Symbol => 'R'; // Rook
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int dx = newX - X;
            int dy = newY - Y;

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Ладья может двигаться только по горизонтали или вертикали
            if (dx != 0 && dy != 0)
                return false;

            // Проверка на возможные препятствия на пути
            if (dx == 0) // Двигаемся по вертикали
            {
                int step = dy > 0 ? 1 : -1; // Направление движения
                for (int y = Y + step; y != newY; y += step)
                {
                    if (board[X, y] != null) // Если встречена фигура
                    {
                        return false; // Невозможно двигаться
                    }
                }
            }
            else if (dy == 0) // Двигаемся по горизонтали
            {
                int step = dx > 0 ? 1 : -1; // Направление движения
                for (int x = X + step; x != newX; x += step)
                {
                    if (board[x, Y] != null) // Если встречена фигура
                    {
                        return false; // Невозможно двигаться
                    }
                }
            }

            // Проверка, не занят ли целевой квадрат фигурами того же цвета
            Figure? target = board[newX, newY];
            if (target != null && target.Color == Color)
            {
                return false; // Цель занята фигурой того же цвета
            }

            return true;
        }

        public override Figure Clone()
        {
            return new Rook(X, Y, Color);
        }
    }
}

namespace Chess.Model
{
    public class Queen : Figure
    {
        public Queen(int x, int y, PieceColor color) : base(x, y, color)
        {
        }
        public override char Symbol => 'Q'; // Queen
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int dx = newX - X;
            int dy = newY - Y;

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Ферзь может двигаться по вертикали, горизонтали или диагонали
            if (dx != 0 && dy != 0 && Math.Abs(dx) != Math.Abs(dy))
                return false; // Если движение не по одной из разрешенных траекторий

            // Проверка на возможные препятствия на пути
            if (dx == 0) // Двигаемся по вертикали
            {
                int step = dy > 0 ? 1 : -1; // Направление движения
                for (int y = Y + step; y != newY; y += step)
                {
                    if (board[X, y] != null) // Если встречена фигура
                    {
                        return false; // Невозможно двигаться
                    }
                }
            }
            else if (dy == 0) // Двигаемся по горизонтали
            {
                int step = dx > 0 ? 1 : -1; // Направление движения
                for (int x = X + step; x != newX; x += step)
                {
                    if (board[x, Y] != null) // Если встречена фигура
                    {
                        return false; // Невозможно двигаться
                    }
                }
            }
            else // Двигаемся по диагонали
            {
                int stepX = dx > 0 ? 1 : -1;
                int stepY = dy > 0 ? 1 : -1;
                int x = X + stepX;
                int y = Y + stepY;

                while (x != newX && y != newY)
                {
                    if (board[x, y] != null) // Если встречена фигура
                    {
                        return false; // Невозможно двигаться
                    }

                    x += stepX;
                    y += stepY;
                }
            }

            // Проверка, не занят ли целевой квадрат фигурами того же цвета
            Figure? target = board[newX, newY];
            if (target != null && target.Color == Color)
            {
                return false; // Цель занята фигурой того же цвета
            }

            return true;
        }

        public override Figure Clone()
        {
            return new Queen(X, Y, Color);
        }
    }
}

namespace Chess.Model
{
    public class Bishop : Figure
    {
        public Bishop(int x, int y, PieceColor color) : base(x, y, color)
        {
        }
        public override char Symbol => 'B'; // Bishop
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int dx = newX - X;
            int dy = newY - Y;

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Слон может двигаться только по диагоналям
            if (Math.Abs(dx) != Math.Abs(dy))
                return false; // Если движение не по диагонали, возвращаем false

            // Проверка на возможные препятствия на пути
            int stepX = dx > 0 ? 1 : -1; // Направление движения по X
            int stepY = dy > 0 ? 1 : -1; // Направление движения по Y
            int x = X + stepX;
            int y = Y + stepY;

            // Двигаемся по диагонали, пока не достигнем целевой клетки
            while (x != newX && y != newY)
            {
                if (board[x, y] != null) // Если встречена фигура
                {
                    return false; // Невозможно двигаться
                }

                x += stepX;
                y += stepY;
            }

            // Проверка, не занят ли целевой квадрат фигурой того же цвета
            Figure? target = board[newX, newY];
            if (target != null && target.Color == Color)
            {
                return false; // Цель занята фигурой того же цвета
            }

            return true;
        }

        public override Figure Clone()
        {
            return new Bishop(X, Y, Color);
        }
    }
}

namespace Chess.Model
{
    public class Knight : Figure
    {
        public Knight(int x, int y, PieceColor color) : base(x, y, color)
        {
        }
        public override char Symbol => 'N'; // Knight (по шахматной нотации)
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int dx = Math.Abs(newX - X);
            int dy = Math.Abs(newY - Y);

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Конь может двигаться буквой "Г" (2 клетки в одном направлении, 1 клетка в другом)
            if ((dx == 2 && dy == 1) || (dx == 1 && dy == 2))
            {
                // Проверка, не занят ли целевой квадрат фигурой того же цвета
                Figure? target = board[newX, newY];
                if (target != null && target.Color == Color)
                {
                    return false; // Цель занята фигурой того же цвета
                }
                return true;
            }

            return false;
        }

        public override Figure Clone()
        {
            return new Knight(X, Y, Color);
        }
    }
}

namespace Chess.Model
{
    public class King : Figure
    {
        public bool hasMoved;

        public King(int x, int y, PieceColor color) : base(x, y, color)
        {
            hasMoved = false;
        }
        public override char Symbol => 'K'; // King
        public override bool CanMoveTo(int newX, int newY, Figure?[,] board)
        {
            int dx = Math.Abs(newX - X);
            int dy = Math.Abs(newY - Y);

            // Проверка на границы доски
            if (!IsInsideBoard(newX, newY))
                return false;

            // Король может двигаться на одну клетку в любом направлении
            if (dx <= 1 && dy <= 1)
            {
                // Проверка, не занят ли целевой квадрат фигурой того же цвета
                Figure? target = board[newX, newY];
                if (target != null && target.Color == Color)
                {
                    return false; // Цель занята фигурой того же цвета
                }
                return true;
            }

            return false;
        }

        public override void MoveTo(int newX, int newY)
        {
            base.MoveTo(newX, newY);
            hasMoved = true;
        }

        public override Figure Clone()
        {
            return new King(X, Y, Color);
        }
    }
}
