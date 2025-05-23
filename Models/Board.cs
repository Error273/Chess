﻿using System;
using System.Collections.Generic;

namespace Chess.Model
{
    public class Board
    {
        private Figure?[,] _board;
        private PieceColor _currentPlayer;
        private King _whiteKing;
        private King _blackKing;
        private bool _gameOver;
        private Move? _lastMove;

        public PieceColor CurrentPlayer => _currentPlayer;
        public bool IsGameOver => _gameOver;
        public Move? LastMove => _lastMove;

        public Board()
        {
            _board = new Figure?[8, 8];
            _currentPlayer = PieceColor.White;
            _gameOver = false;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            //раставляем фигуры
            for (int x = 0; x < 8; x++)
            {
                _board[x, 1] = new Pawn(x, 1, PieceColor.Black);
                _board[x, 6] = new Pawn(x, 6, PieceColor.White);
            }

            _board[0, 0] = new Rook(0, 0, PieceColor.Black);
            _board[7, 0] = new Rook(7, 0, PieceColor.Black);
            _board[0, 7] = new Rook(0, 7, PieceColor.White);
            _board[7, 7] = new Rook(7, 7, PieceColor.White);

            _board[1, 0] = new Knight(1, 0, PieceColor.Black);
            _board[6, 0] = new Knight(6, 0, PieceColor.Black);
            _board[1, 7] = new Knight(1, 7, PieceColor.White);
            _board[6, 7] = new Knight(6, 7, PieceColor.White);

            _board[2, 0] = new Bishop(2, 0, PieceColor.Black);
            _board[5, 0] = new Bishop(5, 0, PieceColor.Black);
            _board[2, 7] = new Bishop(2, 7, PieceColor.White);
            _board[5, 7] = new Bishop(5, 7, PieceColor.White);

            _board[3, 0] = new Queen(3, 0, PieceColor.Black);
            _board[3, 7] = new Queen(3, 7, PieceColor.White);

            _blackKing = new King(4, 0, PieceColor.Black);
            _whiteKing = new King(4, 7, PieceColor.White);
            _board[4, 0] = _blackKing;
            _board[4, 7] = _whiteKing;
        }

        //получаем фигуру по координатам
        public Figure? GetFigureAt(int x, int y)
        {
            if (x < 0 || x >= 8 || y < 0 || y >= 8)
                return null;
            return _board[x, y];
        }
        //проверка можно ли так сходить
        public bool IsValidMove(int fromX, int fromY, int toX, int toY)
        {
            Figure? figure = GetFigureAt(fromX, fromY);
            if (figure == null || figure.Color != _currentPlayer)
                return false;

            if (!figure.CanMoveTo(toX, toY, _board))
                return false;

            // 1. Сохраняем состояние доски
            Figure? originalTarget = _board[toX, toY];
            (int origX, int origY) = (figure.X, figure.Y);
            bool hadMoved = figure is King or Pawn ? ((dynamic)figure).hasMoved : false;

            // 2. Временное перемещение
            _board[fromX, fromY] = null;
            _board[toX, toY] = figure;
            figure.MoveTo(toX, toY);

            //проверка на шах
            King king = figure.Color == PieceColor.White ? _whiteKing : _blackKing;
            bool isValid = !IsSquareUnderAttack(king.X, king.Y,
                           figure.Color == PieceColor.White ? PieceColor.Black : PieceColor.White);

            //Восстановливаем исходную доску
            _board[fromX, fromY] = figure;
            _board[toX, toY] = originalTarget;
            figure.MoveTo(origX, origY);

            // восстанавливаем для пешек и королей их особые ходы
            if (figure is Pawn pawn) pawn.hasMoved = hadMoved;
            else if (figure is King kingFig) kingFig.hasMoved = hadMoved;

            return isValid;
        }

        public bool MakeMove(int fromX, int fromY, int toX, int toY)
        {
            if (_gameOver)
                return false;

            Figure? figure = GetFigureAt(fromX, fromY);
            if (figure == null || figure.Color != _currentPlayer)
                return false;

            // --- ТОПОРНАЯ РЕАЛИЗАЦИЯ РОКИРОВКИ ---
            // Белые короткая (e1 -> h1)
            if (fromX == 4 && fromY == 7 && toX == 7 && toY == 7)
            {
                if (_board[4, 7] is King wk &&
                    _board[7, 7] is Rook wr &&
                    _board[5, 7] == null &&
                    _board[6, 7] == null &&
                    !IsSquareUnderAttack(4, 7, PieceColor.Black))
                {
                    _board[4, 7] = null;
                    _board[7, 7] = null;
                    _board[6, 7] = wk;
                    _board[5, 7] = wr;
                    wk.MoveTo(6, 7);
                    wr.MoveTo(5, 7);
                    _whiteKing = wk;
                    _currentPlayer = PieceColor.Black;
                    _lastMove = new Move(4, 7, 7, 7, wk);
                    return true;
                }
            }

            // Белые длинная (e1 -> a1)
            if (fromX == 4 && fromY == 7 && toX == 0 && toY == 7)
            {
                if (_board[4, 7] is King wk &&
                    _board[0, 7] is Rook wr &&
                    _board[1, 7] == null &&
                    _board[2, 7] == null &&
                    _board[3, 7] == null &&
                    !IsSquareUnderAttack(4, 7, PieceColor.Black))
                {
                    _board[4, 7] = null;
                    _board[0, 7] = null;
                    _board[2, 7] = wk;
                    _board[3, 7] = wr;
                    wk.MoveTo(2, 7);
                    wr.MoveTo(3, 7);
                    _whiteKing = wk;
                    _currentPlayer = PieceColor.Black;
                    _lastMove = new Move(4, 7, 0, 7, wk);
                    return true;
                }
            }

            // Чёрные короткая (e8 -> h8)
            if (fromX == 4 && fromY == 0 && toX == 7 && toY == 0)
            {
                if (_board[4, 0] is King bk &&
                    _board[7, 0] is Rook br &&
                    _board[5, 0] == null &&
                    _board[6, 0] == null &&
                    !IsSquareUnderAttack(4, 0, PieceColor.White))
                {
                    _board[4, 0] = null;
                    _board[7, 0] = null;
                    _board[6, 0] = bk;
                    _board[5, 0] = br;
                    bk.MoveTo(6, 0);
                    br.MoveTo(5, 0);
                    _blackKing = bk;
                    _currentPlayer = PieceColor.White;
                    _lastMove = new Move(4, 0, 7, 0, bk);
                    return true;
                }
            }

            // Чёрные длинная (e8 -> a8)
            if (fromX == 4 && fromY == 0 && toX == 0 && toY == 0)
            {
                if (_board[4, 0] is King bk &&
                    _board[0, 0] is Rook br &&
                    _board[1, 0] == null &&
                    _board[2, 0] == null &&
                    _board[3, 0] == null &&
                    !IsSquareUnderAttack(4, 0, PieceColor.White))
                {
                    _board[4, 0] = null;
                    _board[0, 0] = null;
                    _board[2, 0] = bk;
                    _board[3, 0] = br;
                    bk.MoveTo(2, 0);
                    br.MoveTo(3, 0);
                    _blackKing = bk;
                    _currentPlayer = PieceColor.White;
                    _lastMove = new Move(4, 0, 0, 0, bk);
                    return true;
                }
            }

            // --- ОБЫЧНЫЙ ХОД ---
            if (!IsValidMove(fromX, fromY, toX, toY))
                return false;

            if (figure is Pawn && (toY == 0 || toY == 7))
            {
                _board[fromX, fromY] = null;
                _board[toX, toY] = new Queen(toX, toY, figure.Color);
            }
            else
            {
                _board[fromX, fromY] = null;
                figure.MoveTo(toX, toY);
                _board[toX, toY] = figure;
            }

            _lastMove = new Move(fromX, fromY, toX, toY, figure);

            _currentPlayer = _currentPlayer == PieceColor.White
                            ? PieceColor.Black
                            : PieceColor.White;

            bool inCheck = IsInCheck(_currentPlayer);
            bool hasAnyMove = !IsStalemate(_currentPlayer) || inCheck;

            if (inCheck && !hasAnyMove)
                _gameOver = true;
            else if (!inCheck && !hasAnyMove)
                _gameOver = true;

            return true;
        }

        public bool CanMove(int fromX, int fromY, int toX, int toY)
        {
            Figure? figure = GetFigureAt(fromX, fromY);
            if (figure == null || figure.Color != _currentPlayer)
                return false;

            // Проверяем только правила движения фигуры, без шаха
            return figure.CanMoveTo(toX, toY, _board);
        }


        public bool IsSquareUnderAttack(int x, int y, PieceColor byColor)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Figure? figure = GetFigureAt(i, j);
                    if (figure != null && figure.Color == byColor && figure.CanMoveTo(x, y, _board))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        //проверка на шах
        public bool IsInCheck(PieceColor color)
        {
            King king = color == PieceColor.White ? _whiteKing : _blackKing;
            return IsSquareUnderAttack(king.X, king.Y, color == PieceColor.White ? PieceColor.Black : PieceColor.White);
        }

        public bool IsCheckmate(PieceColor color)//проверка на мат
        {
            if (!IsInCheck(color))
                return false;


            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    Figure? figure = GetFigureAt(fromX, fromY);
                    if (figure == null || figure.Color != color)
                        continue;

                    for (int toX = 0; toX < 8; toX++)
                    {
                        for (int toY = 0; toY < 8; toY++)
                        {
                            if (IsValidMove(fromX, fromY, toX, toY))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }
        //проверка на пат
        public bool IsStalemate(PieceColor color)
        {
            if (IsInCheck(color))
                return false;

            // проерка на возможность какого-либо хода
            for (int fromX = 0; fromX < 8; fromX++)
            {
                for (int fromY = 0; fromY < 8; fromY++)
                {
                    Figure? figure = GetFigureAt(fromX, fromY);
                    if (figure == null || figure.Color != color)
                        continue;

                    for (int toX = 0; toX < 8; toX++)
                    {
                        for (int toY = 0; toY < 8; toY++)
                        {
                            if (IsValidMove(fromX, fromY, toX, toY))
                            {
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        public Board Clone()
        {
            Board clone = new Board();
            clone._currentPlayer = this._currentPlayer;
            clone._gameOver = this._gameOver;

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    Figure? figure = this._board[x, y];
                    clone._board[x, y] = figure?.Clone();
                }
            }


            clone._whiteKing = (King)clone._board[_whiteKing.X, _whiteKing.Y]!;
            clone._blackKing = (King)clone._board[_blackKing.X, _blackKing.Y]!;

            return clone;
        }

        public void print()// вывод доски в консоль (чисто для дебага)
        {
            for (int x = 0; x < 8; x++)
            {
                string p = "";

                for (int y = 0; y < 8; y++)
                {
                    if (_board[x, y] != null)
                    {
                        p += _board[x, y].Symbol;
                        if (_board[x, y].Color == PieceColor.White)
                        {
                            p += "W";
                        }
                        else { p += "B"; }
                        p += " ";
                    }
                    else { p += "   "; }
                }
                Console.WriteLine(p);
            }
        }
        public bool IsCheckmate()


        {


            return IsCheckmate(CurrentPlayer);


        }
    }

    public class Move
    {
        public int FromX { get; }
        public int FromY { get; }
        public int ToX { get; }
        public int ToY { get; }
        public Figure Figure { get; }

        public Move(int fromX, int fromY, int toX, int toY, Figure figure)
        {
            FromX = fromX;
            FromY = fromY;
            ToX = toX;
            ToY = toY;
            Figure = figure;
        }
    }
}