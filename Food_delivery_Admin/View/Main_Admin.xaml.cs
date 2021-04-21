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

namespace Food_delivery_Admin.View
{
    /// <summary>
    /// Логика взаимодействия для Main_Admin.xaml
    /// </summary>
    public partial class Main_Admin : Window
    {
        public Main_Admin()
        {
            InitializeComponent();
            DataContext = new ModelView.ViewModel();
        }
    }
}
