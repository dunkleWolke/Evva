using System;
using System.Collections.Generic;

#nullable disable

namespace Evva.Models
{
    public partial class Report
    {
        public Report()
        {
            Id = 0;
            IdReport = 0;
            ProductName = "";
            ReportDate = DateTime.Today.Date;
            EatPeriod = "Завтрак";
            DayGram = 0;
            DayCalories = 0;
            DayCarbohydrates = 0;
            DayFats = 0;
            MostCategory = "";
        }
        public int Id { get; set; }
        public int IdReport { get; set; }
        public string ProductName { get; set; }
        public DateTime ReportDate { get; set; }
        public string EatPeriod { get; set; }
        public decimal DayGram { get; set; }
        public decimal DayCalories { get; set; }
        public decimal DayProteins { get; set; }
        public decimal DayFats { get; set; }
        public decimal DayCarbohydrates { get; set; }
        public string MostCategory { get; set; }

        public virtual User IdReportNavigation { get; set; }
        public virtual FoodCategory MostCategoryNavigation { get; set; }
        public virtual Product ProductNameNavigation { get; set; }
    }
}
