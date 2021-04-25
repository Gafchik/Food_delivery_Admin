

using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Check_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using Food_delivery_library.About_orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Food_delivery_Admin.ModelView.Chek_ModelView
{
   public class ViewModel_Check : INotifyPropertyChanged // c+urrent+ +compl+ +admin+ +user+ +produc+t
    {

        #region init
      
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();

        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository products_Repository = new Products_Repository();

        public ObservableCollection<Product_Categories> Product_categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();

        public ObservableCollection<Order> Orders { get; set; }
        private Order_Repository order_repository = new Order_Repository();

        public ObservableCollection<Current_Cheсk> Current_Cheсks { get; set; }
        private Current_Chek_Repository current_CH_repository = new Current_Chek_Repository();
        
        public ObservableCollection<Completed_Cheсk> Completed_Cheсks { get; set; }
        private Completed_Chek_Repository completed_CH_repository = new Completed_Chek_Repository();
        public ObservableCollection<Current_Ch_Orders> Current_Ch_Orders { get; set; }
        private Current_Ch_Orders_Repository current_CH_order_repository = new Current_Ch_Orders_Repository();
        public ObservableCollection<Completed_Ch_Orders> Completed_Ch_Orders { get; set; }
        private Completed_Ch_Orders_Repository completed_CH_order_repository = new Completed_Ch_Orders_Repository();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
        public ViewModel_Check() { InitializeComponent(); }

        public void InitializeComponent()
        {
            if (Completed_Ch_Orders != null)
                Completed_Ch_Orders.Clear();
            Completed_Ch_Orders = new ObservableCollection<Completed_Ch_Orders>(completed_CH_order_repository.GetColl());
            OnPropertyChanged("Completed_Ch_Orders");
            
            if (Current_Ch_Orders != null)
                Current_Ch_Orders.Clear();
            Current_Ch_Orders = new ObservableCollection<Current_Ch_Orders>(current_CH_order_repository.GetColl());
            OnPropertyChanged("Current_Ch_Orders");
            
            if (Completed_Cheсks != null)
                Completed_Cheсks.Clear();
            Completed_Cheсks = new ObservableCollection<Completed_Cheсk>(completed_CH_repository.GetColl());
            OnPropertyChanged("Completed_Cheсks");
            
            if (Current_Cheсks != null)
                Current_Cheсks.Clear();
            Current_Cheсks = new ObservableCollection<Current_Cheсk>(current_CH_repository.GetColl());
            OnPropertyChanged("Current_Cheсks");
            
            if (Orders != null)
                Orders.Clear();
            Orders = new ObservableCollection<Order>(order_repository.GetColl());
            OnPropertyChanged("Orders");


            if (Users != null)
                Users.Clear();
            Users = new ObservableCollection<User>(user_repository.GetColl());
            OnPropertyChanged("Users");

            if (Admins != null)
                Admins.Clear();
            Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
            OnPropertyChanged("Admins");

            if (Product_categories != null)
                GC.Collect(GC.GetGeneration(Product_categories));
            Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
            OnPropertyChanged("Product_Categories");

            if (Products != null)
                GC.Collect(GC.GetGeneration(Products));
            Products = new ObservableCollection<Product>(products_Repository.GetColl());
            Products.ToList().ForEach(i => i.Product_category = Product_categories.ToList().Find(j => j.Product_category_Id == i.Product_category_Id));
            OnPropertyChanged("Products");


        }
        #endregion



        #region current check
        public ObservableCollection<Order> Coll_Product_Current_CH { get; set; } // колекция для списка продуктов текущего чека
     
        private Order selected_item_product_current_CH; // выбраный елемент для коллекции выше

        public Order Selected_Item_Product_Current_CH
        {
            get { return selected_item_product_current_CH; }
            set { selected_item_product_current_CH = value; }
        }

        private Current_Cheсk selected_item_Current_Cheсk; // выбраный чек из списка
        public Current_Cheсk Selected_Item_Current_Cheсk
        {
            get { return selected_item_Current_Cheсk; }
            set
            {
                selected_item_Current_Cheсk = value; OnPropertyChanged("selected_item_Current_Cheсk");
                if (Coll_Product_Current_CH != null)
                    GC.Collect(GC.GetGeneration(Coll_Product_Current_CH));
                Coll_Product_Current_CH = new ObservableCollection<Order>();
                foreach (var item in Current_Ch_Orders.ToList().FindAll(i => i.Chek_Id == Selected_Item_Current_Cheсk.Check_Id))
                    Orders.ToList().FindAll(j => j.Order_Id == item.Order_Id).ForEach(q => Coll_Product_Current_CH.Add(q));
                OnPropertyChanged("Coll_Product_Current_CH");
            }
        }

        #region new check
        public ObservableCollection<Order> Coll_Product_New_CH { get; set; } // колекция для списка продуктов новго чека

        private Order selected_item_product_new_CH; // выбраный елемент для коллекции выше

        public Order Selected_Item_Product_New_CH
        {
            get { return selected_item_product_new_CH; }
            set { selected_item_product_new_CH = value; }
        }

        #endregion





        private RelayCommand new_item; // открыть окно  с админами
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => { new New_Check().ShowDialog(); InitializeComponent(); })); }
        }
        #endregion

        #region go to main

        private RelayCommand go_to_Main;
        public RelayCommand Go_to_Main
        {
            get
            {
                return go_to_Main ?? (go_to_Main = new RelayCommand(act => { new Window_Main().Show(); ((Window)act).Close(); }));
            }
        }

        

        #endregion
    }
}
