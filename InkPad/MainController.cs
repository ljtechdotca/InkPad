using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Shapes;
using Color = System.Windows.Media.Color;
using Brushes = System.Windows.Media.Brushes;
using System.Windows.Ink;

namespace InkPad;

public class MainController
{
    public MainView View { get; private set; }

    public MainController(MainView view)
    {
        View = view;
    }

    public (Button button, TextBox textBox) GetColorsWrapPanelContents(int i)
    {
        WrapPanel wrapPanel = (WrapPanel)View.Colors.Children[i];
        Button button = (Button)wrapPanel.Children[0];
        TextBox textBox = (TextBox)wrapPanel.Children[1];

        return (button, textBox);
    }

    public void HandleColorClick(int i)
    {
        View.Window.CanvasWindow.Focus();
        View.Window.CanvasWindow.View.Controller.ChangeColor(i);
    }

    public void HandleToolClick(InkCanvasIconType iconType)
    {

        switch (iconType)
        {
            case InkCanvasIconType.Undo:
                View.Window.CanvasWindow.View.Controller.Undo();
                break;

            case InkCanvasIconType.Redo:
                View.Window.CanvasWindow.View.Controller.Redo();
                break;

            case InkCanvasIconType.Select:
                View.Window.CanvasWindow.Focus();
                View.Window.CanvasWindow.View.Controller.ChangeMode(InkCanvasEditingMode.Select);
                break;

            case InkCanvasIconType.Erase:
                View.Window.CanvasWindow.Focus();
                View.Window.CanvasWindow.View.Controller.ChangeMode(InkCanvasEditingMode.EraseByStroke);
                break;

            case InkCanvasIconType.Fill:
                View.Window.CanvasWindow.Focus();
                View.Window.CanvasWindow.View.Controller.ChangeMode(InkCanvasEditingMode.Ink);
                View.Window.CanvasWindow.View.Controller.ToggleBackground();
                break;

            case InkCanvasIconType.Clear:
                View.Window.CanvasWindow.Focus();
                View.Window.CanvasWindow.View.Controller.ChangeMode(InkCanvasEditingMode.Ink);
                View.Window.CanvasWindow.View.Controller.ClearCanvas();
                break;

            default:
                break;
        }
    }

    public static bool CheckValidColor(string value)
    {
        return Regex.Match(value, "^([a-fA-F0-9]{6}|[a-fA-F0-9]{3})$").Success;
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
            (Button button, TextBox textBox) = GetColorsWrapPanelContents(i);
            button.Background = ColorCollection.GetSolidColorBrush(i);
            textBox.Text = color;
        }
    }

    public void HandleClear()
    {
        View.Window.CanvasWindow.View.Controller.ClearCanvas();
    }
}
