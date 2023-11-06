using Evva.Commands;
using Evva.Context.UnitOfWork;
using Evva.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Evva.ViewModels
{
    class ChangeProductViewModel : BaseViewModel
    {
        private Report report;
        private const decimal V = 0.01M;

        public ChangeProductViewModel(Report selectedReport)
        {
            report = selectedReport;
        }

        #region Properties

        public string ProductName
        {
            get { return report.ProductName; }
            set
            {
                report.ProductName = value;
                OnPropertyChanged("ProductName");
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

        #endregion
        
        #region Commands

        #region Внести изменения

        private DelegateCommand<object> changeProductCommand;

        public ICommand ChangeProductCommand
        {
            get
            {
                if (changeProductCommand == null)
                {
                    changeProductCommand = new DelegateCommand<object>(changeProduct,canChangeProduct);
                }
                return changeProductCommand;
            }
        }

        private void changeProduct(object sender)
        {
            using (UnitOfWork unit = new UnitOfWork())
            {
                Product product = unit.ProductRepository.Get(x => report.ProductName == x.ProductName).First();
                report.DayCarbohydrates = product.CarbohydratesGram * report.DayGram * V;
                report.DayCalories = product.CaloriesGram * report.DayGram * V;
                report.DayFats = product.FatsGram * report.DayGram * V;
                report.DayProteins = product.ProteinsGram * report.DayGram * V;
                unit.ReportRepository.Update(report);
                unit.Save();
            }
            Window wnd = sender as Window;
            wnd.DialogResult = true;
        }

        private bool canChangeProduct(object sender)
        {
            if(GramValue == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion

        #region Закрыть окно

        private DelegateCommand<object> cancelResultCommand;

        public ICommand CancelResultCommand
        {
            get
            {
                if (cancelResultCommand == null)
                {
                    cancelResultCommand = new DelegateCommand<object>(cancelResult);
                }
                return cancelResultCommand;
            }
        }

        private void cancelResult(object sender)
        {
            Window wnd = sender as Window;
            wnd.DialogResult = false;
        }

        #endregion

        #endregion

    }
}
