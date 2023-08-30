using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace invaders
{
    internal class Menu
    {
        Font font;
        int fontSize;
        public Menu(Font font, int  fontSize)
        {
            this.font = font;
            this.fontSize = fontSize;
        }

        public event EventHandler StartButtonPressed;
        public void DrawStartScreen()
        {
            string startText = "Space Invaders";
            int window_width = Raylib.GetScreenWidth();
            int window_height = Raylib.GetScreenHeight();

            int fontSize = 20;
            Vector2 sw = Raylib.MeasureTextEx(font, startText, fontSize, 2);
            Raylib.DrawTextEx(font, startText, new Vector2(window_width / 2 - sw.X / 2, window_height / 2 - 60), fontSize, 2, Raylib.WHITE);

            if (RayGui.GuiButton(new Rectangle(100, 100, 100, 100), "Start Game"))
            {
                StartButtonPressed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
