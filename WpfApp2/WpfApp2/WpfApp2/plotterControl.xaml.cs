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

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        bool cartridgeHidden = true;
        public int plotterId;
        public string plotterIp;
        public string serialnm;
        AutoResetEvent waiter = new AutoResetEvent(false);

        public MainWindow ParentForm { get; set; }

        public UserControl1()
        {
            InitializeComponent();
            btnScan.IsEnabled = false;
        }

        public class cartridges
        {
            public int ID { get; set; }
            public string serial_number { get; set; }
            public int parent_id { get; set; }
            public string cartridge_model { get; set; }
            public string volume { get; set; }
            public string max_volume { get; set; }
        }

        private void SetTimer()
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 1, 0);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            SendPing();
        }


        public void loadData()
        {
            DataTable dataTable = new DataTable();
            SqliteConnection cnn;
            SqliteCommand cmd = null;
            cnn = new SqliteConnection("Data Source=plotterData.db;");
            cnn.Open();

            string query = string.Format("SELECT * FROM `cartridge_reading` where `parent_id` = {0}", plotterId);
            cmd = new SqliteCommand(query, cnn);

            SqliteDataReader reader = cmd.ExecuteReader();
            dataTable.Load(reader);

            foreach (DataRow row in dataTable.Rows)
            {
                RowDefinition rd = new RowDefinition();
                rd.Height = GridLength.Auto;
                plotterControl.RowDefinitions.Add(rd);
                cartridgeControl cartridgeControls = new cartridgeControl();
                cartridgeControls.lblCartridgeNaam.Content = row["cartridge_model"].ToString();
                cartridgeControls.lblCatridgeVolume.Content = row["volume"].ToString();
                plotterControl.Children.Add(cartridgeControls);
                Grid.SetRow(cartridgeControls, plotterControl.RowDefinitions.Count);
            }
            RowDefinition last = new RowDefinition();
            plotterControl.RowDefinitions.Add(last);

            for (int x = plotterControl.RowDefinitions.Count - 1; x > 0; x--)
            {
                plotterControl.RowDefinitions[x].Height = new GridLength(0);
            }

            SetTimer();
            SendPing();
        }

        private void SendPing()
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
                btnScan.IsEnabled = true;
            }
            else
            {
                ellStatus.Fill = new SolidColorBrush(Colors.Red);
                btnScan.IsEnabled = false;
            }
        }
        //public StringBuilder CSVdata()
        //{
        //    StringBuilder sb = new StringBuilder();

        //    DataTable dataTable = new DataTable();
        //    SqliteConnection cnn;
        //    SqliteCommand cmd = null;
        //    cnn = new SqliteConnection("Data Source=plotterData.db;");
        //    cnn.Open();

        //    string query = string.Format("SELECT * FROM `cartridge_reading` where `parent_id` = {0}", plotterId);
        //    cmd = new SqliteCommand(query, cnn);

        //    SqliteDataReader reader = cmd.ExecuteReader();
        //    dataTable.Load(reader);

        //    return sb;
        //}

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {
            if(cartridgeHidden)
            {
                for(int x = 0; x <= plotterControl.RowDefinitions.Count - 1; x++)
                {
                    plotterControl.RowDefinitions[x].Height = GridLength.Auto;
                }

                cartridgeHidden = false;

                Button button = sender as Button;
                MahApps.Metro.IconPacks.PackIconFontAwesome fe = button.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
                fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.AngleDoubleUpSolid;
            }
            else
            {
                for(int x = plotterControl.RowDefinitions.Count - 1; x > 0; x--)
                {
                    plotterControl.RowDefinitions[x].Height = new GridLength(0);
                }
                cartridgeHidden = true;

                Button button = sender as Button;
                MahApps.Metro.IconPacks.PackIconFontAwesome fe = button.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
                fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.AngleDoubleDownSolid;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddPlotter addPlotter = new AddPlotter();
            addPlotter.ParentForm = ParentForm;
            addPlotter.editForm(plotterId);
            addPlotter.editingMode = true;
            addPlotter.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RunScan(lblMerk.Uid.ToString(), plotterIp, lblNaam.Content.ToString());
            MahApps.Metro.IconPacks.PackIconFontAwesome fe = btnScan.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
            fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.SyncSolid;
            btnScan.IsEnabled = false;
        }

        public void RunScan(string Merk, string IP, string Naam)
        {
            var debugField = Path.GetDirectoryName(
    Assembly.GetExecutingAssembly().GetName().CodeBase);

            debugField = debugField.Substring(6);

            var filename = debugField + @"/Selenium.exe";

            //Start the Converted python file and pass the paramater
            string arguments = string.Format(@"{0} {1} {2} {3}", Merk, IP, Settings.Default.sendData, Naam);

            //Process myProcess = new Process();
            //myProcess.Exited += new EventHandler(myProcess_Exited);
            //myProcess.StartInfo.FileName = filename;
            //myProcess.StartInfo.Arguments = arguments;
            //myProcess.Start();

            doStuff(filename, arguments);
        }

        async Task doStuff(string fileName, string args)
        {
            ParentForm.DisableWhileScanning();
            await RunProcessAsync(fileName, args);

            //MahApps.Metro.IconPacks.PackIconFontAwesome fe = btnScan.Content as MahApps.Metro.IconPacks.PackIconFontAwesome;
            //fe.Kind = MahApps.Metro.IconPacks.PackIconFontAwesomeKind.BinocularsSolid;
            //btnScan.IsEnabled = true;

            ParentForm.fillerGrid.RowDefinitions.Clear();
            ParentForm.fillerGrid.Children.Clear();
            ParentForm.LoadData();
        }

        public static async Task<int> RunProcessAsync(string fileName, string args)
        {
            using (var process = new Process
            {
                StartInfo =
        {
            FileName = fileName, Arguments = args,
            UseShellExecute = false, CreateNoWindow = false,
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
    }
}
