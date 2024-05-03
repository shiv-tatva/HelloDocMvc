using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataModels;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IAdminDash
    {
        public List<adminDash> adminData(int[] status, int typeId, int regionId, string sessionName, int dataFlag, string sessionFilter);

        public countMain countService(string sessionName, int flagCount);

        public List<string> GetListOfRoleMenu(int roleId);
        public List<adminDash> adminDataViewCase(int reqId, int flag);
        public viewNotes adminDataViewNote(int reqId);
        public void adminDataViewNote(adminDashData obj);

        public CloseCase closeCaseNote(int reqId);
        public void closeCaseNote(adminDashData obj);

        public List<casetageNote> casetag();

        public AssignCase adminDataAssignCase(int req);
        public void adminDataAssignCase(adminDashData assignObj);

        public AssignCase adminDataAssignCaseDocList(int regionId);

        public blockCaseModel blockcase(int req);

        public void blockcase(adminDashData obj, string sessionEmail);

        public List<viewUploads> viewUploadMain(int reqId, int flag);

        public void viewUploadMain(adminDashData obj);
        public void DeleteFile(bool data, int reqFileId);
        public void sendMail(string emailMain, string[] data, string sessionEmail);

        public activeOrder viewOrder(int reqId);

        public activeOrder businessName(int profession_id);
        public activeOrder businessDetail(int businessDetail);

        public void viewOrder(adminDashData adminDashData);

        public transferRequest transferReq(int req);
        public void transferReq(adminDashData data, string sessionEmail);

        public blockCaseModel clearCase(int reqId);
        public void clearCase(adminDashData block, string sessionEmail);

        public sendAgreement sendAgree(int reqId);
        public void sendAgree(adminDashData dataMain, string sessionEmail);

        public closeCaseMain closeCaseMain(int reqId);

        public void closeCaseSaveMain(adminDashData obj);
        public void closeCaseCloseBtn(int reqId, string sessionEmail);
        public myProfile myProfile(string sessionEmail);

        public bool myProfileReset(myProfile obj, string sessionEmail);

        public myProfile myProfileAdminInfo(myProfile obj, string sessionEmail, List<int> adminRegions);
        public bool myProfileAdminBillingInfo(myProfile obj, string sessionEmail);

        public concludeEncounter concludeEncounter(int data);
        public concludeEncounter concludeEncounter(concludeEncounter obj);
        public sendLink sendLink(sendLink data, string sessionEmail);
        public createRequest createRequest(createRequest data, string sessionEmail, int flag);

        public createRequest verifyState(string state);

        public byte[] GenerateExcelFile(List<adminDash> adminData);

        //***********************************************Provider***************************************
        public provider providerMain(int regionId);

        public provider stopNotification(int phyId);

        public provider providerContact(int phyId);

        public provider providerContactEmail(int phyIdMain, string msg, string sessionEmail);
        public provider providerContactSms(int phyIdMain, string msg, string sessionEmail);

        public AdminEditPhysicianProfile adminEditPhysicianProfile(int phyId, string sessionEmail, int flag, int statusId);

        public List<Region> RegionTable();


        public List<PhysicianRegionTable> PhyRegionTable(int phyId);

        public List<Role> physicainRole();
        public List<Role> adminRole();

        public bool providerResetPass(string email, string password);

        public bool editProviderForm1(int phyId, int roleId, int statusId);
        public bool editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId, int[] phyRegionArray);

        public AdminEditPhysicianProfile editProviderForm3(adminDashData dataMain);
        public AdminEditPhysicianProfile PhysicianBusinessInfoUpdate(adminDashData dataMain);
        public AdminEditPhysicianProfile EditOnBoardingData(adminDashData dataMain);
        public void editProviderDeleteAccount(int phyId);
        public AdminEditPhysicianProfile createProviderAccount(adminDashData obj, List<int> physicianRegions);

        public List<AccountAccess> GetAccountAccessData();
        public List<Aspnetrole> GetAccountType();
        public List<Menu> GetMenu(int accounttype);
        void SetCreateAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string UserSession);

        public List<AccountMenu> GetAccountMenu(int accounttype, int roleid);
        AccountAccess GetEditAccessData(int roleid);
        void SetEditAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string sessionEmail);
        void DeleteAccountAccess(int roleid);

        public List<Aspnetrole> GetAccountTypeRoles();

        public List<UserAccess> GetUserdata(int accounttypeid);

        public void createAdminAccount(adminDashData obj, List<int> regions);

        public AdminEditPhysicianProfile adminEditPage(int adminId);

        public List<AdminregionTable> GetRegionsAdmin(int adminId);

        public bool EditAdminDetailsDb(adminDashData adminDashData, string email, List<int> adminRegions);

        public void editDeleteAdminAccount(int adminId);

        public List<Physicianlocation> GetPhysicianlocations();

        //*****************************************************Partners****************************************

        public List<Healthprofessionaltype> GetProfession();
        public List<Partnersdata> GetPartnersdata(int professionid);
        public bool CreateNewBusiness(partnerModel partnerModel, string sessionEmail);
        public partnerModel GetEditBusinessData(int vendorID);
        public bool UpdateBusiness(partnerModel partnerModel);
        public void DltBusiness(int vendorID);


        //*****************************************************Scheduling****************************************

        public List<Physician> GetPhysicians(int regionid);

        public List<Physician> GetRegionvalue(int selectedregion);

        public List<ShiftDetailsmodal> ShiftDetailsmodal(DateTime date, DateTime sunday, DateTime saturday, string type, int flag, string email);
        ShiftDetailsmodal GetShift(int shiftdetailsid);
        bool createShift(ScheduleModel scheduleModel, int Aspid);
        void SetReturnShift(int status, int shiftdetailid, int Aspid);
        public void SetDeleteShift(int shiftdetailid, int Aspid);
        public bool SetEditShift(ShiftDetailsmodal shiftDetailsmodal, int Aspid);
        public OnCallModal GetOnCallDetails(int regionId);
        public List<ShiftReview> GetShiftReview(int regionId, int callId);
        public void ApproveSelectedShift(int[] shiftDetailsId, int Aspid);
        public void DeleteShiftReview(int[] shiftDetailsId, int Aspid);

        public List<Region> GetRegions();

        //************************************************Records*********************************************************

        public List<requestsRecordModel> searchRecords(recordsModel recordsModel);

        public void DeleteRecords(int reqId);

        public byte[] GenerateExcelFile(List<requestsRecordModel> recordsModel);

        public List<User> patientRecords(GetRecordsModel GetRecordsModel);

        public List<GetRecordExplore> GetPatientRecordExplore(int userId);

        public List<blockHistory> blockHistory(recordsModel recordsModel);


        public void unblockBlockHistoryMain(int blockId);

        public recordsModel emailLogsMain(int tempId, recordsModel recordsModel);

        //public blockHistory stopNotificationBlock(int blockId);
        public List<DateViewModel> GetDates();

        public List<PhysicianViewModel> GetPhysiciansForInvoicing();

        public string CheckInvoicingAproove(string selectedValue, int PhysicianId);

        public InvoicingViewModel GetApprovedViewData(string selectedValue, int PhysicianId);

        public void AprooveTimeSheet(InvoicingViewModel model, int? AdminID);

    }
}
