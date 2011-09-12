using System.Drawing;

namespace CheckersApp3
{
    public class BoardDrawingMachine : IBoardDrawingMachine
    {
        readonly CheckersConstants _checkerConstants = new CheckersConstants();
        private Bitmap _blackCheckerImage;
        private Bitmap _redCheckerImage;

        public void DrawBoard(Graphics formGraphics, Size boardSize, CheckerBoard board)
        {
            SetImages();
            
            Image backBuffer = DrawToBuffer(board, boardSize);

            TransferDrawingToScreen(backBuffer, formGraphics);
        }

        private Image DrawToBuffer(CheckerBoard board, Size boardSize)
        {
            //To allow doublebuffering
            var backbuffer = new Bitmap(boardSize.Width, boardSize.Height);

            using(Graphics graphicsBuffer = Graphics.FromImage(backbuffer))
            {
                DrawBoard(graphicsBuffer);
                DrawCheckers(graphicsBuffer, board, _blackCheckerImage, _redCheckerImage);
                graphicsBuffer.Dispose();
            }

            return backbuffer;
        }

        private void SetImages()
        {
            if (_blackCheckerImage == null)
            {
                _blackCheckerImage = (Bitmap) Image.FromFile("./blackchecker01.png");
                _redCheckerImage = (Bitmap) Image.FromFile("./redchecker01.png");
            }
        }

        private static void TransferDrawingToScreen(Image backbuffer, Graphics formGraphics)
        {
            formGraphics.DrawImageUnscaled(backbuffer, 0, 0);
        }

        private void DrawCheckers(Graphics graphics, CheckerBoard board, Image blackCheckerImage, Image redCheckerImage)
        {
            for (int row = 0; row < _checkerConstants.SquaresInBoard; row++)
            {
                for (int column = 0; column < _checkerConstants.SquaresInBoard; column++)
                {
                    if(board.IsRedChecker(row, column))
                        DrawChecker(row, column, graphics, redCheckerImage);

                    if (board.IsBlackChecker(row, column))
                        DrawChecker(row, column, graphics, blackCheckerImage);

                    if(board.IsSquareSelected(row, column))
                        DrawSelectedSquare(row, column, graphics);
                }
            }
        }

        private void DrawSelectedSquare(int row, int column, Graphics graphics)
        {
            graphics.DrawRectangle(Pens.Yellow,
                                   row * _checkerConstants.SquareWidth,
                                   column * _checkerConstants.SquareWidth,
                                   _checkerConstants.SquareWidth,
                                   _checkerConstants.SquareWidth);
        }

        private void DrawChecker(int row, int column, Graphics graphics, Image checkerImage)
        {
            //If the board square is even or black color
            if((row + column)%2!=0)
                graphics.DrawImage(checkerImage, 
                                   row * _checkerConstants.SquareWidth + 5, 
                                   column * _checkerConstants.SquareWidth + 5, 
                                   _checkerConstants.SquareWidth - 10, 
                                   _checkerConstants.SquareWidth - 10);
        }

        private void DrawBoard(Graphics graphics)
        {
            for (int row = 0; row < _checkerConstants.SquaresInBoard; row++)
            {
                for (int column = 0; column < _checkerConstants.SquaresInBoard; column++)
                {
                    if ((row + column) % 2 == 0)
                        DrawRedSquare(graphics, new Point(row*_checkerConstants.SquareWidth, column*_checkerConstants.SquareWidth), new Size(_checkerConstants.SquareWidth, _checkerConstants.SquareWidth) );
                    else
                        DrawBlackSquare(graphics, new Point(row * _checkerConstants.SquareWidth, column * _checkerConstants.SquareWidth), new Size(_checkerConstants.SquareWidth, _checkerConstants.SquareWidth));
               }
            }
        }

        private void DrawRedSquare(Graphics graphics, Point position, Size squareSize)
        {
            Brush brush = Brushes.Red;
            graphics.FillRectangle(brush,position.X, position.Y, squareSize.Width, squareSize.Height);
        }

        private void DrawBlackSquare(Graphics graphics, Point position, Size squareSize)
        {
            Brush brush = Brushes.Black;
            graphics.FillRectangle(brush, position.X, position.Y, squareSize.Width, squareSize.Height);
        }
    }
}