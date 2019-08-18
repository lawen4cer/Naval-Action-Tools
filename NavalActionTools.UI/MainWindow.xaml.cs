using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
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
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;

namespace NavalActionTools.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int show);

        [DllImport("user32.dll")]
        static extern IntPtr FindWindowA(string classname, string windowname);

        static bool isEnabled = false;
        static Process[] processes;
        static Process process;
        IntPtr p;
        IntPtr chromePointer;

        public MainWindow()
        {
            
            InitializeComponent();
            GetProcess("Client");
        }

        private async void SendKeysButton_Click(object sender, RoutedEventArgs e)
        {
            statusMessageLabel.Content = "The Script is now turned on";
            statusMessageLabel.Foreground = Brushes.Green;
            isEnabled = true;
            await Task.Delay(1000);
            await RunScript();
            
        }
        private void KillScriptButton_Click(object sender, RoutedEventArgs e)
        {
            statusMessageLabel.Content = "The Script is currently turned off";
            statusMessageLabel.Foreground = Brushes.Red;
            isEnabled = false;
        }

        private void HideWindowButton_Click(object sender, RoutedEventArgs e)
        {
            
            MoveWindow(p, 0, 0, 0, 0, true);
        }

        private void ShowWindowButton_Click(object sender, RoutedEventArgs e)
        {
            
            MoveWindow(p, 0, 0, 1920, 1080, true);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MoveWindow(p, 0, 0, 1920, 1080, true);
        }

        private void UpdateProcessButton_Click(object sender, RoutedEventArgs e)
        {
            GetProcess(processNameTextBox.Text);
        }

        private async Task RunScript()
        {
            while (isEnabled)
            {
                bool success = SetForegroundWindow(p);
                Console.WriteLine(success);
                await Task.Delay(5000);
                SendKeys.SendWait("s");
                await Task.Delay(250);
                SendKeys.SendWait("s");
                await Task.Delay(300000);
            }
            
        }

        private void GetProcess(string processName) {
            try
            {
                processes = Process.GetProcessesByName(processName);
                if (processes.Length == 0)
                {
                    System.Windows.MessageBox.Show("Unable to find any processes matching that name, Make sure the game is running before launching the application", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                process = processes[0];
                p = process.MainWindowHandle;
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show("Are you sure the game is running? Naval Action Tools was unable to locate the process", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
            
        }
    }
}
