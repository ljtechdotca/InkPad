using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Brushes = System.Windows.Media.Brushes;

namespace InkPad;


public class CanvasWindow : Window
{
    public CanvasView View { get; private set; }
    public MainWindow MainWindow { get; private set; }

    public CanvasWindow(MainWindow mainWindow)
    {
        Title = "Canvas Window";
        AllowsTransparency = true;
        Background = Brushes.Transparent;
        WindowStyle = WindowStyle.None;
        ResizeMode = ResizeMode.NoResize;
        SizeToContent = SizeToContent.WidthAndHeight;
        View = new CanvasView(this);
        MainWindow = mainWindow;
        Content = View;
        Width = View.Controller.Width;
        Height = View.Controller.Height;
        Top = View.Controller.MinY;
        Left = View.Controller.MinX;
        Topmost = false;

        Activated += (sender, e) =>
        {
            if (View.Controller.isBackgroundActive) return;
 
            SolidColorBrush background = new(Color.FromArgb(1, 0, 0, 0));
            Background = background;
            View.Background = background;
        };

        Deactivated += (sender, e) =>
        {
            if (View.Controller.isBackgroundActive) return;

            SolidColorBrush background = Brushes.Transparent;
            Background = background;
            View.Background = background;
        };

        KeyDown += (sender, e) =>
        {
           MainWindow.KeyBinds.HandleKeyDown(e.Key);
        };

        this.Show();
    }
}
