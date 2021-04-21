﻿using Food_delivery_Admin.View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Food_delivery_Admin.ModelView
{
    public class ViewModel : INotifyPropertyChanged
    {
      
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();

        public ObservableCollection<Current_Orders> Current_Orders { get; set; }
        private Current_Orders_Repository current_Orders_Repository = new Current_Orders_Repository();
        
       public ObservableCollection<Completed_Orders> Completed_Orders { get; set; }
        private Completed_Orders_Repository completed_Orders_Repository = new Completed_Orders_Repository();

        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();

        public ObservableCollection<Product_Categories> Product_Categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository products_Repository = new Products_Repository();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        #region init
        public ViewModel()
        {
            InitializeComponent();
        }
        public void InitializeComponent()
        {
            if (Admins != null)
                Admins.Clear();
            Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
            OnPropertyChanged("Admins");

            if (Current_Orders != null)
                Current_Orders.Clear();
            Current_Orders = new ObservableCollection<Current_Orders>(current_Orders_Repository.GetColl());
             OnPropertyChanged("Current_Orders");

            if (Completed_Orders != null)
                Completed_Orders.Clear();
            Completed_Orders = new ObservableCollection<Completed_Orders>(completed_Orders_Repository.GetColl());
             OnPropertyChanged("Completed_Orders");

            if (Users != null)
                Users.Clear();
            Users = new ObservableCollection<User>(user_repository.GetColl());
             OnPropertyChanged("Users");

            if (Product_Categories != null)
                Product_Categories.Clear();
            Product_Categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
             OnPropertyChanged("Product_Categories");

            if (Products != null)
                Products.Clear();
            Products = new ObservableCollection<Product>(products_Repository.GetColl());
             OnPropertyChanged("Products");

        }
        #endregion

        #region full prop bind
        private string temp_login;
        public string Temp_login
        {
            get { return temp_login; }
            set { temp_login = value; OnPropertyChanged("temp_login"); }
        }


        private string temp_password;
        public string Temp_password
        {
            get { return temp_password; }
            set { temp_password = value; OnPropertyChanged("temp_password"); }
        }
        #endregion

        #region Sing in
        private RelayCommand sing_in;
        public RelayCommand Sing_in
        {
            get
            {
                return sing_in ?? (sing_in = new RelayCommand(act =>
                {
                    if(Admins.ToList().Exists(i=> i.Admins_Login == Temp_login) && Admins.ToList().Exists(i => i.Admins_Password == Temp_password))
                    {
                        new Window_Main().Show();
                        ((Window)act).Close();
                       
                    }   
                    else
                        MessageBox.Show("Не верный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }));
            }
        }
        #endregion

        #region Exit
        private RelayCommand exit;
        public RelayCommand Exit
        {
            get { return exit ?? (exit = new RelayCommand(act => ((Window)act).Close())); }         
        }
        #endregion
    }
}

