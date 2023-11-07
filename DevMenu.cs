using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

internal class DevMenu
{
    Font font;
    int fontSize;
    public DevMenu(Font font, int fontSize)
    {
        this.font = font;
        this.fontSize = fontSize;
    }

    public event EventHandler DevBackButtonPressed;
    public event EventHandler DevModeButtonPressed;

    public void DrawDevMenu()
    {
        Raylib.ClearBackground(Raylib.PURPLE);

        string backText = "Go back to main menu";
        int window_width = Raylib.GetScreenWidth();
        int window_height = Raylib.GetScreenHeight();

        int fontSize = 20;
        Vector2 sw = Raylib.MeasureTextEx(font, backText, fontSize, 2);

        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 - 60, sw.X, sw.Y), "Back"))
        {
            DevBackButtonPressed.Invoke(this, EventArgs.Empty);
        }

        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 - 60, sw.X, sw.Y), "Debug mode"))
        {
            DevModeButtonPressed.Invoke(this, EventArgs.Empty);
        }
    }
}
