using PESEL_Database_Tests.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Statics.Generators
{
    public static class PeselGenerator
    {
        private static readonly byte[] weightsPESELControlNumber = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

        public static PeselModel AssignPesel(DateTime birthDate, bool isMale, short sexBirthDateOccurences)
        {
            var birthYear = (short)birthDate.Year;
            var birthMonth = (byte)birthDate.Month;
            var birthDay = (byte)birthDate.Day;
            var ordinalNumber = GetPeselOrdinalNumber(isMale, sexBirthDateOccurences);
            var contorlDigit = GetPeselControlDigit(birthDate, ordinalNumber);

            return new PeselModel(birthYear, birthMonth, birthDay, ordinalNumber, contorlDigit);
        }
        private static byte GetPeselControlDigit(DateTime birthDate, short ordinalNumber)
        {
            var mainPart = GetPeselMainPart(birthDate, ordinalNumber);

            int[] digitsValue = new int[10];

            for (int i = 0; i < 10; i++)
            {
                digitsValue[i] = (mainPart[i] - '0') * weightsPESELControlNumber[i] % 10;
            }
            return (byte)((10 - digitsValue.Sum() % 10) % 10);
        }
        public static string DisplayPesel(DateTime birthDate, short ordinalNumber, byte controlDigit)
        {
            var sb = new StringBuilder();

            sb.Append(GetPeselMainPart(birthDate, ordinalNumber));
            sb.Append(controlDigit);

            return sb.ToString();
        }
        public static string GetPeselMainPart(DateTime birthDate, short ordinalNumber)
        {
            var sb = new StringBuilder();

            var monthCode = birthDate.Month + (birthDate.Year > 1999 ? 20 : 0);

            sb.Append(birthDate.ToString("yy"));
            sb.Append(monthCode.ToString("D2"));
            sb.Append(birthDate.ToString("dd"));
            sb.Append(ordinalNumber.ToString("D4"));
            
            return sb.ToString();
        }
        private static short GetPeselOrdinalNumber(bool isMale, short sexBirthDateOccurences)
        {
            return (short)(sexBirthDateOccurences * 2 + (isMale ? 1 : 0));
        }

        public static string GetSerializedPeselKey(DateTime birthDate, bool isMale)
        {
            var sb = new StringBuilder();

            sb.Append(birthDate.ToString("yy MM dd").Replace(" ", ""));
            sb.Append(isMale ? "1" : "0");

            return sb.ToString();
        }
    }
}
