using System;
using System.Collections.Generic;

#nullable disable

namespace Evva.Models
{
    public partial class FoodCategory
    {
        public FoodCategory()
        {
            Products = new HashSet<Product>();
            Reports = new HashSet<Report>();
        }

        public int Id { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
