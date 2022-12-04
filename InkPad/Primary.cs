using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InkPad;

public class Primary
{
    [STAThread]
    public static void Main(string[] args)
    {
        Application app = new Application();
        Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        MainWindow mainWindow = new MainWindow();

        app.Run(mainWindow);
    }

}
