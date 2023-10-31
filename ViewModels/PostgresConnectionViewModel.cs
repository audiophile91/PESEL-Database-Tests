using Npgsql;
using PESEL_Database_Tests.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PESEL_Database_Tests.ViewModels
{
    public class PostgresConnectionViewModel : ViewModelBase
    {
        public NpgsqlConnection? DatabaseConnection { get; set; }

        private bool _isConnected;

        private string _displayStatus = "Awaiting input..";
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }
        public string DisplayStatus
        {
            get
            {
                return _displayStatus;
            }
            set
            {
                _displayStatus = value;
                OnPropertyChanged(nameof(DisplayStatus));
            }
        }

        private string _displayDatabaseName = string.Empty;
        public string DisplayDatabaseName
        {
            get
            {
                return _displayDatabaseName;
            }
            set
            {
                _displayDatabaseName = value;
                OnPropertyChanged(nameof(DisplayDatabaseName));
            }
        }

        public string HostIP { get; set; } = "127.0.0.1";
        public string Port { get; set; } = "5432";
        public string DatabaseName { get; set; } = "postgres";
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public bool CanTryConnect => !string.IsNullOrWhiteSpace(HostIP);

        private string? DatabaseConnectionData => $"Host={HostIP};Port={Port};Database={DatabaseName};Username={Login};Password={Password};";
     
        public RelayCommand EstablishTestConnection => new RelayCommand(execute => CheckConnectionData(), canExecute => CanTryConnect);

        public void CheckConnectionData()
        {
            using (var testPostgresConnection = new NpgsqlConnection(DatabaseConnectionData))
            {
                try
                {
                    testPostgresConnection.Open();
                    ConnectionSuccessActions(testPostgresConnection);
                    testPostgresConnection.Close();
                    
                }
                catch (Exception ex)
                {
                    IsConnected = false;
                    DisplayStatus = ex.Message;
                }
            }
        }
        public void ConnectionSuccessActions(NpgsqlConnection connectedDatabase)
        {
            IsConnected = true;
            DisplayStatus = "Successfully connected to the database.";
            DisplayDatabaseName = connectedDatabase.Database;
            DatabaseConnection = new NpgsqlConnection(DatabaseConnectionData);
        }
    }
}
