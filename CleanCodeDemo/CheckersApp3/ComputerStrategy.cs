using System;
using System.Drawing;
using System.Threading;

namespace CheckersApp3
{
    public class ComputerStrategy
    {
        private CheckerBoard _board;
        private CheckersConstants _constants = new CheckersConstants();
        private int _checkerColor;

        public bool Move(CheckerBoard board, int checker)
        {
            bool moved = false;
            _board = board;
            _checkerColor = checker;

            moved = MakeJump();

            if (!moved)
            {
                moved = MakeNonJump();
            }

            return moved;
        }

        private bool MakeNonJump()
        {
            int direction = 1;

            if(_checkerColor == _constants.BlackChecker)
            {
                direction = -1;
            }

            bool moved = false;
            foreach(Point checker in _board.GetAllCheckers(_checkerColor))
            {
                if(moved)
                    break;

                var columnDirections = new[] { 1, -1 };

                for (int columnDirection = 0; columnDirection < columnDirections.Length; columnDirection++)
                {
                    if(moved)
                        break;

                    if (_board.IsFreeSpace(checker.X + direction, checker.Y +columnDirections[columnDirection]))
                    {
                        if ((checker.X + direction > -1 && checker.X + direction < _constants.SquaresInBoard) &&
                            (checker.Y + columnDirections[columnDirection] < _constants.SquaresInBoard))
                        {
                            moved = _board.MoveChecker(checker.X, checker.Y, checker.X + direction, checker.Y + columnDirections[columnDirection]);
                        }
                    }
                }
            }
            GameManager.TheGameManager.Draw();
            Thread.Sleep(500);
            return moved;
        }

        private bool MakeJump()
        {
            int direction = GetDirection();

            bool moved = false;
            Point lastMove = new Point(0, 0);

            foreach (Point checker in _board.GetAllCheckers(_checkerColor))
            {
                if(moved)
                    break;

                var columnDirections = new[] { 2, -2 };

                for (int columnDirection = 0; columnDirection < columnDirections.Length; columnDirection++)
                {
                    if (moved)
                        break;

                    lastMove = new Point(checker.X + direction, checker.Y + columnDirections[columnDirection]);
                     moved = _board.MoveChecker(checker.X, checker.Y, checker.X + direction, checker.Y + columnDirections[columnDirection]);

                    GameManager.TheGameManager.Draw();
                    if(moved)
                        Thread.Sleep(300);
                }
            }

            //Attempt to jump again for multi jumps
            if (moved)
                MakeJump(lastMove);

            return moved;
        }

        private void MakeJump(Point currentChecker)
        {
            int direction = GetDirection();
            bool moved = false;
            var columnDirections = new[] {2, -2};
            Point lastMove = new Point(0, 0);

            for (int columnDirection = 0; columnDirection < columnDirections.Length; columnDirection++)
            {
                if (moved)
                    break;

                lastMove = new Point(currentChecker.X + direction, currentChecker.Y + columnDirections[columnDirection]);
                moved = _board.MoveChecker(currentChecker.X, currentChecker.Y, currentChecker.X + direction,
                                           currentChecker.Y + columnDirections[columnDirection]);
            }

            if (moved)
            {
                MakeJump(lastMove);
                Thread.Sleep(300);
            }
        }

        private int GetDirection()
        {
            int direction = 2;

            if (_checkerColor == _constants.BlackChecker)
            {
                direction = -2;
            }

            return direction;
        }
    }
}