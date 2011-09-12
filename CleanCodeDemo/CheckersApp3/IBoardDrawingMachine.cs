using System.Drawing;

namespace CheckersApp3
{
    public interface IBoardDrawingMachine
    {
        void DrawBoard(Graphics formGraphics, Size boardSize, CheckerBoard board);
    }
}