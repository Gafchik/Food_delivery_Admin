

using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Check_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
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

        #region full prop bind

        private ObservableCollection<Order> current_Ch_orders = new ObservableCollection<Order>(); // список заказов для выбраного чека

        public ObservableCollection<Order> Current_Ch_orders
        {
            get { return current_Ch_orders; }
            set { current_Ch_orders = value; OnPropertyChanged("Current_Ch_products"); }
        }

       



        private Current_Сheck selected_item_current; // выбраный админ для списка

        public Current_Сheck Selected_Item_Current
        {
            get { return selected_item_current; }
            set
            {
                selected_item_current = value; OnPropertyChanged("Selected_Item");
                foreach (var i in Orders.ToList())
                {
                    if (i.Orders_Current_Chek_Id == selected_item_current.Current_Сhecks_Id)
                        Current_Ch_orders.Add(i);
                }
            }
        }

        private Current_Сheck selected_item_current_order; // выбраный заказ из текущего чека 

        public Current_Сheck Selected_Item_Current_Order
        {
            get { return selected_item_current_order; }
            set
            {
                selected_item_current_order = value; OnPropertyChanged("Selected_Item_Current_Order");              
            }
        }


        private string serch_str; // строка поиска админа

        public string Serch_str
        {
            get { return serch_str; }
            set
            {
                serch_str = value; OnPropertyChanged("Serch_srt");
                if (Admins != null)
                    GC.Collect(GC.GetGeneration(Admins));
                Admins = new ObservableCollection<Admin>(admin_repository.GetColl().ToList().FindAll(i => i.Admins_Surname.ToLower().Contains(serch_str.ToLower())));
                OnPropertyChanged("Admins");

            }
        }
        #endregion

        #region comand




        #region new 
        private RelayCommand new_item; // открыть окно  с админами
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => { new New_Check().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close(); })); }
        }
        internal void Add_new(string log, string pass, string name, string surname, Window window) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить чек?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (log == "" && pass == "" && name == "" && surname == "")
            { MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            admin_repository.Create(new Admin
            {
                Admins_Login = log,
                Admins_Name = name,
                Admins_Password = pass,
                Admins_Surname = surname
            });
            window.Close();
            OnPropertyChanged("Admins");
        }


        #endregion

        #region edit 
        private RelayCommand edit_check; // изменение выбраного админа
        public RelayCommand Edit_Check
        {
            get
            {
                return edit_check ?? (edit_check = new RelayCommand(act =>
                {
                    try
                    {
                        current_Ch_repository.Update(Selected_Item_Current);
                        MessageBox.Show("Информация обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Операция не успешна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }
        private RelayCommand edit_order; // изменение выбраного админа
        public RelayCommand Edit_order
        {
            get
            {
                return edit_order ?? (edit_order = new RelayCommand(act =>
                {
                    try
                    {
                       // order_repository.Update(Selected_Item);
                        MessageBox.Show("Информация обновлена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Операция не успешна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        #endregion

        #region dell admin

        private RelayCommand dell; // удаление выбраного админа
        public RelayCommand Dell
        {
            get
            {
                return dell ?? (dell = new RelayCommand(act =>
                {
                    try
                    {
                        if (MessageBox.Show("Удалить администратора?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            return;
                        current_Ch_repository.Delete(Selected_Item_Current);
                        if (Admins != null)
                            GC.Collect(GC.GetGeneration(Admins));
                        Admins = new ObservableCollection<Admin>(admin_repository.GetColl());

                        OnPropertyChanged("Admins");
                        MessageBox.Show("Информация удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Операция не успешна", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }));
            }
        }

        #endregion

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
