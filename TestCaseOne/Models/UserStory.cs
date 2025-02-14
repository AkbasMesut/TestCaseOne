using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCaseOne.Models
{
    public class UserStory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainFeatureId { get; set; }
        public bool Result { get; set; }
        public string Info { get; set; }
    }
}
