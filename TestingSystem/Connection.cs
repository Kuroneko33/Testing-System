using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestingSystem
{
    static class Connection
    {
        private static string fullpath = System.IO.Directory.GetCurrentDirectory();
        private static string path = fullpath.Substring(0, fullpath.IndexOf("bin"));
        public static string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={path}TestingSystemDB.mdf;Integrated Security=True";
    }
}
