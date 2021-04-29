using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Category_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Food_delivery_Admin.ModelView.Categories_Model_View
{
    class ViewModel_Categories : INotifyPropertyChanged
    {

        public ObservableCollection<Product_Categories> Product_categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        public ViewModel_Categories() { InitializeComponent(); }    
        public void InitializeComponent()
        {
            if (Product_categories != null)
                Product_categories.Clear();
            Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
            OnPropertyChanged("Product_Categories");
        }
        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        #region full prop bind
             
        private Product_Categories selected_item; // выбраный админ для списка

        public Product_Categories Selected_Item
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
                if (Product_categories != null)
                    GC.Collect(GC.GetGeneration(Product_categories));
                Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl().ToList().FindAll(i => i.Product_category_Name.ToLower().Contains(serch_str.ToLower())));
                OnPropertyChanged("Product_categories");

            }
        }
        #endregion

        #region new category
        private RelayCommand new_category; // открыть окно  с админами
        public RelayCommand New_category
        {
            get { return new_category ?? (new_category = new RelayCommand(act => { new New_Category().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close(); })); }
        }
        internal void Add_new(string category_name, Window window) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить категорию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (category_name == "")
            {  MessageBox.Show("Поле не заполнено", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
            poduct_Categories_Repository.Create(new Product_Categories { Product_category_Name = category_name });     
            window.Close();
            OnPropertyChanged("Product_categories");
            ViewModel_Admin.client_Host.SendMsg("1", "123");
        }


        #endregion

        #region edit category
        private RelayCommand edit; // изменение выбраного админа
        public RelayCommand Edit
        {
            get
            {
                return edit ?? (edit = new RelayCommand(act =>
                {
                    try
                    {
                        poduct_Categories_Repository.Update(Selected_Item);
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

        #region dell category

        private RelayCommand dell; // удаление выбраного админа
        public RelayCommand Dell
        {
            get
            {
                return dell ?? (dell = new RelayCommand(act =>
                {
                    try
                    {
                        if (MessageBox.Show("Удалить категорию?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                            return;
                        if (Selected_Item != null)
                            poduct_Categories_Repository.Delete(Selected_Item);
                        else
                            MessageBox.Show("Нужно выбрать что удалять", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);

                        
                        if (Product_categories != null)
                            GC.Collect(GC.GetGeneration(Product_categories));
                        Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());

                        OnPropertyChanged("Product_categories");
                        MessageBox.Show("Информация удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch(SqlException)
                    {
                        MessageBox.Show("Нельзя удалить категорию в которой есть продукты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    catch (Exception )
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
            get { return go_to_Main ?? (go_to_Main = new RelayCommand(act => { new Window_Main().Show(); ((Window)act).Close(); })); }           
        }

        #endregion
    }
}