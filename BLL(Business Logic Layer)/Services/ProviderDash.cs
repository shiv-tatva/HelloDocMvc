using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL_Business_Logic_Layer_.Services
{
    public class ProviderDash : IProviderDash
    {
        private readonly ApplicationDbContext _context;

        public ProviderDash(ApplicationDbContext context) 
        {
            _context = context;
        }

        public void ProivderAccept(int reqId)
        {
            var requestDetails = _context.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();

            var reqeustStatusLogDetails = _context.Requeststatuslogs.Where(r => r.Requestid == reqId).Select(r => r).First();

            requestDetails.Status = 2;
            requestDetails.Modifieddate = DateTime.Now;

            reqeustStatusLogDetails.Status = 2;

            _context.SaveChanges();
        }

        public void physicianDataViewNote(adminDashData obj)
        {
            var reqNoteId = _context.Requestnotes.FirstOrDefault(r => r.Requestid == obj._viewNote.reqid);

            if (reqNoteId != null)
            {
                reqNoteId.Physiciannotes = obj._viewNote.PhysicianNotes;
                reqNoteId.Modifiedby = (int)_context.Requests.Where(x => x.Requestid == obj._viewNote.reqid).Select(x => x.User.Aspnetuserid).First();
                reqNoteId.Modifieddate = DateTime.Now.Date;
                //_reqNote.Physiciannotes = obj._viewNote.PhysicianNotes;
                _context.SaveChanges();

            }

        }

        public adminDashData ProviderTransferMain(int reqId) 
        { 
       
            adminDashData data = new adminDashData();
            ProviderTransferTab providerMain = new ProviderTransferTab();

            providerMain.reqId = reqId;

            data._ProviderTransferTab = providerMain;

            return data;
        }

        public void PostTransferRequest(string note, int Requestid, string sessionEmail)
        {
            var providerId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

            var transferdata = _context.Requests.FirstOrDefault(i => i.Requestid == Requestid);

            var transferstatuslog = new Requeststatuslog
            {
                Requestid = Requestid,
                Notes = note,
                Status = 1,
                Transtoadmin = new BitArray(1, true),
                Physicianid = providerId.Physicianid,
                Createddate = DateTime.Now,
            };

            transferdata.Physicianid = null;
            transferdata.Status = 1;
            transferdata.Modifieddate = DateTime.Now;

            _context.Requeststatuslogs.Add(transferstatuslog);
            _context.SaveChanges();
        }


        public adminDashData ProviderEncounterPopUp(int reqId)
        {
            adminDashData data = new adminDashData();
            
            ProviderEncounterPopUp _providerEncounterPopUp = new ProviderEncounterPopUp();

            _providerEncounterPopUp.reqId = reqId;

            data._ProviderEncounterPopUp = _providerEncounterPopUp;

            return data;
        }

        public void ProviderEncounterPopUp(adminDashData data, string sessionEmail)
        {

            var providerId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();
            var reqData = _context.Requests.Where(r => r.Requestid == data._ProviderEncounterPopUp.reqId).Select(r => r).First();
            Requeststatuslog requeststatuslog = new Requeststatuslog();

            if (data._ProviderEncounterPopUp.flag == 1)
            {
                reqData.Status = 5;
                reqData.Calltype = 1;
                reqData.Modifieddate = DateTime.Now;

                _context.SaveChanges();

                requeststatuslog.Requestid = (int)data._ProviderEncounterPopUp.reqId;
                requeststatuslog.Status = 5;
                requeststatuslog.Createddate = DateTime.Now;
                requeststatuslog.Physicianid = providerId.Physicianid;
                requeststatuslog.Notes = "House Call";

                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }
            else
            {
                reqData.Status = 6;
                reqData.Calltype = 2;
                reqData.Modifieddate = DateTime.Now;

                _context.SaveChanges();

                requeststatuslog.Requestid = (int)data._ProviderEncounterPopUp.reqId;
                requeststatuslog.Status = 6;
                requeststatuslog.Createddate = DateTime.Now;
                requeststatuslog.Physicianid = providerId.Physicianid;
                requeststatuslog.Notes = "Consult";

                _context.Requeststatuslogs.Add(requeststatuslog);
                _context.SaveChanges();
            }
        }


        public adminDashData HousecallPopUp(int reqId)
        {
            adminDashData data = new adminDashData();

            ProviderEncounterPopUp _providerEncounterPopUp = new ProviderEncounterPopUp();

            _providerEncounterPopUp.reqId = reqId;

            data._ProviderEncounterPopUp = _providerEncounterPopUp;

            return data;
        }

        public void HouseCallConclude(int reqId, string sessionEmail)
        {
            var reqData = _context.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();
            var providerId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

            Requeststatuslog requeststatuslog = new Requeststatuslog();


            reqData.Status = 6;
            reqData.Modifieddate = DateTime.Now;

            _context.SaveChanges();

            requeststatuslog.Requestid = reqId;
            requeststatuslog.Status = 6;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Physicianid = providerId.Physicianid;
            requeststatuslog.Notes = "House Call Clicked";

            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
        }

        public void FinalizeEncounter(int reqId)
        {

            var encounter = _context.EncounterForms.Where(r => r.Requestid == reqId).Select(r => r).First();

            encounter.IsFinalized = new BitArray(1, true);

            _context.SaveChanges();
        }

    }
}
