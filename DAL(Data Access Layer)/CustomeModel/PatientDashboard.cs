using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

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


        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string fname { get; set; }

        public string phy_fname { get; set; }

        public string? email { get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string lname { get; set; }

        public int? user_id_param { get; set; }

        public string phone_no { get; set; }

        public string street { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]

        public string zipcode { get; set; }

        public string? fulldateofbirth { get; set; }
        public DateTime fulldateofbirthMain { get; set; }

        public string str_month { get; set; }
        public string status { get; set; }

        public int? int_year { get; set; }

        public int? int_date { get; set; }

        public List<string> documentsname { get; set; }


    }

    public class reviewAgreement
    {
        public int reqid { get; set; }
        public int flag { get; set; }


        [Required(ErrorMessage = "Please Enter Notes")]
        public string? notes { get; set; }

    }

    public class createAcc
    {
        public string? email { get; set; }

        public int? aspnetUserId { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
      ErrorMessage = "8 characters long (one uppercase, one lowercase letter, one digit, and one special character.)")]

        public string? password { get; set; }


        [Compare("password", ErrorMessage = "Password Missmatch")]
        public string? confirmPassword { get; set; }

    }

    public class PatientDashboard
    {
        public List<PatientDashboardData> data { get; set; }


        [Required(ErrorMessage = "Please Select Atleast One File")]
        public IFormFile Upload { get; set; }



        public reviewAgreement _reviewAgreement { get; set; }

    }

}