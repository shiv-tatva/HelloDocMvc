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

        public DateTime? created_date { get; set; }

        public string? requestor_mobile_num { get; set; }

        public string? mobile_num { get; set; }

        public string? city { get; set;  }

        public string? street { get; set;  }

        public string? zipcode { get; set;  }

        public string? state { get; set;  }

        public string? address { get; set;  }

        public string? notes { get; set; }

        public string? region { get; set; }
        public int? region_id { get; set; }

        public int? request_type_id { get; set; }

        public int? status { get; set; }

        public string? email { get; set; }

        public int? reqid { get; set; }

        public string? cnf_number { get; set; }

        public List<Region> region_table { get; set; }

        public int? region_table_id { get; set; }
                      
    }

    public class countMain
    {
        public int? count1 { get; set; }
        public int? count2 { get; set; }
        public int? count3 { get; set; }
        public int? count4 { get; set; }
        public int? count5 { get; set; }
        public int? count6 { get; set; }

    }

    public class viewNotes
    {
        public int reqid { get; set; }
        public int cashtagId { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Note")]
        public string? aditional_notes { get; set; }
        public List<char> TransferNotes { get; set; }
        public string? PhysicianNotes { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Note")]
        public string? AdminNotes { get; set; }
        public string? AdminCancleNotes { get; set; }
        public string? PatientCancleNotes { get; set; }
    }
    
    
    public class CloseCase
    {
        public int reqid { get; set; }

        [Required(ErrorMessage = "Please Select a Reason")]
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

        [Required(ErrorMessage = "Please Select Physican Name")]
        public int phy_id_main { get; set; }

        [Required(ErrorMessage = "Please Select Regins")]
        public int region_id_main { get; set; }
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
        public string? description { get; set; }
        public string? first_name { get; set; }
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

    public class transferRequest
    {
        public int reqid { get; set; }
        public string? description { get; set; }
        public int admin_id { get; set; }
        public int phy_id_main { get; set; }
        public List<int> phy_id { get; set; }
        public List<int> region_id { get; set; }
        public List<string> region_name { get; set; }

        public List<Region> regions { get; set; }

        public List<string> phy_name { get; set; }

        //public List<Physicianregion> phy_req { get; set; }

    }

    public class sendAgreement
    {
        public int reqid { get; set; }
        public int? request_type_id { get; set; }

        public int? status { get; set; }

        public string? email { get; set; }
        public string? mobile_num { get; set; }
    }

    public class closeCaseMain
    {
        public int reqid { get; set; }
        public string? email { get; set; }
        public string? mobile_num { get; set; }
        public string fname { get; set; }
        public string lname { get; set; }
        public string? fulldateofbirth { get; set; }
        public string? str_month { get; set; }
        public int? int_year { get; set; }
        public int? int_date { get; set; }
        public string? confirmation_no { get; set; }
        public int? status { get; set; }
        public List<Requestwisefile> _requestWiseFile { get; set; }

        [Required(ErrorMessage = "Please Enter Atleast One File")]
        public IFormFile Upload { get; set; }
    }

    public class myProfile
    {
        public int? admin_id { get; set; }
        public int? aspnetuserid { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "First Name must contain only letters")]
        public string? fname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Last Name must contain only letters")]
        public string? lname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? email { get; set; }

        [Compare("email", ErrorMessage = "Email Missmatch")]
        public string? confirm_email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Mobile number")]
        public string? mobile_no { get; set; }

        [Required(ErrorMessage = "Please Enter address-1")]
        public string? addr1 { get; set; }

        [Required(ErrorMessage = "Please Enter address-2")]
        public string? addr2 { get; set;}

        [Required(ErrorMessage = "Please Enter Your City")]
        public string? city { get; set; }
        public int regionId { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string zip { get; set; }
        public string? altphone { get; set; }
        public int? createdBy {  get; set; } 
        public DateTime createdDate { get; set; }
        public int status { get; set; }
        public int? roleid { get; set; }
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string state { get; set; }
        public List<Aspnetrole> roles { get; set; }
        public int? flag { get; set; }

        public bool indicate { get; set; }
    }

    public class concludeEncounter
    {
        public int reqid { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Location { get; set; }
        public string? BirthDate { get; set; }
        public DateTime? Date { get; set; }
        public string? fullDate { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? HistoryIllness { get; set; }
        public string? MedicalHistory { get; set; }
        public string? Medications { get; set; }
        public string? Allergies { get; set; }
        public decimal? Temp { get; set; }
        public decimal? Hr { get; set; }
        public decimal? Rr { get; set; }
        public int? BpS { get; set; }
        public int? BpD { get; set; }
        public decimal? O2 { get; set; }
        public string? Pain { get; set; }
        public string? Heent { get; set; }
        public string? Cv { get; set; }
        public string? Chest { get; set; }
        public string? Abd { get; set; }
        public string? Extr { get; set; }
        public string? Skin { get; set; }
        public string? Neuro { get; set; }
        public string? Other { get; set; }
        public string? Diagnosis { get; set; }
        public string? TreatmentPlan { get; set; }
        public string? MedicationDispensed { get; set; }
        public string? Procedures { get; set; }
        public string? FollowUp { get; set; }

        public bool? indicate { get; set; }
    }

    public class sendLink
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? indicate { get; set; }
    }

    public class createRequest 
    {

        public int? requesttypeid { get; set; }

        public int? userid { get; set;}
        
        [Required(ErrorMessage = "Please Enter Your Street Name")]
        public string? street { get; set; }

        [Required(ErrorMessage = "Please Enter Your City Name")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter Your State Name")]
        public string? state { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string? zipcode { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Date Of Birth")]
        public string? dateofbirth { get; set; }

        [Required(ErrorMessage = "Please Enter Your Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Your Phone Number")]
        public string? phone { get; set; }

        [Required(ErrorMessage = "Please Enter Your Room")]
        public string? room { get; set; }

        public string? admin_notes { get; set; }

        public bool? indicate { get; set; }
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

        public transferRequest transferRequest { get; set; }

        public sendAgreement _sendAgreement { get; set; }

        public closeCaseMain _closeCaseMain { get; set; }

        public myProfile _myProfile { get; set; }

        public concludeEncounter _encounter { get; set; }

        public sendLink _sendLink { get; set; }

        public createRequest _createRequest { get; set; }

        public countMain _countMain { get; set; }
    }
}
