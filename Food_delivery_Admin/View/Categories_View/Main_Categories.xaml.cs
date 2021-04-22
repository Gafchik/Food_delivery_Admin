using Food_delivery_Admin.ModelView.Categories_Model_View;
using System.Windows;
using System.Windows.Controls;

namespace Food_delivery_Admin.View.Category_View
{
    /// <summary>
    /// Логика взаимодействия для Main_Caterories.xaml
    /// </summary>
    public partial class Main_Categories : Window
    {
        public Main_Categories()
        {
            InitializeComponent();
            DataContext = new ViewModel_Categories();
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
