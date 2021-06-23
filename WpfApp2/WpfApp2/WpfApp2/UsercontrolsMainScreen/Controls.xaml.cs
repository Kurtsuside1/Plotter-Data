using MySql.Data.MySqlClient;
using PlotterDataGH.Properties;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Newtonsoft.Json;
using PlotterDataGH;
using System.Threading;
using System.Net;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using System.Text;
using System.Net.NetworkInformation;
using System.Windows.Media;
using System.Threading.Tasks;
using System;
using Microsoft.Win32.TaskScheduler;
using System.Net.Http;
using WpfApp2;

namespace PlotterDataGH.UsercontrolsMainScreen
{
    /// <summary>
    /// Interaction logic for Controls.xaml
    /// </summary>
    public partial class Controls : UserControl
    {
        public int plotterId;
        public string plotterIp;
        public string serialnm;
        public string plotterNaam;
        public int typeId;
        AutoResetEvent waiter = new AutoResetEvent(false);

        private static readonly HttpClient client = new HttpClient();

        public  NewMainScreen ParentForm { get; set; }
        public tabControl tabForm { get; set; }

        public Controls()
        {
            InitializeComponent();
            btnScan.IsEnabled = false;
        }

        public void SetTimer()
        {
            //Set a timer between pings
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 10, 0);
            dispatcherTimer.Start();
        }

        #region Check status
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //Send a ping to the plotter and if it responds turn the light green
            SendPing();
        }

        public void SendPing()
        {
            Ping pingSender = new Ping();

            // When the PingCompleted event is raised,
            // the PingCompletedCallback method is called.
            pingSender.PingCompleted += new PingCompletedEventHandler(PingCompletedCallback);

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);

            // Wait 12 seconds for a reply.
            int timeout = 12000;

            // Set options for transmission:
            // The data can go through 64 gateways or routers
            // before it is destroyed, and the data packet
            // cannot be fragmented.
            PingOptions options = new PingOptions(64, true);

            pingSender.SendAsync(plotterIp, timeout, buffer, options, waiter);
        }

        private void PingCompletedCallback(object sender, PingCompletedEventArgs e)
        {
            // If the operation was canceled, display a message to the user.
            if (e.Cancelled)
            {
                ellStatus.Fill = new SolidColorBrush(Colors.Red);
                btnScan.IsEnabled = false;

                // Let the main thread resume.
                // UserToken is the AutoResetEvent object that the main thread
                // is waiting for.
                ((AutoResetEvent)e.UserState).Set();
            }

            // If an error occurred, display the exception to the user.
            if (e.Error != null)
            {
                ellStatus.Fill = new SolidColorBrush(Colors.Red);
                btnScan.IsEnabled = false;

                // Let the main thread resume.
                ((AutoResetEvent)e.UserState).Set();
            }

            PingReply reply = e.Reply;

            DisplayReply(reply);

            // Let the main thread resume.
            ((AutoResetEvent)e.UserState).Set();
        }

        public void DisplayReply(PingReply reply)
        {
            if (reply == null)
            {
                return;
            }


            if (reply.Status == IPStatus.Success)
            {
                ellStatus.Fill = new SolidColorBrush(Colors.Green);
                lblstatus.Content = "Status: Online";
                btnScan.IsEnabled = true;
            }
            else
            {
                ellStatus.Fill = new SolidColorBrush(Colors.Red);
                lblstatus.Content = "Status: Offline";
                btnScan.IsEnabled = false;
            }
        }

        #endregion

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPlotter addPlotter = new AddPlotter();
            //addPlotter.ParentForm = ParentForm;
            addPlotter.editForm(plotterId);
            addPlotter.editingMode = true;
            addPlotter.Show();
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {

        }

        #region Scanning

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Run a scan of this specific plotter
            RunScan(typeId.ToString(), plotterIp, plotterNaam); 
            MahApps.Metro.IconPacks.PackIconFontAwesome fe = btnScan.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
            fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.SyncSolid;
            btnScan.IsEnabled = false;
            lblstatus.Content = "Status: Scanning, even geduld AUB";
        }

        public void RunScan(string Merk, string IP, string Naam)
        {
            var debugField = Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().GetName().CodeBase);

            debugField = debugField.Substring(6);

            var filename = debugField + @"/ghWebscraper.exe";

            //Start the Converted python file and pass the paramater
            string arguments = string.Format(@"{0} {1} {2} {3}", Merk, IP, Settings.Default.sendData, Naam);

            ellStatus.Fill = new SolidColorBrush(Colors.Orange);

            //Process myProcess = new Process();
            //myProcess.Exited += new EventHandler(myProcess_Exited);
            //myProcess.StartInfo.FileName = filename;
            //myProcess.StartInfo.Arguments = arguments;
            //myProcess.Start();

            doStuff(filename, arguments);
        }

        async System.Threading.Tasks.Task doStuff(string fileName, string args)
        {
            btnScan.IsEnabled = false;
            await RunProcessAsync(fileName, args);

            //MahApps.Metro.IconPacks.PackIconFontAwesome fe = btnScan.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
            //fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.BinocularsSolid;
            //btnScan.IsEnabled = true;

            //ParentForm.fillerGrid.RowDefinitions.Clear();
            //ParentForm.fillerGrid.Children.Clear();
            tabForm.clear_screen();
            ParentForm.clearTabs();
            ParentForm.LoadData();
        }

        public static async Task<int> RunProcessAsync(string fileName, string args)
        {
            using (var process = new Process
            {
                StartInfo =
        {
            FileName = fileName, Arguments = args,
            UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        },
                EnableRaisingEvents = true
            })
            {
                return await RunProcessAsync(process).ConfigureAwait(false);
            }
        }
        private static Task<int> RunProcessAsync(Process process)
        {
            var tcs = new TaskCompletionSource<int>();

            process.Exited += (s, ea) => tcs.SetResult(process.ExitCode);
            process.OutputDataReceived += (s, ea) => Console.WriteLine(ea.Data);
            process.ErrorDataReceived += (s, ea) => Console.WriteLine("ERR: " + ea.Data);

            bool started = process.Start();
            if (!started)
            {
                //you may allow for the process to be re-used (started = false) 
                //but I'm not sure about the guarantees of the Exited event in such a case
                throw new InvalidOperationException("Could not start process: " + process);
            }

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();

            return tcs.Task;

        }

        #endregion

    }
}
