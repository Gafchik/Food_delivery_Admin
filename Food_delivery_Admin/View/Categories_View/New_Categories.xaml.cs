using Food_delivery_Admin.ModelView.Categories_Model_View;
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

namespace Food_delivery_Admin.View.Category_View
{
    /// <summary>
    /// Логика взаимодействия для New_Category.xaml
    /// </summary>
    public partial class New_Category : Window
    {
        public New_Category()
        {
            InitializeComponent();
            DataContext = new ViewModel_Categories();
            Add.Click += Add_Click;
        }

        private void Add_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel_Categories).Add_new(Name.Text, new_categories);
    }
}
