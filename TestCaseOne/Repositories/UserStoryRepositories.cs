using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestCaseOne.Models;

namespace TestCaseOne.Repositories
{
    internal class UserStoryRepositories : BaseRepositories
    {
        public async Task<List<UserStory>> GetAllByModulIdAsync(int MainFeatureId)
        {
            using (var conn = new SQLiteConnection(connection.ConnectionString))
            {
                await conn.OpenAsync();
                var parameters = new { MainFeatureId = MainFeatureId };
                var query = "SELECT Id, Name, MainFeatureId, Info, Result AS Result FROM UserStory WHERE MainFeatureId = @MainFeatureId";
                var dataItemList = await conn.QueryAsync<UserStory>(query, parameters);
                return dataItemList.ToList();
            }
        }
    }
}
