using PESEL_Database_Tests.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PESEL_Database_Tests.Filters
{
    public class PersonFilterManager
    {
        private ObservableCollection<PersonModel> _collection { get; }

        public PersonFilterManager(ObservableCollection<PersonModel> collection)
        {
            _collection = collection;
        }

        public void ApplyFilter()
        {
            ICollectionView collectionView = CollectionViewSource.GetDefaultView(_collection);

            if (collectionView != null && !string.IsNullOrEmpty(""))
            {
                switch (0)
                {
                    case 0: FilterID(collectionView); break;
                }
            }
        }

        private void FilterID(ICollectionView collectionView)
        {
            collectionView.Filter = item =>
            {
                switch (0)
                {
                    case 0:
                        return ((PersonModel)item)?.ID.ToString().Contains("") == true;
                    default:
                        return true;
                }
            };
        }
    }
}
