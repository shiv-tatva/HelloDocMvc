using DAL_Data_Access_Layer_.CustomeModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Interface
{
    public interface IProviderDash
    {
        public void ProivderAccept(int reqId);

        public void physicianDataViewNote(adminDashData obj);

        public adminDashData ProviderTransferMain(int reqId);

        public void PostTransferRequest(string note, int Requestid,string sessionEmail);

        public adminDashData ProviderEncounterPopUp(int reqId);
        public void ProviderEncounterPopUp(adminDashData data, string sessionEmail);

        public adminDashData HousecallPopUp(int reqId);
        
        public void HouseCallConclude(int reqId, string sessionEmail);
        public void FinalizeEncounter(int reqId);
    }
}
