using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Models
{
    public class PeselModel
    {
        private short BirthYear { get; }
        private byte BirthMonth { get; }
        private byte BirthDay { get; }
        private short OrdinalNumber { get; }
        private byte ControlDigit { get; }

        public PeselModel(short birthYear, byte birthMonth, byte birthDay, short ordinalNumber, byte controlDigit)
        {
            BirthYear = birthYear;
            BirthMonth = birthMonth;
            BirthDay = birthDay;
            OrdinalNumber = ordinalNumber;
            ControlDigit = controlDigit;
        }

        public bool IsMale => OrdinalNumber % 2 == 1;
        public string Display => PeselGenerator.DisplayPesel(BirthDate, OrdinalNumber, ControlDigit);
        public DateTime BirthDate => new DateTime(BirthYear, BirthMonth, BirthDay);
    }
}
