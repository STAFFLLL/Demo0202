using NewTechnology.Calculations;
using System;
using System.Windows;
using System.Windows.Input;

namespace NewTechnology.ViewModels
{
    public class MaterialCalculatorViewModel : BaseClass
    {
        private int productTypeId = 1;
        private int materialTypeId = 1;
        private int requiredProductQuantity = 100;
        private int productInStock = 20;
        private double param1 = 2.5;
        private double param2 = 1.8;
        private int calculationResult;

        public MaterialCalculatorViewModel()
        {
            CalculateCommand = new RelayCommand(Calculate);
        }

        public ICommand CalculateCommand { get; }

        public int ProductTypeId
        {
            get { return productTypeId; }
            set { productTypeId = value; OnPropertyChanged(); }
        }

        public int MaterialTypeId
        {
            get { return materialTypeId; }
            set { materialTypeId = value; OnPropertyChanged(); }
        }

        public int RequiredProductQuantity
        {
            get { return requiredProductQuantity; }
            set { requiredProductQuantity = value; OnPropertyChanged(); }
        }

        public int ProductInStock
        {
            get { return productInStock; }
            set { productInStock = value; OnPropertyChanged(); }
        }

        public double Param1
        {
            get { return param1; }
            set { param1 = value; OnPropertyChanged(); }
        }

        public double Param2
        {
            get { return param2; }
            set { param2 = value; OnPropertyChanged(); }
        }


        public int CalculationResult
        {
            get { return calculationResult; }
            set { calculationResult = value; OnPropertyChanged(); }
        }

        private void Calculate()
        {
            try
            {
                CalculationResult = MaterialCalculator.CalculateRequiredMaterial(
                    ProductTypeId,
                    MaterialTypeId,
                    RequiredProductQuantity,
                    ProductInStock,
                    Param1,
                    Param2);

                if (CalculationResult == -1)
                {
                    MessageBox.Show("Ошибка в входных параметрах! Проверьте корректность данных.",
                        "Ошибка расчета", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    MessageBox.Show($"Расчет завершен успешно!\nНеобходимое количество материала: {CalculationResult}",
                        "Результат расчета", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при расчете: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}