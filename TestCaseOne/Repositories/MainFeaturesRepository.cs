using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCaseOne.Models;


namespace TestCaseOne.Repositories
{
    internal class MainFeaturesRepository :BaseRepositories
    {
        public MainFeaturesRepository() 
        { 
        
        }
        
        public List<MainFeatures> GetAll ()
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                conn.Open();
                var query = "SELECT Id, Name FROM MainFeatures";  
                return conn.Query<MainFeatures>(query).ToList();
            }
        }

        public MainFeatures GetDataById(int id)
        {
            MainFeatures modul = new MainFeatures();
            var parameters = new {ModulId = id };
            modul = connection.QueryFirstOrDefault<MainFeatures>("select * from MainFeatures where MainFeatureId = @Id", parameters);
            return modul;
        } 

    }
}
