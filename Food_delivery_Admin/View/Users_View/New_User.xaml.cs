using Food_delivery_Admin.ModelView.Users_Model_View;
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

namespace Food_delivery_Admin.View.Users_View
{
    /// <summary>
    /// Логика взаимодействия для New_User.xaml
    /// </summary>
    public partial class New_User : Window
    {
        public New_User()
        {
            InitializeComponent();
            DataContext = new ViewModel_Users();
            Add.Click += Add_Click;
        }
      

        private void Add_Click(object sender, RoutedEventArgs e) => (DataContext as ViewModel_Users).Add_new(Name.Text,Surname.Text,Phone.Text,Pass.Text,E_mail.Text,this,Card.Text);

    }
}
