using Food_delivery_Admin.ModelView.Chek_ModelView;
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

namespace Food_delivery_Admin.View.Check_View
{
    /// <summary>
    /// Логика взаимодействия для Main_Check.xaml
    /// </summary>
    public partial class Main_Check : Window
    {
        public Main_Check()
        {
            InitializeComponent();
            DataContext = new ViewModel_Check();
           /* checkEdit.Checked += CheckEdit_Checked;
            checkEdit.Unchecked += CheckEdit_Unchecked;*/
        }

       /* private void CheckEdit_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var el in grid.ColumnDefinitions.ToList())
            {
                if(el)
                if (el is TextBox)
                    (el as TextBox).IsEnabled = false;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = false;
            }
        }

        private void CheckEdit_Checked(object sender, RoutedEventArgs e)
        {
            foreach (UIElement el in current_control.Items)
            {
                if (el is TextBox)
                    (el as TextBox).IsEnabled = true;
                if (el is ComboBox)
                    (el as ComboBox).IsEnabled = true;
            }
        }*/


    }
}

