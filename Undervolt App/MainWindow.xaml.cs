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
        bool cpuState;
        bool gpuState;

        private bool getOffsetInfo(Button button, string[] lines, string find, int index)
        {
            foreach (string line in lines)
            {
                if (line.Contains(find))
                {
                    if (line[index] == '-')
                    {
                        button.Content = FindResource(button.Name + "On");
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
            proc.StartInfo.FileName = "C:/Program Files (x86)/Intel/Intel(R) Extreme Tuning Utility/Client/XtuCLI.exe";
            proc.StartInfo.Arguments = "-i tuning";

            proc.Start();
            proc.WaitForExit();

            string[] lines = System.IO.File.ReadAllLines(@"C:\XTU_xmlFiles\Tuning.txt");

            Button cpuButton = this.FindName("Cpu") as Button;
            Button gpuButton = this.FindName("Gpu") as Button;

            cpuState = getOffsetInfo(cpuButton, lines, "Core Voltage Offset", 44);
            gpuState = getOffsetInfo(gpuButton, lines, "Processor Graphics Voltage Offset", 47);
        }

        private void runXtu(string id, string offset)
        {
            var proc = new Process();
            proc.StartInfo.FileName = "C:/Program Files (x86)/Intel/Intel(R) Extreme Tuning Utility/Client/XtuCLI.exe";
            proc.StartInfo.Arguments = "-t -id " + id + " -v " + offset;

            proc.Start();
            proc.WaitForExit();
        }

        private void ClickButtonCpu(object sender, RoutedEventArgs E)
        {
            Image newImg = new Image();
            Button snd = (Button)sender;
            if (cpuState == true)
            {
                runXtu("34", "0");
                cpuState = false;
                snd.Content = FindResource("CpuOff");
            }
            else
            {
                runXtu("34", "-120");
                cpuState = true;
                snd.Content = FindResource("CpuOn");
            }
        }

        private void ClickButtonGpu(object sender, RoutedEventArgs E)
        {
            Image newImg = new Image();
            Button snd = (Button)sender;

            if (gpuState == true)
            {
                runXtu("83", "0");
                gpuState = false;
                snd.Content = FindResource("GpuOff");
            }
            else
            {
                runXtu("83", "-75");
                gpuState = true;
                snd.Content = FindResource("GpuOn");
            }
        }

        private void ClickButtonUpdate(object sender, RoutedEventArgs E)
        {
        }
    }
}
