using PESEL_Database_Tests.Static;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PESEL_Database_Tests
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Person> Records { get; set; }

        private Dictionary<string, int> _recordsDatesUsage = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Records = new();
        }


        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GenerateDatabase();
            _recordsDatesUsage.Clear();
        }


        private async void GenerateDatabase()
        {
            int recordsCount = 10000000;
            DataGenerationProgress.Maximum = recordsCount;

            List<Person> batch = new();

            await Task.Run(() =>
            {          
                for (int i = 0; i < recordsCount; i++)
                {
                    batch.Add(GeneratePerson(i));

                    if (i % 1000 == 0)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            Records.AddRange(batch);
                            batch.Clear();
                            DataGenerationProgress.Value = i;
                            DataGenerationProgressInfo.Content = $"{i}/{recordsCount}";
                        });
                    }
                }
            });
            Records.AddRange(batch);

            AssignPESELs();

            DataGenerationProgress.Visibility = Visibility.Collapsed;
            DataGenerationProgressInfo.Visibility = Visibility.Collapsed;
            DataDisplay.Visibility = Visibility.Visible;
        }
        private Person GeneratePerson(int id)
        {
            Person person = new Person();

            person.ID = id;
            person.IsMale = Generator.IsMale();
            person.DateBirth = Generator.PickDateBirth();
            person.Name = Generator.PickName(person.IsMale);

            return person;
        }
        private void AssignPESELs()
        {
            foreach(Person person in Records)
            {
                string birthKey = person.DateBirth.ToString("yyMMdd") + (person.IsMale ? "1" : "0");

                if (!_recordsDatesUsage.ContainsKey(birthKey))
                {
                    _recordsDatesUsage.Add(birthKey, 0);
                }
                int ordinalNumber = _recordsDatesUsage[birthKey]++;

                person.PESEL = Generator.GetPESEL(person.DateBirth, person.IsMale, ordinalNumber);
            } 
        }
    }
}