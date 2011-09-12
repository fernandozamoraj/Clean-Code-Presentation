using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckersApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public bool MouseIsDown
        {
            get; set;
        }

        public Point MousePosition
        {
            get; set;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CheckerGame checkerGame = new CheckerGame();

            checkerGame.Run(this);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            MouseIsDown = true;
            MousePosition = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            MouseIsDown = false;
        }
    }
}
