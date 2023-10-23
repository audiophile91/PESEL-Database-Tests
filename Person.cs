using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests
{
    public class Person
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Lastname { get; set; }
        public bool IsMale { get; set; }
        public DateTime DateBirth { get; set; }
        public string PESEL {  get; set; }
    }
}
