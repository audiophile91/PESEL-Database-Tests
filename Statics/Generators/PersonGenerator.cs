using Microsoft.VisualBasic;
using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading.Tasks;
using static PESEL_Database_Tests.Models.PersonModel;

namespace PESEL_Database_Tests.Statics.Generators
{
    public static class PersonGenerator
    {
        private const int BIRTH_MONTH_MIN = 1;
        private const int BIRTH_MONTH_MAX = 12;

        public static readonly DateTime Today = DateTime.Today;
        private static readonly int BirthYearMax = Today.Year;

        private static readonly Random random = new();

        private static readonly Dictionary<AgeRange, (int MinAge, int MaxAge)> ageRanges = new()
        {
            { AgeRange.Infant, (0, 2) },
            { AgeRange.Preschooler, (3, 5) },
            { AgeRange.SchoolAgeChild, (6, 12) },
            { AgeRange.Teenager, (13, 19) },
            { AgeRange.YoungAdult, (20, 35) },
            { AgeRange.Adult, (36, 60) },
            { AgeRange.Senior, (61, 85) },
            { AgeRange.OldSenior, (86, 112) },
        };
        private static readonly Dictionary<int, int> monthDaysCount = new()
        {
            {1, 31 }, {2, 28}, {3, 31}, {4, 30}, {5, 31}, {6, 30}, {7, 31}, {8, 31}, {9, 30}, {10, 31}, {11, 30}, {12, 31}
        };

        public static readonly string[] namesMale = new string[] {
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

        public static readonly string[] namesFemale = new string[] {
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

        public static bool PickSexIsMale()
        {
            return random.NextDouble() > 0.5;
        }
        public static short PickName(bool isMale)
        {
            var maxIndex = isMale ? namesMale.Length : namesFemale.Length;

            return (short)random.Next(0, maxIndex);
        }
        public static DateTime PickDateBirth()
        {
            var year = PickYearOfBirth(PickAgeRange());

            var maxMonth = year == BirthYearMax ? Today.Month : BIRTH_MONTH_MAX;

            var month = random.Next(BIRTH_MONTH_MIN, maxMonth + 1);

            var maxDay = year == Today.Year && month == Today.Month ? Today.Day
                          : month == 2 && DateTime.IsLeapYear(year) ? 29
                                                                    : monthDaysCount[month];
            var day = random.Next(1, maxDay + 1);

            return new DateTime(year, month, day);
        }
        private static int PickYearOfBirth(AgeRange ageRange) 
        {
            var maxYear = Today.Year - ageRanges[ageRange].MinAge;
            var minYear = Today.Year - ageRanges[ageRange].MaxAge;

            return random.Next(minYear, maxYear);
        }
        private static AgeRange PickAgeRange()
        {
            var chance = random.NextDouble();

            switch (chance)
            {
                case var _ when chance < 0.04: return AgeRange.Infant;
                case var _ when chance < 0.11: return AgeRange.Preschooler;
                case var _ when chance < 0.22: return AgeRange.SchoolAgeChild;
                case var _ when chance < 0.35: return AgeRange.Teenager;
                case var _ when chance < 0.55: return AgeRange.YoungAdult;
                case var _ when chance < 0.88: return AgeRange.Adult;
                case var _ when chance < 0.99: return AgeRange.Senior;
                default: return AgeRange.OldSenior;
            }
        }

        public static PersonModel GeneratePerson(this int id, Dictionary<string, short> recordsDatesUsage)
        {
            var dateBirth = PickDateBirth();
            var isMale = PickSexIsMale();
            var nameID = PickName(isMale);

            var serializedPeselKey = PeselGenerator.GetSerializedPeselKey(dateBirth, isMale);
            var sexBirthDateOccurences = GetRecordsDatesUsage(recordsDatesUsage, serializedPeselKey);

            var pesel = PeselGenerator.AssignPesel(dateBirth, isMale, sexBirthDateOccurences);

            PersonModel person = new PersonModel(id, nameID, 0, pesel);

            return person;
        }

        public static short GetRecordsDatesUsage(Dictionary<string, short> recordsDatesUsage, string key)
        {
            if (!recordsDatesUsage.ContainsKey(key))
            {
                recordsDatesUsage.Add(key, 0);
            }
            return recordsDatesUsage[key]++;
        }
        public static byte GetPersonAge(DateTime birthDate)
        {
            var personAge = (byte)(Today.Year - birthDate.Year);

            if (Today.Month > birthDate.Month || (Today.Month == birthDate.Month && Today.Day >= birthDate.Day))
            {
                return personAge;
            }
            else
            {
                return --personAge;
            }
        }

        private enum AgeRange
        {
            Infant,
            Preschooler,
            SchoolAgeChild,
            Teenager,
            YoungAdult,
            Adult,
            Senior,
            OldSenior,
        }
    }
}
