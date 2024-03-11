using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public string? email { get; set; }

        public string lname { get; set; }

        public int? user_id_param { get; set; }

        public string phone_no { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public string zipcode { get; set; }

        public string? fulldateofbirth { get; set; }

        public string str_month { get; set; }

        public int? int_year { get; set; }

        public int? int_date { get; set; }

        public List<string> documentsname { get; set; }


    }

    public class reviewAgreement
    {
        public int reqid { get; set; }
        public int flag { get; set; }

        public string? notes { get; set; }

    }

    public class PatientDashboard {
    public List<PatientDashboardData> data{  get; set; }


        [Required(ErrorMessage = "Please Enter Atleast One File")]
        public IFormFile Upload { get; set; }


        public reviewAgreement _reviewAgreement { get; set; }

    }

}