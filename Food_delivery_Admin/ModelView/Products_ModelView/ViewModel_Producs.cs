using Food_delivery_Admin.View;
using Food_delivery_Admin.View.Products_View;
using Food_delivery_Admin.View.ViewModel;
using Food_delivery_library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Food_delivery_Admin.ModelView.Products_ModelView
{
    public class ViewModel_Producs : INotifyPropertyChanged
    {
        public ObservableCollection<Product> Products { get; set; }
        private Products_Repository products_Repository = new Products_Repository();

        public ObservableCollection<Product_Categories> Product_categories { get; set; }
        private Product_Categories_Repository poduct_Categories_Repository = new Product_Categories_Repository();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged; // ивент обновления
        public void OnPropertyChanged([CallerMemberName] string prop = "")
           => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        #endregion

        #region init
        public ViewModel_Producs() { InitializeComponent(); }

        public void InitializeComponent()
        {
            if (Products != null)
                Products.Clear();
            Products = new ObservableCollection<Product>(products_Repository.GetColl());
            OnPropertyChanged("Products");

            if (Product_categories != null)
                Product_categories.Clear();
            Product_categories = new ObservableCollection<Product_Categories>(poduct_Categories_Repository.GetColl());
            OnPropertyChanged("Product_Categories");
        }
        #endregion

       

        #region full prop bind
       

        private Product selected_item; // выбраный продукт для списка

        public Product Selected_Item
        {
            get { return selected_item; }
            set { selected_item = value; OnPropertyChanged("Selected_Item");
                Selected_categories = selected_item.Product_category;
            }
        }

        private Product_Categories selected_categories; // выбраный админ для списка

        public Product_Categories Selected_categories
        {
            get { return selected_categories; }
            set { selected_categories = value; OnPropertyChanged("Selected_categories"); }
        }

        private string serch_str; // строка поиска 

        public string Serch_str
        {
            get { return serch_str; }
            set
            {
                serch_str = value; OnPropertyChanged("Serch_srt");
                if (Products != null)
                    GC.Collect(GC.GetGeneration(Products));
                Products = new ObservableCollection<Product>(products_Repository.GetColl().ToList().FindAll(i => i.Product_Name.ToLower().Contains(serch_str.ToLower())));
                OnPropertyChanged("Products");

            }
        }
        #endregion

        #region comand


        #region new window
        private RelayCommand new_item; // открыть окно  с админами
        public RelayCommand New_item
        {
            get { return new_item ?? (new_item = new RelayCommand(act => { new New_Products().ShowDialog(); InitializeComponent(); })); }
        }
        private RelayCommand cansel_new; // отмена  создания нового админа
        public RelayCommand Cansel_new
        {
            get { return cansel_new ?? (cansel_new = new RelayCommand(act => { (act as Window).Close(); })); }
        }
        internal void Add_new(string name, string discount, string price, Product_Categories categories, Window window) //кнопка добавления нового админа
        {
            if (MessageBox.Show("Добавить продукт?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            if (name == "" && discount == "" && price == "" && categories == null)
                MessageBox.Show("Не все поля заполнены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            products_Repository.Create(new Product
            {
                Product_Name = name,
                Product_Discount = Convert.ToDouble(discount.Replace('.',',')),
                Product_Price = Convert.ToDouble(price.Replace('.', ',')),
                 Product_category = categories,
                 Product_category_Id = categories.Product_category_Id
            }) ;
            window.Close();
            OnPropertyChanged("Products");
        }


        #endregion

        #region edit 
        private RelayCommand edit; // изменение выбраного 
        public RelayCommand Edit
        {
            get
            {
                return edit ?? (edit = new RelayCommand(act =>
                {
                    try
                    {
                        products_Repository.Update(Selected_Item);
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

        #region dell 

        private RelayCommand dell; // удаление выбраного 
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
                        products_Repository.Delete(Selected_Item);
                        if (Products != null)
                            GC.Collect(GC.GetGeneration(Products));
                        Products = new ObservableCollection<Product>(products_Repository.GetColl());

                        OnPropertyChanged("Products");
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
