using Food_delivery_Admin.ModelView.Products_ModelView;
using Food_delivery_library;
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
    /// Логика взаимодействия для New_Products.xaml
    /// </summary>
    public partial class New_Products : Window
    {
        public New_Products()
        {
            InitializeComponent();
            DataContext = new ViewModel_Producs();
            Add.Click += Add_Click;
        }


        private void Add_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel_Producs).Add_new((Category.SelectedItem as Product_Categories), Name.Text, Discount.Text, Prise.Text, new_products);  


    }
}

