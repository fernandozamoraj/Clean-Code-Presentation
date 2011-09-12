using System;
using System.Collections.Generic;
using System.Drawing;

namespace CheckersApp3
{
    public class CheckerBoard
    {
        private readonly int TwoSquaresApart = 2;
        private readonly CheckersConstants _checkerConstants = new CheckersConstants();
        private readonly int[,] _board = new[,]
                                     {
                                         {0, 1, 0, 1, 0, 1, 0, 1},
                                         {1, 0, 1, 0, 1, 0, 1, 0},
                                         {0, 1, 0, 1, 0, 1, 0, 1},
                                         {0, 0, 0, 0, 0, 0, 0, 0},
                                         {0, 0, 0, 0, 0, 0, 0, 0},
                                         {2, 0, 2, 0, 2, 0, 2, 0},
                                         {0, 2, 0, 2, 0, 2, 0, 2},
                                         {2, 0, 2, 0, 2, 0, 2, 0}
                                     };

        public int[,] Board
        {
            get { return _board; }
        }

        public int Square(int row, int column)
        {
            TryToThrowInvalidRowColumnException(row, column);

            return _board[row,column];
        }

        public void SetCellToSelected(int row, int column)
        {
            TryToThrowInvalidRowColumnException(row, column);

            if(IsValidSelection(row, column))
                _board[row,column] |= _checkerConstants.SelectedChecker;
        }

        public bool IsRedChecker(int row, int column)
        {
            if(!SquareIsWithinBoardRange(row, column))
                return false;

            return (_board[row,column] & _checkerConstants.RedChecker) == _checkerConstants.RedChecker;
        }

        public bool IsBlackChecker(int row, int column)
        {
            if (!SquareIsWithinBoardRange(row, column))
                return false;

            return (_board[row,column] & _checkerConstants.BlackChecker) == _checkerConstants.BlackChecker;
        }

        private bool SquareIsWithinBoardRange(int row, int column)
        {
            if(row < 0 || row >= _checkerConstants.SquaresInBoard ||
                column < 0 || column >= _checkerConstants.SquaresInBoard)
                return false;

            return true;
        }

        public bool IsSquareSelected(int row, int column)
        {
            TryToThrowInvalidRowColumnException(row, column);

            return (_board[row,column] & _checkerConstants.SelectedChecker) == _checkerConstants.SelectedChecker;
        }

        public bool MoveChecker(int checkerSquareRow, int checkerSquareColumn, int freeSquareRow, int freeSquareColumn)
        {
            if (!SquareIsWithinBoardRange(freeSquareRow, freeSquareColumn))
                return false;

            bool moved = false;
            if (IsFreeSpace(freeSquareRow, freeSquareColumn))
            {
                //Check if move is valid
                if (IsValidMove(new Point(checkerSquareRow, checkerSquareColumn), new Point(freeSquareRow, freeSquareColumn) ))
                {
                    _board[freeSquareRow,freeSquareColumn] = _board[checkerSquareRow,checkerSquareColumn];
                    _board[checkerSquareRow,checkerSquareColumn] = _checkerConstants.FreeSpace;

                    moved = true;

                    ClearJumpedChecker( new Point(freeSquareRow, freeSquareColumn),
                        new Point(checkerSquareRow, checkerSquareColumn));
                }
            }

            return moved;
        }

        private void ClearJumpedChecker(Point freeSquare, Point checkerSquare)
        {
            if(Math.Abs(freeSquare.X - checkerSquare.X ) == TwoSquaresApart)
            {
                Point squareInBetween = GetSquareInBetween(
                    new Point(checkerSquare.X, checkerSquare.Y),
                    new Point(freeSquare.X, freeSquare.Y));

                if(SquareIsWithinBoardRange(squareInBetween.Y, squareInBetween.X))
                {
                    _board[squareInBetween.X, squareInBetween.Y] = _checkerConstants.FreeSpace;
                }
            }
        }

        public void DeSelect(int row, int column)
        {
            TryToThrowInvalidRowColumnException(row, column);

            _board[row,column] = _board[row,column] - _checkerConstants.SelectedChecker;
        }

        public List<Point> GetAllCheckers(int color)
        {
            var checkers = new List<Point>();

            for (int row = 0; row < _checkerConstants.SquaresInBoard; row++)
            {
                for (int column = 0; column < _checkerConstants.SquaresInBoard; column++)
                {
                    if((_board[row, column] & color) == color)
                    {
                        checkers.Add(new Point(row, column));
                    }
                }
            }

            return checkers;
        }

        public Point GetSquare(Point mousePosition)
        {
            int col = mousePosition.X / _checkerConstants.SquareWidth;
            int row = mousePosition.Y / _checkerConstants.SquareWidth;

            return new Point(col, row);
        }

        public Point GetSelectedSquare()
        {
            var selectedSquare = new Point(-1, -1);

            for (int row = 0; row < _checkerConstants.SquaresInBoard; row++)
            {
                for (int column = 0; column < _checkerConstants.SquaresInBoard; column++)
                {
                    if (IsSquareSelected(row, column))
                    {
                        selectedSquare = new Point(row, column);
                    }
                }
            }

            return selectedSquare;
        }

        public bool IsFreeSpace(int row, int column)
        {
            if(row < 0 || row >= _checkerConstants.SquaresInBoard ||
                column < 0 || column >= _checkerConstants.SquaresInBoard)
                return false;

            bool isBlackSpace = (row + column)%2 != 0;

            return isBlackSpace && _board[row,column] == 0;
        }

        public void DeSelectAllSelectionsExcept(int currentRow, int currentColumn)
        {
            //deselect any other selections
            for (int row = 0; row < _checkerConstants.SquaresInBoard; row++)
            {
                for (int column = 0; column < _checkerConstants.SquaresInBoard; column++)
                {
                    //Ignore the current selection
                    if (row == currentRow && column == currentColumn)
                        continue;

                    if (IsSquareSelected(row, column))
                    {
                        DeSelect(row, column);
                    }
                }
            }
        }

        public bool IsValidSelection(int row, int column)
        {
            TryToThrowInvalidRowColumnException(row, column);
 
            return !IsSquareSelected(row, column) &&
                   (IsRedChecker(row, column) ||
                    IsBlackChecker(row, column));
        }

        public bool IsValidMove(Point fromSquare, Point toSquare)
        {
            if(IsValidNonJumpMove(fromSquare, toSquare ))
            {
                return true;
            }

            if(IsValidJumpMove(fromSquare, toSquare))
            {
                return true;
            }

            return false;
        }

        public bool IsValidNonJumpMove(Point fromSquare, Point toSquare)
        {
            int direction = 0;

            if (IsRedChecker(fromSquare.X, fromSquare.Y))
                direction = 1;
            else if (IsBlackChecker(fromSquare.X, fromSquare.Y))
                direction = -1;
            
            if(fromSquare.X + direction == toSquare.X)
            {
                if(IsFreeSpace(toSquare.X, toSquare.Y))
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool IsValidJumpMove(Point fromSquare, Point toSquare)
        {
            Func<int, int, bool> isOppositeColorChecker = IsRedChecker;

            if (IsRedChecker(fromSquare.X, fromSquare.Y))
            {
                isOppositeColorChecker = IsBlackChecker;
            }

            if (IsFreeSpace(toSquare.X, toSquare.Y))
            {
                Point squareInBetween = GetSquareInBetween(fromSquare, toSquare);

                if (squareInBetween.X > -1)
                {
                    if (isOppositeColorChecker.Invoke(squareInBetween.X, squareInBetween.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public Point GetSquareInBetween(Point fromSquare, Point toSquare)
        {
            Point squareInBetweeen;

            bool itsValidJumpMove = Math.Abs(fromSquare.X - toSquare.X) == TwoSquaresApart &&
                                    Math.Abs(fromSquare.Y - toSquare.Y) == TwoSquaresApart;

            if(itsValidJumpMove)
                squareInBetweeen = new Point(
                toSquare.X + ((fromSquare.X - toSquare.X) / 2),
                toSquare.Y + ((fromSquare.Y - toSquare.Y) / 2));
            else
                squareInBetweeen = new Point(-1, -1);
            
            return squareInBetweeen;
        }

        private void TryToThrowInvalidRowColumnException(int row, int column)
        {
            if(row < 0 || row >= _checkerConstants.SquaresInBoard)
                throw new ArgumentException("Row is out of range");

            if (column < 0 || column >= _checkerConstants.SquaresInBoard)
                throw new ArgumentException("Column is out of range");
        }
    }
}