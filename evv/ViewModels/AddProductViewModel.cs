using Evva.Context.UnitOfWork;
using Evva.Commands;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Text.RegularExpressions;
using Evva.Models;
using System.Windows;
using Evva.DeserializedUserNamespace;

namespace Evva.ViewModels
{
    public class AddProductViewModel : BaseViewModel
    {
        private const decimal V = 0.01M;
        private string searchText;
        private IEnumerable collectionOfProducts;
        private Product selectedProduct;
        private DateTime dateToChoose;
        public string lastChoosenCategory;

        private string stateOfSearch;
        private Report report;
        private bool foodCategoryCheck;

        private string selectedCategory;

        public string[] foodCategories;

        private List<string> categoryCollection;
        
        private IEnumerable<Product> productsCollection;
        private bool nameTBEnabled;

        private Product product = new();


        public AddProductViewModel()
        {
            try
            { 
            report = new Report();
            report.IdReport = DeserializedUser.deserializedUser.Id;
            SearchText = "";
            using (UnitOfWork unit = new UnitOfWork())
            {
                IEnumerable products = unit.ProductRepository.Get();
                CollectionOfProducts = products;
                FoodCategories = unit.FoodCategoryRepository.Get().Select(x=>x.CategoryName).ToArray();
            }
            DateToChoose = DateTime.Now;
            GramValue = default;
            SelectedProduct = default;
            SelectedPeriod = "Завтрак";
            lastChoosenCategory = "Завтрак";
            StateOfSearch = "All";
            FoodCategoryCheck = false;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
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
        
        public string ProductName
        {
            get { return product.ProductName; }
            set
            {
                product.ProductName = value;
                OnPropertyChanged("ProductName");
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
        public decimal Calories
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

        public string SelectedCategory
        {
            get { return selectedCategory; }
            set
            {
                selectedCategory = value;
                OnPropertyChanged("SelectedCategory");
                findProducts();
            }
        }

        public string[] FoodCategories
        {
            get { return foodCategories; }
            set
            {
                foodCategories = value;
                OnPropertyChanged("FoodCategories");
            }
        }


        public bool FoodCategoryCheck
        {
            get { return foodCategoryCheck; }
            set
            {
                foodCategoryCheck = value;
                OnPropertyChanged("FoodCategoryCheck");
            }
        }

        public string StateOfSearch
        {
            get { return stateOfSearch; }
            set
            {
                stateOfSearch = value;
                OnPropertyChanged("GramValue");
            }
        }

        public decimal GramValue
        {
            get { return report.DayGram; }
            set
            {
                report.DayGram = value;
                OnPropertyChanged("GramValue");
            }
        }

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                OnPropertyChanged("SearchText");
            }
        }


        public IEnumerable CollectionOfProducts
        {
            get { return collectionOfProducts; }
            set
            {
                collectionOfProducts = value;
                OnPropertyChanged("CollectionOfProducts");
            }
        }
        
        public DateTime DateToChoose
        {
            get { return dateToChoose; }
            set
            {
                dateToChoose = value;
                OnPropertyChanged("DateToChoose");
            }
        }

        public string SelectedPeriod
        {
            get { return report.EatPeriod; }
            set
            {
                lastChoosenCategory = value;
                report.EatPeriod = value;
                OnPropertyChanged("SelectedPeriod");
            }
        }

        #endregion

        #region Commands

        #region Показать продукты пользователя

        private DelegateCommand showUserProductsCommand;

        public ICommand ShowUserProductsCommand
        {
            get
            {
                if (showUserProductsCommand == null)
                {
                    showUserProductsCommand = new DelegateCommand(showUserProducts);
                }
                return showUserProductsCommand;
            }
        }

        private void showUserProducts()
        {
            try
            {
                StateOfSearch = "User";
                using (UnitOfWork unit = new UnitOfWork())
                {
                    if (FoodCategoryCheck)
                    {
                        if (SearchText != "")
                        {
                            IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && Regex.IsMatch(x.ProductName, "^" + SearchText) && x.FoodCategory == SelectedCategory);
                            CollectionOfProducts = products;

                        }
                        else if (SearchText == "")
                        {
                            IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && x.FoodCategory == SelectedCategory);
                            CollectionOfProducts = products;
                        }
                    }
                    else
                    {
                        if (SearchText != "")
                        {
                            IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && Regex.IsMatch(x.ProductName, "^" + SearchText));
                            CollectionOfProducts = products;

                        }
                        else if (SearchText == "")
                        {
                            IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
                            CollectionOfProducts = products;
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion
        #region Показать все продукты

        private DelegateCommand showAllProductsCommand;

        public ICommand ShowAllProductsCommand
        {
            get
            {
                if (showAllProductsCommand == null)
                {
                    showAllProductsCommand = new DelegateCommand(showAllProducts);
                }
                return showAllProductsCommand;
            }
        }

        private void showAllProducts()
        {
            try
            { 
            StateOfSearch = "All";
            using (UnitOfWork unit = new UnitOfWork())
            {
                if (FoodCategoryCheck)
                {
                    if (SearchText != "")
                    {
                        IEnumerable products = unit.ProductRepository.Get(x => Regex.IsMatch(x.ProductName, "^" + SearchText ) && x.FoodCategory == SelectedCategory);
                        CollectionOfProducts = products;

                    }
                    else if (SearchText == "")
                    {
                        IEnumerable products = unit.ProductRepository.Get(x => x.FoodCategory == SelectedCategory);
                        CollectionOfProducts = products;
                    }
                }
                else
                {
                    if (SearchText != "")
                    {
                        IEnumerable products = unit.ProductRepository.Get(x => Regex.IsMatch(x.ProductName, "^" + SearchText ));
                        CollectionOfProducts = products;

                    }
                    else if (SearchText == "")
                    {
                        IEnumerable products = unit.ProductRepository.Get();
                        CollectionOfProducts = products;
                    }
                }
            }
             }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
}

        #endregion

        #region Добавить продукт

        private DelegateCommand addProductCommand;

        public ICommand AddProductCommand
        {
            get
            {
                if (addProductCommand == null)
                {
                    addProductCommand = new DelegateCommand(addProduct,canAddProduct);
                }
                return addProductCommand;
            }
        }

        private void addProduct()
        {
            try
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    report.DayGram = GramValue;

                    report.DayCarbohydrates = SelectedProduct.CarbohydratesGram * GramValue * V % 100000;
                    report.DayCalories = SelectedProduct.CaloriesGram * GramValue * V % 100000;
                    report.DayFats = SelectedProduct.FatsGram * GramValue * V % 100000;
                    report.DayProteins = SelectedProduct.ProteinsGram * GramValue * V % 100000;

                    report.ProductName = SelectedProduct.ProductName;
                    report.ReportDate = DateToChoose;
                    report.MostCategory = SelectedProduct.FoodCategory;

                    unit.ReportRepository.Create(report);
                    unit.Save();

                    report = new Report();
                    report.IdReport = DeserializedUser.deserializedUser.Id;
                    GramValue = default;
                    SelectedProduct = default;
                    SelectedPeriod = lastChoosenCategory;
                    StateOfSearch = "All";
                    FoodCategoryCheck = false;
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        private bool canAddProduct()
        {
            try
            {
                if (SelectedProduct == null || GramValue == 0 || DateToChoose.Date.CompareTo(DateTime.Now.Date) > 0)
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

        #region Увеличить дату на день

        private DelegateCommand addDayCommand;

        public ICommand AddDayCommand
        {
            get
            {
                if (addDayCommand == null)
                {
                    addDayCommand = new DelegateCommand(addDay);
                }
                return addDayCommand;
            }
        }

        private void addDay()
        {
            try
            {
                DateToChoose = DateToChoose.AddDays(1);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion
        #region Уменьшить дату на день

        private DelegateCommand removeDayCommand;

        public ICommand RemoveDayCommand
        {
            get
            {
                if (removeDayCommand == null)
                {
                    removeDayCommand = new DelegateCommand(removeDay);
                }
                return removeDayCommand;
            }
        }

        private void removeDay()
        {
            try
            {
                DateToChoose = DateToChoose.AddDays(-1);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Поиск

        private DelegateCommand searchCommand;

        public ICommand SearchCommand
        {
            get
            {
                if (searchCommand == null)
                {
                    searchCommand = new DelegateCommand(search);
                }
                return searchCommand;
            }
        }

        private void search()
        {
                findProducts();
        }


        #endregion

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
                    editProductCommand = new DelegateCommand(editProduct, canEditProduct);
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
        #region Public methods

        public void findProducts()
        {
            try
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    if (SearchText != null)
                    {
                        if (StateOfSearch == "All")
                        {
                            if (FoodCategoryCheck)
                            {
                                if (SearchText != "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => Regex.IsMatch(x.ProductName, "^" + SearchText) && x.FoodCategory == SelectedCategory);
                                    CollectionOfProducts = products;

                                }
                                else if (SearchText == "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => x.FoodCategory == SelectedCategory);
                                    CollectionOfProducts = products;
                                }
                            }
                            else
                            {
                                if (SearchText != "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => Regex.IsMatch(x.ProductName, "^" + SearchText));
                                    CollectionOfProducts = products;

                                }
                                else if (SearchText == "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get();
                                    CollectionOfProducts = products;
                                }
                            }
                        }
                        else if (StateOfSearch == "User")
                        {
                            if (FoodCategoryCheck)
                            {
                                if (SearchText != "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && Regex.IsMatch(x.ProductName, "^" + SearchText) && x.FoodCategory == SelectedCategory);
                                    CollectionOfProducts = products;

                                }
                                else if (SearchText == "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && x.FoodCategory == SelectedCategory);
                                    CollectionOfProducts = products;
                                }
                            }
                            else
                            {
                                if (SearchText != "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id && Regex.IsMatch(x.ProductName, "^" + SearchText));
                                    CollectionOfProducts = products;

                                }
                                else if (SearchText == "")
                                {
                                    IEnumerable products = unit.ProductRepository.Get(x => x.IdAdded == DeserializedUser.deserializedUser.Id);
                                    CollectionOfProducts = products;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        #endregion
        #endregion
    }
}
