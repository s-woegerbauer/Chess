using Chess_Cabs.Game;
using System.Runtime.InteropServices;

namespace Chess_Cabs.Utils
{
    class Mouse
    {
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);
        public struct Point
        {
            public int X;
            public int Y;
        }
        public static int[] GetCur()
        {
            Point a = new Point();
            GetCursorPos(out a);
            int[] sol = new int[2];
            sol[0] = a.X;
            sol[1] = a.Y;
            return sol;
        }

        [DllImport("user32.dll")]
        public static extern bool GetAsyncKeyState(int button);
        public static bool IsMouseButtonPressed(MouseButton button)
        {
            return GetAsyncKeyState((int)button);
        }
        public enum MouseButton
        {
            LeftMouseButton = 0x01,
            RightMouseButton = 0x02,
            MiddleMouseButton = 0x04,
        }

        public static int[] WaitForInput()
        {
            while (true)
            {
                if (IsMouseButtonPressed(MouseButton.LeftMouseButton))
                {
                    int[] cur = GetCur();
                    return cur;
                }
            }
        }

        public static int[] GetCoords(int xPixel, int yPixel)
        {

            int yTop = 48;
            int yBottom = 78;
            int xLeft = 25;
            int xRight = 63;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (xPixel <= xRight && xPixel >= xLeft && yPixel <= yBottom && yPixel >= yTop)
                    {
                        return new int[] { i, j };
                    }

                    xRight += 38;
                    xLeft += 38;
                }
                yTop += 32;
                yBottom += 32;
                xRight = 63;
                xLeft = 25;
            }
            

            return new int[] { -1, -1 };
        }
    }
}