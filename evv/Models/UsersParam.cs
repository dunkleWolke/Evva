using System;
using System.Collections.Generic;

#nullable disable

namespace Evva.Models
{
    public partial class UsersParam
    {
        public UsersParam()
        {
            Id = 0;
            IdParams = 0;
            ParamsDate = DateTime.Today.Date;
            UserWeight = 0;
            UserHeight = 0;
        }

        public UsersParam(int id, int idParams, DateTime paramsDate, decimal userWeight, int userHeight)
        {
            Id = id;
            IdParams = idParams;
            ParamsDate = paramsDate;
            UserWeight = userWeight;
            UserHeight = userHeight;
        }

        public int Id { get; set; }
        public int IdParams { get; set; }
        public DateTime ParamsDate { get; set; }
        public decimal UserWeight { get; set; }
        public int UserHeight { get; set; }

        public virtual User IdParamsNavigation { get; set; }
    }
}
