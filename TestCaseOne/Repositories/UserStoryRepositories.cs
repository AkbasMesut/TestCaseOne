using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestCaseOne.Models;

namespace TestCaseOne.Repositories
{
    internal class UserStoryRepositories : BaseRepositories
    {

    public List<UserStory> GetAllByModulId (int modulId)
        {
            List<UserStory> dataItemList = new List<UserStory>();
            var parameters = new { ModulId = modulId };
            dataItemList = connection.Query<UserStory>("select * from UserStroy where ModulId = @ModulId", parameters).ToList();
            return dataItemList;
        }
    }
}
