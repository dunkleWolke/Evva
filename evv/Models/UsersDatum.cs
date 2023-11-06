using System;
using System.Collections.Generic;

#nullable disable

namespace Evva.Models
{
    public partial class UsersDatum
    {
        public UsersDatum()
        {
            Id = 0;
            IdData = 0;
            FullName = "";
            Birthday = DateTime.Today.Date;
        }
        public int Id { get; set; }
        public int IdData { get; set; }
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }

        public virtual User IdDataNavigation { get; set; }
    }
}
