using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersApp1
{
    internal class CheckerGame
    {
        private Timer _timer;
        private Form1 _parentForm;
        private int _status;

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
            _timer.Tick += new EventHandler(_timer_Tick);
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
            _parentForm.Size = new Size(80*8, 80*8);
        }

        private Random _randomGenerator = new Random(0);

        private void Update()
        {
            if(_parentForm.MouseIsDown)
            {
                Point cell = MouseCell(_parentForm.MousePosition);

                //If not selected
                //select it
                if((_board[cell.X][cell.Y] & 4) != 4  &&
                    ((_board[cell.X][cell.Y] & 1) == 1 ||  //Is red checker
                     (_board[cell.X][cell.Y] & 2) == 2)) //Is black checker
                {
                    _board[cell.X][cell.Y] |= 4;
                }

                bool bSelected = false;
                Point selectedChecker = new Point(0, 0);

                //deselect any other selections
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((_board[i][j] & 4) == 4)
                        {
                            bSelected = true;
                            selectedChecker = new Point(i, j);
                        }
                    }
                }

                if(bSelected)
                {
                    if (_board[cell.X][cell.Y] == 0)
                    {
                        //Check if move is valid
                        if ( (selectedChecker.X + 1 == cell.X && 
                               (_board[selectedChecker.X][selectedChecker.Y] & 1) == 1) ||
                             (selectedChecker.X - 1 == cell.X && 
                               (_board[selectedChecker.X][selectedChecker.Y] & 2) == 2)                          )
                        {

                            //Move checker
                            _board[cell.X][cell.Y] = _board[selectedChecker.X][selectedChecker.Y];
                            _board[selectedChecker.X][selectedChecker.Y] = 0;
                        }
                    }
                }

                //deselect any other selections
                for (int i = 0; i < 8; i++)
                {
                    for(int j = 0 ; j < 8; j++)
                    {
                        //Ignore the current selection
                        if(i == cell.X && j == cell.Y)
                            continue;

                        if((_board[i][j] & 4) == 4)
                        {
                            _board[i][j] = _board[i][j] - 4;
                        }
                    }
                }
            }
        }

        private Point MouseCell(Point position)
        {
            int col = position.X/80;
            int row = position.Y/80;

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
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if ((i + j) % 2 == 0)
                        {
                            graphics.FillRectangle(Brushes.Red, i * 80, j * 80, 80, 80);
                        }
                        else
                        {
                            graphics.FillRectangle(Brushes.Black, i * 80, j * 80, 80, 80);
                        }
                    }
                }

                //Draw Checkers
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        //if black checker
                        //black checker has a value of 1
                        if ((_board[i][j] & 1) == 1)
                        {
                            //If the board square is even or black color
                            if((i + j)%2!=0)
                                graphics.DrawImage(blackCheckerImage, i*80+5, j*80+5, 70, 70);
                        }

                        //If red checker
                        //Red checker has a value of 2
                        if ((_board[i][j] & 2) == 2 && (i + j) % 2 != 0)
                        {
                            graphics.DrawImage(redCheckerImage, i * 80 + 5, j * 80 + 5, 70, 70);
                        }

                        //If the square is selected
                        if((_board[i][j] & 4) == 4)
                        {
                            graphics.DrawRectangle(Pens.Yellow, i*80+5, j*80+5, 70, 70);
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