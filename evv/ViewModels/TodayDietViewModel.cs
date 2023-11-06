using Evva.Context.UnitOfWork;
using Evva.Commands;
using Evva.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using Evva.Views.Windows;
using Evva.DeserializedUserNamespace;

namespace Evva.ViewModels
{
    public class TodayDietViewModel : BaseViewModel
    {
        private IEnumerable<Report> tableToShow;
        private DateTime dateToChoose;
        private Report lastSelected;
        public ICommand UpdateViewCommand { get; set; }

        public TodayDietViewModel()
        {
            try
            { 
            DateToChoose = DateTime.Today;
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
            tableChangeToBreakfast();
        }

        #region Properties

       

        public string LastSelectedTable { get; set; }

        public Report LastSelected
        {
            get { return lastSelected; }
            set
            {
                lastSelected = value;
                OnPropertyChanged("LastSelected");
            }
        }

        public IEnumerable<Report> TableToShow
        {
            get { return tableToShow; }
            set
            {
                tableToShow = value;
                OnPropertyChanged("TableToShow");
            }
        }

        public DateTime DateToChoose
        {
            get { return dateToChoose; }
            set
            {
                dateToChoose = value;
                OnPropertyChanged("DateToChoose");
                try
                { 
                refreshTable();
                }
                catch (Exception exception)
                {
                    System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                }
            }
        }


        #endregion

        #region Commands

        #region Изменить таблицу "завтрак"

        private DelegateCommand tableChangeToBreakfastCommand;

        public ICommand TableChangeToBreakfastCommand
        {
            get
            {
                if (tableChangeToBreakfastCommand == null)
                {
                    tableChangeToBreakfastCommand = new DelegateCommand(tableChangeToBreakfast);
                }
                return tableChangeToBreakfastCommand;
            }
        }

        private void tableChangeToBreakfast()
        {
            try { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == "Завтрак");
                TableToShow = reports;
                LastSelectedTable = "Завтрак";
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }

        }

        #endregion
        #region Изменить таблицу "ланч"

        private DelegateCommand tableChangeToLunchCommand;

        public ICommand TableChangeToLunchCommand
        {
            get
            {
                if (tableChangeToLunchCommand == null)
                {
                    tableChangeToLunchCommand = new DelegateCommand(tableChangeToLunch);
                }
                return tableChangeToLunchCommand;
            }
        }

        private void tableChangeToLunch()
        {
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == "Ланч");
                TableToShow = reports;
                LastSelectedTable = "Ланч";
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion
        #region Изменить таблицу "обед"

        private DelegateCommand tableChangeToDinnerCommand;

        public ICommand TableChangeToDinnerCommand
        {
            get
            {
                if (tableChangeToDinnerCommand == null)
                {
                    tableChangeToDinnerCommand = new DelegateCommand(tableChangeToDinner);
                }
                return tableChangeToDinnerCommand;
            }
        }

        private void tableChangeToDinner()
        {
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == "Обед");
                TableToShow = reports;
                LastSelectedTable = "Обед";
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }

        }

        #endregion
        #region Изменить таблицу "полдник"

        private DelegateCommand tableChangeToSnackCommand;

        public ICommand TableChangeToSnackCommand
        {
            get
            {
                if (tableChangeToSnackCommand == null)
                {
                    tableChangeToSnackCommand = new DelegateCommand(tableChangeToSnack);
                }
                return tableChangeToSnackCommand;
            }
        }

        private void tableChangeToSnack()
        {
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == "Полдник");
                TableToShow = reports;
                LastSelectedTable = "Полдинк";
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }

        }

        #endregion
        #region Изменить таблицу "ужин"

        private DelegateCommand tableChangeToSupperCommand;

        public ICommand TableChangeToSupperCommand
        {
            get
            {
                if (tableChangeToSupperCommand == null)
                {
                    tableChangeToSupperCommand = new DelegateCommand(tableChangeToSupper);
                }
                return tableChangeToSupperCommand;
            }
        }

        private void tableChangeToSupper()
        {
            try
            { 
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == "Ужин");
                TableToShow = reports;
                LastSelectedTable = "Ужин";
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
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
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
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
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Удалить строку таблицы

        private DelegateCommand removeRowCommand;

        public ICommand RemoveRowCommand
        {
            get
            {
                if (removeRowCommand == null)
                {
                    removeRowCommand = new DelegateCommand(removeRow);
                }
                return removeRowCommand;
            }
        }

        private void removeRow()
        {
            try
            { 
            if (LastSelected != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    unit.ReportRepository.Remove(LastSelected);
                    unit.Save();
                }
                refreshTable();
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        #endregion

        #region Открыть окно изменения продукта

        private DelegateCommand openChangeProductWindowCommand;

        public ICommand OpenChangeProductWindowCommand
        {
            get
            {
                if (openChangeProductWindowCommand == null)
                {
                    openChangeProductWindowCommand = new DelegateCommand(openChangeProductWindow,canOpenChangeProductWindow);
                }
                return openChangeProductWindowCommand;
            }
        }

        private void openChangeProductWindow()
        {
            try
            { 
            ChangeProductModalWindow window = new(LastSelected);
            if(window.ShowDialog() == true)
            {
                refreshTable();
            }
            }
            catch (Exception exception)
            {
                System.Windows.MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }

        private bool canOpenChangeProductWindow()
        {
            if (LastSelected == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #endregion

        #region Открытые методы

        public void refreshTable()
        {
            using (UnitOfWork unit = new UnitOfWork())
            {
                User foundUser = unit.UserRepository.Get(x => x.UserLogin == DeserializedUser.deserializedUser.UserLogin).First<User>();
                IEnumerable<Report> reports = unit.ReportRepository.Get(x => x.ReportDate.Date.Equals(DateToChoose.Date) && x.IdReport == foundUser.Id && x.EatPeriod == LastSelectedTable);              
                TableToShow = reports;
            }
        }
        #endregion

    }
}
