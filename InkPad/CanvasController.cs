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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;
using Screen = System.Windows.Forms.Screen;
using Cursors = System.Windows.Input.Cursors;
using Button = System.Windows.Controls.Button;

namespace InkPad;

public class CanvasController
{
    public bool isBackgroundActive { get; set; } = false;
    public double MinX { get; set; }
    public double MinY { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public CanvasView View { get; private set; }
    public List<StrokeCollection> StrokeCollectionStack { get; private set; } = new();
    public List<StrokeCollection> TempStrokeCollectionStack { get; private set; } = new();

    public CanvasController(CanvasView view)
    {
        View = view;

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

    public void AddStrokeCollection(StrokeCollection strokes)
    {
        StrokeCollectionStack.Add(strokes);
        Debug.WriteLine(StrokeCollectionStack.Count);
        TempStrokeCollectionStack.Clear();
    }

    public void Redo()
    {
        if (TempStrokeCollectionStack.Count > 0)
        {
            int lastIndex = TempStrokeCollectionStack.Count - 1;
            StrokeCollection lastStrokeCollection = TempStrokeCollectionStack[lastIndex];
            TempStrokeCollectionStack.RemoveAt(lastIndex);
            StrokeCollectionStack.Add(lastStrokeCollection);
            View.Strokes = lastStrokeCollection;
        }
    }

    public void Undo()
    {
        //Debug.WriteLine($"Undoing the last collection, current count: {StrokeCollectionStack.Count}");
        if (StrokeCollectionStack.Count > 0)
        {
            int currentCollectionIndex = StrokeCollectionStack.Count - 1;
            StrokeCollection currentCollection = StrokeCollectionStack[currentCollectionIndex];
            StrokeCollectionStack.RemoveAt(currentCollectionIndex);
            TempStrokeCollectionStack.Add(currentCollection);
            //Debug.WriteLine($"Removing current strokes collection. Your current stroke collection stack size is now : {StrokeCollectionStack.Count}");
            //Debug.WriteLine($"Adding current strokes collection to temp collection stack. Temp stack size is now : {TempStrokeCollectionStack.Count}");
            StrokeCollection previousCollection = new();
            if (StrokeCollectionStack.Count > 0)
            {
                int lastCollectionIndex = StrokeCollectionStack.Count - 1;
                previousCollection = StrokeCollectionStack[lastCollectionIndex];
                //Debug.WriteLine($"Found a previous stroke collection on index {lastCollectionIndex}, it has {previousCollection.Count} strokes");
            }
            View.Strokes = previousCollection;
        }
    }


    public void ChangeMode(InkCanvasEditingMode mode)
    {
        View.UseCustomCursor = false;
        View.EditingMode = mode;

        if (mode is InkCanvasEditingMode.Ink)
        {
            View.UseCustomCursor = true;
            View.Cursor = Cursors.Pen;
        }
    }

    public void ChangeColor(int i)
    {
        ChangeMode(InkCanvasEditingMode.Ink);
        
        View.DefaultDrawingAttributes.Color = ColorCollection.GetColor(i);

        (Button prevButton, _) = View.Window.MainWindow.View.Controller.GetColorsWrapPanelContents(ColorCollection.ActiveColorIndex);
        Ellipse prevCircle = (Ellipse)prevButton.Content;
        prevCircle.Stroke = Brushes.Transparent;
        prevCircle.Fill = Brushes.Transparent;

        ColorCollection.ActiveColorIndex = i; // Updates the active color index

        (Button button, _) = View.Window.MainWindow.View.Controller.GetColorsWrapPanelContents(i);
        Ellipse circle = (Ellipse)button.Content;
        circle.Stroke = Brushes.Black;
        circle.StrokeThickness = 0.5;
        circle.Fill = new SolidColorBrush(Color.FromArgb(155, 255, 255, 255));
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
            View.Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0));
            isBackgroundActive = true;
        }
    }

    public void ClearCanvas()
    {
        StrokeCollection strokes = new();
        View.Strokes = strokes;
        AddStrokeCollection(strokes);
    }
}
