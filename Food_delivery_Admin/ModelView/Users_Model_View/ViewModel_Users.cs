using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Users_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace Food_delivery_Admin.ModelView.Users_Model_View
{
    public class ViewModel_Users : INotifyPropertyChanged
    {
        public ObservableCollection<User> Users { get; set; }
        private User_Repository user_repository = new User_Repository();

        public ObservableCollection<Blocked_user> Blocked_Users { get; set; }
        private Blocked_users_Repository blocked_users_repository = new Blocked_users_Repository();

        #region init
        public ViewModel_Users() { InitializeComponent(); }

        public async void InitializeComponent()
        {
            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Users != null)
                    Users.Clear();
                Users = new ObservableCollection<User>(user_repository.GetColl());
                OnPropertyChanged("Users");
            }));
            await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
            {
                if (Blocked_Users != null)
                    Blocked_Users.Clear();
                Blocked_Users = new ObservableCollection<Blocked_user>(blocked_users_repository.GetColl());
                OnPropertyChanged("Blocked_Users");
            }));

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

        private Blocked_user selected_item_block; // выбраный клиент для списка

        public Blocked_user Selected_Item_Block
        {
            get { return selected_item_block; }
            set { selected_item_block = value; OnPropertyChanged("Selected_Item_Block"); }
        }


        private string serch_str; // строка поиска клиента

        public string Serch_str
        {
            get { return serch_str; }
            set
            {
                serch_str = value; OnPropertyChanged("Serch_str");
                if (Users != null)
                    GC.Collect(GC.GetGeneration(Users));
                Users = new ObservableCollection<User>(user_repository.GetColl().ToList().FindAll(i => i.User_Phone.ToLower().Contains(serch_str.ToLower())));
                OnPropertyChanged("Users");

            }
        }

        private string serch_str_block; // строка поиска клиента

        public string Serch_str_Block
        {
            get { return serch_str_block; }
            set
            {
                serch_str_block = value; OnPropertyChanged("Serch_str_Block");
                if (Blocked_Users != null)
                    GC.Collect(GC.GetGeneration(Blocked_Users));
                Blocked_Users = new ObservableCollection<Blocked_user>(blocked_users_repository.GetColl().ToList().FindAll(i => i.Blocked_user_Phone.ToLower().Contains(serch_str_block.ToLower())));
                OnPropertyChanged("Blocked_Users");

            }
        }
        #endregion

        #region new user

        private RelayCommand block; // открыть окно  с админами
        public RelayCommand Block
        {
            get { return block ?? (block = new RelayCommand(async (act) =>
            {
                if (Selected_Item != null)
                {
                    Blocked_user temp = new Blocked_user
                    {
                        Blocked_user_Name = Selected_Item.User_Name,
                        Blocked_user_Phone = Selected_Item.User_Phone,
                        Blocked_user_Email = Selected_Item.User_Email,
                        Blocked_user_Bank_card = Selected_Item.User_Bank_card,
                        Blocked_user_Surname = Selected_Item.User_Surname
                    };
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        blocked_users_repository.Create(temp);
                        GC.Collect(GC.GetGeneration(temp));
                    }));
                    await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        user_repository.Delete(Selected_Item);
                    }));
                }
                else
                    MessageBox.Show("Вы не выбрали пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                InitializeComponent();
            })); }
        }

      //  private async 
        private RelayCommand unblock; // открыть окно  с админами
        public RelayCommand UnBlock
        {
            get
            {
                return unblock ?? (unblock = new RelayCommand(async(act) =>
                {
                    if (Selected_Item_Block != null)
                    {
                        User temp = new User
                        {
                            User_Name = Selected_Item_Block.Blocked_user_Name,
                            User_Phone = Selected_Item_Block.Blocked_user_Phone,
                            User_Email = Selected_Item_Block.Blocked_user_Email,
                            User_Bank_card = Selected_Item_Block.Blocked_user_Bank_card,
                            User_Surname = Selected_Item_Block.Blocked_user_Surname
                        };
                        await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            user_repository.Create(temp);
                            GC.Collect(GC.GetGeneration(temp));
                        }));
                        await Task.Run(() => App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            blocked_users_repository.Delete(Selected_Item_Block);
                        }));
                    }
                    else
                        MessageBox.Show("Вы не выбрали пользователя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    InitializeComponent();
                }));
            }
        }


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
            if (phone == "" || e_mail == "" || name == "" || surname == "")
            { MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
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
                        if (MessageBox.Show("Удалить клиента?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            return;
                        if (Selected_Item != null)
                            user_repository.Delete(Selected_Item);
                        else
                            MessageBox.Show("Нужно кого что удалять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);            
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
