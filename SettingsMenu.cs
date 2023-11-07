using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

internal class SettingsMenu
{
    Font font;
    int fontSize;
    public SettingsMenu(Font font, int fontSize)
    {
        this.font = font;
        this.fontSize = fontSize;
    }

    public event EventHandler BackButtonPressed;

    public void DrawSettingsMenu()
    {
        Raylib.ClearBackground(Raylib.PURPLE);

        string backText = "Go back to main menu";
        int window_width = Raylib.GetScreenWidth();
        int window_height = Raylib.GetScreenHeight();

        int fontSize = 20;
        Vector2 sw = Raylib.MeasureTextEx(font, backText, fontSize, 2);
        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 - 60, sw.X, sw.Y), "Back"))
        {
            BackButtonPressed.Invoke(this, EventArgs.Empty);
        }
    }
}
