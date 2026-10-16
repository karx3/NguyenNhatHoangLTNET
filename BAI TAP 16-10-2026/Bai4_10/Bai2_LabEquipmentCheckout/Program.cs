using System;
using System.Windows.Forms;

namespace Bai2_LabEquipmentCheckout;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}
