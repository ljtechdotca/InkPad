using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace InkPad;

public class CanvasView : InkCanvas
{
	public CanvasController Controller { get; private set; }

	public CanvasView()
	{
		Controller = new CanvasController(this);
		Width = Controller.Width;
		Height = Controller.Height;
		DefaultDrawingAttributes.Color = ColorCollection.GetColor(2);
		DefaultDrawingAttributes.FitToCurve = true;
        Background = Brushes.Transparent;
	}

}
