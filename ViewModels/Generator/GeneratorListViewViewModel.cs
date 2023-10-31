using PESEL_Database_Tests.Commands;
using PESEL_Database_Tests.Models;
using PESEL_Database_Tests.Resources.ClassExtensions;
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

namespace PESEL_Database_Tests.ViewModels.Generator
{
    public class GeneratorListViewViewModel : ViewModelBase
    {
        private ObservableCollection<PersonModel> _generatorRecords;
        public ObservableCollection<PersonModel> GeneratorRecords
        {
            get
            {
                return _generatorRecords;
            }
            set
            {
                _generatorRecords = value;
                OnPropertyChanged(nameof(GeneratorRecords));
            }
        }

        public GeneratorListViewViewModel()
        {
            _generatorRecords = new();
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

        public bool IsGenerated { get; set; }

        public bool CanGenerateData => DatabaseDesiredSize > 0 && GeneratorRecords.Count == 0;
        public string DisplayProgressInfo => $"{DataGenerationProgress}/{DataGenerationProgressMaximum}";

        public async Task GenerateDatabase()
        {
            try
            {
                DisplayDatabase = false;
                DisplayProgress = true;

                DataGenerationProgressMaximum = DatabaseDesiredSize;

                GeneratorRecords.Clear();
                List<PersonModel> batch = new();

                await Task.Run(() =>
                {
                    Dictionary<string, short> _recordsDatesUsage = new();

                    for (int i = 0; i < DatabaseDesiredSize; i++)
                    {
                        batch.Add(PersonGenerator.GeneratePerson(i, _recordsDatesUsage));

                        if (i % 1000 == 0)
                        {
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                _generatorRecords.AddRange(batch);
                                batch.Clear();
                                DataGenerationProgress = i;
                            });
                        }
                    }
                });
                GeneratorRecords.AddRange(batch);
                IsGenerated = true;

                DisplayDatabase = true;
                DisplayProgress = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
