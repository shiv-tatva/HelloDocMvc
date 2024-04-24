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
    }
}
