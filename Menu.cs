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
        public Menu(Font font, int fontSize)
        {
            this.font = font;
            this.fontSize = fontSize;
        }

        public event EventHandler StartButtonPressed;
        public event EventHandler SettingsButtonPressed;
        public event EventHandler CloseButtonPressed;
        public void DrawStartScreen()
        {
            Raylib.ClearBackground(Raylib.BLACK);

            string gameText = "Space Invaders";
            string settingsText = "Settings";
            string closeText = "Close";
            int window_width = Raylib.GetScreenWidth();
            int window_height = Raylib.GetScreenHeight();


            int fontSize = 20;
            Vector2 sw = Raylib.MeasureTextEx(font, gameText, fontSize, 2);
            Raylib.DrawTextEx(font, gameText, new Vector2(window_width / 2 - sw.X / 2, window_height / 2 - 110), fontSize, 2, Raylib.PURPLE);

            if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 - 60, sw.X, sw.Y), "Start"))
            {
                StartButtonPressed.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2, sw.X, sw.Y), "Settings"))
            {
                SettingsButtonPressed.Invoke(this, EventArgs.Empty);
            }

            if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 + 60, sw.X, sw.Y), "Close"))
            {
                CloseButtonPressed.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
