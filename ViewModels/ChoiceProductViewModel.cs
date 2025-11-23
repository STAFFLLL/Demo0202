using NewTechnology.Data;
using NewTechnology.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NewTechnology.ViewModels
{
    public class ChoiceProductViewModel : BaseClass
    {
        private ObservableCollection<Products> allProducts;
        private Products selectedProduct;
        private int quantity = 1;
        private AddNewAppViewModel parentViewModel;

        public ChoiceProductViewModel(AddNewAppViewModel parent)
        {
            parentViewModel = parent;
            try
            {
                AllProducts = new ObservableCollection<Products>(
                    parent.ListProducts.ToList());
                AddProductCommand = new RelayCommand(AddProduct);
            }
            catch (System.Exception ex)
            {
                System.Windows.MessageBox.Show($"Ошибка при загрузке продуктов: {ex.Message}",
                    "Ошибка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        public ICommand AddProductCommand { get; }

        public ObservableCollection<Products> AllProducts
        {
            get { return allProducts; }
            set
            {
                allProducts = value;
                OnPropertyChanged();
            }
        }

        public Products SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged();
            }
        }

        public int Quantity
        {
            get { return quantity; }
            set
            {
                if (value >= 0)
                {
                    quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        private void AddProduct()
        {
            if (SelectedProduct == null)
            {
                System.Windows.MessageBox.Show("Выберите продукт", "Предупреждение",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
                return;
            }

            if (Quantity <= 0)
            {
                System.Windows.MessageBox.Show("Введите корректное количество", "Ошибка",
                    System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                return;
            }

            parentViewModel.AddProductToApplication(SelectedProduct, Quantity);

            // Закрываем окно после добавления
            CloseWindow();
        }

        private void CloseWindow()
        {
            // Поиск и закрытие текущего окна
            var window = System.Windows.Application.Current.Windows
                .OfType<System.Windows.Window>()
                .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}