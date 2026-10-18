using System;
using System.Windows.Forms;

namespace Bai5_CryptoStockTicker;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new frmCryptoStockTicker());
    }
}
