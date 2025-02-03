using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestCaseOne.Models
{
    public class TestCase
    {
        public int Id { get; set; }

        public int UserStoryId { get; set; }

        public string TestStep { get; set; }

        public string ExpectedResult { get; set; }



    }
}
