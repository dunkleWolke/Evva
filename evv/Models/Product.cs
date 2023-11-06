using System;
using System.Collections.Generic;

#nullable disable

namespace Evva.Models
{
    public partial class Product
    {
        public Product()
        {
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public int IdAdded { get; set; }
        public string ProductName { get; set; }
        public decimal CaloriesGram { get; set; }
        public decimal ProteinsGram { get; set; }
        public decimal FatsGram { get; set; }
        public decimal CarbohydratesGram { get; set; }
        public string FoodCategory { get; set; }

        public virtual FoodCategory FoodCategoryNavigation { get; set; }
        public virtual User IdAddedNavigation { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
