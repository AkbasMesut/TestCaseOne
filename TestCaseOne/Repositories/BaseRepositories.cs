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
            string connectionString = ConfigurationManager.ConnectionStrings["dbConnection"]?.ConnectionString;
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("Bağlantı dizesi bulunamadı!");
            }
            connection = new SQLiteConnection(connectionString);
        }

        public void Dispose() 
        { 
        GC.SuppressFinalize(this);
        }

    }
}

