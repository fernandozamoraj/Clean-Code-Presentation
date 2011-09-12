using CheckersApp3;
using NUnit.Framework;

namespace CheckersAppTests
{
    [TestFixture]
    public class BoardStrategyFixture
    {
        private CheckerBoard _board;
        private CheckersConstants _constants;
        private ComputerStrategy _strategy;

        [SetUp]
        public void Setup()
        {
            _board = new CheckerBoard();
            _constants = new CheckersConstants();
            _strategy = new ComputerStrategy();
            ClearBoard();
        }

        private void ClearBoard()
        {
            for(int row = 0; row < _constants.SquaresInBoard; row++)
            {
                for(int column = 0; column < _constants.SquaresInBoard; column++)
                {
                    _board.Board[row, column] = _constants.FreeSpace;
                }
            }
        }

        [Test]
        public void TestNonJump()
        {
            _board.Board[1, 0] |= _constants.RedChecker;

            _strategy.Move(_board, _constants.RedChecker);

            Assert.AreEqual(_constants.FreeSpace, _board.Board[1, 0], "0,0 should be free space now");
            Assert.AreEqual(_constants.RedChecker, _board.Board[2, 1], "2,1 should be a redchecker due to jump");
        }

        [Test]
        public void Test_jump_south_east()
        {
            _board.Board[1, 0] |= _constants.RedChecker;
            _board.Board[2, 1] |= _constants.BlackChecker;

            _strategy.Move(_board, _constants.RedChecker);

            Assert.AreEqual(_constants.FreeSpace, _board.Board[1, 0], "0,0 should be free space now");
            Assert.AreEqual(_constants.FreeSpace, _board.Board[2, 1], "2,1 should be a free due to jump");
            Assert.AreEqual(_constants.RedChecker, _board.Board[3, 2], "3,2 should be occupied by red checker");
        }


        [Test]
        public void Test_jump_south_west()
        {
            _board.Board[2, 1] |= _constants.BlackChecker;
            _board.Board[1, 2] |= _constants.RedChecker;

            _strategy.Move(_board, _constants.BlackChecker);

            Assert.AreEqual(_constants.FreeSpace, _board.Board[2, 1], "2,1 should be free space now");
            Assert.AreEqual(_constants.FreeSpace, _board.Board[1, 2], "1,2 should be a free due to jump");
            Assert.AreEqual(_constants.BlackChecker, _board.Board[0, 3], "2,1 should be occupied by red checker");
        }
    }
}
