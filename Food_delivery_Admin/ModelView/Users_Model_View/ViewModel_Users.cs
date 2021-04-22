using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Users_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Food_delivery_Admin.ModelView.Users_Model_View
{
    public class ViewModel_Users : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();

        #region init
        public ViewModel_Users() { InitializeComponent(); }

        public void InitializeComponent()
        {
           
            if (Users != null)
                Users.Clear();
            Users = new ObservableCollection<User>(user_repository.GetColl());
            OnPropertyChanged("Users");         

        }
        #endregion

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion


        #region full prop bind
        private User selected_item; // выбраный клиент для списка

        public User Selected_Item
        {
            get { return selected_item; }
            set { selected_item = value; OnPropertyChanged("Selected_Item"); }
        }


        private string serch_srt; // строка поиска клиента

        public string Serch_srt
        {
            get { return serch_srt; }
            set
            {
                serch_srt = value; OnPropertyChanged("Serch_srt");
                if (Users != null)
                    GC.Collect(GC.GetGeneration(Users));
                Users = new ObservableCollection<User>(user_repository.GetColl().ToList().FindAll(i => i.User_Surname.ToLower().Contains(serch_srt.ToLower())));
                OnPropertyChanged("Users");

            }
        }
        #endregion

        #region new user
        private RelayCommand new_item; // открыть окно  с админами
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => { new New_User().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close(); })); }
        }
        internal void Add_new( string name, string surname,string phone, string e_mail, Window window, string bank) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (phone == "" && e_mail == "" && name == "" && surname == "")
                MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            user_repository.Create(new User
            {
               User_Name =name,
               User_Surname = surname,
               User_Phone = phone,
               User_Email = e_mail,
               User_Bank_card = bank
               
               
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
                        user_repository.Update(Selected_Item);
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
                        user_repository.Delete(Selected_Item);
                        if (Users != null)
                            GC.Collect(GC.GetGeneration(Users));
                        Users = new ObservableCollection<User>(user_repository.GetColl());

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
