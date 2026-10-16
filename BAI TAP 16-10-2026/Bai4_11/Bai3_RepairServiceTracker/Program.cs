using System;
using System.Windows.Forms;

namespace Bai3_RepairServiceTracker;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}
