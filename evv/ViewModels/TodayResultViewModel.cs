using Evva.Commands;
using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using Evva.XMLSerializer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Evva.ViewModels
{
    class TodayResultViewModel : BaseViewModel
    {
        private int caloriesPercent;
        private int proteinsPercent;
        private int fatsPercent;
        private int carbohydratesPercent;

        private decimal caloriesNeeded;
        private decimal proteinsNeeded;
        private decimal fatsNeeded;
        private decimal carbohydratesNeeded;

        private decimal caloriesEaten;
        private decimal proteinsEaten;
        private decimal fatsEaten;
        private decimal carbohydratesEaten;

        private string lastReport;
        private DateTime dateToChoose;

        private decimal userWeight;
        private int userHeight;
        private string productName;


        public TodayResultViewModel()
        {
            try
            { 
            DateToChoose = DateTime.Today.Date;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #region Properties

        public int ProteinsPercent
        {
            get { return proteinsPercent; }
            set
            {
                proteinsPercent = value;
                OnPropertyChanged("ProteinsPercent");
            }
        }
        public int FatsPercent
        {
            get { return fatsPercent; }
            set
            {
                fatsPercent = value;
                OnPropertyChanged("FatsPercent");
            }
        }
        public int CarbohydratesPercent
        {
            get { return carbohydratesPercent; }
            set
            {
                carbohydratesPercent = value;
                OnPropertyChanged("CarbohydratesPercent");
            }
        }
        public int CaloriesPercent
        {
            get { return caloriesPercent; }
            set
            {
                caloriesPercent = value;
                OnPropertyChanged("CaloriesPercent");
            }
        }

        public decimal CaloriesNeeded
        {
            get { return caloriesNeeded; }
            set
            {
                caloriesNeeded = value;
                OnPropertyChanged("CaloriesNeeded");
            }
        }
        public decimal ProteinsNeeded
        {
            get { return proteinsNeeded; }
            set
            {
                proteinsNeeded = value;
                OnPropertyChanged("ProteinsNeeded");
            }
        }
        public decimal FatsNeeded
        {
            get { return fatsNeeded; }
            set
            {
                fatsNeeded = value;
                OnPropertyChanged("FatsNeeded");
            }
        }
        public decimal CarbohydratesNeeded
        {
            get { return carbohydratesNeeded; }
            set
            {
                carbohydratesNeeded = value;
                OnPropertyChanged("CarbohydratesNeeded");
            }
        }

        public decimal CaloriesEaten
        {
            get { return caloriesEaten; }
            set
            {
                caloriesEaten = value;
                OnPropertyChanged("CaloriesEaten");
            }
        }
        public decimal ProteinsEaten
        {
            get { return proteinsEaten; }
            set
            {
                proteinsEaten = value;
                OnPropertyChanged("ProteinsEaten");
            }
        }
        public decimal FatsEaten
        {
            get { return fatsEaten; }
            set
            {
                fatsEaten = value;
                OnPropertyChanged("FatsEaten");
            }
        }
        public decimal CarbohydratesEaten
        {
            get { return carbohydratesEaten; }
            set
            {
                carbohydratesEaten = value;
                OnPropertyChanged("CarbohydratesEaten");
            }
        }

        public DateTime DateToChoose
        {
            get { return dateToChoose; }
            set
            {
                dateToChoose = value;
                OnPropertyChanged("DateToChoose");
                refreshStatistic();
            }
        }
        public string LastReport
        {
            get { return lastReport; }
            set
            {
                lastReport = value;
                OnPropertyChanged("LastReport");
            }
        }
        
        public decimal UserWeight
        {
            get { return userWeight; }
            set
            {
                userWeight = value;
                OnPropertyChanged("UserWeight");
            }
        }
        public int UserHeight
        {
            get { return userHeight; }
            set
            {
                userHeight = value;
                OnPropertyChanged("UserHeight");
            }
        }
        public string ProductName
        {
            get { return productName; }
            set
            {
                productName = value;
                OnPropertyChanged("ProductName");
            }
        }

        #endregion

        #region Commands
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
            refreshStatistic();
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
            refreshStatistic();
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Открытые методы

        public void refreshStatistic()
        {
            if (DateToChoose.Date.CompareTo(DateTime.Today.Date) != 1)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UsersParam userParam = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id && x.ParamsDate.Date.Equals(DateToChoose.Date)).LastOrDefault();
                    UsersDatum usersDatum = unit.UserDatumRepository.Get(x => x.IdData == DeserializedUser.deserializedUser.Id ).LastOrDefault();
                    List<Report> reports = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id && DateToChoose.Equals(x.ReportDate.Date));
                    List<Report> productReportName = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id && DateToChoose.Date.Date.Equals(x.ReportDate.Date));
                    List<Report> lastReports = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id);

                    if (lastReports.Capacity != 0)
                    {
                        LastReport = lastReports.Last().ReportDate.ToString();
                    }
                    else
                    {
                        LastReport = "---";
                    }

                    if (productReportName.Capacity != 0)
                    {
                        ProductName = productReportName.GroupBy(i => i.ProductName).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                    }
                    else
                    {
                        ProductName = "---";
                    }

                    if (userParam != null)
                    {
                        CaloriesNeeded = Math.Round((10M * userParam.UserWeight) + (6.25M * userParam.UserHeight) - (5 * (DateTime.Now.Year - usersDatum.Birthday.Year)) + 78);
                        UserWeight = userParam.UserWeight;
                        UserHeight = userParam.UserHeight;
                    }
                    else
                    {
                        UsersParam userParamChanged = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id ).LastOrDefault();                       
                        CaloriesNeeded = Math.Round((10M * userParamChanged.UserWeight) + (6.25M * userParamChanged.UserHeight) - (5 * (DateTime.Now.Year - usersDatum.Birthday.Year)) + 78);
                        UserWeight = userParamChanged.UserWeight;
                        UserHeight = userParamChanged.UserHeight;
                    }
                    ProteinsNeeded = Math.Round(CaloriesNeeded * 0.3M / 4);
                    FatsNeeded = Math.Round(CaloriesNeeded * 0.2M / 9);
                    CarbohydratesNeeded = Math.Round(CaloriesNeeded * 0.5M / 4);

                    if(ProteinsNeeded <= 1)
                    {
                        ProteinsNeeded = 1;
                    }
                    if (FatsNeeded <= 1)
                    {
                        FatsNeeded = 1;
                    }
                    if (CarbohydratesNeeded <= 1)
                    {
                        CarbohydratesNeeded = 1;
                    }
                    if (CaloriesNeeded <= 1)
                    {
                        CaloriesNeeded = 1;
                    }

                    CaloriesEaten = Math.Round(reports.Sum(x => x.DayCalories), 2) % 100000;
                    ProteinsEaten = Math.Round(reports.Sum(x => x.DayProteins), 2) % 100000;
                    FatsEaten = Math.Round(reports.Sum(x => x.DayFats), 2) % 100000;
                    CarbohydratesEaten = Math.Round(reports.Sum(x => x.DayCarbohydrates), 2) % 100000;

                    if ((int)(CaloriesEaten / (CaloriesNeeded / 100)) > 100)
                    {
                        CaloriesPercent = 100;
                    }
                    else
                    {
                        CaloriesPercent = (int)(CaloriesEaten / (CaloriesNeeded / 100));
                    }

                    if ((int)(ProteinsEaten / (ProteinsNeeded / 100)) > 100)
                    {
                        ProteinsPercent = 100;
                    }
                    else
                    {
                        ProteinsPercent = (int)(ProteinsEaten / (ProteinsNeeded / 100));
                    }

                    if ((int)(FatsEaten / (FatsNeeded / 100)) > 100)
                    {
                        FatsPercent = 100;
                    }
                    else
                    {
                        FatsPercent = (int)(FatsEaten / (FatsNeeded / 100));
                    }

                    if ((int)(CarbohydratesEaten / (CarbohydratesNeeded / 100)) > 100)
                    {
                        CarbohydratesPercent = 100;
                    }
                    else
                    {
                        CarbohydratesPercent = (int)(CarbohydratesEaten / (CarbohydratesNeeded / 100));
                    }

                }
            }
            else
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UsersParam userParam = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id && x.ParamsDate.Date.Equals(DateTime.Today.Date)).LastOrDefault();
                    UsersDatum usersDatum = unit.UserDatumRepository.Get(x => x.IdData == DeserializedUser.deserializedUser.Id).LastOrDefault();
                    List<Report> reports = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id && DateToChoose.Equals(x.ReportDate.Date));
                    List<Report> lastReports = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id);

                    if (lastReports.Capacity != 0)
                    {
                        LastReport = lastReports.Last().ReportDate.ToString();
                    }
                    else
                    {
                        LastReport = "---";
                    }
                        ProductName = "---";
                    if (userParam != null)
                    {
                        CaloriesNeeded = Math.Round((10M * userParam.UserWeight) + (6.25M * userParam.UserHeight) - (5 * (DateTime.Now.Year - usersDatum.Birthday.Year)) + 78);
                        UserWeight = userParam.UserWeight;
                        UserHeight = userParam.UserHeight;
                    }
                    else
                    {
                        UsersParam userParamChanged = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id).LastOrDefault();
                        CaloriesNeeded = Math.Round((10M * userParamChanged.UserWeight) + (6.25M * userParamChanged.UserHeight) - (5 * (DateTime.Now.Year - usersDatum.Birthday.Year)) + 78);
                        UserWeight = userParamChanged.UserWeight;
                        UserHeight = userParamChanged.UserHeight;
                    }
                    ProteinsNeeded = Math.Round(CaloriesNeeded * 0.3M / 4);
                    FatsNeeded = Math.Round(CaloriesNeeded * 0.2M / 9);
                    CarbohydratesNeeded = Math.Round(CaloriesNeeded * 0.5M / 4);

                    if (ProteinsNeeded <= 1)
                    {
                        ProteinsNeeded = 1;
                    }
                    if (FatsNeeded <= 1)
                    {
                        FatsNeeded = 1;
                    }
                    if (CarbohydratesNeeded <= 1)
                    {
                        CarbohydratesNeeded = 1;
                    }
                    if (CaloriesNeeded <= 1)
                    {
                        CaloriesNeeded = 1;
                    }

                    CaloriesEaten = Math.Round(reports.Sum(x => x.DayCalories), 2) % 100000;
                    ProteinsEaten = Math.Round(reports.Sum(x => x.DayProteins), 2) % 100000;
                    FatsEaten = Math.Round(reports.Sum(x => x.DayFats), 2) % 100000;
                    CarbohydratesEaten = Math.Round(reports.Sum(x => x.DayCarbohydrates), 2) % 100000;

                    CaloriesPercent = (int)(CaloriesEaten / (CaloriesNeeded / 100));
                    ProteinsPercent = (int)(ProteinsEaten / (ProteinsNeeded / 100));
                    FatsPercent = (int)(FatsEaten / (FatsNeeded / 100));
                    CarbohydratesPercent = (int)(CarbohydratesEaten / (CarbohydratesNeeded / 100));
                }
            }

        }

        #endregion

        #endregion

    }
}
