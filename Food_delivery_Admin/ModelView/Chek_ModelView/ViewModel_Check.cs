using Food_delivery_library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Food_delivery_Admin.ModelView.Chek_ModelView
{
   public class ViewModel_Check : INotifyPropertyChanged // current compl +admin+ +user+ product
    {
        #region init
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();

        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository  = new User_Repository();

        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository product_repository = new Products_Repository();

        public ObservableCollection<Product_Categories> Product_categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        public ObservableCollection<Order> Orders { get; set; }
        private Orders_Repository order_repository = new Orders_Repository();

        public ObservableCollection<Completed_Сheck> Completed_Сhecks { get; set; }
        private Completed_Сhecks_Repository completed_Ch_repository = new Completed_Сhecks_Repository();

        public ObservableCollection<Current_Сheck> Current_Сhecks { get; set; }
        private Current_Сhecks_Repository current_Ch_repository = new Current_Сhecks_Repository();
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

       
        public ViewModel_Check() { InitializeComponent(); }

        public void InitializeComponent()
        {
            if (Admins != null)
                Admins.Clear();
            Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
            OnPropertyChanged("Admins");

            if (Users != null)
                Users.Clear();
            Users = new ObservableCollection<User>(user_repository.GetColl());
            OnPropertyChanged("Users");

            if (Product_categories != null)
                Product_categories.Clear();
            Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
            OnPropertyChanged("Product_categories");

            if (Products != null)
                Products.Clear();
            Products = new ObservableCollection<Product>(product_repository.GetColl());
            OnPropertyChanged("Products");
            Products.ToList().ForEach(i => i.Product_category = Product_categories.ToList().Find(j => j.Product_category_Id == i.Product_category_Id));
            
            if (Completed_Сhecks != null)
                Completed_Сhecks.Clear();
            Completed_Сhecks = new ObservableCollection<Completed_Сheck>(completed_Ch_repository.GetColl());
            OnPropertyChanged("Completed_Сhecks");
            Completed_Сhecks.ToList().ForEach(i => i.admin = Admins.ToList().Find(j => j.Admins_Id == i.Completed_Checks_Admin_Id));
            foreach (var i in Completed_Сhecks.ToList())
            {
                if (Users.ToList().Exists(j => j.User_Id == i.Completed_Сhecks_User_Id))
                    i.user = Users.ToList().Find(j => j.User_Id == i.Completed_Сhecks_User_Id);
                else
                    i.user = null;
            }

            if(Current_Сhecks != null)
                Current_Сhecks.Clear();
            Current_Сhecks = new ObservableCollection<Current_Сheck>(current_Ch_repository.GetColl());
            OnPropertyChanged("Current_Сhecks");
            Current_Сhecks.ToList().ForEach(i => i.admin = Admins.ToList().Find(j => j.Admins_Id == i.Current_Checks_Admin_Id));
            foreach (var i in Current_Сhecks.ToList())
            {
                if (Users.ToList().Exists(j => j.User_Id == i.Current_Сhecks_User_Id))
                    i.user = Users.ToList().Find(j => j.User_Id == i.Current_Сhecks_User_Id);
                else
                    i.user = null;
            }

            if (Orders != null)
                Orders.Clear();
            Orders = new ObservableCollection<Order>(order_repository.GetColl());
            OnPropertyChanged("Orders");
            Orders.ToList().ForEach(i => i.product = Products.ToList().Find(j => j.Product_Id == i.Orders_Products_Id));
            foreach (var i in Orders.ToList())
            {
                if (Current_Сhecks.ToList().Exists(j => j.Current_Сhecks_Id == i.Orders_Current_Chek_Id))
                    i.Current_Сheck = Current_Сhecks.ToList().Find(j => j.Current_Сhecks_Id == i.Orders_Current_Chek_Id);
                else
                    i.Current_Сheck = null;
            }
            foreach (var i in Orders.ToList())
            {
                if (Completed_Сhecks.ToList().Exists(j => j.Completed_Сhecks_Id == i.Orders_Completed_Chek_Id))
                    i.Completed_Сheck = Completed_Сhecks.ToList().Find(j => j.Completed_Сhecks_Id == i.Orders_Completed_Chek_Id);
                else
                    i.Completed_Сheck = null;
            }

        }
        #endregion
    }
}
