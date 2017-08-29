using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
namespace hki.web.Helpers
{
    public class AWS
    {
        public static string GetConnectionString()
        {

            const string dbname = "hkidb";
            const string username = "optimo";
            const string password = "Agusvaldes1!";
            const string hostname = "hkidb.cckhmhed4xqi.us-east-1.rds.amazonaws.com:1433 ";
            const string port = "1433";

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}