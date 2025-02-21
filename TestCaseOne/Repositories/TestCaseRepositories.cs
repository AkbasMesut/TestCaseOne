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
    internal class TestCaseRepositories : BaseRepositories
    {
        public List<TestCase> GetAllByUserStoryId (int userStoryId)
        {
            List<TestCase> dataItemList = new List<TestCase>();
            var parameters = new { UserStoryId = userStoryId };
            dataItemList = connection.Query<TestCase>("select * from where UserStoryId = @UserStoryId", parameters).ToList();
            return dataItemList;
        }



    }
}
