using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Ink;
using Brushes = System.Windows.Media.Brushes;

namespace InkPad;

public class CanvasController
{
    public bool isBackgroundActive { get; set; } = false;
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public CanvasView View { get; private set; }
    public StrokeCollection Clipboard { get; private set; }

    public CanvasController(CanvasView view)
    {
        View = view;

        Clipboard = new StrokeCollection();

        GetDrawingArea();
    }

    public void GetDrawingArea()
    {
        double minX = 0, minY = 0, maxX = 0, maxY = 0;

        foreach (var screen in Screen.AllScreens)
        {
            double currWidth = screen.Bounds.Width;
            double currHeight = screen.Bounds.Height;
            double currX = screen.Bounds.X;
            double currY = screen.Bounds.Y;

            if (currX < minX)
            {
                MinX = currX;
            }
            if (currX + currWidth > maxX)
            {
                maxX = currX + currWidth;
            }
            if (currY < minY)
            {
                MinY = currY;
            }
            if (currY + currHeight > maxY)
            {
                maxY = currY + currHeight;
            }
        }

        Width = maxX - MinX;
        Height = maxY - MinY;
    }

    public void UndoStroke()
    {
        if (View.Strokes.Count > 0)
        {
            int lastIndex = View.Strokes.Count - 1;
            Stroke stroke = View.Strokes[lastIndex];
            Clipboard.Add(stroke);
            View.Strokes.RemoveAt(lastIndex);
        }
    }

    public void RedoStroke()
    {
        if (Clipboard.Count > 0)
        {
            int lastIndex = Clipboard.Count - 1;
            Stroke stroke = Clipboard[lastIndex];
            View.Strokes.Add(stroke);
            Clipboard.RemoveAt(lastIndex);
        }
    }

    public void ChangeTool(InkCanvasEditingMode tool)
    {
        View.EditingMode = tool;
    }

    public void ChangeColor(int i)
    {
        View.EditingMode = InkCanvasEditingMode.Ink;
        View.DefaultDrawingAttributes.Color = ColorCollection.GetColor(i);
    }

    public void ToggleBackground()
    {
        if (isBackgroundActive)
        {
            View.Background = Brushes.Transparent;
            isBackgroundActive = false;
        }
        else
        {
            isBackgroundActive = true;
            View.Background = Brushes.Black;
        }
    }

    public void ClearCanvas()
    {
        View.Strokes = new StrokeCollection();
    }
}
