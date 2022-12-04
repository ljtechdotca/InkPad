using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace InkPad;

public class KeyBinds

{
    public MainWindow MainWindow { get; set; }
    public CanvasWindow CanvasWindow { get; set; }

    public KeyBinds(MainWindow mainWindow, CanvasWindow canvasWindow)
    {
        MainWindow = mainWindow;
        CanvasWindow = canvasWindow;
    }

    public void HandleKeyDown(Key key)
    {
        Debug.WriteLine($"Handling the key {key}");

        switch (key)
        {
            // Color 1
            case Key.D1:
                CanvasWindow.View.Controller.ChangeColor(0);
                break;

            // Color 2
            case Key.D2:
                CanvasWindow.View.Controller.ChangeColor(1);
                break;

            // Color 3
            case Key.D3:
                CanvasWindow.View.Controller.ChangeColor(2);
                break;

            // Color 4
            case Key.D4:
                CanvasWindow.View.Controller.ChangeColor(3);
                break;

            case Key.Q:
                CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.Select);
                break;

            case Key.W:
                CanvasWindow.View.Controller.ToggleBackground();
                break;

            case Key.E:
                CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.EraseByStroke);
                break;

            case Key.R:
                CanvasWindow.View.Controller.ClearCanvas();
                break;

            // Copy
            //case Key.C:
            //    break;

            // Paste
            //case Key.V:
            //    break;

            // Cut
            //case Key.X:
            //    break;

            // Redo
            case Key.Y:
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    CanvasWindow.View.Controller.RedoStroke();
                }
                break;

            // Undo
            case Key.Z:
                if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
                {
                    CanvasWindow.View.Controller.UndoStroke();
                }
                break;
        }
    }
}
