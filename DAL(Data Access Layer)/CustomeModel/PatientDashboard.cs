using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class PatientDashboardData
    {


        public int reqid { get; set; }

        public DateTime created_date { get; set; }

        public int? current_status { get; set; }

        public int? physician_id { get; set; }

        public int? req_type_id { get; set; }

        public string? docname { get; set; }

        public int? doc_Count { get; set; }

        public string? cnf_number { get; set; }

        public string fname { get; set; }

        public string phy_fname { get; set; }

        public string lname { get; set; }

        public List<string> documentsname { get; set; }
    }

    public class PatientDashboard {
    public List<PatientDashboardData> data{  get; set; }

    }

}