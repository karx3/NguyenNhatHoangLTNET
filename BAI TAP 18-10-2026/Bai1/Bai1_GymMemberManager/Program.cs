using System;
using System.Windows.Forms;

namespace Bai1_GymMemberManager;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new frmGymMemberManager());
    }
}
