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
    /// Логика взаимодействия для ListProductsWindow.xaml
    /// </summary>
    public partial class ListProductsWindow : Window
    {
        public ListProductsWindow()
        {
            InitializeComponent();
        }

        public ListProductsWindow(object dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
