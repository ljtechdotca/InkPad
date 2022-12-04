using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;

namespace InkPad;

public class MainWindow : Window
{
    public KeyBinds KeyBinds { get; private set; }
    public MainView View { get; private set; }
    public CanvasWindow CanvasWindow { get; private set; }
    public MainWindow()
    {
        Title = "InkPad";
        Width = 0;
        Height = 0;
        SizeToContent = SizeToContent.WidthAndHeight;
        ResizeMode = ResizeMode.NoResize;
        WindowStyle = WindowStyle.ToolWindow;
        View = new MainView(this);
        Content = View;
        Topmost = true;
        CanvasWindow = new CanvasWindow(this);
        KeyBinds = new (this, CanvasWindow);

        LoadConfig();

        Closed += OnClosed;

        KeyDown += (sender, e) =>
        {
            KeyBinds.HandleKeyDown(e.Key);
        };
    }

    private void OnClosed(object? sender, EventArgs e)
    {
        SaveConfig();
    }

    private void LoadConfig()
    {
        try
        {
            using var stream = System.IO.File.OpenRead("config.bin");
            using var reader = new BinaryReader(stream);

            double top = reader.ReadDouble();
            double left = reader.ReadDouble();
            Top = top;
            Left = left;

            int colorsCount = reader.ReadInt32();
            for (int i = 0; i < colorsCount; i++)
            {
                string color = reader.ReadString();
                View.InvokeColor(i, color);
            }

            int strokesCount = reader.ReadInt32();
            for (int i = 0; i < strokesCount; i++)
            {
                byte a = reader.ReadByte();
                byte r = reader.ReadByte();
                byte g = reader.ReadByte();
                byte b = reader.ReadByte();

                double width = reader.ReadDouble();
                double height = reader.ReadDouble();
                bool isFitToCurve = reader.ReadBoolean();

                StylusPointCollection points = new();
                int pointsCount = reader.ReadInt32();
                for (int j = 0; j < pointsCount; j++)
                {
                    double x = reader.ReadDouble();
                    double y = reader.ReadDouble();
                    StylusPoint point = new(x, y);
                    points.Add(point);
                }

                Stroke stroke = new(points);
                stroke.DrawingAttributes.Color = Color.FromArgb(a, r, g, b);
                stroke.DrawingAttributes.Width = width;
                stroke.DrawingAttributes.Height = height;
                stroke.DrawingAttributes.FitToCurve = isFitToCurve;
                CanvasWindow.View.Strokes.Add(stroke);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            SaveConfig();
        }
    }

    private void SaveConfig()
    {
        try
        {
            using var stream = System.IO.File.OpenWrite("config.bin");
            using var writer = new BinaryWriter(stream);

            writer.Write(Top);
            writer.Write(Left);

            writer.Write(ColorCollection.Count);
            foreach (string color in ColorCollection.Colors)
            {
                writer.Write(color);
            }

            writer.Write(CanvasWindow.View.Strokes.Count);
            foreach (Stroke stroke in CanvasWindow.View.Strokes)
            {
                writer.Write(stroke.DrawingAttributes.Color.A);
                writer.Write(stroke.DrawingAttributes.Color.R);
                writer.Write(stroke.DrawingAttributes.Color.G);
                writer.Write(stroke.DrawingAttributes.Color.B);

                writer.Write(stroke.DrawingAttributes.Width);
                writer.Write(stroke.DrawingAttributes.Height);
                writer.Write(stroke.DrawingAttributes.FitToCurve);

                writer.Write(stroke.StylusPoints.Count);
                foreach (var point in stroke.StylusPoints)
                {
                    writer.Write(point.X);
                    writer.Write(point.Y);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}
