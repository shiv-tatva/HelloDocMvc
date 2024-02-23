using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class adminDash
    {
         public string? first_name {  get; set; }

        public string? last_name { get; set; }


        public string? fulldateofbirth { get; set; }

        public string str_month { get; set; }
        public int? int_year { get; set; }
        public int? int_date { get; set; }

        public string? phy_name { get; set; }
        public string? requestor_fname { get; set;}

        public string? responseor_lname { get;set;} 

        public DateTime created_date { get; set; }

        public string? requestor_mobile_num { get; set; }

        public string? mobile_num { get; set; }

        public string? city { get; set;  }

        public string? street { get; set;  }

        public string? zipcode { get; set;  }

        public string? state { get; set;  }

        public string? address { get; set;  }

        public string? notes { get; set; }

        public string? region { get; set; }

        public int? request_type_id { get; set; }

        public int? status { get; set; }

        public string? email { get; set; }

        public int reqid { get; set; }

        public string? cnf_number { get; set; }


    }


    public class adminDashData
    {
        public List<adminDash> data { get; set;}
    }
}
