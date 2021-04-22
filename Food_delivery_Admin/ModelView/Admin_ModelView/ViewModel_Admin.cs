using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Admin_View;
using Food_delivery_Admin.View.Category_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.Generic;
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

        public ObservableCollection<Current_Orders> Current_Orders { get; set; }
        private Current_Orders_Repository current_Orders_Repository = new Current_Orders_Repository();
        
       public ObservableCollection<Completed_Orders> Completed_Orders { get; set; }
        private Completed_Orders_Repository completed_Orders_Repository = new Completed_Orders_Repository();

        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();


        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository products_Repository = new Products_Repository();

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
           

            if (Products != null)
                Products.Clear();
            Products = new ObservableCollection<Product>(products_Repository.GetColl());
             OnPropertyChanged("Products");

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

        private Admin selected_Admin; // выбраный админ для списка

        public Admin Selected_Admin
        {
            get { return selected_Admin; }
            set { selected_Admin = value; OnPropertyChanged("Selected_Admin"); }
        }
             

        private string serch_srt_Admin; // строка поиска админа

        public string Serch_srt_Admin
        {
            get { return serch_srt_Admin; }
            set
            {
                serch_srt_Admin = value; OnPropertyChanged("Serch_srt");
                if (Admins != null)
                    GC.Collect(GC.GetGeneration(Admins));
                Admins = new ObservableCollection<Admin>(admin_repository.GetColl().ToList().FindAll(i => i.Admins_Surname.ToLower().Contains(serch_srt_Admin.ToLower())));
                OnPropertyChanged("Admins");

            }
        }
        #endregion

        #region comand

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
      

        #region new admin
        private RelayCommand new_admin; // открыть окно  с админами
        public RelayCommand New_admin
        {
            get { return new_admin ?? (new_admin = new RelayCommand(act => { new New_Admin().ShowDialog(); InitializeComponent(); })); }
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
                MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            admin_repository.Create(new Admin
            {
                Admins_Login = log,
                Admins_Name =pass,
                Admins_Password = name,
                Admins_Surname = surname
            });            
            window.Close();
            OnPropertyChanged("Admins");
        }
       

        #endregion

        #region edit admin
        private RelayCommand edit_admin; // изменение выбраного админа
        public RelayCommand Edit_admin
        {
            get
            {
                return edit_admin ?? (edit_admin = new RelayCommand(act =>
                {
                    try
                    {
                        admin_repository.Update(Selected_Admin);
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

        private RelayCommand dell_admin; // удаление выбраного админа
        public RelayCommand Dell_admin
        {
            get
            {
                return dell_admin ?? (dell_admin = new RelayCommand(act =>
                {
                    try
                    {
                        if (MessageBox.Show("Удалить администратора?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            return;
                        admin_repository.Delete(Selected_Admin);
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
