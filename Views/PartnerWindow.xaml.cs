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
    /// Логика взаимодействия для PartnerWindow.xaml
    /// </summary>
    public partial class PartnerWindow : Window
    {
        public PartnerWindow()
        {
            InitializeComponent();
            DataContext = new PartnerViewModel();
        }

        public PartnerWindow(Partners partner)
        {
            InitializeComponent();
            DataContext = new PartnerViewModel(partner);
        }
    }
}
