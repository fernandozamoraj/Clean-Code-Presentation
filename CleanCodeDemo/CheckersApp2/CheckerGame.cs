using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersApp2
{
    /// <summary>
    /// Used constants instead of magic numbers
    /// </summary>
    internal class CheckerGame
    {
        private Timer _timer;
        private Form1 _parentForm;
        private int _status;
        private readonly int SQUARE_WIDTH = 80;
        private readonly int SQUARES_IN_BOARD = 8;
        private readonly int RED_CHECKER = 1;
        private readonly int BLACK_CHECKER = 2;
        private readonly int SELECTED_CHECKER = 4;
        private readonly int FREE_SPACE = 0;

        private int[][] _board = new[]
                                     {
                                        new[] {0, 1, 0, 1, 0, 1, 0, 1},
                                        new[] {1, 0, 1, 0, 1, 0, 1, 0},
                                        new[] {0, 1, 0, 1, 0, 1, 0, 1},
                                        new[] {0, 0, 0, 0, 0, 0, 0, 0},
                                        new[] {0, 0, 0, 0, 0, 0, 0, 0},
                                        new[] {2, 0, 2, 0, 2, 0, 2, 0},
                                        new[] {0, 2, 0, 2, 0, 2, 0, 2},
                                        new[] {2, 0, 2, 0, 2, 0, 2, 0}
                                    };
        

        public void Run(Form1 parent)
        {
            
            _status = 1;
            _parentForm = parent;

            InitializeForm();

            _timer = new Timer();
            _timer.Interval = 30;
            _timer.Tick += _timer_Tick;
            _timer.Enabled = true;
            _timer.Start();
        }

        void _timer_Tick(object sender, EventArgs e)
        {
            if(_status == 1)
            {
                Update();
            }

            Draw();
        }

        private void InitializeForm()
        {
            _parentForm.ShowIcon = false;
            _parentForm.FormBorderStyle = FormBorderStyle.None;
            _parentForm.BackColor = Color.Black;
            _parentForm.MaximizeBox = false;
            _parentForm.Size = new Size(SQUARE_WIDTH*SQUARES_IN_BOARD, SQUARE_WIDTH*SQUARES_IN_BOARD);
        }

        private void Update()
        {
            if(_parentForm.MouseIsDown)
            {
                Point cell = MouseCell(_parentForm.MousePosition);

                //If not selected
                //select it
                if((_board[cell.X][cell.Y] & SELECTED_CHECKER) != SELECTED_CHECKER  &&
                    ((_board[cell.X][cell.Y] & RED_CHECKER) == RED_CHECKER ||  //Is red checker
                     (_board[cell.X][cell.Y] & BLACK_CHECKER) == BLACK_CHECKER)) //Is black checker
                {
                    _board[cell.X][cell.Y] |= SELECTED_CHECKER;
                }

                bool bSelected = false;
                Point selectedChecker = new Point(0, 0);

                //deselect any other selections
                for (int row = 0; row < SQUARES_IN_BOARD; row++)
                {
                    for (int column = 0; column < SQUARES_IN_BOARD; column++)
                    {
                        if ((_board[row][column] & SELECTED_CHECKER) == SELECTED_CHECKER)
                        {
                            bSelected = true;
                            selectedChecker = new Point(row, column);
                        }
                    }
                }

                if(bSelected)
                {
                    if (_board[cell.X][cell.Y] == FREE_SPACE)
                    {
                        //Check if move is valid
                        if ( (selectedChecker.X + 1 == cell.X && 
                               (_board[selectedChecker.X][selectedChecker.Y] & RED_CHECKER) == RED_CHECKER) ||
                             (selectedChecker.X - 1 == cell.X && 
                               (_board[selectedChecker.X][selectedChecker.Y] & BLACK_CHECKER) == BLACK_CHECKER)                          )
                        {

                            //Move checker
                            _board[cell.X][cell.Y] = _board[selectedChecker.X][selectedChecker.Y];
                            _board[selectedChecker.X][selectedChecker.Y] = FREE_SPACE;
                        }
                    }
                }

                //deselect any other selections
                for (int row = 0; row < SQUARES_IN_BOARD; row++)
                {
                    for(int column = 0 ; column < SQUARES_IN_BOARD; column++)
                    {
                        //Ignore the current selection
                        if(row == cell.X && column == cell.Y)
                            continue;

                        if((_board[row][column] & SELECTED_CHECKER) == SELECTED_CHECKER)
                        {
                            _board[row][column] = _board[row][column] - SELECTED_CHECKER;
                        }
                    }
                }
            }
        }

        private Point MouseCell(Point position)
        {
            int col = position.X/SQUARE_WIDTH;
            int row = position.Y/SQUARE_WIDTH;

            return new Point(col, row);
        }

        private void Draw()
        {
            Bitmap blackCheckerImage = (Bitmap)Bitmap.FromFile("./blackchecker01.png");
            Bitmap redCheckerImage = (Bitmap)Bitmap.FromFile("./redchecker01.png");
            
            //To allow doublebuffering
            Bitmap backbuffer = new Bitmap(_parentForm.ClientSize.Width, _parentForm.ClientSize.Height);

            //Draw to the backbuffer
            using(Graphics graphics = Graphics.FromImage(backbuffer))
            {
                //Draw board
                for (int row = 0; row < SQUARES_IN_BOARD; row++)
                {
                    for (int column = 0; column < SQUARES_IN_BOARD; column++)
                    {
                        if ((row + column) % 2 == 0)
                        {
                            graphics.FillRectangle(Brushes.Red, row * SQUARE_WIDTH, column * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH);
                        }
                        else
                        {
                            graphics.FillRectangle(Brushes.Black, row * SQUARE_WIDTH, column * SQUARE_WIDTH, SQUARE_WIDTH, SQUARE_WIDTH);
                        }
                    }
                }

                //Draw Checkers
                for (int row = 0; row < SQUARES_IN_BOARD; row++)
                {
                    for (int column = 0; column < SQUARES_IN_BOARD; column++)
                    {
                        //if black checker
                        //black checker has a value of 1
                        if ((_board[row][column] & RED_CHECKER) == RED_CHECKER)
                        {
                            //If the board square is even or black color
                            if((row + column)%2!=0)
                                graphics.DrawImage(blackCheckerImage, row*SQUARE_WIDTH+5, column*SQUARE_WIDTH+5, SQUARE_WIDTH-10, SQUARE_WIDTH-10);
                        }

                        //If red checker
                        //Red checker has a value of 2
                        if ((_board[row][column] & BLACK_CHECKER) == BLACK_CHECKER && (row + column) % 2 != 0)
                        {
                            graphics.DrawImage(redCheckerImage, row * SQUARE_WIDTH + 5, column * SQUARE_WIDTH + 5, SQUARE_WIDTH-10, SQUARE_WIDTH-10);
                        }

                        //If the square is selected
                        if((_board[row][column] & 4) == 4)
                        {
                            graphics.DrawRectangle(Pens.Yellow, row*SQUARE_WIDTH+5, column*SQUARE_WIDTH+5, SQUARE_WIDTH-10, SQUARE_WIDTH-10);
                        }
                    }
                }
            }

            //Draw onto the window
            using (Graphics windowGraphics = _parentForm.CreateGraphics())
            {
                windowGraphics.DrawImageUnscaled(backbuffer, 0, 0);
            }
        }
    }
}