using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestCaseOne.Models;

namespace TestCaseOne.Repositories
{
    internal class UserStoryRepositories : BaseRepositories
    {

    public List<UserStory> GetAllByModulId (int MainFeatureId)
        {
            List<UserStory> dataItemList = new List<UserStory>();
            var parameters = new { MainFeatureId = MainFeatureId };
            dataItemList = connection.Query<UserStory>("SELECT Id, Name, MainFeatureId,Info, Result AS Result FROM UserStory WHERE MainFeatureId = @MainFeatureId", parameters).ToList();
            return dataItemList;
        }
    }
}
