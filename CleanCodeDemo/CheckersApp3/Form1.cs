using System;
using System.Drawing;
using System.Windows.Forms;

namespace CheckersApp3
{
    public partial class Form1 : Form, IGameForm
    {
        readonly CheckersConstants _checkerConstants = new CheckersConstants();

        public Form1()
        {
            InitializeComponent();
            MouseState = new MouseState
                             {
                                 IsMouseButtonDown = false, 
                                 MousePosition = new Point(-1, -1)
                             };
        }

        public MouseState MouseState
        {
            get; set;
        }

        public void Initialize()
        {
            ShowIcon = false;
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.Black;
            MaximizeBox = false;
            Size = new Size(_checkerConstants.SquareWidth * _checkerConstants.SquaresInBoard,
                                        _checkerConstants.SquareWidth * _checkerConstants.SquaresInBoard);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var checkerGame = new CheckerGame(this, new BoardDrawingMachine());

            GameManager.CreateTheGame(checkerGame);

            checkerGame.Run();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseState.IsMouseButtonDown = true;
            MouseState.MousePosition = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseState.IsMouseButtonDown = false;
        }
    }
}
