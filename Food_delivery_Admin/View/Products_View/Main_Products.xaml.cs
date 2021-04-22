using Food_delivery_Admin.ModelView.Products_ModelView;
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

namespace Food_delivery_Admin.View.Products_View
{
    /// <summary>
    /// Логика взаимодействия для Main_Products.xaml
    /// </summary>
    public partial class Main_Products : Window
    {
        public Main_Products()
        {
            InitializeComponent();
            this.DataContext = new ViewModel_Producs();
            checkEdit.Checked += CheckEdit_Checked;
            checkEdit.Unchecked += CheckEdit_Unchecked;
           
        }     

        private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement el in stack.Children)
            {
                if (el is TextBox)
                    (el as TextBox).IsEnabled = false;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = false;
            }
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement el in stack.Children)
            {
                if (el is TextBox)
                    (el as TextBox).IsEnabled = true;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = true;
            }
        }
    }
}
