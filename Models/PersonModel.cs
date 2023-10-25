using Microsoft.VisualBasic;
using PESEL_Database_Tests.Statics;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Models
{
    public class PersonModel
    {
        public int ID { get; set; }
        public short NameID { get; set; }
        public int LastnameID { get; set; }
        private PeselModel PESEL { get; }

        public PersonModel(int id, short nameID, int lastNameID, PeselModel pesel)
        {
            ID = id;
            NameID = nameID;
            LastnameID = lastNameID;
            PESEL = pesel;
        }

        public bool IsMale => PESEL.IsMale;
        public string BirthDate => PESEL.BirthDate.ToString("yyyy-MM-dd");
        public DateTime BirthExactDate => PESEL.BirthDate;
        public string Pesel => PESEL.Display;
        public byte Age => PersonGenerator.GetPersonAge(PESEL.BirthDate);
    }
}
