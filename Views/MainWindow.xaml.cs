using NewTechnology.Data;
using NewTechnology.ViewModels;
using System.Windows;

namespace NewTechnology.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new RequestsViewModel();
        }
    }
}
