using PESEL_Database_Tests.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PESEL_Database_Tests.ViewModels.Generator;
using System.Collections.ObjectModel;
using Npgsql;
using System.Windows;
using PESEL_Database_Tests.Statics.Generators;
using System.Windows.Input;
using System.Windows.Controls;
using System.Xml.Linq;

namespace PESEL_Database_Tests.Models
{
    public class DatabaseTransferModel
    {
        public DatabaseTransferModel(PostgresConnectionViewModel connectionViewModel, GeneratorMainViewModel generatorViewModel)
        {
            ConnectionViewModel = connectionViewModel;
            GeneratorViewModel = generatorViewModel;
        }

        private PostgresConnectionViewModel ConnectionViewModel { get; }
        private GeneratorMainViewModel GeneratorViewModel { get; }

        private ObservableCollection<PersonModel> Records => GeneratorViewModel.ListViewViewModel.GeneratorRecords;
        private NpgsqlConnection? Connection => ConnectionViewModel.DatabaseConnection;


        private string maleTableName = "name_male";
        private string femaleTableName = "name_female";
        private string sexTableName = "sex";
        private string voivodeshipTableName = "voivodeship";

        private void TransferStaticTables()
        {
            if (Connection != null)
            {
                Connection.Open();

                if (GenerateOneColumnTable(maleTableName, "Name", 50))
                {
                    foreach (string name in PersonGenerator.namesMale)
                    {
                        string insertQuery = $"INSERT INTO {maleTableName} (Name) VALUES (@Name)";

                        using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, Connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (GenerateOneColumnTable(femaleTableName, "Name", 50))
                {
                    foreach (string name in PersonGenerator.namesFemale)
                    {
                        string insertQuery = $"INSERT INTO {femaleTableName} (Name) VALUES (@Name)";

                        using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, Connection))
                        {
                            command.Parameters.AddWithValue("@Name", name);
                            command.ExecuteNonQuery();
                        }
                    }
                }

                if (GenerateOneColumnTable(sexTableName, "Sex", 10))
                {
                    var insertSexFQuery = $"INSERT INTO {sexTableName} (Sex) VALUES (@Name)";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertSexFQuery, Connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Female");
                        command.ExecuteNonQuery();
                    }

                    var insertSexMQuery = $"INSERT INTO {sexTableName} (Sex) VALUES (@Name)";

                    using (NpgsqlCommand command = new NpgsqlCommand(insertSexMQuery, Connection))
                    {
                        command.Parameters.AddWithValue("@Name", "Male");
                        command.ExecuteNonQuery();
                    }
                }

                if (GenerateOneColumnTable(voivodeshipTableName, "Voivodeship", 50))
                {
                    foreach (string voivodeship in PersonGenerator.voivodeships)
                    {
                        string insertQuery = $"INSERT INTO {voivodeshipTableName} (Voivodeship) VALUES (@Name)";

                        using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, Connection))
                        {
                            command.Parameters.AddWithValue("@Name", voivodeship);
                            command.ExecuteNonQuery();
                        }
                    }
                }
                Connection.Close();
            }
        }

        private void TransferGeneratedDatabase()
        {
            Connection.Open();

            foreach(PersonModel person in Records)
            {
                var birth = person.BirthExactDate;

                var insertQuery = "INSERT INTO person (Sex, Name, pesel, birth, voivodeship)" +
                                    $"VALUES ({(person.IsMale ? 2 : 1)}, {person.NameID + 1}, '{person.Pesel}', '{birth.Year}-{birth.Month}-{birth.Day}', {person.VoivodeshipID + 1});";

                using (NpgsqlCommand command = new NpgsqlCommand(insertQuery, Connection))
                {
                    command.ExecuteNonQuery();
                }   
            }

            Connection.Close();
        }

        public void TransferDataToPostgreDatabase()
        {
            TransferStaticTables();
            GeneratePersonTable();
            TransferGeneratedDatabase();
        }

        private bool GenerateOneColumnTable(string name, string columnName, int dataMaxLength)
        {
            if (Connection != null)
            {
                try
                {
                    string createTableQuery = $"CREATE TABLE {name} (ID SERIAL PRIMARY KEY, {columnName} VARCHAR({dataMaxLength}))";

                    using (NpgsqlCommand command = new NpgsqlCommand(createTableQuery, Connection))
                    {
                        command.ExecuteNonQuery(); ;
                    }
                    return true;
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    return false;
                }
            }
            return false;
        }

        private void GeneratePersonTable()
        {
            using (NpgsqlCommand command = new NpgsqlCommand())
            {
                command.Connection = Connection;

                Connection.Open();

                string createTableQuery = "CREATE TABLE IF NOT EXISTS person (" +
                                          "ID serial PRIMARY KEY," +
                                          "Sex integer CHECK(Sex >= 1 AND Sex <= 2)," +
                                          "Name int," +
                                          "PESEL character varying(11)," +
                                          "Birth date," +
                                          "Voivodeship int);";

                command.CommandText = createTableQuery;

                command.ExecuteNonQuery();

                Connection.Close();
            }
        }
    }
}
