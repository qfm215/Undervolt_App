using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Undervolt_App
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string clientLoc = "C:/Program Files (x86)/Intel/Intel(R) Extreme Tuning Utility Old/Client/XtuCLI.exe";

        private bool getOffsetInfo(string[] lines, string find, int index)
        {
            foreach (string line in lines)
            {
                if (line.Contains(find))
                {
                    if (line[index] == '-')
                    {
                        return (true);
                    }
                    return (false);
                }
            }
            return (false);
        }

        public MainWindow()
        {
            InitializeComponent();

            var proc = new Process();
            proc.StartInfo.FileName = "cmd.exe";
            proc.StartInfo.Arguments = "/c net start XTU3SERVICE";
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.CreateNoWindow = true;

            proc.Start();
            proc.WaitForExit();

            proc = new Process();
            proc.StartInfo.FileName = clientLoc;
            proc.StartInfo.Arguments = "-i tuning";

            proc.Start();
            proc.WaitForExit();

            string[] lines = System.IO.File.ReadAllLines(@"C:\XTU_xmlFiles\Tuning.txt");

            Cpu_Checkbox.IsChecked = getOffsetInfo(lines, "Core Voltage Offset", 44);
            Gpu_Checkbox.IsChecked = getOffsetInfo(lines, "Processor Graphics Voltage Offset", 47);
        }

        private void runXtu(string id, string offset)
        {
            var proc = new Process();
            proc.StartInfo.FileName = clientLoc;
            proc.StartInfo.Arguments = "-t -id " + id + " -v " + offset;

            proc.Start();
            proc.WaitForExit();
        }

        private void Cpu_Checked(object sender, RoutedEventArgs e)
        {
            runXtu("34", "-120");
        }

        private void Cpu_Unchecked(object sender, RoutedEventArgs e)
        {
            runXtu("34", "0");
        }

        private void Gpu_Checked(object sender, RoutedEventArgs e)
        {
            runXtu("83", "-75");
        }

        private void Gpu_Unchecked(object sender, RoutedEventArgs e)
        {
            runXtu("83", "0");
        }

        private void ClickButtonUpdate(object sender, RoutedEventArgs E)
        {
        }
    }
}
