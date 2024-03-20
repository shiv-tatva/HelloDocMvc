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
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using OfficeOpenXml;

namespace BLL_Business_Logic_Layer_.Services
{
    public class AdminDash : IAdminDash
    {
        private readonly ApplicationDbContext _context;

        public AdminDash(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<adminDash> adminData(int[] status, int typeId, int regionId)
        {
            var requestList = _context.Requests.Where(i => status.Contains(i.Status));

            var query = (from r in requestList
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
                            address = r.Requestclients.Select(x => x.Street).First() + "," + r.Requestclients.Select(x => x.City).First() + "," + r.Requestclients.Select(x => x.State).First(),
                            request_type_id = r.Requesttypeid,
                            status = r.Status,
                            region_id = rc.Regionid,
                            //phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                            //region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                            region_table = _context.Regions.ToList(),
                            reqid = r.Requestid,
                            email = rc.Email,
                            fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
                        }).ToList();

            if(typeId > 0)
            {
                query = query.Where(x => x.request_type_id == typeId).Select(r => r).ToList();
            }

            if(regionId > 0)
            {
                query = query.Where(x => x.region_id == regionId).Select(r => r).ToList();
            }

            //var result = query.ToList();

            return query;
        }

        public countMain countService()
        {
            var requestsWithClients = _context.Requests
                   .Join(_context.Requestclients,
                       r => r.Requestid,
                       rc => rc.Requestid,
                       (r, rc) => new { Request = r, RequestClient = rc })
                   .ToList();

            countMain statusCount = new countMain
            {
                count1 = requestsWithClients.Count(x => x.Request.Status == 1),
                count2 = requestsWithClients.Count(x => x.Request.Status == 2),
                count3 = requestsWithClients.Count(x => x.Request.Status == 4 || x.Request.Status == 5),
                count4 = requestsWithClients.Count(x => x.Request.Status == 6),
                count5 = requestsWithClients.Count(x => x.Request.Status == 3 || x.Request.Status == 7 || x.Request.Status == 8),
                count6 = requestsWithClients.Count(x => x.Request.Status == 9)
            };

            return statusCount;
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
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
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
                Subject = "Attachment",
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

        public sendAgreement sendAgree(int reqId)
        {
            sendAgreement sendAgreement = new sendAgreement();
            sendAgreement.reqid = reqId;
            sendAgreement.request_type_id = _context.Requests.Where(x => x.Requestid == reqId).Select(r => r.Requesttypeid).First();
            sendAgreement.email = _context.Requestclients.Where(x => x.Requestid == reqId).Select(r => r.Email).First();
            sendAgreement.mobile_num = _context.Requestclients.Where(x => x.Requestid == reqId).Select(r => r.Phonenumber).First();
            return sendAgreement;
        }

        public void sendAgree(adminDashData dataMain)
        {
            string registrationLink = "http://localhost:5145/patientDashboard/pendingReviewAgreement?reqId=" + dataMain._sendAgreement.reqid;

            try
            {
                SendRegistrationEmailMain(dataMain._sendAgreement.email, registrationLink);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        public void SendRegistrationEmailMain(string toEmail, string registrationLink)
        {
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
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
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Review Agreement",
                IsBodyHtml = true,
                Body = $"Click the following link to Review Agreement: <a href='{registrationLink}'>{registrationLink}</a>"
            };



            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }

        public closeCaseMain closeCaseMain(int reqId)
        {
            var reqClient = _context.Requestclients.Where(r => r.Requestid == reqId).Select(r=>new closeCaseMain()
            {
                mobile_num = r.Phonenumber,
                email = r.Email,
                fname = r.Firstname,
                lname = r.Lastname,
                fulldateofbirth = new DateTime((int)(r.Intyear), Convert.ToInt16(r.Strmonth), (int)(r.Intdate)).ToString("yyyy-MM-dd"),
                reqid=r.Requestid,
            }).ToList().FirstOrDefault() ;

            var cnf = _context.Requests.Where(r => r.Requestid == reqId).FirstOrDefault().Confirmationnumber;
            var requestWiseFile = _context.Requestwisefiles.Where(r => r.Requestid == reqId && r.Isdeleted == null).Select(r => r).ToList();

            reqClient.confirmation_no = cnf;
            reqClient._requestWiseFile = requestWiseFile;

            return reqClient;
        }


        public void closeCaseSaveMain(adminDashData obj) 
        { 

        var requestStatus = _context.Requestclients.Where(r => r.Requestid == obj._closeCaseMain.reqid).Select(r => r).First();
        requestStatus.Email = obj._closeCaseMain.email;
        requestStatus.Phonenumber = obj._closeCaseMain.mobile_num;

        _context.SaveChanges();         
            
        }

        public void closeCaseCloseBtn(int reqId, string sessionEmail)
        {

            var requestStatus = _context.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();

            requestStatus.Status = 9;

            _context.Requeststatuslogs.Add(new Requeststatuslog()
            {
                Requestid = reqId,
                Status = 9,
                Adminid = _context.Admins.FirstOrDefault(x => x.Email == sessionEmail).Adminid,
                Createddate = DateTime.Now,
            });
            _context.SaveChanges();         
        }

        public myProfile myProfile(string sessionEmail)
        {
            var myProfileMain = _context.Admins.Where(x => x.Email == sessionEmail).Select(x => new myProfile()
            {
                fname = x.Firstname,
                lname = x.Lastname,
                email = x.Email,
                confirm_email = x.Email,
                mobile_no = x.Mobile,
                addr1 = x.Address1,
                addr2 = x.Address2,
                city = x.City,
                zip = x.Zip,
                state = _context.Regions.Where(r => r.Regionid == x.Regionid).Select(r => r.Name).First(),
                roles = _context.Aspnetroles.ToList(),
            }).ToList().FirstOrDefault();

            var userName = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Username).First();
            var pass = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Passwordhash).First();

            myProfileMain.username = userName;
            myProfileMain.password = pass;

            return myProfileMain;
        }


        public bool myProfileReset(myProfile obj, string sessionEmail)
        {
            var aspUser = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if(aspUser.Passwordhash != obj.password)
            {
                var pass = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();
                pass.Passwordhash = obj.password;

                _context.SaveChanges();

                return true;
            }
            return false;

        }


        public myProfile myProfileAdminInfo(myProfile obj, string sessionEmail)
        {
            myProfile _myprofile = new myProfile();
            var aspUser = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

            var adminInfo = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if(adminInfo.Firstname != obj.fname || adminInfo.Lastname != obj.lname || adminInfo.Email != obj.email || adminInfo.Mobile != obj.mobile_no)
            {
                if(adminInfo.Firstname != obj.fname)
                {
                    adminInfo.Firstname = obj.fname;
                    aspUser.Username = obj.fname;
                }
                
                if(adminInfo.Lastname != obj.lname)
                {
                    adminInfo.Lastname = obj.lname;
                }

                if(adminInfo.Email != obj.email)
                {
                    adminInfo.Email = obj.email;
                    aspUser.Email = obj.email;
                }

                if(adminInfo.Mobile != obj.mobile_no)
                {
                    adminInfo.Mobile = obj.mobile_no;
                    aspUser.Phonenumber = obj.mobile_no;
                }
                    
                
                aspUser.Modifieddate = DateTime.Now;
                _myprofile.indicate = true;
                _myprofile.email = obj.email;
                _context.SaveChanges();
            }
            else
            {
                _myprofile.indicate = false;
            }
            _myprofile.email = obj.email;
            return _myprofile;
        }

        public bool myProfileAdminBillingInfo(myProfile obj, string sessionEmail)
        {
            var adminInfo = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if(adminInfo.Address1 != obj.addr1 || adminInfo.Address2 != obj.addr2 || adminInfo.City != obj.city ||adminInfo.Zip != obj.zip)
            {

                if(adminInfo.Address1 != obj.addr1)
                {
                    adminInfo.Address1 = obj.addr1;
                }
                
                if(adminInfo.Address2 != obj.addr2)
                {
                    adminInfo.Address2 = obj.addr2;
                }
               
                if(adminInfo.City != obj.city)
                {
                    adminInfo.City = obj.city;
                }
                    
                if(adminInfo.Zip != obj.zip)
                {
                    adminInfo.Zip = obj.zip;
                }
                
                _context.SaveChanges();

                return true;
            }

            return false;
        }


        public concludeEncounter concludeEncounter(int data)
        {
            var obj = _context.Requestclients.Where(r => r.Requestid == data).Select(r => r).First();
           
            var obj3 = _context.EncounterForms.FirstOrDefault(r => r.Requestid == data);

            concludeEncounter _encounterData = new concludeEncounter();

            _encounterData.reqid = data;
            _encounterData.FirstName = obj.Firstname;
            _encounterData.LastName = obj.Lastname;
            _encounterData.Location = obj.Street + obj.City + obj.State + obj.Zipcode;
            _encounterData.BirthDate = new DateTime((int)(obj.Intyear), Convert.ToInt16(obj.Strmonth), (int)(obj.Intdate)).ToString("yyyy-MM-dd");
            _encounterData.PhoneNumber = obj.Phonenumber;
            _encounterData.Email = obj.Email;
            if(obj3 != null)
            {
                var obj2 = _context.EncounterForms.Where(r => r.Requestid == data).Select(r => r).First();

                _encounterData.HistoryIllness = obj2.HistoryIllness;
                _encounterData.MedicalHistory = obj2.MedicalHistory;
                _encounterData.Date = obj2.Date;
                _encounterData.Medications = obj2.Medications;
                _encounterData.Allergies = obj2.Allergies;
                _encounterData.Temp = obj2.Temp;
                _encounterData.Hr = obj2.Hr;
                _encounterData.Rr = obj2.Rr;
                _encounterData.BpS = obj2.BpS;
                _encounterData.BpD = obj2.BpD;
                _encounterData.O2 = obj2.O2;
                _encounterData.Pain = obj2.Pain;
                _encounterData.Heent = obj2.Heent;
                _encounterData.Cv = obj2.Cv;
                _encounterData.Chest = obj2.Chest;
                _encounterData.Abd = obj2.Abd;
                _encounterData.Extr = obj2.Extr;
                _encounterData.Skin = obj2.Skin;
                _encounterData.Neuro = obj2.Neuro;
                _encounterData.Other = obj2.Other;
                _encounterData.Diagnosis = obj2.Diagnosis;
                _encounterData.TreatmentPlan = obj2.TreatmentPlan;
                _encounterData.MedicationDispensed = obj2.MedicationDispensed;
                _encounterData.Procedures = obj2.Procedures;
                _encounterData.FollowUp = obj2.FollowUp;
            }
            

            return _encounterData;
 
        }


        public concludeEncounter concludeEncounter(concludeEncounter obj)
        {
            concludeEncounter _obj = new concludeEncounter();

                var obj1 = _context.EncounterForms.FirstOrDefault(r => r.Requestid == obj.reqid);

                if(obj1 == null)
                {
                    EncounterForm _encounter = new EncounterForm()
                    {
                        Requestid = obj.reqid,
                        HistoryIllness = obj.HistoryIllness,
                        MedicalHistory = obj.MedicalHistory,
                        Date = obj.Date,
                        Medications = obj.Medications,
                        Allergies = obj.Allergies,
                        Temp = obj.Temp,
                        Hr = obj.Hr,
                        Rr = obj.Rr,
                        BpS = obj.BpS,
                        BpD = obj.BpD,
                        O2 = obj.O2,
                        Pain = obj.Pain,
                        Heent = obj.Heent,
                        Cv = obj.Cv,
                        Chest = obj.Chest,
                        Abd = obj.Abd,
                        Extr = obj.Extr,
                        Skin = obj.Skin,
                        Neuro = obj.Neuro,
                        Other = obj.Other,
                        Diagnosis = obj.Diagnosis,
                        TreatmentPlan = obj.TreatmentPlan,
                        MedicationDispensed = obj.MedicationDispensed,
                        Procedures = obj.Procedures,
                        FollowUp = obj.FollowUp,
                    };

                _context.EncounterForms.Add(_encounter);

                _obj.indicate = true;
            }
                else
                {
                    var obj2 = _context.EncounterForms.Where(r => r.Requestid == obj.reqid).Select(r => r).First();
                    obj2.Requestid = obj.reqid;
                    obj2.HistoryIllness = obj.HistoryIllness;
                    obj2.MedicalHistory = obj.MedicalHistory;
                    obj2.Date = obj.Date;
                    obj2.Medications = obj.Medications;
                    obj2.Allergies = obj.Allergies;
                    obj2.Temp = obj.Temp;
                    obj2.Hr = obj.Hr;
                    obj2.Rr = obj.Rr;
                    obj2.BpS = obj.BpS;
                    obj2.BpD = obj.BpD;
                    obj2.O2 = obj.O2;
                    obj2.Pain = obj.Pain;
                    obj2.Heent = obj.Heent;
                    obj2.Cv = obj.Cv;
                    obj2.Chest = obj.Chest;
                    obj2.Abd = obj.Abd;
                    obj2.Extr = obj.Extr;
                    obj2.Skin = obj.Skin;
                    obj2.Neuro = obj.Neuro;
                    obj2.Other = obj.Other;
                    obj2.Diagnosis = obj.Diagnosis;
                    obj2.TreatmentPlan = obj.TreatmentPlan;
                    obj2.MedicationDispensed = obj.MedicationDispensed;
                    obj2.Procedures = obj.Procedures;
                    obj2.FollowUp = obj.FollowUp;

                    _obj.indicate = true;
            };

           
            _context.SaveChanges();

            return _obj;
        }


        public sendLink sendLink(sendLink data)
        {
            sendLink _send = new sendLink();

            string registrationLink = "http://localhost:5145/Patient/SubmitRequest";

            try
            {
                SendRegistrationEmailSendLink(data.Email, registrationLink);
                _send.indicate = true;
            }
            catch (Exception e)
            {
                _send.indicate = false;
            }

            return _send;
        }

        public void SendRegistrationEmailSendLink(string email,string registrationLink)
        {
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
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
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Review Agreement",
                IsBodyHtml = true,
                Body = $"Click the following link to Create Request: <a href='{registrationLink}'>{registrationLink}</a>"
            };



            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }

      

        public createRequest createRequest(createRequest data, string sessionEmail)
        {
            createRequest _create = new createRequest();

            var stateMain = _context.Regions.Where(r => r.Name.ToLower() == data.state.ToLower()).FirstOrDefault();

            if (stateMain == null)
            {
                _create.indicate = false;
            }
            else
            {
                Request _req = new Request();
                Requestclient _reqClient = new Requestclient();
                User _user = new User();
                Aspnetuser _asp = new Aspnetuser();
                Requestnote _note = new Requestnote();

                var _admin = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

                var existUser = _context.Aspnetusers.FirstOrDefault(r => r.Email == data.email);

                if (existUser == null)
                {
                    _asp.Username = data.firstname + "_" + data.lastname;
                    _asp.Email = data.email;
                    _asp.Phonenumber = data.phone;
                    _asp.Createddate = DateTime.Now;
                    _context.Aspnetusers.Add(_asp);
                    _context.SaveChanges();

                    _user.Aspnetuserid = _asp.Id;
                    _user.Firstname = data.firstname;
                    _user.Lastname = data.lastname;
                    _user.Email = data.email;
                    _user.Mobile = data.phone;
                    _user.City = data.city;
                    _user.State = data.state;
                    _user.Street = data.street;
                    _user.Zipcode = data.zipcode;
                    _user.Strmonth = data.dateofbirth.Substring(5, 2);
                    _user.Intdate = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                    _user.Intyear = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
                    _user.Createdby = _asp.Id;
                    _user.Createddate = DateTime.Now;
                    _user.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.ToLower()).Select(r => r.Regionid).FirstOrDefault();
                    _context.Users.Add(_user);
                    _context.SaveChanges();

                    string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _asp.Id;

                    try
                    {
                        SendRegistrationEmailCreateRequest(data.email, registrationLink);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                _req.Requesttypeid = 1;
                _req.Userid = _admin.Aspnetuserid;
                _req.Firstname = _admin.Firstname;
                _req.Lastname = _admin.Lastname;
                _req.Phonenumber = _admin.Mobile;
                _req.Email = _admin.Email;
                _req.Status = 1;
                _req.Confirmationnumber = _admin.Firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
                _req.Createddate = DateTime.Now;

                _context.Requests.Add(_req);
                _context.SaveChanges();

               

                _reqClient.Requestid = _req.Requestid;
                _reqClient.Firstname = data.firstname;
                _reqClient.Lastname = data.lastname;
                _reqClient.Phonenumber = data.phone;
                _reqClient.Strmonth = data.dateofbirth.Substring(5, 2);
                _reqClient.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2)); 
                _reqClient.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                _reqClient.Street = data.street;
                _reqClient.City = data.city;
                _reqClient.State = data.state;
                _reqClient.Zipcode = data.zipcode;
                _reqClient.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.ToLower()).Select(r => r.Regionid).FirstOrDefault();
                _reqClient.Email = data.email;

                _context.Requestclients.Add(_reqClient);
                _context.SaveChanges();

                _note.Requestid = _req.Requestid;
                _note.Adminnotes = data.admin_notes;
                _note.Createdby = _context.Aspnetusers.Where(r => r.Email == data.email).Select(r => r.Id).FirstOrDefault();
                _note.Createddate = DateTime.Now;
                _context.Requestnotes.Add(_note);
                _context.SaveChanges();

                _create.indicate = true;
            }

            return _create;
        }


        public void SendRegistrationEmailCreateRequest(string email,string registrationLink)
        {
            string senderEmail = "shivsantoki303@outlook.com";
            string senderPassword = "Shiv@123";
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
                From = new MailAddress(senderEmail, "HalloDoc"),
                Subject = "Create Account",
                IsBodyHtml = true,
                Body = $"Click the following link to Create Account: <a href='{registrationLink}'>{registrationLink}</a>"
            };



            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }

        public createRequest verifyState(string state)
        {
            createRequest _create = new createRequest();

            var stateMain = _context.Regions.Where(r => r.Name.ToLower() == state.ToLower()).FirstOrDefault();
                        
            if(stateMain == null)
            {
                _create.indicate = false;
            }
            else
            {
                _create.indicate = true;
            }

            return _create;
        }


        public byte[] GenerateExcelFile(List<adminDash> adminData)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Requests");

                // Add headers
                worksheet.Cells[1, 1].Value = "Request ID";
                worksheet.Cells[1, 2].Value = "Name";
                worksheet.Cells[1, 3].Value = "DOB";
                worksheet.Cells[1, 4].Value = "Address";
                worksheet.Cells[1, 5].Value = "Requestor";
                worksheet.Cells[1, 6].Value = "Phone";
                worksheet.Cells[1, 7].Value = "Request Date";
                worksheet.Cells[1, 8].Value = "Request Type ID";
                worksheet.Cells[1, 9].Value = "Notes";
                worksheet.Cells[1, 10].Value = "Email";

                // Populate data
                for (int i = 0; i < adminData.Count; i++)
                {
                    var rowData = adminData[i];
                    worksheet.Cells[i + 2, 1].Value = rowData.reqid;
                    worksheet.Cells[i + 2, 2].Value = rowData.first_name;
                    worksheet.Cells[i + 2, 3].Value = rowData.fulldateofbirth;
                    worksheet.Cells[i + 2, 4].Value = rowData.address;
                    worksheet.Cells[i + 2, 5].Value = rowData.requestor_fname;
                    worksheet.Cells[i + 2, 6].Value = rowData.mobile_num;
                    worksheet.Cells[i + 2, 7].Value = rowData.created_date;
                    worksheet.Cells[i + 2, 8].Value = rowData.request_type_id;
                    worksheet.Cells[i + 2, 9].Value = rowData.notes;
                    worksheet.Cells[i + 2, 10].Value = rowData.email;
                }

                // Convert package to bytes for download
                return excelPackage.GetAsByteArray();
            }
        }
    }
}
