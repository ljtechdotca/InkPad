using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ColorConverter = System.Windows.Media.ColorConverter;
using Color = System.Windows.Media.Color;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace InkPad;

public class MainController
{
    public MainView View { get; private set; }

    public MainController(MainView view)
    {
        View = view;
    }

    public void HandleColorClick(int i)
    {
        View.MainWindow.CanvasWindow.Focus();
        View.MainWindow.CanvasWindow.View.Controller.ChangeColor(i);
    }

    public void HandleToolClick(InkCanvasIconType iconType)
    {

        switch(iconType)
        {
            case InkCanvasIconType.Undo:
                View.MainWindow.CanvasWindow.View.Controller.UndoStroke();
            break;

            case InkCanvasIconType.Redo:
                View.MainWindow.CanvasWindow.View.Controller.RedoStroke();
                break;

            case InkCanvasIconType.Select:
                View.MainWindow.CanvasWindow.Focus();
                View.MainWindow.CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.Select);
                break;

            case InkCanvasIconType.Erase:
                View.MainWindow.CanvasWindow.Focus();
                View.MainWindow.CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.EraseByStroke);
                break;

            case InkCanvasIconType.Fill:
                View.MainWindow.CanvasWindow.Focus();
                View.MainWindow.CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.Ink);
                View.MainWindow.CanvasWindow.View.Controller.ToggleBackground();
                break;

            case InkCanvasIconType.Clear:
                View.MainWindow.CanvasWindow.Focus();
                View.MainWindow.CanvasWindow.View.Controller.ChangeTool(InkCanvasEditingMode.Ink);
                View.MainWindow.CanvasWindow.View.Controller.ClearCanvas();
                break;

            default: 
                break;
        }
    }


    public static bool CheckValidColor(string value)
    {
        if (Regex.Match(value, "^(?:[0-9a-fA-F]{3}){1,2}$").Success)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void HandleTextBox(int i, string color)
    {
        if (CheckValidColor(color))
        {
            View.InvokeColor(i, color);
        }
    }
    public void UpdateColors(int i, string color)
    {
        if (CheckValidColor(color))
        {
            ColorCollection.UpdateColor(i, color);
            WrapPanel wrapPanel = (WrapPanel)View.Children[i];
            Button button = (Button)wrapPanel.Children[0];
            button.Background = ColorCollection.GetSolidColorBrush(i);
            TextBox textBox = (TextBox)wrapPanel.Children[1];
            textBox.Text = color;
        }
    }

    public void HandleClear()
    {
        View.MainWindow.CanvasWindow.View.Controller.ClearCanvas();
    }
}
