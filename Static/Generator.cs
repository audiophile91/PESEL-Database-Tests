using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.Static
{
    public static class Generator
    {
        private static readonly Random random = new();
        private static readonly DateTime today = DateTime.Now;
        private static readonly Dictionary<int, int> monthDays = new()
        {
            {1, 31 }, {2, 28}, {3, 31}, {4, 30}, {5, 31}, {6, 30}, {7, 31}, {8, 31}, {9, 30}, {10, 31}, {11, 30}, {12, 31}
        };
        private static readonly byte[] weightsPESELControlNumber = { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3 };

        private static readonly string[] namesMale = new string[] {
            "Artur", "Andrzej",
            "Bartosz", "Bartłomiej",
            "Cezary",
            "Daniel", "Denis", "Damian",
            "Ernest",
            "Filip", "Franciszek",
            "Grzegorz",
            "Henryk",
            "Ireneusz",
            "Jan",
            "Kamil", "Kacper", "Karol",
            "Lech",
            "Marek", "Mariusz",
            "Nikodem",
            "Olaf",
            "Patryk", "Piotr", "Paweł",
            "Rafał", "Radosław",
            "Stanisław", "Stefan", "Sławomir",
            "Tomasz",
            "Ulrich",
            "Władysław",
            "Zbigniew"};

        private static readonly string[] namesFemale = new string[] {
            "Anna", "Agata", "Agnieszka", "Alicja", "Aneta",
            "Barbara", "Beata",
            "Celina",
            "Dorota", "Danuta",
            "Ewelina",
            "Felicja",
            "Grażyna",
            "Hanna",
            "Izabela",
            "Joanna",
            "Kamila", "Karolina", "Klaudia", "Kinga",
            "Lucyna", "Luiza",
            "Maria", "Magda", "Monika",
            "Natalia",
            "Oliwia",
            "Patrycja", "Paulina",
            "Renata",
            "Sylwia", "Sandra",
            "Teresa", "Teodora",
            "Urszula",
            "Wiesława",
            "Zofia", "Zuzanna"};

        public static bool IsMale()
        {
            return random.NextDouble() > 0.5;
        }
        public static string PickName(bool isMale)
        {
            if (isMale)
            {
                var index = random.Next(0, namesMale.Length);
                return namesMale[index];
            }
            else
            {
                var index = random.Next(0, namesFemale.Length);
                return namesFemale[index];
            }
        }
        public static DateTime PickDateBirth()
        {
            var year = random.Next(1900, today.Year + 1);
            var month = year == today.Year ? random.Next(1, today.Month + 1) : random.Next(1, 13);
            var maxDay = year == today.Year && month == today.Month ? today.Day 
                                                                    : month == 2 && DateTime.IsLeapYear(year) ? 29 
                                                                    : monthDays[month];
            var day = random.Next(1, maxDay + 1);

            return new DateTime(year, month, day);
        }
        public static string GetPESEL(DateTime birth, bool isMale, int ordinalNumber)
        {
            string RR = birth.ToString("yy");
            string MM = (birth.Month + (birth.Year > 1999 ? 20 : 0)).ToString("D2");
            string DD = birth.ToString("dd");
            string PPPP = (2 * ordinalNumber + (isMale ? 1 : 0)).ToString("D4");

            string mainPart = string.Concat(RR, MM, DD, PPPP);

            return string.Concat(mainPart, GetPESELControlDigit(mainPart));
        }
        private static string GetPESELControlDigit(string mainPart)
        {
            int[] digitsValue = new int[10];

            for (int i = 0; i < 10; i++)
            {
                digitsValue[i] = (mainPart[i] - '0') * weightsPESELControlNumber[i] % 10;
            }
            return ((10 - (digitsValue.Sum() % 10)) % 10).ToString();
        }
    }
}
