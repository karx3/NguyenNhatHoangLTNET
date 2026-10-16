using System;
using System.Windows.Forms;

namespace Bai1_ClinicAppointmentManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new FormMain());
    }
}
