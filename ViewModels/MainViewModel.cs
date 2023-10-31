using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics.Generators;
using PESEL_Database_Tests.Statics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PESEL_Database_Tests.ViewModels.Generator;
using PESEL_Database_Tests.Commands;
using System.Windows.Controls;
using PESEL_Database_Tests.Views;
using PESEL_Database_Tests.Views.Generator;
using Npgsql;
using System.Security.AccessControl;

namespace PESEL_Database_Tests.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private UserControl _currentView;
        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public UserControl ConnectionView { get; }
        public UserControl GeneratorView { get; }

        private PostgresConnectionViewModel ConnectionViewModel { get; }
        private GeneratorMainViewModel GeneratorViewModel { get; }

        private DatabaseTransferModel TransfterModel { get; }

        public MainViewModel()
        {
            ConnectionViewModel = new PostgresConnectionViewModel();
            GeneratorViewModel = new GeneratorMainViewModel();

            TransfterModel = new DatabaseTransferModel(ConnectionViewModel, GeneratorViewModel);

            ConnectionView = new EstablishConnectionSQLView();
            ConnectionView.DataContext = ConnectionViewModel;

            GeneratorView = new MainView();
            GeneratorView.DataContext = GeneratorViewModel;

            _currentView = ConnectionView;
        }

        public RelayCommand SwitchViewConnectionCommand => new RelayCommand(execute => SwitchViewConnection(), canExecute => CurrentView != ConnectionView);
        public RelayCommand SwitchViewGeneratorCommand => new RelayCommand(execute => SwitchViewGenerator(), canExecute => CurrentView != GeneratorView);
        public RelayCommand SwitchViewTransferCommand => new RelayCommand(execute => TransferData(), canExecute => ConnectionViewModel.IsConnected && GeneratorViewModel.ListViewViewModel.IsGenerated);

        public void SwitchViewConnection()
        {
            CurrentView = ConnectionView;
        }
        public void SwitchViewGenerator()
        {
            CurrentView = GeneratorView;
        }
        public void TransferData()
        {
            TransfterModel.TransferDataToPostgreDatabase();

            MessageBox.Show("Transfer completed.");
        }
    }
}
