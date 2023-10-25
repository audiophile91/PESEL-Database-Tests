using PESEL_Database_Tests.Commands;
using PESEL_Database_Tests.Filters;
using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Statics;
using PESEL_Database_Tests.Statics.Generators;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace PESEL_Database_Tests.ViewModels.Database
{
    public class ListViewViewModel : ViewModelBase
    {
        private ObservableCollection<PersonModel> _records;
        public ObservableCollection<PersonModel> Records
        {
            get
            {
                return _records;
            }
            set
            {
                _records = value;
                OnPropertyChanged(nameof(Records));
            }
        }

        public ListViewViewModel()
        {
            _records = new();
            Display = true;
        }

        private bool _display;
        private bool _displayProgress;
        private bool _displayDatabase;
        private double _dataGenerationProgress;
        private double _dataGenerationProgressMaximum;
        private int _databaseDesiredSize;


        public bool Display
        {
            get
            {
                return _display;
            }
            set
            {
                _display = value;
                OnPropertyChanged(nameof(Display));
            }
        }
        public bool DisplayProgress
        {
            get
            {
                return _displayProgress;
            }
            set
            {
                _displayProgress = value;
                OnPropertyChanged(nameof(DisplayProgress));
            }
        }
        public bool DisplayDatabase
        {
            get
            {
                return _displayDatabase;
            }
            set
            {
                _displayDatabase = value;
                OnPropertyChanged(nameof(DisplayDatabase));
            }
        }
        public double DataGenerationProgress
        {
            get
            {
                return _dataGenerationProgress;
            }
            set
            {
                _dataGenerationProgress = value;
                OnPropertyChanged(nameof(DataGenerationProgress), nameof(DisplayProgressInfo));
            }
        }
        public double DataGenerationProgressMaximum
        {
            get
            {
                return _dataGenerationProgressMaximum;
            }
            set
            {
                _dataGenerationProgressMaximum = value;
                OnPropertyChanged(nameof(DataGenerationProgressMaximum));
            }
        }
        public int DatabaseDesiredSize
        {
            get
            {
                return _databaseDesiredSize;
            }
            set
            {
                _databaseDesiredSize = value;
                OnPropertyChanged(nameof(DatabaseDesiredSize));
            }
        }

        public bool IsGeneratingData { get; set; }

        public bool CanGenerateData => DatabaseDesiredSize > 0 && Display && !IsGeneratingData;
        public string DisplayProgressInfo => $"{DataGenerationProgress}/{DataGenerationProgressMaximum}";

        public async Task GenerateDatabase()
        {
            try
            {
                IsGeneratingData = true;
                DisplayDatabase = false;
                DisplayProgress = true;

                DataGenerationProgressMaximum = DatabaseDesiredSize;

                Records.Clear();
                List<PersonModel> batch = new();

                await Task.Run(() =>
                {
                    Dictionary<string, short> _recordsDatesUsage = new();

                    for (int i = 0; i < DatabaseDesiredSize; i++)
                    {
                        batch.Add(i.GeneratePerson(_recordsDatesUsage));

                        if (i % 1000 == 0)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _records.AddRange(batch);
                                batch.Clear();
                                DataGenerationProgress = i;
                            });
                        }
                    }
                });
                Records.AddRange(batch);

                DisplayDatabase = true;
                DisplayProgress = false;
                IsGeneratingData = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                IsGeneratingData = false;
            }
        }
    }
}
