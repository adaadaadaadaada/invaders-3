using Raylib_CsLo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

internal class PauseMenu
{
    Font font;
    int fontSize;
    public PauseMenu(Font font, int fontSize)
    {
        this.font = font;
        this.fontSize = fontSize;
    }

    public event EventHandler BackButtonPressed;
    public event EventHandler QuitButtonPressed;
    public event EventHandler SettingsButtonPressed;

    public void DrawPauseMenu()
    {
        Raylib.ClearBackground(Raylib.PURPLE);

        string backText = "Quit back to menu";
        string quitText = "Quit";
        int window_width = Raylib.GetScreenWidth();
        int window_height = Raylib.GetScreenHeight();

        int fontSize = 20;
        Vector2 sw = Raylib.MeasureTextEx(font, backText, fontSize, 2);
        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 + 60, sw.X, sw.Y), "Back to game"))
        {
            BackButtonPressed.Invoke(this, EventArgs.Empty);
        }
        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2, sw.X, sw.Y), "Quit"))
        {
            QuitButtonPressed.Invoke(this, EventArgs.Empty);
            return;
        }
        if (RayGui.GuiButton(new Rectangle(window_width / 2 - sw.X / 2, window_height / 2 - 60, sw.X, sw.Y), "Settings"))
        {
            SettingsButtonPressed.Invoke(this, EventArgs.Empty);
        }
    }
}
