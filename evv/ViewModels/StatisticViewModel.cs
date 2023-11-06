using Evva.Commands;
using Evva.Context.UnitOfWork;
using Evva.DeserializedUserNamespace;
using Evva.Models;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Evva.ViewModels
{
    class StatisticViewModel : BaseViewModel
    {

        private IEnumerable<UsersParam> statisticCollection;

        private decimal weight;
        private int height;
        private string lastReportDate;
        private string mostCategory;
        private UsersParam lastSelected;

        public SeriesCollection seriesCollection;
        private string[] labels;

        public StatisticViewModel()
        {
            try
                { 
            using(UnitOfWork unit = new UnitOfWork())
            {
                
                LastSelected = default;

                StatisticCollection = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id);
                IEnumerable<Report> report = unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id);
                List<Report> mostReportCategory = (List<Report>)unit.ReportRepository.Get(x => x.IdReport == DeserializedUser.deserializedUser.Id && DateTime.Today.Date.Date.Equals(x.ReportDate.Date));

                if (mostReportCategory.Capacity != 0)
                {
                    MostCategory = mostReportCategory.GroupBy(i => i.MostCategory).OrderByDescending(grp => grp.Count()).Select(grp => grp.Key).First();
                }
                else
                {
                    MostCategory = "---";
                }

                if (report.Count() != 0)
                {
                    LastReportDate = report.Last().ReportDate.ToString();
                }
                else
                {
                    LastReportDate = "---";
                }

                if (StatisticCollection.Count() != 0)
                {
                    Height = StatisticCollection.Last().UserHeight;
                    Weight = StatisticCollection.Last().UserWeight;


                    ChartValues<decimal> weights = new ChartValues<decimal>(StatisticCollection.Select(x => x.UserWeight));
                    List<string> labels = new List<string>();

                    foreach(DateTime x in StatisticCollection.Select(x => x.ParamsDate))
                    {
                        labels.Add(x.ToString("d"));
                    }

                    Labels = labels.ToArray();
                    YFormatter = value => value.ToString("0.00");

                    SeriesCollection = new SeriesCollection();

                    SeriesCollection.Add(new LineSeries
                    {
                        Title = "Вес",
                        Values = weights,
                        LineSmoothness = 0,
                        PointGeometrySize = 15,                     
                        PointForeground = Brushes.Coral,
                        Stroke = Brushes.Coral,
                        Fill = Brushes.Transparent
                    });

                }
                else
                {
                    Height = 0;
                    Weight = 0;
                }                
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        
        #region Properties

        public Func<double, string> YFormatter { get; set; }

        public UsersParam LastSelected
        {
            get { return lastSelected; }
            set
            {
                lastSelected = value;
                OnPropertyChanged("LastSelected");
            }
        }

        public string[] Labels
        {
            get { return labels; }
            set
            {
                labels = value;
                OnPropertyChanged("Labels");
            }
        }

        public SeriesCollection SeriesCollection
        {
            get { return seriesCollection; }
            set
            {
                seriesCollection = value;
                OnPropertyChanged("SeriesCollection");
            }
        }

        public string MostCategory
        {
            get { return mostCategory; }
            set
            {
                mostCategory = value;
                OnPropertyChanged("MostCategory");
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
        public string LastReportDate
        {
            get { return lastReportDate; }
            set
            {
                lastReportDate = value;
                OnPropertyChanged("LastReportDate");
            }
        }

        public IEnumerable<UsersParam> StatisticCollection
        {
            get { return statisticCollection; }
            set
            {
                statisticCollection = value;
                OnPropertyChanged("StatisticCollection");
            }
        }
        #endregion

        #region Commands


        #region Добавить отчёт по пользователю

        private DelegateCommand addParamsReportCommand;

        public ICommand AddParamsReportCommand
        {
            get
            {
                if (addParamsReportCommand == null)
                {
                    addParamsReportCommand = new DelegateCommand(addParamsReport, canAddParamsReport);
                }
                return addParamsReportCommand;
            }
        }

        private void addParamsReport()
        {
            if (!Regex.IsMatch(Weight.ToString(), "^[0-9]{1,3}([.][0-9]{1,2})?$"))
            {

            }
            else if (!Regex.IsMatch(Height.ToString(), "^[1-9]{1}[0-9]{0,2}$"))
            {

            }
            else
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UsersParam usersParam = new UsersParam();
                    usersParam.ParamsDate = DateTime.Now;
                    usersParam.UserHeight = Height;
                    usersParam.UserWeight = Weight;
                    usersParam.IdParams = DeserializedUser.deserializedUser.Id;
                    unit.UserParamRepository.Create(usersParam);
                    unit.Save();
                    Height = default;
                    Weight = default;
                }
            }
        }

        private bool canAddParamsReport()
        {
            if (Weight != 0 && Height != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
        #region Удалить запись в таблице вес

        private DelegateCommand deleteWeightRowCommand;

        public ICommand DeleteWeightRowCommand
        {
            get
            {
                if (deleteWeightRowCommand == null)
                {
                    deleteWeightRowCommand = new DelegateCommand(deleteWeightRow, canDeleteWeightRow);
                }
                return deleteWeightRowCommand;
            }
        }


        private void deleteWeightRow()
        {
            try
            { 
            if (LastSelected != null)
            {
                using (UnitOfWork unit = new UnitOfWork())
                {
                    UsersParam toDelete = unit.UserParamRepository.Get(x => x.ParamsDate == lastSelected.ParamsDate && x.IdParams == DeserializedUser.deserializedUser.Id).First();
                    unit.UserParamRepository.Remove(toDelete);
                    unit.Save();

                    StatisticCollection = unit.UserParamRepository.Get(x => x.IdParams == DeserializedUser.deserializedUser.Id);
                }

                SeriesCollection.Clear();

                ChartValues<decimal> weights = new ChartValues<decimal>(StatisticCollection.Select(x => x.UserWeight));
                List<string> labels = new List<string>();

                foreach (DateTime x in StatisticCollection.Select(x => x.ParamsDate))
                {
                    labels.Add(x.ToString("d"));
                }

                Labels = labels.ToArray();
                SeriesCollection.Add(new LineSeries
                {
                    Title = "Вес",
                    Values = weights,
                    LineSmoothness = 0,
                    PointGeometrySize = 15,
                    PointForeground = Brushes.Coral,
                    Stroke = Brushes.Coral,
                    Fill = Brushes.Transparent
                });
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
            }
        }
        private bool canDeleteWeightRow()
        {
            try
            { 
            if(StatisticCollection.Count() >1)
            {
                return true;
            }
            else
            {
                return false;
            }
            }
            catch (Exception exception)
            {
                MessageBox.Show("Сообщение ошибки: " + exception.Message, "Произошла ошибка");
                return false;
            }
        }

            #endregion


        #endregion

        }
}
