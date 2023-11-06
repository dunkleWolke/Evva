using System;
using System.Collections.Generic;
using System.Xml.Serialization;

#nullable disable

namespace Evva.Models
{
    public partial class User
    {
        public User()
        {
            Products = new HashSet<Product>();
            Reports = new HashSet<Report>();
            UsersData = new HashSet<UsersDatum>();
            UsersParams = new HashSet<UsersParam>();
            Id = 0;
            IsAdmin = false;
            Salt = default;
            UserLogin = "";
        }

        public int Id { get; set; }

        [XmlIgnore]
        public bool? IsAdmin { get; set; }

        public string UserLogin { get; set; }
        public byte[] UserPassword { get; set; }
        
        [XmlIgnore]
        public string Salt { get; set; }

        [XmlIgnore]
        public virtual ICollection<Product> Products { get; set; }
        [XmlIgnore]
        public virtual ICollection<Report> Reports { get; set; }
        [XmlIgnore]
        public virtual ICollection<UsersDatum> UsersData { get; set; }
        [XmlIgnore]
        public virtual ICollection<UsersParam> UsersParams { get; set; }
    }
}