using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
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

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? first_name {  get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
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

        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]

        public string? zipcode { get; set;  }

        public string? state { get; set;  }

        public string? address { get; set;  }

        public List<Requeststatuslog>? notes { get; set; }
        public string? transfer_notes { get; set; }

        public string? region { get; set; }
        public int? region_id { get; set; }
        public int? call_type { get; set; }

        public int? request_type_id { get; set; }

        public int? status { get; set; }

        public string? email { get; set; }

        public int? reqid { get; set; }

        public string? cnf_number { get; set; }

        public List<Region> region_table { get; set; }

        public int? region_table_id { get; set; }
        public int? phy_id { get; set; }

        public int? flagId {  get; set; }

        public int? assignCaseIndicate {  get; set; }
        public BitArray isFinalize {  get; set; }
                      
    }

    public class countMain
    {
        public int? count1 { get; set; }
        public int? count2 { get; set; }
        public int? count3 { get; set; }
        public int? count4 { get; set; }
        public int? count5 { get; set; }
        public int? count6 { get; set; }

        public int roleId { get; set; }

    }

    public class viewNotes
    {
        public int reqid { get; set; }
        public int cashtagId { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Note")]
        public string? aditional_notes { get; set; } 
        public List<char> TransferNotes { get; set; }


        [Required(ErrorMessage = "Please Enter Physician Note")]
        public string? PhysicianNotes { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Note")]
        public string? AdminNotes { get; set; }
        public string? AdminCancleNotes { get; set; }
        public string? PatientCancleNotes { get; set; }

        public List<Requeststatuslog> requeststatuslogs { get; set; }
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


        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string fname { get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string lname { get; set; }

        public int? user_id_param { get; set; }

        public List<string> documentsname { get; set; }
        public List<int> requestWiseFileId { get; set; }


        [Required(ErrorMessage = "Please Enter Atleast One File")]
        public IFormFile Upload { get; set; }

        public bool dlt_data { get; set; }

        public int? flagId {  get; set; }


        [Required(ErrorMessage = "Please Enter Note")]
        public string? notes {  get; set; }

        public BitArray isFinalize { get; set; }
    }



    public class activeOrder
    {
        public int reqid { get; set; }

        [Required(ErrorMessage = "Please Select Business")]
        public int vendorid { get; set; }

        public List<string> business_data { get; set; }
        public List<int> business_id { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Select Profession")]
        public int? healthId { get; set; }

        
        public List<Healthprofessionaltype> profession  {  get; set; }

        [Required(ErrorMessage = "Please Enter Prescription")]
        public string? prescription { get;  set; }

        [Required(ErrorMessage = "Please Enter Fax Number")]
        public string? fax_num { get; set; }

        [Required(ErrorMessage = "Please Enter Number")]
        public string? business_contact { get; set; }

        public int Refill {  get; set; }
    }

    public class transferRequest
    {
        public int reqid { get; set; }

        [Required(ErrorMessage = "Please Select Region")]
        public int regionId { get; set; }
        public string? description { get; set; }
        public int admin_id { get; set; }

        [Required(ErrorMessage = "Please Select Physician")]
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

        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile Number")]
        public string? mobile_num { get; set; }
    }

    public class closeCaseMain
    {
        public int reqid { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Mobile Number")]
        public string? mobile_num { get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string fname { get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
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
        public int? phy_id { get; set; }
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
        public string? username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Please Enter State")]
        public string state { get; set; }
        public List<Aspnetrole> roles { get; set; }
        public int? flag { get; set; }

        public bool indicate { get; set; }
    }

    public class concludeEncounter
    {
        public int reqid { get; set; }


        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? FirstName { get; set; }


        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? LastName { get; set; }
        public string? Location { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        public string? BirthDate { get; set; }
        public DateTime? Date { get; set; }
        public string? fullDate { get; set; }
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter HistoryIllness")]
        public string? HistoryIllness { get; set; }

        [Required(ErrorMessage = "Please Enter MedicalHistory")]
        public string? MedicalHistory { get; set; }

        [Required(ErrorMessage = "Please Enter Medications")]
        public string? Medications { get; set; }

        [Required(ErrorMessage = "Please Enter Allergies")]
        public string? Allergies { get; set; }

        [Required(ErrorMessage = "Please Enter Temp")]
        public decimal? Temp { get; set; }

        [Required(ErrorMessage = "Please Enter Hr")]
        public decimal? Hr { get; set; }

        [Required(ErrorMessage = "Please Enter Rr")]
        public decimal? Rr { get; set; }

        [Required(ErrorMessage = "Please Enter BpS")]
        public int? BpS { get; set; }

        [Required(ErrorMessage = "Please Enter BpD")]
        public int? BpD { get; set; }

        [Required(ErrorMessage = "Please Enter O2")]
        public decimal? O2 { get; set; }

        [Required(ErrorMessage = "Please Enter Pain")]
        public string? Pain { get; set; }

        [Required(ErrorMessage = "Please Enter Heent")]
        public string? Heent { get; set; }

        [Required(ErrorMessage = "Please Enter Cv")]
        public string? Cv { get; set; }

        [Required(ErrorMessage = "Please Enter Chest")]
        public string? Chest { get; set; }

        [Required(ErrorMessage = "Please Enter Abd")]
        public string? Abd { get; set; }

        [Required(ErrorMessage = "Please Enter Extr")]
        public string? Extr { get; set; }

        [Required(ErrorMessage = "Please Enter Skin")]
        public string? Skin { get; set; }


        [Required(ErrorMessage = "Please Enter Neuro")]
        public string? Neuro { get; set; }

        [Required(ErrorMessage = "Please Enter Other")]
        public string? Other { get; set; }

        [Required(ErrorMessage = "Please Enter Diagnosis")]
        public string? Diagnosis { get; set; }

        [Required(ErrorMessage = "Please Enter TreatmentPlan")]
        public string? TreatmentPlan { get; set; }

        [Required(ErrorMessage = "Please Enter MedicationDispensed")]
        public string? MedicationDispensed { get; set; }


        [Required(ErrorMessage = "Please Enter Procedures")]
        public string? Procedures { get; set; }


        [Required(ErrorMessage = "Please Enter FollowUp")]
        public string? FollowUp { get; set; }

        public bool? indicate { get; set; }
        public BitArray isFinalize { get; set; }
    }

    public class sendLink
    {

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? FirstName { get; set; }

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
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
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]

        public string? zipcode { get; set; }

        [Required(ErrorMessage = "Please Enter Your Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Last Name")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
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




    //**********************************************Provider************************************************


    public class provider
    {
        public List<Physician> _physician {  get; set; }

        public List<Physiciannotification> _notification {  get; set; }

        public List<Role> _roles { get; set; }
        public List<Shift> _shift { get; set; }
        public List<Shiftdetail> _shiftDetails { get; set; }
        
        public string? onCallStatus { get; set; }

        public string? status { get; set; }

        public bool indicate { get; set; }

        public int? phyId { get; set; }

        [Required(ErrorMessage = "Please Enter A Message")]
        public string? message { get; set; }

        public int? statusId {  get; set; }
        public int? shiftId {  get; set; }

        public DateTime DateTime { get; set; }
        
        

    }

    public class AdminEditPhysicianProfile
    {

        [Required(ErrorMessage = "Please Enter Your User Name")]
        public string? username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [Compare("Email", ErrorMessage = "Email Missmatch")]
        public string? Con_Email { get; set; }

        [Required(ErrorMessage = "Please Enter Your PhoneNumber")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Your Street Name")]
        public string? Status { get; set; }

        [Required(ErrorMessage = "Please Enter Your City Name")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter Your Country Name")]
        public string? country { get; set; }

        [Required(ErrorMessage = "Please Enter Your Zipcode")]
        [StringLength(6, ErrorMessage = "Zipcode should less than 6 char")]

        public string? zipcode { get; set; }

        [Required(ErrorMessage = "Please Enter Your Firstname")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "First Name must contain only letters")]
        public string? Firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your Lastname")]
        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Last Name must contain only letters")]
        public string? Lastname { get; set; }

        [Required(ErrorMessage = "Please Select Atleast One State")]
        public int? Regionid { get; set; }

        [Required(ErrorMessage = "Please Enter Atleast One Role")]
        public int? Roleid { get; set; }

        [Required(ErrorMessage = "Please Enter Your MedicalLicesnse Number")]
        public string? MedicalLicesnse { get; set; }

        [Required(ErrorMessage = "Please Enter Your NPInumber")]
        public string? NPInumber { get; set; }

        [Required(ErrorMessage = "Please Enter Your SycnEmail")]
        public string? SycnEmail { get; set; }

        [Required(ErrorMessage = "Please Enter Your Businessname")]
        public string? Businessname { get; set; }

        [Required(ErrorMessage = "Please Enter Your BusinessWebsite")]
        public string? BusinessWebsite { get; set; }

        [Required(ErrorMessage = "Please Enter Adminnotes")]
        public string? Adminnotes { get; set; }

        [Required(ErrorMessage = "Please Enter Address1")]
        public string? Address1 { get; set; }

        [Required(ErrorMessage = "Please Enter Your Address2")]
        public string? Address2 { get; set; }


        public int PhyID { get; set; }


        public int statusId { get; set; }


        public int adminId { get; set; }

    
        public int aspnetUserId { get; set; }

        public List<Region> regions { get; set; }

        //public List<Physicianregion> physicianregions { get; set; }

        [Required(ErrorMessage = "Please Enter Alternative Phonenumber")]
        public string? altPhone { get; set; }

        [Required(ErrorMessage = "Please Enter Your State Name")]
        public string? State { get; set; }

        [Required(ErrorMessage = "Please Enter Your Street Name")]
        public int? StateId { get; set; }

        public string? flag { get; set; }

        public IFormFile? Photo { get; set; }

        public string? PhotoValue { get; set; }

        public IFormFile? Signature { get; set; }

        public string? SignatureValue { get; set; }

        public IFormFile? ContractorAgreement { get; set; }

        public bool IsContractorAgreement { get; set; }

        public IFormFile? BackgroundCheck { get; set; }

        public bool IsBackgroundCheck { get; set; }

        public IFormFile? HIPAA { get; set; }

        public bool IsHIPAA { get; set; }

        public IFormFile? NonDisclosure { get; set; }

        public bool IsNonDisclosure { get; set; }

        public IFormFile? LicenseDocument { get; set; }

        public bool IsLicenseDocument { get; set; }

        public List<Role> roles { get; set; }

        public bool? indicate { get; set; }
        public string? indicateTwo { get; set; }

        public decimal longitude { get; set; }

        public decimal latitude { get; set; }

        public DateTime? created_date { get; set; }

        public int? createdBy { get; set; }
        
        public DateTime? modified_date { get; set; }

        public int? modifiedBy { get; set; }
        public int? flagId { get; set; }
    }


    public class PhysicianRegionTable
    {
        public int PhysicianId { get; set; }

        public int Regionid { get; set; }

        public string Name { get; set; }

        public bool ExistsInTable { get; set; }
    }

    public class AdminregionTable
    {
        public int Adminid { get; set; }

        public int Regionid { get; set; }

        public string Name { get; set; }

        public bool ExistsInTable { get; set; }
    }


    public class ProviderTransferTab
    {
        public int? reqId { get; set; }

        [Required(ErrorMessage = "Please Enter Description")]
        public string? Note { get; set; }

    }
    
    
    public class ProviderEncounterPopUp
    {
        public int? reqId { get; set; }

        public int? flag { get; set; }

    }


    //*****************************************************************************************************
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

        public provider _provider { get; set; }

        public AdminEditPhysicianProfile _providerEdit { get; set; }
        public List<Region> _RegionTable { get; set; }
        public List<PhysicianRegionTable> _phyRegionTable { get; set; }
        public List<AdminregionTable> _adminRegionTable { get; set; }

        public List<Role> _role {  get; set; }

        public int? flag {  get; set; }

        public ProviderTransferTab _ProviderTransferTab {  get; set; }

        public ProviderEncounterPopUp _ProviderEncounterPopUp {  get; set; }
    }
}
