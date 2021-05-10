using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace PlotterDataGH
{
    public static class mysql
    {
       public static MySqlConnection openConnection()
        {
            string connetionString;
        MySqlConnection cnn;
        connetionString = @"server=10.0.0.125;user id=Test;password=Testing123!;database=plotter_data;";
                    cnn = new MySqlConnection(connetionString);
        cnn.Open();

            return cnn;
        }
    }
}
