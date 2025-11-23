using NewTechnology.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NewTechnology.Views
{
    /// <summary>
    /// Логика взаимодействия для ChoiceProductWindow.xaml
    /// </summary>
    public partial class ChoiceProductWindow : Window
    {
        public ChoiceProductWindow(AddNewAppViewModel addNewAppViewModel)
        {
            InitializeComponent();
            ChoiceProductViewModel choiceProductViewModel = new ChoiceProductViewModel(addNewAppViewModel);
            DataContext = choiceProductViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
