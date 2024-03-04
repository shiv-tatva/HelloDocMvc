using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_Business_Logic_Layer_.Services
{
    public class AdminDash : IAdminDash
    {
        private readonly ApplicationDbContext _context;

        public AdminDash(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<adminDash> adminData()
        {


            var query = (from r in _context.Requests
                        join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                        select new adminDash
                        {
                            first_name = rc.Firstname,
                            last_name = rc.Lastname,
                            int_date = rc.Intdate,
                            int_year = rc.Intyear,
                            str_month = rc.Strmonth,
                            requestor_fname = r.Firstname,
                            responseor_lname = r.Lastname,
                            created_date = r.Createddate,
                            mobile_num = rc.Phonenumber,
                            requestor_mobile_num = r.Phonenumber,
                            city = rc.City,
                            state = rc.State,
                            street = rc.Street,
                            zipcode = rc.Zipcode,
                            //address = r.Requestclients.Select(x => x.Street).First() + "," + r.Requestclients.Select(x => x.City).First() + "," + r.Requestclients.Select(x => x.State).First(),
                            request_type_id = r.Requesttypeid,
                            status = r.Status,
                            //phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                            //region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                            reqid = r.Requestid,
                            email = rc.Email,
                            //fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
                        }).ToList();



            //var result = query.ToList();


            return query;
        }
        
        
        public List<adminDash> adminDataViewCase(int reqId)
        {
            

            var query = from r in _context.Requests
                        join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                        where r.Requestid== reqId
                        select new adminDash
                        {
                            first_name = rc.Firstname,
                            last_name = rc.Lastname,
                            int_date = rc.Intdate,
                            int_year = rc.Intyear,
                            str_month = rc.Strmonth,
                            requestor_fname = r.Firstname,
                            responseor_lname = r.Lastname,
                            created_date = r.Createddate,
                            mobile_num = rc.Phonenumber,
                            city = rc.City,
                            state = rc.State,
                            street = rc.Street,
                            zipcode = rc.Zipcode,
                            request_type_id = r.Requesttypeid,
                            status = r.Status,
                            //phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                            //region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                            reqid = r.Requestid,
                            email = rc.Email,
                            //fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
                            cnf_number = r.Confirmationnumber,
                        };
                        

            var result = query.ToList();


            return result;
        } 
        
        
        
        public viewNotes adminDataViewNote(int reqId)
        {
            var a = _context.Requestnotes.FirstOrDefault(r => r.Requestid == reqId);

            viewNotes viewNotes = new viewNotes();
            Requestnote _reqNote = new Requestnote();
            
            var b = _context.Requeststatuslogs.FirstOrDefault(r => r.Requestid == reqId);

            if (a != null)
             {
                viewNotes.AdminNotes = _context.Requestnotes.FirstOrDefault(r => r.Requestid == reqId).Adminnotes;
                viewNotes.PhysicianNotes = _context.Requestnotes.FirstOrDefault(p => p.Requestid == reqId).Physiciannotes;
                //viewNotes.TransferNotes = _context.Requeststatuslogs.Where()
                viewNotes.cashtagId = Convert.ToInt16(_context.Requests.FirstOrDefault(r => r.Requestid == reqId).Casetag);
                if(b != null)
                {
                    viewNotes.aditional_notes = _context.Requeststatuslogs.FirstOrDefault(r => r.Requestid == reqId).Notes;
                }
                
             }else
             {
                _reqNote.Createddate = DateTime.Now.Date;
                _reqNote.Createdby = (int)_context.Requests.Where(x => x.Requestid == reqId).Select(x => x.User.Aspnetuserid).First();
                _reqNote.Requestid = _context.Requests.FirstOrDefault(r => r.Requestid == reqId).Requestid;
                _context.Add(_reqNote);
                _context.SaveChanges();
            }

            viewNotes.reqid = reqId;
            return viewNotes;
        }
        
        
        
        public void adminDataViewNote(adminDashData obj)
        {
            var reqNoteId = _context.Requestnotes.FirstOrDefault(r => r.Requestid == obj._viewNote.reqid);

            if(reqNoteId != null)
            {
                reqNoteId.Adminnotes = obj._viewNote.AdminNotes;
                reqNoteId.Modifiedby = (int)_context.Requests.Where(x => x.Requestid == obj._viewNote.reqid).Select(x => x.User.Aspnetuserid).First();
                reqNoteId.Modifieddate = DateTime.Now.Date;
                //_reqNote.Physiciannotes = obj._viewNote.PhysicianNotes;
                _context.SaveChanges();

            }

        }

        public CloseCase closeCaseNote(int reqId)
        {
          
            CloseCase _closeCase = new CloseCase();

            _closeCase.first_name = _context.Requestclients.FirstOrDefault(r => r.Requestid == reqId).Firstname;
            _closeCase.reqid = reqId;
            _closeCase.status = _context.Requests.FirstOrDefault(r => r.Requestid == reqId).Status;
           
            return _closeCase;
        }

        public List<casetageNote> casetag()
        {
            var query = from r in _context.Casetags
                        select (new casetageNote()
                        {
                            reasons = r.Name,
                            reqid = r.Casetagid,
                        });
            return query.ToList();
        }

        public void closeCaseNote(adminDashData obj)
        {
            Requeststatuslog _log = new Requeststatuslog();

            _log.Requestid = obj.closeCase.reqid;
            _log.Status = 3;
            _log.Notes = obj.closeCase.aditional_notes;
            _log.Createddate = DateTime.Now;

            _context.Add(_log);
            _context.SaveChanges();

            //var req = _context.Requests.Where(_req => _req.Requestid == obj.closeCase.reqid).Select(r => r).First();
            //req.Casetag = obj.closeCase.cashtagId.ToString();

            var req = _context.Requests.FirstOrDefault(x => x.Requestid == obj.closeCase.reqid);
            req.Casetag = obj.closeCase.cashtagId.ToString();
            req.Status = 3;
            _context.SaveChanges();

        }


        public AssignCase adminDataAssignCase(int req)
        {
            AssignCase assignCase = new AssignCase();

            assignCase.region_name = _context.Regions.Select(x => x.Name).ToList();
            assignCase.region_id = _context.Regions.Select(y => y.Regionid).ToList();
            assignCase.regions = _context.Regions.ToList();
            assignCase.reqid = req;
            
            //assignCase.phy_req = _context.Physicianregions.ToList();
            return assignCase;
        }

        public AssignCase adminDataAssignCaseDocList(int regionId)
        {
            AssignCase assignCase = new AssignCase();

            assignCase.phy_name = _context.Physicianregions.Where(x=>x.Regionid == regionId).Select(x=>x.Physician.Firstname).ToList();
            assignCase.phy_id = _context.Physicianregions.Where(x=>x.Regionid == regionId).Select(x=>x.Physician.Physicianid).ToList();
            
            return assignCase;
        }

        public void adminDataAssignCase(adminDashData assignObj)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            Request _req = new Request();
            
                var requestedRowPatient = _context.Requests.Where(x => x.Requestid == assignObj.assignCase.reqid).Select(r => r).First();

                requeststatuslog.Requestid = assignObj.assignCase.reqid;
                requeststatuslog.Notes = assignObj.assignCase.description;
                requeststatuslog.Createddate = DateTime.Now;
                requeststatuslog.Status = 2;

                _context.Add(requeststatuslog);
                _context.SaveChanges();

                requestedRowPatient.Physicianid = assignObj.assignCase.phy_id_main;
                requestedRowPatient.Status = 2;
                _context.Add(_req);

                _context.SaveChanges();
            

        }

        public blockCaseModel blockcase(int req)
        {
            blockCaseModel _block = new blockCaseModel();

            _block.reqid = req;
            _block.first_name = _context.Requestclients.Where(r => r.Requestid == req).Select(r => r.Firstname).First();

            return _block;
        }

        public void blockcase(adminDashData obj)
        {
            var request = _context.Requests.FirstOrDefault(r => r.Requestid == obj._blockCaseModel.reqid);

            if(request != null)
            {
                if (request.Isdeleted == null)
                {
                    request.Isdeleted = new BitArray(1);
                    request.Isdeleted[0] = true;
                    request.Status = 10;

                    _context.Requests.Update(request);
                    _context.SaveChanges();
                }
            }

            Blockrequest blockrequest = new Blockrequest();

            blockrequest.Phonenumber = request.Phonenumber;
            blockrequest.Email = request.Email;
            blockrequest.Reason = obj._blockCaseModel.description;
            blockrequest.Requestid = obj._blockCaseModel.reqid;
            blockrequest.Createddate = DateTime.Now;

            _context.Blockrequests.Add(blockrequest);
            _context.SaveChanges();
        }

        public List<viewUploads> viewUploadMain(int reqId)
        {


            var reqIdDeleted = _context.Requestwisefiles.Where(r => r.Requestid == reqId).Select(r => r.Isdeleted[0]);

            var query = _context.Requests.Where(r => r.Requestid == reqId).AsNoTracking().Select(r => new viewUploads()
            {
                reqid = reqId,
                fname = r.Firstname,
                lname = r.Lastname,
                cnf_number = r.Confirmationnumber,
                documentsname = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Filename).ToList(),
                created_date = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Createddate).ToList(),
                requestWiseFileId = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Requestwisefileid).ToList(),
            }).ToList();

            return query;
        }


        public void viewUploadMain(adminDashData obj)
        {
            Request _request = new Request();

            if (obj._viewUpload[0].Upload != null)
            {
                string filename = obj._viewUpload[0].Upload.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", filename);
                IFormFile file = obj._viewUpload[0].Upload;

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }

                Request? req = _context.Requests.FirstOrDefault(i => i.Requestid == obj._viewUpload[0].reqid);
                int ReqId = req.Requestid;

                var data3 = new Requestwisefile()
                {
                    Requestid = ReqId,
                    Filename = filename,
                    Createddate = DateTime.Now,
                };
                _context.Requestwisefiles.Add(data3);
                _context.SaveChanges();
            }
        }


        public void DeleteFile(bool data,int reqFileId)
        {
            var reqWiseFile = _context.Requestwisefiles.Where(r => r.Requestwisefileid == reqFileId).Select(r => r).First();

            Requestwisefile _req = new Requestwisefile();

            reqWiseFile.Isdeleted = new BitArray(1);
            reqWiseFile.Isdeleted[0] = data;

            _context.SaveChanges();
        }
    }
}
