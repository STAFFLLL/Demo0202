using NewTechnology.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NewTechnology.ViewModels
{
    public class BaseClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<Products> listProducts;

        public ObservableCollection<Products> ListProducts
        {
            get { return listProducts; }
            set
            {
                listProducts = value;
                OnPropertyChanged();
            }
        }

        public void OnPropertyChanged([CallerMemberName] string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}