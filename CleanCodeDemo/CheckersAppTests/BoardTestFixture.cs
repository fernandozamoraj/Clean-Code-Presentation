using System.Drawing;
using CheckersApp3;
using NUnit.Framework;

namespace CheckersAppTests
{
    [TestFixture]
    public class BoardTestFixture
    {
        private CheckerBoard _board;

        [SetUp]
        public void Setup()
        {
            _board = new CheckerBoard();
        }

        [Test]
        public void Check_that_board_is_initialized()
        {
            int redCheckerCount = 0;
            int blackCheckerCount = 0;

            for (int row = 0; row < _board.Board.GetLength(0); row++ )
            {
                for(int column = 0; column < _board.Board.GetLength(1); column++)
                {
                    if(_board.IsRedChecker(row, column))
                    {
                        redCheckerCount++;
                    }
                    if(_board.IsBlackChecker(row, column))
                    {
                        blackCheckerCount++;
                    }
                }
            }

            Assert.AreEqual(12, redCheckerCount);
            Assert.AreEqual(12, blackCheckerCount);
        }

        [Test]
        public void Check_selects_only_valid_spaces()
        {
            _board.SetCellToSelected(0,0);
            Assert.IsTrue(!_board.IsSquareSelected(0, 0), "Selected 0, 0");

            _board.SetCellToSelected(0, 1);
            Assert.IsTrue(_board.IsSquareSelected(0, 1), "Did not select 0, 1");
        }

        [Test]
        public void Check_that_it_gets_square_in_between_souteast()
        {
            Point squareInBetween = _board.GetSquareInBetween(new Point(0, 0), new Point(2, 2));

            Assert.AreEqual(1, squareInBetween.X, "Row is not correct");
            Assert.AreEqual(1, squareInBetween.Y, "Column is not correct");
        }

        [Test]
        public void Check_that_it_gets_square_in_between_nortWest()
        {
            Point squareInBetween = _board.GetSquareInBetween(new Point(2, 2), new Point(0, 0));

            Assert.AreEqual(1, squareInBetween.X, "Row is not correct");
            Assert.AreEqual(1, squareInBetween.Y, "Column is not correct");
        }

        [Test]
        public void Check_that_it_gets_square_in_between_soutwest()
        {
            Point squareInBetween = _board.GetSquareInBetween(new Point(2, 0), new Point(0, 2));

            Assert.AreEqual(1, squareInBetween.X, "Row is not correct");
            Assert.AreEqual(1, squareInBetween.Y, "Column is not correct");
        }

        [Test]
        public void Check_that_it_gets_square_in_between_northeast()
        {
            Point squareInBetween = _board.GetSquareInBetween(new Point(0, 2), new Point(2, 0));

            Assert.AreEqual(1, squareInBetween.X, "Row is not correct");
            Assert.AreEqual(1, squareInBetween.Y, "Column is not correct");
        }

        [Test]
        public void Check_that_it_doesnot_get_square_in_between_bogus_settings()
        {
            Point squareInBetween = _board.GetSquareInBetween(new Point(0, 1), new Point(2, 0));

            Assert.AreEqual(-1, squareInBetween.X, "Row is not correct");
            Assert.AreEqual(-1, squareInBetween.Y, "Column is not correct");
        }
    }
}
