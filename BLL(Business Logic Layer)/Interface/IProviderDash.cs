using DAL_Data_Access_Layer_.CustomeModel;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IProviderDash
    {
        public void ProivderAccept(int reqId);
        public void physicianDataViewNote(adminDashData obj);
        public adminDashData ProviderTransferMain(int reqId);
        public void PostTransferRequest(string note, int Requestid, string sessionEmail);
        public adminDashData ProviderEncounterPopUp(int reqId);
        public void ProviderEncounterPopUp(adminDashData data, string sessionEmail);
        public adminDashData HousecallPopUp(int reqId);
        public void HouseCallConclude(int reqId, string sessionEmail);
        public bool FinalizeEncounter(int reqId);
        public adminDashData ProviderEncounterFormDownload(int reqId);
        public void ProviderConcludeCarePost(adminDashData adminDashData, string sessionEmail);
        public SchedulingCm PhysicainRegionTable(string sessionEmail);
        public void RequestAdmin(ProviderTransferTab _ProviderTransferTab, string sessionEmail);
        public List<DateViewModel> GetDates();
        public int GetPhyID(string sessionEmail);
        public InvoicingViewModel GetInvoicingDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID);
        public InvoicingViewModel GetUploadedDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int pageNumber, int pagesize);
        public InvoicingViewModel getDataOfTimesheet(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID);
        public void AprooveTimeSheet(InvoicingViewModel model, int? AdminID);
        public void SubmitTimeSheet(InvoicingViewModel model, int? PhysicianId);
        public void DeleteBill(int id);
        public void FinalizeTimeSheet(int id);       
    }
}
