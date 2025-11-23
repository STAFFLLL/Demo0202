using NewTechnology.Data;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace NewTechnology.ViewModels
{
    public class PartnerViewModel : BaseClass
    {
        private DZ0202Entities context = new DZ0202Entities();
        private Partners currentPartner;
        private bool isEditMode;

        public PartnerViewModel(Partners partner = null)
        {
            try
            {
                // Загрузка типов партнеров
                PartnerTypes = new ObservableCollection<TypePartners>(context.TypePartners.ToList());

                if (partner != null)
                {
                    // Режим редактирования
                    CurrentPartner = partner;
                    IsEditMode = true;
                }
                else
                {
                    // Режим добавления
                    CurrentPartner = new Partners();
                    IsEditMode = false;
                }

                SaveCommand = new RelayCommand(SavePartner, CanSavePartner);
                CancelCommand = new RelayCommand(Cancel);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при инициализации: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public Partners CurrentPartner
        {
            get { return currentPartner; }
            set
            {
                currentPartner = value;
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

        public string WindowTitle => IsEditMode ? "Редактирование партнера" : "Добавление партнера";

        public ObservableCollection<TypePartners> PartnerTypes { get; set; }

        private bool CanSavePartner()
        {
            return CurrentPartner != null &&
                   !string.IsNullOrWhiteSpace(CurrentPartner.Name) &&
                   !string.IsNullOrWhiteSpace(CurrentPartner.FIODirector) &&
                   !string.IsNullOrWhiteSpace(CurrentPartner.Address) &&
                   CurrentPartner.Rating >= 0 &&
                   CurrentPartner.TypePartners != null;
        }

        private void SavePartner()
        {
            try
            {
                if (!CanSavePartner())
                {
                    MessageBox.Show("Заполните все обязательные поля корректно", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!IsEditMode)
                {
                    context.Partners.Add(CurrentPartner);
                }

                context.SaveChanges();

                MessageBox.Show($"Партнер успешно {(IsEditMode ? "обновлен" : "добавлен")}!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                CloseWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancel()
        {
            if (MessageBox.Show("Вы уверены, что хотите отменить изменения?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CloseWindow();
            }
        }

        private void CloseWindow()
        {
            var window = Application.Current.Windows
                .OfType<Window>()
                .FirstOrDefault(w => w.DataContext == this);
            window?.Close();
        }
    }
}