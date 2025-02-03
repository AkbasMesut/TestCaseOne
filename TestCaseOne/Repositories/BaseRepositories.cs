using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCaseOne.Repositories
{
    public class BaseRepositories : IDisposable
    {
        public SQLiteConnection connection;
        public BaseRepositories() 
        {
        connection = new SQLiteConnection (ConfigurationManager.ConnectionStrings["dbConnection"].ConnectionString);
        }

        public void Dispose() 
        { 
        GC.SuppressFinalize(this);
        }

    }
}

