using NewTechnology.Data;
using NewTechnology.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NewTechnology.ViewModels
{
    public class AddNewAppViewModel : BaseClass
    {
        private DZ0202Entities context = new DZ0202Entities();
        private ObservableCollection<Partners> partners = new ObservableCollection<Partners>();
        private ObservableCollection<ApplicationProduct> addProducts = new ObservableCollection<ApplicationProduct>();

        private Partners selectedPartner;
        private DateTime? selectedDate = DateTime.Now;
        private decimal totalCost;
        private Applications currentApplication;
        private bool isEditMode;

        public AddNewAppViewModel(Applications application = null)
        {
            try
            {
                ListProducts = new ObservableCollection<Products>(context.Products.ToList());
                ListPartners = new ObservableCollection<Partners>(context.Partners.ToList());

                AddNewProductCommand = new RelayCommand(AddNewProductM);
                SaveApplicationCommand = new RelayCommand(SaveApplication, CanSaveApplication);
                RemoveProductCommand = new RelayCommand<ApplicationProduct>(RemoveProduct);

                if (application != null)
                {
                    // Режим редактирования
                    CurrentApplication = application;
                    IsEditMode = true;
                    LoadApplicationData();
                }
                else
                {
                    // Режим создания
                    IsEditMode = false;
                    SelectedDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand AddNewProductCommand { get; }
        public ICommand SaveApplicationCommand { get; }
        public ICommand RemoveProductCommand { get; }

        public ObservableCollection<Partners> ListPartners
        {
            get { return partners; }
            set
            {
                partners = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ApplicationProduct> AddProducts
        {
            get { return addProducts; }
            set
            {
                addProducts = value;
                OnPropertyChanged();
                CalculateTotalCost();
            }
        }

        public Partners SelectedPartner
        {
            get { return selectedPartner; }
            set
            {
                selectedPartner = value;
                OnPropertyChanged();
            }
        }

        public DateTime? SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                OnPropertyChanged();
            }
        }

        public decimal TotalCost
        {
            get { return totalCost; }
            set
            {
                totalCost = value;
                OnPropertyChanged();
            }
        }

        public Applications CurrentApplication
        {
            get { return currentApplication; }
            set
            {
                currentApplication = value;
                OnPropertyChanged();
            }
        }

        public bool IsEditMode
        {
            get { return isEditMode; }
            set
            {
                isEditMode = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        public string WindowTitle => IsEditMode ? "Редактирование заявки" : "Создание заявки";

        private void LoadApplicationData()
        {
            try
            {
                if (CurrentApplication != null)
                {
                    SelectedPartner = ListPartners.FirstOrDefault(p => p.Id == CurrentApplication.IdPartner);
                    SelectedDate = CurrentApplication.Date;

                    // Загрузка продуктов заявки
                    var requestProducts = context.RequestsPartners
                        .Where(rp => rp.IdApplication == CurrentApplication.Id)
                        .ToList();

                    AddProducts.Clear();
                    foreach (var rp in requestProducts)
                    {
                        var product = ListProducts.FirstOrDefault(p => p.Id == rp.IdProduct);
                        if (product != null)
                        {
                            AddProducts.Add(new ApplicationProduct
                            {
                                Product = product,
                                Quantity = int.Parse(rp.CountProducts.ToString())
                            });
                        }
                    }

                    CalculateTotalCost();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных заявки: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNewProductM()
        {
            try
            {
                ChoiceProductWindow choiceProductWindow = new ChoiceProductWindow(this);
                choiceProductWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна выбора продукции: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void AddProductToApplication(Products product, int quantity)
        {
            try
            {
                if (product == null)
                {
                    MessageBox.Show("Продукт не выбран", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (quantity <= 0)
                {
                    MessageBox.Show("Количество должно быть больше 0", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var existingProduct = AddProducts.FirstOrDefault(p => p.Product.Id == product.Id);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += quantity;
                    OnPropertyChanged(nameof(AddProducts));
                }
                else
                {
                    AddProducts.Add(new ApplicationProduct
                    {
                        Product = product,
                        Quantity = quantity
                    });
                }

                CalculateTotalCost();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении продукта: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RemoveProduct(ApplicationProduct product)
        {
            try
            {
                if (product != null)
                {
                    AddProducts.Remove(product);
                    CalculateTotalCost();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении продукта: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CalculateTotalCost()
        {
            TotalCost = decimal.Parse(AddProducts.Sum(p => p.Product.MinPriceForPartner * p.Quantity).ToString());
            OnPropertyChanged(nameof(TotalCost));
        }

        private bool CanSaveApplication()
        {
            return SelectedPartner != null && AddProducts.Any();
        }

        private void SaveApplication()
        {
            try
            {
                if (!CanSaveApplication())
                {
                    MessageBox.Show("Заполните все обязательные поля и добавьте хотя бы один продукт",
                        "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (IsEditMode)
                {
                    // Режим редактирования
                    UpdateExistingApplication();
                }
                else
                {
                    // Режим создания
                    CreateNewApplication();
                }

                MessageBox.Show($"Заявка успешно {(IsEditMode ? "обновлена" : "создана")}!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении заявки: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateNewApplication()
        {
            // Создание новой заявки
            var newApplication = new Applications
            {
                IdPartner = SelectedPartner.Id,
                Date = SelectedDate,
                Cost = TotalCost,
            };

            context.Applications.Add(newApplication);
            context.SaveChanges();

            // Добавление продуктов в заявку
            foreach (var appProduct in AddProducts)
            {
                var requestPartner = new RequestsPartners
                {
                    IdApplication = newApplication.Id,
                    IdProduct = appProduct.Product.Id,
                    CountProducts = appProduct.Quantity
                };
                context.RequestsPartners.Add(requestPartner);
            }

            context.SaveChanges();
        }

        private void UpdateExistingApplication()
        {
            if (CurrentApplication == null) return;

            // Обновление данных заявки
            CurrentApplication.IdPartner = SelectedPartner.Id;
            CurrentApplication.Date = SelectedDate;
            CurrentApplication.Cost = TotalCost;

            // Удаление старых продуктов заявки
            var oldProducts = context.RequestsPartners
                .Where(rp => rp.IdApplication == CurrentApplication.Id)
                .ToList();

            context.RequestsPartners.RemoveRange(oldProducts);

            // Добавление новых продуктов заявки
            foreach (var appProduct in AddProducts)
            {
                var requestPartner = new RequestsPartners
                {
                    IdApplication = CurrentApplication.Id,
                    IdProduct = appProduct.Product.Id,
                    CountProducts = appProduct.Quantity
                };
                context.RequestsPartners.Add(requestPartner);
            }

            context.SaveChanges();
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }

    public class ApplicationProduct
    {
        public Products Product { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice => decimal.Parse((Product?.MinPriceForPartner * Quantity ?? 0).ToString());
    }
}