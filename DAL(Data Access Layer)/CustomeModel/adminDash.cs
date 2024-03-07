using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Printing;
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

        public string? str_month { get; set; }
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

        public int? reqid { get; set; }

        public string? cnf_number { get; set; }

        

    }

    public class viewNotes
    {
        public int reqid { get; set; }
        public int cashtagId { get; set; }
        public string? aditional_notes { get; set; }
        public List<char> TransferNotes { get; set; }
        public string? PhysicianNotes { get; set; }
        public string? AdminNotes { get; set; }
        public string? AdminCancleNotes { get; set; }
        public string? PatientCancleNotes { get; set; }
    }
    
    
    public class CloseCase
    {
        public int reqid { get; set; }
        public int cashtagId { get; set; }
        public string? aditional_notes { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public int? status { get; set; }

    }
    
    
   
    public class casetageNote
    {
        public int reqid { get; set; }
        public string? reasons { get; set; }
      
    }

    public class AssignCase
    {
        public int reqid { get; set; }
        public string? description { get; set; }
        public int phy_id_main { get; set; }
        public List<int> phy_id { get; set; }
        public List<int> region_id { get; set; }
        public List<string> region_name { get; set; }

        public List<Region> regions { get; set; }

        public List<string> phy_name { get; set; }

        //public List<Physicianregion> phy_req { get; set; }

    }

    public class blockCaseModel
    {
        public int reqid { get; set; }
        public string? first_name { get; set; }
        public string? description { get; set; }
    }


    public class viewUploads
    {
        public int reqid { get; set; }
        public string? email { get; set; }

        public List<DateTime> created_date { get; set; }

        public string? docname { get; set; }

        public string? cnf_number { get; set; }

        public string fname { get; set; }
        public string lname { get; set; }

        public int? user_id_param { get; set; }

        public List<string> documentsname { get; set; }
        public List<int> requestWiseFileId { get; set; }


        [Required(ErrorMessage = "Please Enter Atleast One File")]
        public IFormFile Upload { get; set; }

        public bool dlt_data { get; set; }
    }



    public class activeOrder
    {
        public int reqid { get; set; }
        public int vendorid { get; set; }

        public List<string> business_data { get; set; }
        public List<int> business_id { get; set; }
        public string? email { get; set; }

        public List<Healthprofessionaltype> profession  {  get; set; }
        public string? prescription { get;  set; }

        public string? fax_num { get; set; }
        public string? business_contact { get; set; }

        public int Refill {  get; set; }
    }
   

    public class adminDashData
    {
        public List<adminDash> data { get; set;}

        public viewNotes? _viewNote { get; set; }

        public CloseCase? closeCase { get; set; }

        public List<casetageNote> casetagNote { get; set; }

        public AssignCase assignCase { get; set; }

        public blockCaseModel _blockCaseModel { get; set; }

        public List<viewUploads> _viewUpload {  get; set; }

        public activeOrder _activeOrder { get; set; }

    }
}
