using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Admin_View;
using Food_delivery_Admin.View.Category_View;
using Food_delivery_Admin.View.Check_View;
using Food_delivery_Admin.View.Products_View;
using Food_delivery_Admin.View.Users_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Food_delivery_Admin.ModelView
{
    public class ViewModel_Admin : INotifyPropertyChanged
    {
      
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();
                
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        #region init
        public ViewModel_Admin() { InitializeComponent(); }

        public void InitializeComponent()
        {
            if (Admins != null)
                Admins.Clear();
            Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
            OnPropertyChanged("Admins");
     
        }
        #endregion

        #region Admin

        #region full prop bind
        private static Admin curent_Admin; // админ который вошел

        public Admin Curent_Admin 
        {
            get { return curent_Admin; }
            set { curent_Admin = value; OnPropertyChanged("Curent_Admin"); }
        }

        private Admin selected_item; // выбраный админ для списка

        public Admin Selected_Item
        {
            get { return selected_item; }
            set { selected_item = value; OnPropertyChanged("Selected_Item"); }
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

      
      

        #region new admin
        private RelayCommand new_item; // открыть окно  с админами
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => { new New_Admin().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close();})); }
        }
        internal void Add_new(string log, string pass, string name, string surname, Window window) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить администартора?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (log == "" && pass == "" && name == "" && surname == "")
            { MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                admin_repository.Create(new Admin
            {
                Admins_Login = log,
                Admins_Name =name,
                Admins_Password = pass,
                Admins_Surname = surname
            });            
            window.Close();
            OnPropertyChanged("Admins");
        }
       

        #endregion

        #region edit admin
        private RelayCommand edit; // изменение выбраного админа
        public RelayCommand Edit
        {
            get
            {
                return edit ?? (edit = new RelayCommand(act =>
                {
                    try
                    {
                        admin_repository.Update(Selected_Item);
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
                        admin_repository.Delete(Selected_Item);
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


        #endregion

        #region login password  in



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

        #region go to admin main
        private RelayCommand go_to_Admins; // запуск окна настройки админов
        public RelayCommand Go_to_Admins
        {
            get
            {
                return go_to_Admins ?? (go_to_Admins = new RelayCommand(act => { new Main_Admin().Show(); ((Window)act).Close(); }));
            }
        }
        #endregion 

        #region go to categories
        private RelayCommand go_to_categories; // запуск окна настройки категорий
        public RelayCommand Go_to_Categories
        {
            get
            {
                return go_to_categories ?? (go_to_categories = new RelayCommand(act => { new Main_Categories().Show(); ((Window)act).Close(); }));
            }
        }
        #endregion

        #region go to products
        private RelayCommand go_to_products; // запуск окна настройки категорий
        public RelayCommand Go_to_Products
        {
            get
            {
                return go_to_products ?? (go_to_products = new RelayCommand(act => { new Main_Products().Show(); ((Window)act).Close(); }));
            }
        }
        #endregion

        #region go to Users

        private RelayCommand go_to_user; // запуск окна настройки Клиентов
        public RelayCommand Go_to_user
        {
            get
            {
                return go_to_user ?? (go_to_user = new RelayCommand(act => { new Main_User().Show(); ((Window)act).Close(); }));
            }
        }

        #endregion

        #region go to Orders
        private RelayCommand go_to_orders; // запуск окна управления  заказами
        public RelayCommand Go_to_Orders
        {
            get
            {
                return go_to_orders ?? (go_to_orders = new RelayCommand(act => { new Main_Check().Show(); ((Window)act).Close(); }));
            }
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
                        Curent_Admin = Admins.ToList().FirstOrDefault(i => i.Admins_Login == Temp_login);
                        OnPropertyChanged("Curent_Admin");
                        new Window_Main().Show();
                        ((Window)act).Close();
                       
                    }   
                    else
                        MessageBox.Show("Не верный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                }));
            }
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

        #region Exit
        private RelayCommand exit;
        public RelayCommand Exit
        {
            get
            {
                return exit ?? (exit = new RelayCommand(act =>
                 {
                      new Authorization_Window().Show();
                       ((Window)act).Close();
                 }));
            }
        }
        #endregion
    }
}
