using System.Drawing;

namespace CheckersApp3
{
    /// <summary>
    /// Separation of concerns
    /// </summary>
    public class CheckerGame : Game
    {
        readonly CheckerBoard _board = new CheckerBoard();
        readonly IBoardDrawingMachine _drawingMachine;
        private int _currentPlayer = 1;
        private CheckersConstants _constants = new CheckersConstants();
        private ComputerStrategy _strategy = new ComputerStrategy();
        
        public CheckerGame(IGameForm form, IBoardDrawingMachine drawingMachine): base(form)
        {
            _drawingMachine = drawingMachine;
        }

        void SwitchPlayer()
        {
            if (_currentPlayer == _constants.PlayerOne)
                _currentPlayer = _constants.PlayerTwo;
            else
                _currentPlayer = _constants.PlayerOne;
        }
        
        protected override void Update()
        {
            bool moveWasMade = false;

            if(PlayerClickedMouse())
            {
                moveWasMade = SetPlayerMove();
            }
            else if(IsTheComputersTurn())
            {
                moveWasMade = _strategy.Move(_board, _constants.BlackChecker);
            }

            if (moveWasMade)
               SwitchPlayer();
        }

        private bool SetPlayerMove()
        {
            bool moveWasMade;
            Point selectedSquare = _board.GetSquare(ParentForm.MouseState.MousePosition);

            if (_board.IsRedChecker(selectedSquare.X, selectedSquare.Y))
                _board.SetCellToSelected(selectedSquare.X, selectedSquare.Y);

            Point selectedChecker = _board.GetSelectedSquare();
            moveWasMade = _board.MoveChecker(selectedChecker.X, selectedChecker.Y, selectedSquare.X, selectedSquare.Y);

            _board.DeSelectAllSelectionsExcept(selectedSquare.X, selectedSquare.Y);
            return moveWasMade;
        }

        private bool IsTheComputersTurn()
        {
            return _currentPlayer == _constants.PlayerTwo;
        }

        private bool PlayerClickedMouse()
        {
            return _currentPlayer == _constants.PlayerOne && ParentForm.MouseState.IsMouseButtonDown;
        }

        public override void Draw()
        {
            using (Graphics graphics = ParentForm.CreateGraphics())
            {
                _drawingMachine.DrawBoard(graphics,
                                     new Size(ParentForm.FormWidth, ParentForm.FomrHeight),
                                     _board);
                graphics.Dispose();
            }
        }
    }
}