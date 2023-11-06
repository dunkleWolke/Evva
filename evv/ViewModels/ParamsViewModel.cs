using Evva.Commands;
using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Evva.ViewModels
{
    class ParamsViewModel : BaseViewModel
    {
        private List<string> categoryCollection;
        private decimal weight;
        private int height;
        private IEnumerable<Product> productsCollection;
        private bool nameTBEnabled;

        private Product product = new();


        public ParamsViewModel()
        {
            try
            { 
            ProductName = "";
            Fats = 1;
            Proteins = 1;
            Carbohydrates = 1;
            Calories = 1;
            nameTBEnabled = true;
            using (UnitOfWork unit = new UnitOfWork())
            {
                IEnumerable categories = unit.FoodCategoryRepository.Get();
                CategoryCollection = new List<string>();
                foreach (FoodCategory x in categories)
                {
                    CategoryCollection.Add(x.CategoryName);
                }
                List<Product> products = unit.ProductRepository.GetWithInclude(a => a.FoodCategoryNavigation, b => b.IdAddedNavigation, c => c.Reports).ToList();
                ProductsCollection = products.FindAll(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
            }
            if (categoryCollection != null)
            {
                SelectedCategory = categoryCollection.First();
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #region Properties
        
        public List<string> CategoryCollection
        {
            get { return categoryCollection; }
            set
            {
                categoryCollection = value;
                OnPropertyChanged("CategoryCollection");
            }
        }
        public bool NameTBEnabled
        {
            get { return nameTBEnabled; }
            set
            {
                nameTBEnabled = value;
                OnPropertyChanged("NameTBEnabled");
            }
        }
        public string SelectedCategory
        {
            get { return product.FoodCategory; }
            set
            {
                product.FoodCategory = value;
                OnPropertyChanged("SelectedCategory");
            }
        }
        public string ProductName
        {
            get { return product.ProductName; }
            set
            {
                product.ProductName = value;
                OnPropertyChanged("ProductName");
            }
        }
        public decimal Weight
        {
            get { return weight; }
            set
            {
                weight = value;
                OnPropertyChanged("Weight");
            }
        }
        public int Height
        {
            get { return height; }
            set
            {
                height = value;
                OnPropertyChanged("Height");
            }
        }
        public decimal Proteins
        {
            get { return product.ProteinsGram; }
            set
            {
                product.ProteinsGram = value;
                OnPropertyChanged("Proteins");
            }
        }
        public decimal Carbohydrates
        {
            get { return product.CarbohydratesGram; }
            set
            {
                product.CarbohydratesGram = value;
                OnPropertyChanged("Carbohydrates");
            }
        }
        public decimal Fats
        {
            get { return product.FatsGram; }
            set
            {
                product.FatsGram = value;
                OnPropertyChanged("Fats");
            }
        }
        public decimal  Calories
        {
            get { return product.CaloriesGram; }
            set
            {
                product.CaloriesGram = value;
                OnPropertyChanged("Calories");
            }
        }
        public IEnumerable<Product> ProductsCollection
        {
            get { return productsCollection; }
            set
            {
                productsCollection = value;
                OnPropertyChanged("ProductsCollection");
            }
        }
        public Product LastSelected
        {
            get { return product; }
            set
            {
                product = value;
                if (value != null)
                {
                    ProductName = value.ProductName;
                    Fats = value.FatsGram;
                    Proteins = value.ProteinsGram;
                    Carbohydrates = value.CarbohydratesGram;
                    Calories = value.CaloriesGram;
                    SelectedCategory = value.FoodCategory;
                }
                OnPropertyChanged("LastSelected");
            }
        }


        #endregion

        #region Commands



        #region Добавить продукт в коллекцию

        private DelegateCommand addProductToCollectionCommand;

        public ICommand AddProductToCollectionCommand
        {
            get 
            {
                if (addProductToCollectionCommand == null)
                {
                    addProductToCollectionCommand = new DelegateCommand(addProductToCollection, canAddProductToCollection);
                }
                return addProductToCollectionCommand;
            }
        }

        private void addProductToCollection()
        {    
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                product.IdAdded = DeserializedUser.deserializedUser.Id;
                product.FoodCategory = SelectedCategory;

                unit.ProductRepository.Create(product);
                unit.Save();

                List<Product> products = unit.ProductRepository.GetWithInclude(a => a.FoodCategoryNavigation, b => b.IdAddedNavigation, c => c.Reports).ToList();
                ProductsCollection = products.FindAll(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
            }
            LastSelected = new();
            ProductName = "";
            Fats = 1;
            Proteins = 1;
            Carbohydrates = 1;
            Calories = 1;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        private bool canAddProductToCollection()
        {
            try
            { 
            bool productExist = false;

            using (UnitOfWork unit = new UnitOfWork())
            {
                List<Product> productsList = unit.ProductRepository.Get().ToList();
                foreach (Product x in productsList)
                {
                    if (x.ProductName.ToLower() == LastSelected.ProductName.ToLower())
                    {
                        productExist = true;
                        break;
                    }
                }
            }
            if (ProductName == "" || !Regex.IsMatch(ProductName, "^([А-Я]|[а-я]|[A-Z]|[a-z]|[0-9]){1,199}$") || Calories == 0 || Proteins == 0 || Fats == 0 || Carbohydrates == 0 || SelectedCategory == "" || productExist || LastSelected.Id != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                return false;
            }
        }

        #endregion

        #region Удалить запись в таблице продуктов

        private DelegateCommand deleteProductRowCommand;

        public ICommand DeleteProductRowCommand
        {
            get
            {
                if (deleteProductRowCommand == null)
                {
                    deleteProductRowCommand = new DelegateCommand(deleteProductRow);
                }
                return deleteProductRowCommand;
            }
        }


        private void deleteProductRow()
        {
            try
            { 
            if (LastSelected != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    Product toDelete = unit.ProductRepository.GetWithInclude(x => x.ProductName == LastSelected.ProductName, a => a.FoodCategoryNavigation, b => b.IdAddedNavigation, c => c.Reports).First();
                    unit.ProductRepository.Remove(toDelete);
                    unit.Save();

                    List<Product> products = unit.ProductRepository.GetWithInclude(a => a.FoodCategoryNavigation, b => b.IdAddedNavigation, c => c.Reports).ToList();
                    ProductsCollection = products.FindAll(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
                }
                LastSelected = new();
                ProductName = "";
                Fats = 1;
                Proteins = 1;
                Carbohydrates = 1;
                Calories = 1;
            };
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Изменить запись в таблице продуктов

        private DelegateCommand editProductCommand;

        public ICommand EditProductCommand
        {
            get
            {
                if (editProductCommand == null)
                {
                    editProductCommand = new DelegateCommand(editProduct,canEditProduct);
                }
                return editProductCommand;
            }
        }


        private void editProduct()
        {
            try
            { 
            if (LastSelected != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {

                    unit.ProductRepository.Update(LastSelected);
                    unit.Save();

                    List<Product> products = unit.ProductRepository.GetWithInclude(a => a.FoodCategoryNavigation, b => b.IdAddedNavigation, c => c.Reports).ToList();
                    ProductsCollection = products.FindAll(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
                }
                LastSelected = new();
                ProductName = "";
                Fats = 1;
                Proteins = 1;
                Carbohydrates = 1;
                Calories = 1;
                SelectedCategory = "";
                NameTBEnabled = true;
            };
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        private bool canEditProduct()
        {
            try
            { 
            int productExist = 0;

            using (UnitOfWork unit = new UnitOfWork())
            {
                List<Product> productsList = unit.ProductRepository.Get().ToList();
                foreach (Product x in productsList)
                {
                    if (x.ProductName.ToLower() == ProductName.ToLower())
                    {
                        productExist++;
                    }
                }
            }
            if (ProductName == "" || Calories == 0 || Proteins == 0 || Fats == 0 || Carbohydrates == 0 || SelectedCategory == "" || productExist == 0 || LastSelected.Id == 0)
            {
                return false;
            }
            else
            {
                NameTBEnabled = false;
                return true;
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                return false;
            }
        }


        #endregion

        #region Изменить запись в таблице продуктов

        private DelegateCommand сlearProductCommand;

        public ICommand ClearProductCommand
        {
            get
            {
                if (сlearProductCommand == null)
                {
                    сlearProductCommand = new DelegateCommand(сlearProduct);
                }
                return сlearProductCommand;
            }
        }


        private void сlearProduct()
        {
            try 
            { 
                LastSelected = new();
                ProductName = "";
                Fats = 1;
                Proteins = 1;
                Carbohydrates = 1;
                Calories = 1;
                SelectedCategory = "";
                NameTBEnabled = true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion

        #endregion
    }
}

