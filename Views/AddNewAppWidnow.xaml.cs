using NewTechnology.Data;
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
    /// Логика взаимодействия для AddNewAppWidnow.xaml
    /// </summary>
    public partial class AddNewAppWidnow : Window
    {
        public AddNewAppWidnow()
        {
            InitializeComponent();
            DataContext = new AddNewAppViewModel();
        }

        public AddNewAppWidnow(Applications application)
        {
            InitializeComponent();
            DataContext = new AddNewAppViewModel(application);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}