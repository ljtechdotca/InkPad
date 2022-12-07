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
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace InkPad;

public class CanvasView : InkCanvas
{

	public CanvasWindow Window { get; private set; }
	public CanvasController Controller { get; private set; }

	public CanvasView(CanvasWindow window)
	{
        Window = window;
        Controller = new CanvasController(this);
		Width = Controller.Width;
		Height = Controller.Height;
		DefaultDrawingAttributes.Color = ColorCollection.GetColor(2);
		DefaultDrawingAttributes.FitToCurve = true;
        Background = Brushes.Transparent;

		StrokeCollected += (sender, e) =>
		{
			Debug.WriteLine("Stroke was collected!");
			StrokeCollection strokes = new(Strokes);
			Controller.AddStrokeCollection(strokes);
		};

        SelectionChanged += (sender, e) =>
        {
            Debug.WriteLine("A stroke was moved!");
            StrokeCollection strokes = new(Strokes);
            Controller.AddStrokeCollection(strokes);
        };

		StrokeErased += (sender, e) =>
        {
            Debug.WriteLine("A stroke has been erased!");
            StrokeCollection strokes = new(Strokes);
            Controller.AddStrokeCollection(strokes);
        };

		StrokesReplaced += (sender, e) =>
		{
			Debug.WriteLine("The strokes collection has been updated!");
            //Controller.AddStrokeCollection(Strokes);
        };
    }

}
