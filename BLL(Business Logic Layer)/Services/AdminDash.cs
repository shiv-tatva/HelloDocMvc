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
using System.Net.Mail;
using System.Net;
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
                            phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                            region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                            reqid = r.Requestid,
                            email = rc.Email,
                            fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
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

                _context.SaveChanges();
            

        }

        public blockCaseModel blockcase(int req)
        {
            blockCaseModel _block = new blockCaseModel();

            _block.reqid = req;
            _block.first_name = _context.Requestclients.Where(r => r.Requestid == req).Select(r => r.Firstname).First();

            return _block;
        }

        public void blockcase(adminDashData obj, string sessionEmail)
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

            _context.Requeststatuslogs.Add(new Requeststatuslog
            {
                Requestid = obj._blockCaseModel.reqid,
                Status = 10,
                Notes = obj._blockCaseModel.description,
                Adminid = _context.Admins.FirstOrDefault(x => x.Email == sessionEmail).Adminid,
                Createddate = DateTime.Now,
            });

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
                email = r.Requestclients.Where(r =>r.Requestid == reqId).Select(r => r.Email).First(),
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

        //*************************************Mail**********************************************
        public void SendRegistrationEmail(string emailMain, string[] data)
        {
            string senderEmail = "nisargsojitra1234@outlook.com";
            string senderPassword = "Nisarg@#1705";
            SmtpClient client = new SmtpClient("smtp.office365.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(senderEmail, senderPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };


            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail),
                Subject = "Update Password for Account ",
                IsBodyHtml = true,
            };

                for(var i = 0;i< data.Length;i++)
            {
                string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data[i]);
                Attachment attachment = new Attachment(pathname);
                mailMessage.Attachments.Add(attachment);
            }
                
            


            mailMessage.To.Add(emailMain);

            client.Send(mailMessage);
        }

    

        public void sendMail(string emailMain, string[] data)
        {
            string emailConfirmationToken = Guid.NewGuid().ToString();
            
            try
            {
                SendRegistrationEmail(emailMain, data);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        //**************************************************************************************

        public activeOrder viewOrder(int reqId)
        {
            activeOrder _active = new activeOrder();

            _active.reqid = reqId;
            _active.profession = _context.Healthprofessionaltypes.ToList(); 

            return _active;
        }



        public activeOrder businessName(int profession_id)
        {
            activeOrder _active = new activeOrder();

            _active.business_data = _context.Healthprofessionals.Where(r => r.Profession == profession_id).Select(r => r.Vendorname).ToList();
            _active.business_id = _context.Healthprofessionals.Where(r => r.Profession == profession_id).Select(r => r.Vendorid).ToList();

            return _active;
        }


        public activeOrder businessDetail(int businessDetail)
        {
            activeOrder _active = new activeOrder();

            var query = _context.Healthprofessionals.Where(r => r.Vendorid == businessDetail).Select(r => new activeOrder()
            {
                email = r.Email,
                business_contact = r.Businesscontact,
                fax_num = r.Faxnumber
            });

            return query.ToList().FirstOrDefault();
        }


        public void viewOrder(adminDashData adminDashData)
        {
            Orderdetail orderdetail = new Orderdetail();

            orderdetail.Prescription = adminDashData._activeOrder.prescription;
            orderdetail.Email = adminDashData._activeOrder.email;
            orderdetail.Requestid = adminDashData._activeOrder.reqid;
            orderdetail.Vendorid = adminDashData._activeOrder.vendorid;
            orderdetail.Faxnumber = adminDashData._activeOrder.fax_num;
            orderdetail.Createddate = DateTime.Now;
            orderdetail.Businesscontact = adminDashData._activeOrder.business_contact;
            orderdetail.Noofrefill = adminDashData._activeOrder.Refill;

            _context.Add(orderdetail);
            _context.SaveChanges();

            var _health = _context.Healthprofessionals.Where(r => r.Vendorid == adminDashData._activeOrder.vendorid).Select(r => r).First();

            if(_health.Email != adminDashData._activeOrder.email || _health.Faxnumber != adminDashData._activeOrder.fax_num || _health.Businesscontact != adminDashData._activeOrder.business_contact)
            {
                _health.Email = adminDashData._activeOrder.email;
                _health.Faxnumber = adminDashData._activeOrder.fax_num;
                _health.Businesscontact = adminDashData._activeOrder.business_contact;

                _context.Healthprofessionals.Update(_health);
                _context.SaveChanges();
            }
        }
        public transferRequest transferReq(int req)
        {
            transferRequest transferRequest = new transferRequest();

            transferRequest.region_name = _context.Regions.Select(x => x.Name).ToList();
            transferRequest.region_id = _context.Regions.Select(y => y.Regionid).ToList();
            transferRequest.regions = _context.Regions.ToList();
            transferRequest.reqid = req;

            //assignCase.phy_req = _context.Physicianregions.ToList();
            return transferRequest;
        }

        public void transferReq(adminDashData data, string sessionEmail)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();

            var requestedRowPatient = _context.Requests.Where(x => x.Requestid == data.transferRequest.reqid).Select(r => r).First();


            requeststatuslog.Requestid = data.transferRequest.reqid;
            requeststatuslog.Notes = data.transferRequest.description;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Status = 2;
            requeststatuslog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            requeststatuslog.Transtophysicianid = data.transferRequest.phy_id_main;

            _context.Add(requeststatuslog);
            _context.SaveChanges();

            requestedRowPatient.Physicianid = data.transferRequest.phy_id_main;
            requestedRowPatient.Modifieddate = DateTime.Now;

            _context.SaveChanges();
        }

        public blockCaseModel clearCase(int reqId)
        {
            blockCaseModel blockCaseModel = new blockCaseModel();
            blockCaseModel.reqid = reqId;
            return blockCaseModel;
        }
        public void clearCase(adminDashData block, string sessionEmail)
        {
            var req_id = _context.Requests.Where(x => x.Requestid == block._blockCaseModel.reqid).Select(r => r).First();
            req_id.Status = 11;

            _context.SaveChanges();

            _context.Requeststatuslogs.Add(new Requeststatuslog
            {
                Requestid = block._blockCaseModel.reqid,
                Status = 11,
                Adminid = _context.Admins.FirstOrDefault(x => x.Email == sessionEmail).Adminid,
                Createddate = DateTime.Now,
            });

            _context.SaveChanges();

           
        }
    }
}
