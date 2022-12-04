using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace InkPad;

public class ColorCollection
{
    public static int ActiveColorIndex = 0;

    public static int Count => Colors.Count;

    public static List<string> Colors = new List<string>() { "ff0000", "fff000", "00ff00", "000fff"};

    public static string GetColorCode(int i) => Colors[i];

    public static string GetColorHex(int i) => $"#{Colors[i]}";

    public static Color GetColor(int i) => (Color)ColorConverter.ConvertFromString(GetColorHex(i));

    public static SolidColorBrush GetSolidColorBrush(int i) => new SolidColorBrush(GetColor(i));

    public static void UpdateColor(int i, string color) => Colors[i] = color;
}
