using NewTechnology.Data;
using NewTechnology.Views;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NewTechnology.ViewModels
{
    public class RequestsViewModel : BaseClass
    {
        private ObservableCollection<Applications> applications;
        private ObservableCollection<Partners> partners;
        private DZ0202Entities context = new DZ0202Entities();
        private Applications selectedApplication;
        private Partners selectedPartner;

        public RequestsViewModel()
        {
            try
            {
                LoadApplications();
                LoadPartners();

                // Команды для заявок
                AddNewApplicationCommand = new RelayCommand(AddNewApplicationM);
                EditApplicationCommand = new RelayCommand(EditApplicationM, CanEditApplication);
                ViewProductsCommand = new RelayCommand(ViewProductsM, CanViewProducts);

                // Команды для партнеров
                AddNewPartnerCommand = new RelayCommand(AddNewPartnerM);
                EditPartnerCommand = new RelayCommand(EditPartnerM, CanEditPartner);

                // Дополнительные команды
                TestMaterialCalculatorCommand = new RelayCommand(TestMaterialCalculatorM);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Команды для заявок
        public ICommand AddNewApplicationCommand { get; }
        public ICommand EditApplicationCommand { get; }
        public ICommand ViewProductsCommand { get; }

        // Команды для партнеров
        public ICommand AddNewPartnerCommand { get; }
        public ICommand EditPartnerCommand { get; }

        // Дополнительные команды
        public ICommand TestMaterialCalculatorCommand { get; }

        public ObservableCollection<Applications> ListApplications
        {
            get { return applications; }
            set
            {
                applications = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Partners> ListPartners
        {
            get { return partners; }
            set
            {
                partners = value;
                OnPropertyChanged();
            }
        }

        public Applications SelectedApplication
        {
            get { return selectedApplication; }
            set
            {
                selectedApplication = value;
                OnPropertyChanged();
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

        // Методы для заявок
        private void AddNewApplicationM()
        {
            try
            {
                AddNewAppWidnow addNewAppWindow = new AddNewAppWidnow();
                addNewAppWindow.Closed += (s, e) => LoadApplications();
                addNewAppWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна создания заявки: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditApplicationM()
        {
            try
            {
                if (SelectedApplication != null)
                {
                    AddNewAppWidnow editAppWindow = new AddNewAppWidnow(SelectedApplication);
                    editAppWindow.Closed += (s, e) => LoadApplications();
                    editAppWindow.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна редактирования заявки: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanEditApplication()
        {
            return SelectedApplication != null;
        }

        private void ViewProductsM()
        {
            try
            {
                if (SelectedApplication != null)
                {
                    SelectionApp();
                }
                else
                {
                    MessageBox.Show("Выберите заявку для просмотра продуктов", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна просмотра продуктов: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanViewProducts()
        {
            return SelectedApplication != null;
        }

        // Методы для партнеров
        private void AddNewPartnerM()
        {
            try
            {
                PartnerWindow partnerWindow = new PartnerWindow();
                partnerWindow.Closed += (s, e) => LoadPartners();
                partnerWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна добавления партнера: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditPartnerM()
        {
            try
            {
                if (SelectedPartner != null)
                {
                    PartnerWindow partnerWindow = new PartnerWindow(SelectedPartner);
                    partnerWindow.Closed += (s, e) => LoadPartners();
                    partnerWindow.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Выберите партнера для редактирования", "Информация",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна редактирования партнера: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanEditPartner()
        {
            return SelectedPartner != null;
        }

        private void TestMaterialCalculatorM()
        {
            try
            {
                MaterialCalculatorWindow testWindow = new MaterialCalculatorWindow();
                testWindow.ShowDialog();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии окна тестирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SelectionApp()
        {
            try
            {
                if (selectedApplication != null)
                {
                    // Загрузка продуктов для выбранной заявки
                    var products = context.RequestsPartners
                        .Where(rp => rp.IdApplication == selectedApplication.Id)
                        .Select(rp => rp.Products)
                        .ToList();

                    ListProducts = new ObservableCollection<Products>(products);

                    ListProductsWindow listProductsWindow = new ListProductsWindow(this);
                    listProductsWindow.ShowDialog();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных заявки: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadApplications()
        {
            try
            {
                ListApplications = new ObservableCollection<Applications>(
                    context.Applications
                        .Include(a => a.Partners)
                        .Include(a => a.Partners.TypePartners)
                        .OrderByDescending(a => a.Date)
                        .ToList());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке заявок: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadPartners()
        {
            try
            {
                ListPartners = new ObservableCollection<Partners>(
                    context.Partners
                        .Include(p => p.TypePartners)
                        .OrderBy(p => p.Name)
                        .ToList());
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке партнеров: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}