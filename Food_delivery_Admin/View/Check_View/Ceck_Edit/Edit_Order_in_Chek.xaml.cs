using Food_delivery_Admin.ModelView.Chek_ModelView;
using Food_delivery_library.About_orders;
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

namespace Food_delivery_Admin.View.Check_View.Ceck_Edit
{
    /// <summary>
    /// Логика взаимодействия для Edit_Order_in_Chek.xaml
    /// </summary>
    public partial class Edit_Order_in_Chek : Window
    {
     public  static int id;
        public Edit_Order_in_Chek(Current_Cheсk current_Check)
        {
            InitializeComponent();
            DataContext = new ViewModel_Check();
            id = current_Check.Id;
        }
    }
}
