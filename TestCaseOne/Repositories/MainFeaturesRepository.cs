using Dapper;
using System;
using System.Collections.Generic;
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
            List<MainFeatures> dataItemList = new List<MainFeatures>();
            dataItemList = connection.Query<MainFeatures>("select Name from MainFeatures").ToList();
            return dataItemList;
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
