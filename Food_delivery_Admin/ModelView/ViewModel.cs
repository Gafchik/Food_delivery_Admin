using Dapper;
using Food_delivery_Admin.Properties;
using Food_delivery_Admin.View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Food_delivery_Admin.ModelView
{
    public class ViewModel : INotifyPropertyChanged
    {
      
        public ObservableCollection<Admin> Admins { get; set; }
        private Admins_Repository admin_repository = new Admins_Repository();
     
        
      

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

       
        public ViewModel()
        {
            Admins = new ObservableCollection<Admin>(admin_repository.GetColl());
         
        }
     
       


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

