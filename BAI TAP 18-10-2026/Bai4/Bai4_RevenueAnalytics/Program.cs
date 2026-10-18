using System;
using System.Windows.Forms;

namespace Bai4_RevenueAnalytics;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new frmRevenueAnalytics());
    }
}
