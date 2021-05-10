using PlotterDataGH.Properties;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PlotterDataGH
{
    public static class PublicMethods
    {
        public static string CheckJson(string jsonFilePath)
        {
            try
            {
                string json = File.ReadAllText(jsonFilePath);
                return json;
            }
            catch
            {
                return null;
            }
        }

        public static async Task<HttpWebResponse> GetResponseAsync(this HttpWebRequest request, CancellationToken ct)
        {
            using (ct.Register(() => request.Abort(), useSynchronizationContext: false))
            {
                try
                {
                    var response = await request.GetResponseAsync();
                    return (HttpWebResponse)response;
                }
                catch (WebException ex)
                {
                    // WebException is thrown when request.Abort() is called,
                    // but there may be many other reasons,
                    // propagate the WebException to the caller correctly
                    if (ct.IsCancellationRequested)
                    {
                        // the WebException will be available as Exception.InnerException
                        throw new OperationCanceledException(ex.Message, ex, ct);
                    }

                    // cancellation hasn't been requested, rethrow the original WebException
                    throw;
                }
            }
        }

    //    public static void RunScan(string Merk, string IP, string Naam)
    //    {
    //        var debugField = Path.GetDirectoryName(
    //Assembly.GetExecutingAssembly().GetName().CodeBase);

    //        debugField = debugField.Substring(6);

    //        var filename = debugField + @"\\Selenium.exe";

    //        //Start the Converted python file and pass the paramater
    //        string arguments = string.Format(@"{0} {1} {2} {3}", Merk, IP, Settings.Default.sendData, Naam);

    //        Process myProcess = new Process();
    //        myProcess.Exited += new EventHandler(myProcess_Exited);
    //        myProcess.StartInfo.FileName = filename;
    //        myProcess.StartInfo.Arguments = arguments;
    //        myProcess.Start();

    //    }

        private static void myProcess_Exited(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public static string RenderDataTableToHtml(DataTable dtInfo)
        {
            StringBuilder tableStr = new StringBuilder();

            if (dtInfo.Rows != null && dtInfo.Rows.Count > 0)
            {
                int columnsQty = dtInfo.Columns.Count;
                int rowsQty = dtInfo.Rows.Count;

                tableStr.Append("<TABLE>");
                tableStr.Append("<TR>");
                for (int j = 0; j < columnsQty; j++)
                {
                    tableStr.Append("<TH>" + dtInfo.Columns[j].ColumnName + "</TH>");
                }
                tableStr.Append("</TR>");

                for (int i = 0; i < rowsQty; i++)
                {
                    tableStr.Append("<TR>");
                    for (int k = 0; k < columnsQty; k++)
                    {
                        tableStr.Append("<TD>");
                        tableStr.Append(dtInfo.Rows[i][k].ToString());
                        tableStr.Append("</TD>");
                    }
                    tableStr.Append("</TR>");
                }

                tableStr.Append("</TABLE>");
            }

            return tableStr.ToString();
        }
    }
}

