

using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Check_View;
using Food_delivery_Admin.View.Check_View.Ceck_Edit;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using Food_delivery_library.About_orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace Food_delivery_Admin.ModelView.Chek_ModelView
{
   public class ViewModel_Check : INotifyPropertyChanged // 
    {

        #region init
      
        static ViewModel_Check()
        {               
            selected_item_product_new_CH = new Order();
            Coll_Product_New_CH = new ObservableCollection<Order>();
        }
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();

        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository products_Repository = new Products_Repository();

        public ObservableCollection<Product_Categories> Product_categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();

        public ObservableCollection<Order> Current_Orders { get; set; }
        private Current_Orders_Repository current_order_repository = new Current_Orders_Repository();

        public ObservableCollection<Current_Cheсk> Current_Cheсks { get; set; }
        private Current_Chek_Repository current_CH_repository = new Current_Chek_Repository();
        
        public ObservableCollection<Completed_Cheсk> Completed_Cheсks { get; set; }
        private Completed_Chek_Repository completed_CH_repository = new Completed_Chek_Repository();

        public ObservableCollection<Order> Completed_Orders { get; set; }
        private  Completed_Orders_Repository completed_order_repository = new Completed_Orders_Repository();
        

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion
        public ViewModel_Check() { InitializeComponent(); }

        public async void InitializeComponent()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 5000;
            timer.Start();

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate  
            {
                
                    if (Completed_Cheсks != null)
                        Completed_Cheсks.Clear();
                    Completed_Cheсks = new ObservableCollection<Completed_Cheсk>(completed_CH_repository.GetColl());
                    OnPropertyChanged("Completed_Cheсks");
                
            }));

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Current_Cheсks != null)
                    Current_Cheсks.Clear();
                Current_Cheсks = new ObservableCollection<Current_Cheсk>(current_CH_repository.GetColl());
                OnPropertyChanged("Current_Cheсks");
            }));

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Current_Orders != null)
                    Current_Orders.Clear();
                Current_Orders = new ObservableCollection<Order>(current_order_repository.GetColl());
                OnPropertyChanged("Current_Orders");
                count_current_orders = Current_Cheсks.Count();               
            }));
            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Completed_Orders != null)
                    Completed_Orders.Clear();
                Completed_Orders = new ObservableCollection<Order>(completed_order_repository.GetColl());
                OnPropertyChanged("Completed_Orders");
            }));

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Users != null)
                    Users.Clear();
                Users = new ObservableCollection<User>(user_repository.GetColl());
                OnPropertyChanged("Users");
            }));

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Admins != null)
                    Admins.Clear();
                Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
                OnPropertyChanged("Admins");
            }));

            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Product_categories != null)
                    GC.Collect(GC.GetGeneration(Product_categories));
                Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
                OnPropertyChanged("Product_Categories");

                if (Products != null)
                    GC.Collect(GC.GetGeneration(Products));
                Products = new ObservableCollection<Product>(products_Repository.GetColl());
                Products.ToList().ForEach(i => i.Product_category = Product_categories.ToList().Find(j => j.Product_category_Id == i.Product_category_Id));
                OnPropertyChanged("Products");
            }));

        }

        #endregion

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e) // таймер ивент типа слушатель
        {
            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                lock (this)
                {
                    int temp;                   
                    
                    temp = current_CH_repository.GetColl().Count();
                    if (temp != count_current_orders)
                    {
                        using (var Player = new SoundPlayer(Environment.CurrentDirectory + "\\Sound\\new_order.wav"))
                            Player.PlaySync();
                        Current_Cheсks = new ObservableCollection<Current_Cheсk>(current_CH_repository.GetColl());
                        OnPropertyChanged("Current_Cheсks");
                    }
                    count_current_orders = temp;
                }
               
            }));
        }
        public static Timer timer = new Timer();
        public static int count_current_orders; 


        #region new check

        #region new order

        private Product_Categories selected_item_new_Or; //категория в новом заказе

        public Product_Categories Selected_Item_New_Or
        {
            get { return selected_item_new_Or; }
            set
            {
                selected_item_new_Or = value;
                OnPropertyChanged("Selected_Item_New_Or"); Serch_str_New_Or = Serch_str_New_Or;
            }

        }


        private string serch_str_new_Or = ""; // строка поиска в новом закзае

        public string Serch_str_New_Or
        {
            get { return serch_str_new_Or; }
            set
            {
                serch_str_new_Or = value; OnPropertyChanged("Serch_str_New_Or");
                if (Products != null)
                    GC.Collect(GC.GetGeneration(Products));
                Products = new ObservableCollection<Product>(products_Repository.GetColl());
                Products.ToList().ForEach(i => i.Product_category = Product_categories.ToList().Find(j => j.Product_category_Id == i.Product_category_Id));
                if (Selected_Item_New_Or != null)
                {
                    Products = new ObservableCollection<Product>(products_Repository.GetColl().ToList()
                        .FindAll(i => i.Product_category_Id == Selected_Item_New_Or.Product_category_Id)
                        .FindAll(i => i.Product_Name.ToLower().Contains(serch_str_new_Or.ToLower())));
                }
                else
                    Products = new ObservableCollection<Product>(products_Repository.GetColl().ToList()
                        .FindAll(i => i.Product_Name.ToLower().Contains(serch_str_new_Or.ToLower())));
                Products.ToList().ForEach(i => i.Product_category = Product_categories.ToList().Find(j => j.Product_category_Id == i.Product_category_Id));
                OnPropertyChanged("Products");

            }
        }

        private static Product selected_item_product_new_Or; // выбраный елемент для коллекции продуктов в новом заказе

        public Product Selected_Item_Product_New_Or
        {
            get { return selected_item_product_new_Or; }
            set { selected_item_product_new_Or = value; OnPropertyChanged("Selected_Item_Product_New_Or"); }
        }

        private RelayCommand add_product; // добавить продукт в новом чеке

        public RelayCommand Add_Product
        {
            get
            {
                return add_product ?? (add_product = new RelayCommand(act =>
                {
                    Coll_Product_New_CH.Add(new Order
                    {
                        Order_Products_Name = Selected_Item_Product_New_Or.Product_Name,
                        Order_Price = (float)Selected_Item_Product_New_Or.Product_Price,
                        Order_Discount = (float)Selected_Item_Product_New_Or.Product_Discount,
                        Order_Final_Price = (float)Selected_Item_Product_New_Or.Product_Price -
                            (((float)Selected_Item_Product_New_Or.Product_Price / 100) * (float)Selected_Item_Product_New_Or.Product_Discount)
                    });
                    OnPropertyChanged("Coll_Product_New_CH");

                }));
            }
        }

        #endregion
        private RelayCommand dell_order; // удалить продукт в новом чеке

        public RelayCommand Dell_order
        {
            get
            {
                return dell_order ?? (dell_order = new RelayCommand(act =>
                {
                    if (MessageBox.Show("Удалить продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    Coll_Product_New_CH.Remove(Selected_Item_Product_New_CH);
                    New_CH_Final_Price = 0;
                    Coll_Product_New_CH.ToList().ForEach(i => New_CH_Final_Price += i.Order_Final_Price);
                    OnPropertyChanged("New_CH_Final_Price");

                }));
            }
        }

        private RelayCommand add_order; // открыть окно новый продукт нового чека

        public RelayCommand Add_order
        {
            get
            {
                return add_order ?? (add_order = new RelayCommand(act => {

                    new New_Order().ShowDialog();
                    New_CH_Final_Price = 0;
                    Coll_Product_New_CH.ToList().ForEach(i => New_CH_Final_Price += i.Order_Final_Price);
                    OnPropertyChanged("New_CH_Final_Price");
                    OnPropertyChanged("Coll_Product_New_CH");

                }));
            }
        }


        private RelayCommand edit_add_order; // открыть окно добавления продукта в текущий чек

        public RelayCommand Edit_add_Order
        {
            get
            {
                return edit_add_order ?? (edit_add_order = new RelayCommand(act =>
                {

                    if (Selected_Item_Current_Cheсk == null)
                        return;

                    else
                    {
                        new Edit_Order_in_Chek(Selected_Item_Current_Cheсk).ShowDialog();
                        Coll_Product_Current_CH = new ObservableCollection<Order>(current_order_repository.GetColl()
                   .ToList().FindAll(i => i.Order_Chek_Id == Selected_Item_Current_Cheсk.Check_Id));
                        OnPropertyChanged("Coll_Product_Current_CH");
                        Selected_Item_Current_Cheсk.Check_Final_Price = 0;
                        Coll_Product_Current_CH.ToList()
                        .ForEach(i => Selected_Item_Current_Cheсk.Check_Final_Price += i.Order_Final_Price);
                        OnPropertyChanged("Selected_Item_Current_Cheсk");
                    }
                }));
            }
        }



        private RelayCommand add_current_product; // добавление продукта из нового окна в текущий чек

        public RelayCommand Add_Current_Product 
        {
            get
            {
                return add_current_product ?? (add_current_product = new RelayCommand(act =>
                {

                    
                    if (MessageBox.Show("Добавить продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    else
                    {
                        current_order_repository.Create(new Order
                        {
                            Order_Chek_Id = Edit_Order_in_Chek.id,
                            Order_Discount = (float)Selected_Item_Product_New_Or.Product_Discount,
                            Order_Final_Price = (float)Selected_Item_Product_New_Or.Product_Price,
                            Order_Price = (float)Selected_Item_Product_New_Or.Product_Price,
                            Order_Products_Name = Selected_Item_Product_New_Or.Product_Name
                        });
                        MessageBox.Show("Добавлено", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }));
            }
        }



        private RelayCommand edit_dell_order; // удалить продукт из текущего чека

        public RelayCommand Edit_dell_Order
        {
            get
            {
                return edit_dell_order ?? (edit_dell_order = new RelayCommand(act =>
                {

                    if (Selected_Item_Current_Cheсk == null)
                        return;
                    if (MessageBox.Show("Удалить продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                        return;
                    else
                    {
                        current_order_repository.Delete(Selected_Item_Product_Current_CH);
                        Coll_Product_Current_CH = new ObservableCollection<Order>(current_order_repository.GetColl()
                  .ToList().FindAll(i => i.Order_Chek_Id == Selected_Item_Current_Cheсk.Check_Id));
                        OnPropertyChanged("Coll_Product_Current_CH");
                        Selected_Item_Current_Cheсk.Check_Final_Price = 0;
                        Coll_Product_Current_CH.ToList()
                        .ForEach(i => Selected_Item_Current_Cheсk.Check_Final_Price += i.Order_Final_Price);
                        OnPropertyChanged("Selected_Item_Current_Cheсk");

                    }
                }));
            }
        }


        private RelayCommand add_check; // добавить новый чек

        public RelayCommand Add_Check 
        {
            get
            {
                return add_check ?? (add_check = new RelayCommand(act =>
                {
                    try
                    {
                        List<int> q = new List<int>();
                        current_CH_repository.GetColl().ToList().ForEach(i => q.Add(i.Check_Id));
                        completed_CH_repository.GetColl().ToList().ForEach(i => q.Add(i.Check_Id));
                        int temp;
                        if (q.Count > 0)
                            temp = q.Max() + 1;
                        else
                            temp = 1;
                        GC.Collect(GC.GetGeneration(q));
                        current_CH_repository.Create(new Current_Cheсk
                        {

                            Check_Id = temp,
                            Check_Admin = ViewModel_Admin.curent_Admin.Admins_Surname,
                            Check_Date = new DateTime(DateTime.Now.Year, DateTime.Now.Month,
                                            DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            Check_Final_Price = New_CH_Final_Price,
                            Check_User_Phone = Selected_Item_User_New_CH.User_Phone
                        }) ;
                    }
                    catch (Exception)
                    {
                      
                        MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    Coll_Product_New_CH.ToList().ForEach(i =>
                    {
                        current_order_repository.Create(new Order
                        {
                            Order_Discount = i.Order_Discount,
                            Order_Final_Price = i.Order_Final_Price,
                            Order_Price = i.Order_Price,
                            Order_Products_Name = i.Order_Products_Name,
                            Order_Chek_Id = current_CH_repository.GetColl().ToList()
                            .Find(j => j.Check_User_Phone == Selected_Item_User_New_CH.User_Phone &&
                            j.Check_Final_Price == New_CH_Final_Price).Check_Id
                        });
                    });
                    count_current_orders++;
                    (act as Window).Close();
                }));
            }
        }


        private string serch_str_user_new_CH; // строка поиска клиента в новом чеке

        public string Serch_Str_User_New_CH
        {
            get { return serch_str_user_new_CH; }
            set
            {
                serch_str_user_new_CH = value; OnPropertyChanged("Serch_Str_User_New_CH");
                if (Users != null)
                    GC.Collect(GC.GetGeneration(Users));
                Users = new ObservableCollection<User>(user_repository.GetColl().ToList().FindAll(i => i.User_Name.ToLower().Contains(serch_str_user_new_CH.ToLower())));
                OnPropertyChanged("Users");

            }
        }


        private User selected_item_user_new_CH; // выбраный елемент для коллекции пользователей в новом чеке

        public User Selected_Item_User_New_CH
        {
            get { return selected_item_user_new_CH; }
            set { selected_item_user_new_CH = value; OnPropertyChanged("Selected_Item_User_New_CH"); }
        }

        public static float new_CH_final_price; // фул проп доя отображения цены  в новом чеке

        public float New_CH_Final_Price
        {
            get { return new_CH_final_price; }
            set { new_CH_final_price = value; OnPropertyChanged("New_CH_Final_Price"); }
        }



        public static ObservableCollection<Order> Coll_Product_New_CH { get; set; } // колекция для списка продуктов новго чека

        private static Order selected_item_product_new_CH; // выбраный елемент для коллекции выше

        public Order Selected_Item_Product_New_CH
        {
            get { return selected_item_product_new_CH; }
            set { selected_item_product_new_CH = value; OnPropertyChanged("Selected_Item_Product_New_CH"); }
        }

        #endregion

        #region current check

        private string serch_str_current_ch =""; // строка поиска в текущих чеках по номеру чека

        public string Serch_Str_Current_Ch
        {
            get { return serch_str_current_ch; }
            set
            {
                serch_str_current_ch = value; OnPropertyChanged("Serch_Str_User_New_CH");
                if (Current_Cheсks != null)
                    GC.Collect(GC.GetGeneration(Current_Cheсks));
                Current_Cheсks = new ObservableCollection<Current_Cheсk>(current_CH_repository.GetColl().ToList().FindAll(i => i.Check_Id.ToString().ToLower().Contains(serch_str_current_ch.ToLower())));
                OnPropertyChanged("Current_Cheсks");

            }
        }

        public ObservableCollection<Order> Coll_Product_Current_CH { get; set; } // колекция для списка продуктов текущего чека
     
        private Order selected_item_product_current_CH; // выбраный елемент для коллекции выше

        public Order Selected_Item_Product_Current_CH
        {
            get { return selected_item_product_current_CH; }
            set { selected_item_product_current_CH = value; }
        }

        private  Current_Cheсk selected_item_Current_Cheсk; // выбраный чек из списка
        public Current_Cheсk Selected_Item_Current_Cheсk
        {
            get { return selected_item_Current_Cheсk; }
            set
            {
                selected_item_Current_Cheсk = value; OnPropertyChanged("Selected_Item_Current_Cheсk");
                if (Coll_Product_Current_CH != null)
                    GC.Collect(GC.GetGeneration(Coll_Product_Current_CH));
                try
                {
                    Coll_Product_Current_CH = new ObservableCollection<Order>(current_order_repository.GetColl()
                   .ToList().FindAll(i => i.Order_Chek_Id == selected_item_Current_Cheсk.Check_Id));
                    OnPropertyChanged("Coll_Product_Current_CH");

                    selected_item_Current_Cheсk.Check_Final_Price = 0;
                    Coll_Product_Current_CH.ToList()
                    .ForEach(i => selected_item_Current_Cheсk.Check_Final_Price += i.Order_Final_Price);
                    OnPropertyChanged("Selected_Item_Current_Cheсk");
                }
                catch (Exception)
                {

                    Coll_Product_Current_CH=null;
                }
               

            }
        }


       


        #region add dell 

        private RelayCommand new_item; // открыть окно  создания нового чека
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => {
                Coll_Product_New_CH.Clear(); New_CH_Final_Price = 0;
                new New_Check().ShowDialog(); InitializeComponent(); }));
            }
        } 
        
        private RelayCommand dell_item; // удалить выбраный чек
        public RelayCommand Dell_item
        {            
            get { return dell_item ?? (dell_item = new RelayCommand(act => {
                try
                {
                if (MessageBox.Show("Удалить чек?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;
                current_order_repository.GetColl().ToList().FindAll(i => i.Order_Chek_Id == Selected_Item_Current_Cheсk.Check_Id)
                 .ForEach(i => current_order_repository.Delete(i));
                current_CH_repository.Delete(Selected_Item_Current_Cheсk);
                InitializeComponent();

                }
                catch (Exception) 
                { MessageBox.Show("Повторите попытку", "Извине не вышло", MessageBoxButton.OK, MessageBoxImage.Exclamation); }
               
                 }));
            }
        }
        #endregion


        #endregion

        #region completed check 

        private DateTime selected_date = DateTime.Now;
        public DateTime Selected_Date
        {
            get { return selected_date; }
            set
            {
                selected_date = value; OnPropertyChanged("selected_date");
                Serch_Str_Completed_Ch = Serch_Str_Completed_Ch;
            }
        }



        private string serch_str_completed_ch = ""; // строка поиска в выполненых чеках по номеру чека

        public string Serch_Str_Completed_Ch
        {
            get { return serch_str_completed_ch; }
            set
            {
                serch_str_completed_ch = value; OnPropertyChanged("Serch_Str_Completed_Ch");
                if (Completed_Cheсks != null)
                    GC.Collect(GC.GetGeneration(Completed_Cheсks));
                Completed_Cheсks = new ObservableCollection<Completed_Cheсk>(completed_CH_repository.GetColl().ToList()
                    .FindAll(i=> i.Check_Date.Day == Selected_Date.Day&&i.Check_Date.Month == Selected_Date.Month&& i.Check_Date.Year == Selected_Date.Year)
                    .FindAll(i => i.Check_Id.ToString().ToLower().Contains(serch_str_completed_ch.ToLower())));
                OnPropertyChanged("Completed_Cheсks");

            }
        }


        public ObservableCollection<Order> Coll_Product_Completed_CH { get; set; } // колекция для списка продуктов

        private Completed_Cheсk selected_item_completed_CH; // выбраный элемент для выполненых чеков

        public Completed_Cheсk Selected_Item_Completed_CH 
        {
            get { return selected_item_completed_CH; }
            set {
                selected_item_completed_CH = value;  OnPropertyChanged("Selected_Item_Completed_CH");
                if (Coll_Product_Completed_CH != null)
                    GC.Collect(GC.GetGeneration(Coll_Product_Completed_CH));
                try
                {
                    Coll_Product_Completed_CH = new ObservableCollection<Order>(completed_order_repository.GetColl()
                   .ToList().FindAll(i => i.Order_Chek_Id == selected_item_completed_CH.Check_Id));
                    OnPropertyChanged("Coll_Product_Completed_CH");

                    selected_item_completed_CH.Check_Final_Price = 0;
                    Coll_Product_Completed_CH.ToList()
                    .ForEach(i => selected_item_completed_CH.Check_Final_Price += i.Order_Final_Price);
                    OnPropertyChanged("Coll_Product_Completed_CH");
                }
                catch (Exception)
                {

                    Coll_Product_Completed_CH = null;
                }
            }
        }
        
        private Order selected_item_product_completed_CH; // выбраный элемент для продуктов в выполненых чеках

        public Order Selected_Item_Product_Completed_CH
        {
            get { return selected_item_product_completed_CH; }
            set { selected_item_product_completed_CH = value; OnPropertyChanged("Selected_Item_Product_Completed_CH");
                
            }
        }


        

        #endregion

        #region go to completed


        private RelayCommand ready; // добавить новый чек

        public RelayCommand Ready
        {
            get
            {
                return ready ?? (ready = new RelayCommand(async (act) => 
                {
                    if (Selected_Item_Current_Cheсk == null)
                        return;
                    Completed_Cheсk temp_check = new Completed_Cheсk
                    {
                        Check_Id = Selected_Item_Current_Cheсk.Check_Id,
                        Check_Admin = Selected_Item_Current_Cheсk.Check_Admin,
                        Check_Date = new DateTime(Selected_Item_Current_Cheсk.Check_Date.Year,
                        Selected_Item_Current_Cheсk.Check_Date.Month,
                                            Selected_Item_Current_Cheсk.Check_Date.Day,
                                            Selected_Item_Current_Cheсk.Check_Date.Hour,
                                            Selected_Item_Current_Cheсk.Check_Date.Minute,
                                            Selected_Item_Current_Cheсk.Check_Date.Second),
                        Check_Final_Price = Selected_Item_Current_Cheсk.Check_Final_Price,
                        Check_User_Phone = Selected_Item_Current_Cheсk.Check_User_Phone
                    };
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        completed_CH_repository.Create(temp_check);
                        GC.Collect(GC.GetGeneration(temp_check));
                    }));


                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Coll_Product_Current_CH.ToList().ForEach(i =>
                    {
                        completed_order_repository.Create(new Order
                        {
                            Order_Discount = i.Order_Discount,
                            Order_Final_Price = i.Order_Final_Price,
                            Order_Price = i.Order_Price,
                            Order_Products_Name = i.Order_Products_Name,
                            Order_Chek_Id = i.Order_Chek_Id
                        });
                    });
                    }));

                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Coll_Product_Current_CH.ToList().ForEach(i => current_order_repository.Delete(i));
                    }));
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        current_CH_repository.Delete(Selected_Item_Current_Cheсk);
                    }));
                    InitializeComponent();
                }));
            }
        }

        #endregion

        #region go to current
        // NotReady
        private RelayCommand notready; // вернуть в текущие

        public RelayCommand NotReady
        {
            get
            {
                return notready ?? (notready = new RelayCommand(async (act) =>
                {
                    if (Selected_Item_Completed_CH == null)
                        return;
                    Current_Cheсk current_Cheсk = new Current_Cheсk
                    {
                        Check_Id = Selected_Item_Completed_CH.Check_Id,
                        Check_Admin = Selected_Item_Completed_CH.Check_Admin,
                        Check_Date = new DateTime(Selected_Item_Completed_CH.Check_Date.Year,
                        Selected_Item_Completed_CH.Check_Date.Month,
                                            Selected_Item_Completed_CH.Check_Date.Day,
                                            Selected_Item_Completed_CH.Check_Date.Hour,
                                            Selected_Item_Completed_CH.Check_Date.Minute,
                                            Selected_Item_Completed_CH.Check_Date.Second),

                        Check_Final_Price = Selected_Item_Completed_CH.Check_Final_Price,
                        Check_User_Phone = Selected_Item_Completed_CH.Check_User_Phone
                    };
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        current_CH_repository.Create(current_Cheсk);
                        GC.Collect(GC.GetGeneration(current_Cheсk));
                    }));
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Coll_Product_Completed_CH.ToList().ForEach(i =>
                        {
                             current_order_repository.Create(new Order
                           {
                              Order_Discount = i.Order_Discount,
                              Order_Final_Price = i.Order_Final_Price,
                              Order_Price = i.Order_Price,
                               Order_Products_Name = i.Order_Products_Name,
                              Order_Chek_Id = i.Order_Chek_Id
                           });
                         });
                    }));


                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        Coll_Product_Completed_CH.ToList().ForEach(i => completed_order_repository.Delete(i));
                    }));
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        completed_CH_repository.Delete(Selected_Item_Completed_CH);
                    }));
                    InitializeComponent();

                }));
            }
        }
        #endregion

        #region go to main

        private RelayCommand go_to_Main; // выход в меню
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
