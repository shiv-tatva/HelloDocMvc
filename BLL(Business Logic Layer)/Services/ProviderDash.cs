using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
