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

namespace Food_delivery_Admin.View.Admin_View
{
    /// <summary>
    /// Логика взаимодействия для New_Admin.xaml
    /// </summary>
    public partial class New_Admin : Window
    {
        public New_Admin()
        {
            InitializeComponent();
            DataContext = new ModelView.ViewModel_Admin();
            Add.Click += Add_Click;
        }

        private void Add_Click(object sender, RoutedEventArgs e) => (DataContext as ModelView.ViewModel_Admin).Add_new_Admin(Login.Text,Pass.Text,Name.Text,Sur.Text, new_admin);

    }
}
