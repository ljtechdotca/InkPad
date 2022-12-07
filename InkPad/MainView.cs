using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using FontFamily = System.Windows.Media.FontFamily;
using VerticalAlignment = System.Windows.VerticalAlignment;
using Path = System.Windows.Shapes.Path;
using System.Windows.Controls.Primitives;
using System.Windows.Shapes;

namespace InkPad;

public enum InkCanvasIconType
{
    Clear,
    Erase,
    Fill,
    Redo,
    Select,
    Undo,
}

public class Tool
{
    public InkCanvasIconType Icon;
    public int Row;
    public int Column;
    public string ToolTip;

    public Tool(string toolTip, InkCanvasIconType icon, int row, int column)
    {
        ToolTip = toolTip;
        Icon = icon;
        Row = row;
        Column = column;
    }
}

public delegate void RoutedColorHandler(int i, string color);
public class MainView : StackPanel
{
    public StackPanel Colors { get; private set; }
    public Grid Tools { get; private set; }
    public MainWindow Window { get; private set; }
    public MainController Controller { get; private set; }

    public event RoutedColorHandler ColorChanged;

    public MainView(MainWindow mainWindow)
    {
        Window = mainWindow;
        Controller = new(this);
        ColorChanged += (i, color) => Controller.UpdateColors(i, color);

        Colors = new();
        InitColors(); // Builds colors inside a public stack panel named 'Colors'
        Children.Add(Colors);

        Tools = new();
        InitTools(); // Builds tools inside a public grid named 'Tools'
        Children.Add(Tools);

    }

    public void InitTools()
    {
        Tool selectTool = new("[Q] Select", InkCanvasIconType.Select, 0, 0);
        Tool fillTool = new("[W] Fill", InkCanvasIconType.Fill, 0, 1);
        Tool eraseTool = new("[E] Erase", InkCanvasIconType.Erase, 1, 0);
        Tool clearTool = new("[R] Clear", InkCanvasIconType.Clear, 1, 1);
        Tool undoTool = new("[CTRL + Z] Undo", InkCanvasIconType.Undo, 2, 0);
        Tool redoTool = new("[CTRL + Y] Redo", InkCanvasIconType.Redo, 2, 1);
        Tool[] toolsArray = {
        clearTool,
        fillTool,
        selectTool,
        eraseTool,
        redoTool,
        undoTool,
        };

        for (int i = 0; i < 2; i++)
        {
            ColumnDefinition colDef = new();
            Tools.ColumnDefinitions.Add(colDef);
        }
        for (int j = 0; j < toolsArray.Length / 2; j++)
        {
            RowDefinition rowDef = new();
            Tools.RowDefinitions.Add(rowDef);
        }
        foreach (Tool tool in toolsArray)
        {
            Button button = new()
            {
                Height = 35,
                Width = 35,
                Content = CreateIconPath(tool.Icon)
            };
            ToolTip toolTip = new()
            {
                Content = tool.ToolTip,
                Placement = PlacementMode.Left,
            };
            button.ToolTip = toolTip;
            button.Click += (sender, e) => Controller.HandleToolClick(tool.Icon);
            Grid.SetRow(button, tool.Row);
            Grid.SetColumn(button, tool.Column);
            Tools.Children.Add(button);
        }
    }

    public void InitColors()
    {
        for (int i = 0; i < 4; i++)
        {
            int j = i;
            WrapPanel wrapPanel = new();

            Ellipse circle = new()
            {
                Fill = Brushes.Transparent,
                Height = 6,
                Width = 6,
            };

            ToolTip toolTip = new()
            {
                Content = $"[{j + 1}]",
                Placement = PlacementMode.Left,
            };

            Button button = new()
            {
                Background = ColorCollection.GetSolidColorBrush(j),
                Height = 24,
                Width = 24,
                Content = circle,
                ToolTip = toolTip,
            };

            button.Click += (sender, e) => Controller.HandleColorClick(j);
            wrapPanel.Children.Add(button);
            TextBox textBox = new()
            {
                FontFamily = new FontFamily("Consolas"),
                Text = ColorCollection.GetColorCode(i),
                Height = 24,
                MaxLength = 6,
            };
            textBox.TextChanged += (sender, e) => Controller.HandleTextBox(j, textBox.Text);
            wrapPanel.Children.Add(textBox);
            Colors.Children.Add(wrapPanel);
        }

        //Colors.Children[0].Children[0].Content.Fill = Brushes.White;
    }

    public void InvokeColor(int i, string color)
    {
        ColorChanged.Invoke(i, color);
    }

    public static Path CreateIconPath(InkCanvasIconType type)
    {
        Path icon = new()
        {
            Fill = Brushes.Black,
            Width = 16,
            Height = 16,
        };

        var assembly = Assembly.GetExecutingAssembly();
        var name = $"InkPad.Resources.{type}.txt";

        using Stream? stream = assembly.GetManifestResourceStream(name);

        if (stream != null)
        {
            using StreamReader reader = new(stream);
            string source = reader.ReadToEnd();
            icon.Data = Geometry.Parse(source);
        }
        else
        {
            Debug.WriteLine($"Failed to load icon data type: {type}");
            icon.Data = Geometry.Parse("");
        }

        return icon;
    }
}
