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

        // Async versiyonu
        public async Task<List<MainFeatures>> GetAllAsync()
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT Id, Name FROM MainFeatures";
                var result = await conn.QueryAsync<MainFeatures>(query);
                return result.ToList();
            }
        }

        // Async olarak GetDataById metodu
        public async Task<MainFeatures> GetDataByIdAsync(int id)
        {
            var parameters = new { Id = id };
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                await conn.OpenAsync();
                var query = "SELECT * FROM MainFeatures WHERE MainFeatureId = @Id";
                return await conn.QueryFirstOrDefaultAsync<MainFeatures>(query, parameters);
            }
        }

        public List<string> GetFeatureNames()
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                conn.Open();
                var query = "SELECT Name FROM MainFeatures";
                return conn.Query<string>(query).ToList();
            }
        }

    }
}
