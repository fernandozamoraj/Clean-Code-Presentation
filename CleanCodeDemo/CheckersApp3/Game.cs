using System;
using System.Windows.Forms;

namespace CheckersApp3
{
    public class Game
    {
        private readonly IGameForm _parentForm;
        private readonly GameStatus _status;
        private Timer _timer;
       
        public Game(IGameForm parent)
        {

            _status = new GameStatus {GameOver = false};
            _parentForm = parent;

            InitializeForm();
        }

        private void InitializeForm()
        {
            _parentForm.Initialize();
        }

        void TimerTick(object sender, EventArgs e)
        {
            if (!_status.GameOver)
            {
                Update();
            }

            Draw();
        }

        protected virtual void Update()
        {
        }

        public virtual void Draw()
        {
        }

        public virtual void Run()
        {
            _timer = new Timer
                         {
                             Interval = 30
                         };

            _timer.Tick += TimerTick;
            _timer.Enabled = true;
            _timer.Start();
        }

        protected IGameForm ParentForm
        {
            get
            {
                return _parentForm;
            }
        }
    }
}