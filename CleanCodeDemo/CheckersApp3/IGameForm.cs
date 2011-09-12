using System.Drawing;

namespace CheckersApp3
{
    public interface IGameForm
    {
        MouseState MouseState {
            get;
            set;
        }

        int FormWidth { get; set; }
        int FomrHeight { get; set; }
        void Initialize();
        Graphics CreateGraphics();
    }
}