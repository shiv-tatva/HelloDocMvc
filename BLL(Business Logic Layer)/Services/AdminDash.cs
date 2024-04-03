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
using Microsoft.CodeAnalysis.CSharp.Syntax;
using GoogleMaps.LocationServices;
using System;
using System.Threading.Tasks;
using Geocoding.Microsoft;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
                            //region_table = _context.Regions.ToList(),
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
            //if(query.Count() == 0)
            //{
            //    query[0].status = status[0];
            //}
            //var result = query.ToList();

            return query;
        }

        public countMain countService(string sessionName)
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

        public List<string> GetListOfRoleMenu(int roleId)
        {
            List<Rolemenu> roleMenus = _context.Rolemenus.Where(u => u.Roleid == roleId).ToList();
            if (roleMenus.Count > 0)
            {
                List<string> menus = new List<string>();
                foreach (var item in roleMenus)
                {
                    menus.Add(_context.Menus.FirstOrDefault(u => u.Menuid == item.Menuid).Name);
                }
                return menus;
            }
            else
            {
                return new List<string>();
            }
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
                Aspnetuserrole _role = new Aspnetuserrole();

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
                    _user.Intdate = Convert.ToInt16(data.dateofbirth.Substring(8, 2));
                    _user.Intyear =  Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                    _user.Createdby = _asp.Id;
                    _user.Createddate = DateTime.Now;
                    _user.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.ToLower()).Select(r => r.Regionid).FirstOrDefault();
                    _context.Users.Add(_user);
                    _context.SaveChanges();

                    _role.Userid = _asp.Id;
                    _role.Roleid = 2;

                    _context.Aspnetuserroles.Add(_role);
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
                _req.Userid = _context.Users.Where(r => r.Email == data.email).Select(r => r.Userid).First();
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

        //***********************************Provider*************************************

        public provider providerMain(int regionId=0)
        {
            BitArray deletedBit = new BitArray(new[] { false });

            provider provider = new provider()
            {
                _physician = _context.Physicians.ToList(),
                _notification = _context.Physiciannotifications.ToList(),
                _roles = _context.Roles.Where(r => r.Isdeleted.Equals(deletedBit)).Select(r =>r).ToList(),
                _shift = _context.Shifts.ToList(),
                _shiftDetails = _context.Shiftdetails.ToList(),
                DateTime = DateTime.Now,
            };

            if(regionId != 0)
            {
                provider._physician = _context.Physicians.Where(r => r.Regionid == regionId).ToList();
            }

            return provider;
        }

        public provider stopNotification(int phyId)
        {
            provider provider = new provider();

            var phyNotification = _context.Physiciannotifications.Where(r => r.Physicianid == phyId).Select(r => r).First();

            var notification = new BitArray(1);
            notification[0] = false;

            if (phyNotification.Isnotificationstopped[0] == notification[0])
            {
                phyNotification.Isnotificationstopped = new BitArray(1);
                phyNotification.Isnotificationstopped[0] = true;
                _context.SaveChanges();

                provider.indicate = true;
                return provider;
            }
            else
            {
                phyNotification.Isnotificationstopped = new BitArray(1);
                phyNotification.Isnotificationstopped[0] = false;
                _context.SaveChanges();

                provider.indicate = false;
                return provider;
            }
        }

        public provider providerContact(int phyId)
        {
            provider _provider = new provider()
            {
                phyId = phyId,
            };

            return _provider;
        }

        public provider providerContactEmail(int phyIdMain,string msg, string sessionEmail)
        {
            provider _provider = new provider();
            
            _provider.phyId = phyIdMain;

            var provider = _context.Physicians.Where(r => r.Physicianid == phyIdMain).Select(r => r.Email).First();                       

            try
            {
                SendRegistrationproviderContactEmail(provider, msg, sessionEmail, phyIdMain);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
                    
            return _provider;
        }

        private void SendRegistrationproviderContactEmail(string provider,string msg, string sessionEmail, int phyIdMain)
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
                Subject = "Mail For provider",
                IsBodyHtml = true,
                Body = $"{msg}",
            };

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + provider + "Subject : " + mailMessage.Subject + "Message : " + msg,
                Emailid = provider,
                Roleid = 1,
                Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First(),
                Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),

            };

            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();

            mailMessage.To.Add(provider);

            client.Send(mailMessage);
        }

        public AdminEditPhysicianProfile adminEditPhysicianProfile(int phyId, string sessionEmail)
        {
            var phy = _context.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            var user = _context.Aspnetusers.Where(r => r.Email == phy.Email).First();

            AdminEditPhysicianProfile _profile = new AdminEditPhysicianProfile()
            {              
                //username = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Username).First(),
                Firstname = phy.Firstname,
                Lastname = phy.Lastname,
                Email = phy.Email,
                PhoneNumber = phy.Mobile,
                MedicalLicesnse = phy.Medicallicense,
                NPInumber = phy.Npinumber,
                SycnEmail = phy.Syncemailaddress,
                Address1 = phy.Address1,
                Address2 = phy.Address2,
                city = phy.City,
                zipcode = phy.Zip,
                altPhone = phy.Altphone,
                Businessname = phy.Businessname,
                BusinessWebsite = phy.Businesswebsite,
                Adminnotes = phy.Adminnotes,
                statusId = (int)phy.Status,
                PhyID = phyId,
                Roleid = phy.Roleid,
                Regionid = phy.Regionid,
                PhotoValue = phy.Photo,
                SignatureValue = phy.Signature,
                IsContractorAgreement = phy.Isagreementdoc == null ? false : true,
                IsBackgroundCheck = phy.Isbackgrounddoc == null ? false : true,
                IsHIPAA = phy.Istrainingdoc == null ? false : true,
                IsNonDisclosure = phy.Isnondisclosuredoc == null ? false : true,
                IsLicenseDocument = phy.Islicensedoc == null ? false : true,                

                username = user.Username,
                password = user.Passwordhash,
            };

            return _profile;
        }

        List<DAL_Data_Access_Layer_.DataModels.Region> IAdminDash.RegionTable()
        {
            var region = _context.Regions.ToList();
            return region;
        }

        public List<PhysicianRegionTable> PhyRegionTable(int phyId)
        {
            var region = _context.Regions.ToList();
            var phyRegion = _context.Physicianregions.ToList();

            var checkedRegion = region.Select(r1 => new PhysicianRegionTable
            {
                Regionid = r1.Regionid,
                Name = r1.Name,
                ExistsInTable = phyRegion.Any(r2 => r2.Physicianid == phyId && r2.Regionid == r1.Regionid),
            }).ToList();

            return checkedRegion;
        }

        public List<Role> physicainRole()
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var role = _context.Roles.Where(i => i.Isdeleted.Equals(deletedBit)).ToList();

            return role;
        }

        public bool providerResetPass(string email, string password)
        {
            var resetPass = _context.Aspnetusers.Where(r => r.Email == email).Select(r => r).First();

            if(resetPass.Passwordhash != password)
            {
                resetPass.Passwordhash = password;
                _context.SaveChanges();

                return true;
            }
            return false;

        }

        public bool editProviderForm1(int phyId, int roleId,int statusId )
        {
            var user = _context.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            if(user.Status != (short)statusId || user.Roleid != roleId)
            {
                user.Status = (short)statusId;
                user.Roleid = roleId;
                    
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public bool editProviderForm2(string fname, string lname, string email, string phone, string medical, string npi, string sync, int phyId, int[] phyRegionArray)
        {
            var indicate = false;

            var user = _context.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();
            var aspUser = _context.Aspnetusers.Where(r => r.Id == user.Aspnetuserid).Select(r => r).First();
            var location = _context.Physicianlocations.Where(r => r.Physicianid == phyId).Select(r => r).First();


            if (user.Firstname != fname || user.Lastname != lname || user.Email != email || user.Mobile != phone || user.Medicallicense != medical || user.Npinumber != npi || user.Syncemailaddress != sync)
            {
                user.Firstname = fname;
                user.Lastname = lname;
                if(user.Email != email)
                {
                    user.Email = email;
                    aspUser.Email = email;
                }
                
                user.Mobile = phone;
                user.Medicallicense = medical;
                user.Npinumber = npi;
                user.Syncemailaddress = sync;

                _context.SaveChanges();

                indicate = true;

                location.Physicianname = fname;

                _context.SaveChanges();
            }

            var abc = _context.Physicianregions.Where(x => x.Physicianid == phyId).Select(r => r.Regionid).ToList();

            var changes = abc.Except(phyRegionArray);


            if (changes.Any() || abc.Count() != phyRegionArray.Length)
            {
                if (_context.Physicianregions.Any(x => x.Physicianid == phyId))
                {
                    var physicianRegion = _context.Physicianregions.Where(x => x.Physicianid == phyId).ToList();

                    _context.Physicianregions.RemoveRange(physicianRegion);
                    _context.SaveChanges();
                }

                var phyRegion = _context.Physicianregions.ToList();

                foreach (var item in phyRegionArray)
                {
                    var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                    _context.Physicianregions.Add(new Physicianregion
                    {
                        Physicianid = phyId,
                        Regionid = region.Regionid,
                    });
                }
                _context.SaveChanges();
                indicate = true;
            }         

            return indicate;
        }


        public AdminEditPhysicianProfile editProviderForm3(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var data = _context.Physicians.Where(r => r.Physicianid == dataMain._providerEdit.PhyID).Select(r => r).First();
            var location = _context.Physicianlocations.Where(r => r.Physicianid == dataMain._providerEdit.PhyID).Select(r => r).First();

            if(data.Address1 != dataMain._providerEdit.Address1 || data.Address2 != dataMain._providerEdit.Address2 || data.City != dataMain._providerEdit.city || data.Regionid != dataMain._providerEdit.Regionid || data.Zip != dataMain._providerEdit.zipcode || data.Altphone != dataMain._providerEdit.altPhone)
            {
                data.Address1 = dataMain._providerEdit.Address1;
                data.Address2 = dataMain._providerEdit.Address2;
                data.City = dataMain._providerEdit.city;
                data.Regionid = dataMain._providerEdit.Regionid;
                data.Zip = dataMain._providerEdit.zipcode;
                data.Altphone = dataMain._providerEdit.altPhone;

                _context.SaveChanges();

                location.Address = dataMain._providerEdit.Address1;
                location.Latitude = dataMain._providerEdit.latitude;
                location.Longitude = dataMain._providerEdit.longitude;

                _context.SaveChanges();

                flag.PhyID = dataMain._providerEdit.PhyID;
                flag.indicate = true;

                return flag;
            }

            flag.PhyID = dataMain._providerEdit.PhyID;
            flag.indicate = false;

            return flag;
        }

       public AdminEditPhysicianProfile PhysicianBusinessInfoUpdate(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var physician = _context.Physicians.FirstOrDefault(x => x.Physicianid == dataMain._providerEdit.PhyID);

            if (physician != null)
            {                
                    physician.Businessname = dataMain._providerEdit.Businessname;
                    physician.Businesswebsite = dataMain._providerEdit.BusinessWebsite;
                    physician.Adminnotes = dataMain._providerEdit.Adminnotes;
                    physician.Modifieddate = DateTime.Now;

                    _context.SaveChanges();

                    if (dataMain._providerEdit.Photo != null || dataMain._providerEdit.Signature != null)
                    {
                        AddProviderBusinessPhotos(dataMain._providerEdit.Photo, dataMain._providerEdit.Signature, dataMain._providerEdit.PhyID);
                    }                   
                
            }
            flag.indicate = true;
            flag.PhyID = dataMain._providerEdit.PhyID;
            return flag;
        }

        public void AddProviderBusinessPhotos(IFormFile photo, IFormFile signature, int phyId)
        {
            var physician = _context.Physicians.FirstOrDefault(x => x.Physicianid == phyId);

            if (photo != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", photo.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }

                physician.Photo = photo.FileName;
            }

            if (signature != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", signature.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    signature.CopyTo(fileStream);
                }

                physician.Signature = signature.FileName;
            }

            _context.SaveChanges();

        }




        public AdminEditPhysicianProfile EditOnBoardingData(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var physicianData = _context.Physicians.FirstOrDefault(x => x.Physicianid == dataMain._providerEdit.PhyID);

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", physicianData.Physicianid.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (dataMain._providerEdit.ContractorAgreement != null)
            {
                string path = Path.Combine(directory, "Independent_Contractor" + Path.GetExtension(dataMain._providerEdit.ContractorAgreement.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain._providerEdit.ContractorAgreement.CopyTo(fileStream);
                }

                physicianData.Isagreementdoc = new BitArray(1, true);
            }

            if (dataMain._providerEdit.BackgroundCheck != null)
            {
                string path = Path.Combine(directory, "Background" + Path.GetExtension(dataMain._providerEdit.BackgroundCheck.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain._providerEdit.BackgroundCheck.CopyTo(fileStream);
                }

                physicianData.Isbackgrounddoc = new BitArray(1, true);
            }

            if (dataMain._providerEdit.HIPAA != null)
            {
                string path = Path.Combine(directory, "HIPAA" + Path.GetExtension(dataMain._providerEdit.HIPAA.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain._providerEdit.HIPAA.CopyTo(fileStream);
                }

                physicianData.Istrainingdoc = new BitArray(1, true);
            }

            if (dataMain._providerEdit.NonDisclosure != null)
            {
                string path = Path.Combine(directory, "Non_Disclosure" + Path.GetExtension(dataMain._providerEdit.NonDisclosure.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain._providerEdit.NonDisclosure.CopyTo(fileStream);
                }

                physicianData.Isnondisclosuredoc = new BitArray(1, true);
            }

            if (dataMain._providerEdit.LicenseDocument != null)
            {
                string path = Path.Combine(directory, "Licence" + Path.GetExtension(dataMain._providerEdit.LicenseDocument.FileName));

                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    dataMain._providerEdit.LicenseDocument.CopyTo(fileStream);
                }

                physicianData.Islicensedoc = new BitArray(1, true);
            }

            _context.SaveChanges();

            flag.PhyID = dataMain._providerEdit.PhyID;
            flag.indicate = true;

            return flag;

        }

        public AdminEditPhysicianProfile createProviderAccount(adminDashData obj, List<int> physicianRegions)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var aspUser = _context.Aspnetusers.FirstOrDefault(r => r.Email == obj._providerEdit.Email);


            if(aspUser == null && obj._providerEdit.latitude != null)
            {
                

                Aspnetuser _user = new Aspnetuser();
                Physician phy = new Physician();

                _user.Username = obj._providerEdit.username;
                _user.Passwordhash = obj._providerEdit.password;
                _user.Email = obj._providerEdit.Email;
                _user.Phonenumber = obj._providerEdit.PhoneNumber;
                _user.Createddate = DateTime.Now;

                _context.Aspnetusers.Add(_user);
                _context.SaveChanges();



                phy.Aspnetuserid = _user.Id;
                phy.Firstname = obj._providerEdit.Firstname;
                phy.Lastname = obj._providerEdit.Lastname;
                phy.Email = obj._providerEdit.Email;
                phy.Mobile = obj._providerEdit.PhoneNumber;
                phy.Medicallicense = obj._providerEdit.MedicalLicesnse;
                phy.Adminnotes = obj._providerEdit.Adminnotes;
                phy.Address1 = obj._providerEdit.Address1;
                phy.Address2 = obj._providerEdit.Address2;
                phy.City = obj._providerEdit.city;
                phy.Regionid = obj._providerEdit.Regionid;
                phy.Zip = obj._providerEdit.zipcode;
                phy.Altphone = obj._providerEdit.altPhone;
                phy.Createdby = null;
                phy.Createddate = _user.Createddate;
                phy.Status = 1;
                phy.Businessname = obj._providerEdit.Businessname;
                phy.Businesswebsite = obj._providerEdit.BusinessWebsite;
                phy.Roleid = obj._providerEdit.Roleid;
                phy.Syncemailaddress = obj._providerEdit.SycnEmail;

                _context.Physicians.Add(phy);
                _context.SaveChanges();



                foreach (var item in physicianRegions)
                {
                    var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                    _context.Physicianregions.Add(new Physicianregion
                    {
                        Physicianid = phy.Physicianid,
                        Regionid = region.Regionid,
                    });
                }
                _context.SaveChanges();

                Physiciannotification notification = new Physiciannotification();
                notification.Physicianid = phy.Physicianid;

                _context.Physiciannotifications.Add(notification);
                _context.SaveChanges();

                Aspnetuserrole _userRole = new Aspnetuserrole();
                _userRole.Userid = _user.Id;
                _userRole.Roleid = 3;

                _context.Aspnetuserroles.Add(_userRole);
                _context.SaveChanges();

                
                    Physicianlocation _phyLoc = new Physicianlocation();
                    _phyLoc.Physicianid = phy.Physicianid;
                    _phyLoc.Latitude = obj._providerEdit.latitude;
                    _phyLoc.Longitude = obj._providerEdit.longitude;
                    _phyLoc.Createddate = DateTime.Now;
                    _phyLoc.Physicianname = phy.Firstname;
                    _phyLoc.Address = phy.Address1;

                    _context.Physicianlocations.Add(_phyLoc);
                    _context.SaveChanges();
                             

                AddProviderDocuments(phy.Physicianid, obj._providerEdit.Photo, obj._providerEdit.ContractorAgreement, obj._providerEdit.BackgroundCheck, obj._providerEdit.HIPAA, obj._providerEdit.NonDisclosure);

                flag.indicateTwo = "done";
                return flag;
            }
            else if(aspUser != null)
            {
                flag.indicateTwo = "email";
            }
            else
            {
                flag.indicateTwo = "zip";
            }

            return flag;
        }

        public void AddProviderDocuments(int Physicianid, IFormFile Photo, IFormFile ContractorAgreement, IFormFile BackgroundCheck, IFormFile HIPAA, IFormFile NonDisclosure)
        {
            var physicianData = _context.Physicians.FirstOrDefault(x => x.Physicianid == Physicianid);

            string directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", Physicianid.ToString());

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            if (Photo != null)
            {
                string path = Path.Combine(directory, "Profile" + Path.GetExtension(Photo.FileName));
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", Photo.FileName);

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    Photo.CopyTo(fileStream);
                }

                physicianData.Photo = Photo.FileName;
            }


            if (ContractorAgreement != null)
            {
                string path = Path.Combine(directory, "Independent_Contractor" + Path.GetExtension(ContractorAgreement.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    ContractorAgreement.CopyTo(fileStream);
                }

                physicianData.Isagreementdoc = new BitArray(1, true);
            }

            if (BackgroundCheck != null)
            {
                string path = Path.Combine(directory, "Background" + Path.GetExtension(BackgroundCheck.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    BackgroundCheck.CopyTo(fileStream);
                }

                physicianData.Isbackgrounddoc = new BitArray(1, true);
            }

            if (HIPAA != null)
            {
                string path = Path.Combine(directory, "HIPAA" + Path.GetExtension(HIPAA.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    HIPAA.CopyTo(fileStream);
                }

                physicianData.Istrainingdoc = new BitArray(1, true);
            }

            if (NonDisclosure != null)
            {
                string path = Path.Combine(directory, "Non_Disclosure" + Path.GetExtension(NonDisclosure.FileName));

                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    NonDisclosure.CopyTo(fileStream);
                }

                physicianData.Isnondisclosuredoc = new BitArray(1, true);
            }

            _context.SaveChanges();
        }

        public void editProviderDeleteAccount(int phyId)
        {
            var phy = _context.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            if (phy.Isdeleted == null)
            {
                phy.Isdeleted = new BitArray(1);
                phy.Isdeleted[0] = true;

                _context.SaveChanges();
            }     

            var phyReg = _context.Physicianregions.Where(r => r.Physicianid == phyId).ToList();
            if (phyReg != null)
            { 
                _context.Physicianregions.RemoveRange(phyReg);
                _context.SaveChanges();
            }

        }

        


        //*************************************************************Access***********************************************************

        public List<AccountAccess> GetAccountAccessData()
        {
            BitArray deletedBit = new BitArray(new[] { false });
            var Roles = _context.Roles.Where(i => i.Isdeleted.Equals(deletedBit));
            var Accessdata = Roles.Select(r => new AccountAccess()
            {
                name = r.Name,
                accounttype = _context.Aspnetroles.FirstOrDefault(x => x.Id == r.Accounttype).Name,
                accounttypeid = r.Accounttype,
                roleid = r.Roleid,
            }).ToList();
            return Accessdata;
        }

        public List<Aspnetrole> GetAccountType()
        {
            var role = _context.Aspnetroles.ToList();
            return role;
        }

        public List<Menu> GetMenu(int accounttype)
        {
            if (accounttype != 0)
            {
                var menu = _context.Menus.Where(r => r.Accounttype == accounttype).ToList();
                return menu;
            }
            else
            {

                var menu = _context.Menus.ToList();
                return menu;
            }
        }

        public void SetCreateAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string UserSession)
        {
            var user = _context.Aspnetusers.Where(r => r.Email == UserSession).Select(r => r).First();
            
            if (accountAccess != null)
            {
                var role = new Role()
                {
                    Name = accountAccess.name,
                    Accounttype = (short)accountAccess.accounttypeid,
                    Createdby = user.Id,
                    Createddate = DateTime.Now,
                    Isdeleted = new BitArray(1, false),
                };
                _context.Add(role);
                _context.SaveChanges();

                if (AccountMenu != null)
                {
                    foreach (int menuid in AccountMenu)
                    {
                        _context.Rolemenus.Add(new Rolemenu
                        {
                            Roleid = role.Roleid,
                            Menuid = menuid,
                        });
                    }
                    _context.SaveChanges();
                }
            }
        }

        public List<AccountMenu> GetAccountMenu(int accounttype, int roleid)
        {

            var menu = _context.Menus.Where(r => r.Accounttype == accounttype).ToList();


            var rolemenu = _context.Rolemenus.ToList();

            var checkedMenu = menu.Select(r1 => new AccountMenu
            {
                menuid = r1.Menuid,
                name = r1.Name,
                ExistsInTable = rolemenu.Any(r2 => r2.Roleid == roleid && r2.Menuid == r1.Menuid),

            }).ToList();

            return checkedMenu;

        }
        public AccountAccess GetEditAccessData(int roleid)
        {
            var role = _context.Roles.FirstOrDefault(i => i.Roleid == roleid);
            if (role != null)
            {
                var roledata = new AccountAccess()
                {
                    name = role.Name,
                    roleid = roleid,
                    accounttypeid = role.Accounttype,
                };
                return roledata;
            }
            return null;
        }

        public void SetEditAccessAccount(AccountAccess accountAccess, List<int> AccountMenu, string sessionEmail)
        {
            var user = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

            var role = _context.Roles.FirstOrDefault(x => x.Roleid == accountAccess.roleid);
            if (role != null)
            {
                role.Name = accountAccess.name;
                role.Accounttype = (short)accountAccess.accounttypeid;
                role.Modifiedby = user.Id;
                role.Modifieddate = DateTime.Now;

                _context.SaveChanges();

                var rolemenu = _context.Rolemenus.Where(i => i.Roleid == accountAccess.roleid).ToList();
                if (rolemenu != null)
                {
                    _context.Rolemenus.RemoveRange(rolemenu);
                }

                if (AccountMenu != null)
                {
                    foreach (int menuid in AccountMenu)
                    {
                        _context.Rolemenus.Add(new Rolemenu
                        {
                            Roleid = role.Roleid,
                            Menuid = menuid,
                        });
                    }
                    _context.SaveChanges();
                }
            }
        }

        public void DeleteAccountAccess(int roleid)
        {
            var role = _context.Roles.FirstOrDefault(x => x.Roleid == roleid);
            if (role != null)
            {
                role.Isdeleted = new BitArray(1, true);
                _context.SaveChanges();
            }

            var rolemenu = _context.Rolemenus.Where(i => i.Roleid == roleid).ToList();
            if (rolemenu != null)
            {
                _context.Rolemenus.RemoveRange(rolemenu);
                _context.SaveChanges();
            }
        }

        public List<Aspnetrole> GetAccountTypeRoles()
        {
            var role = _context.Aspnetroles.Where(i => i.Id != 2).ToList();
            return role;
        }

        public List<UserAccess> GetUserdata(int accounttypeid)
        {
            var Admindata = _context.Admins.Where(i => i.Isdeleted == null).ToList();

            var Providerdata = _context.Physicians.Where(i => i.Isdeleted == null).ToList();

            var Adrequest = _context.Requests.Where(i => i.Status != 10 || i.Status != 11).Count();

            if (accounttypeid == 1)
            {
                var Admin = Admindata.Select(r => new UserAccess()
                {

                    aspnetid = r.Aspnetuserid,
                    Accounttype = "Admin",
                    accountname = r.Firstname + ", " + r.Lastname,
                    phone = r.Mobile,
                    status = (int)r.Status,
                    openrequest = Adrequest,
                    roleid = 1,
                }).ToList();

                return Admin;
            }

            if (accounttypeid == 3)
            {
                var physician = Providerdata.Select(r => new UserAccess()
                {
                    aspnetid = (int)r.Aspnetuserid,
                    Accounttype = "Provider",
                    accountname = r.Firstname + ", " + r.Lastname,
                    phone = r.Mobile,
                    status = (int)r.Status,
                    openrequest = _context.Requests.Where(i => i.Physicianid == r.Physicianid && (i.Status == 1 || i.Status == 2 || i.Status == 4 || i.Status == 5 || i.Status == 6)).Count(),
                    roleid = 2,
                }).ToList();

                return physician;
            }

            var Addata = Admindata.Select(r => new UserAccess()
            {

                aspnetid = r.Aspnetuserid,
                Accounttype = "Admin",
                accountname = r.Firstname + ", " + r.Lastname,
                phone = r.Mobile,
                status = (int)r.Status,
                openrequest = Adrequest,
                roleid = 1,
            }).ToList();


            var phdata = Providerdata.Select(r => new UserAccess()
            {
                aspnetid = (int)r.Aspnetuserid,
                Accounttype = "Provider",
                accountname = r.Firstname + ", " + r.Lastname,
                phone = r.Mobile,
                status = (int)r.Status,
                openrequest = _context.Requests.Where(i => i.Physicianid == r.Physicianid && (i.Status == 1 || i.Status == 2 || i.Status == 4 || i.Status == 5 || i.Status == 6)).Count(),
                roleid = 2,
            }).ToList();


            var combineddata = Addata.Concat(phdata).ToList();

            return combineddata;
        }

        public void createAdminAccount(adminDashData obj, List<int> regions)
        {
            var aspUser = _context.Aspnetusers.FirstOrDefault(r => r.Email == obj._providerEdit.Email);

            if (aspUser == null)
            {
                Aspnetuser _user = new Aspnetuser();
                Admin admin = new Admin();

                _user.Username = obj._providerEdit.username;
                _user.Passwordhash = obj._providerEdit.password;
                _user.Email = obj._providerEdit.Email;
                _user.Phonenumber = obj._providerEdit.PhoneNumber;
                _user.Createddate = DateTime.Now;

                _context.Aspnetusers.Add(_user);
                _context.SaveChanges();

                admin.Aspnetuserid = _user.Id;
                admin.Firstname = obj._providerEdit.Firstname;
                admin.Lastname = obj._providerEdit.Lastname;
                admin.Email = obj._providerEdit.Email;
                admin.Mobile = obj._providerEdit.PhoneNumber;
                admin.Address1 = obj._providerEdit.Address1;
                admin.Address2 = obj._providerEdit.Address2;
                admin.City = obj._providerEdit.city;
                admin.Regionid = obj._providerEdit.Regionid;
                admin.Zip = obj._providerEdit.zipcode;
                admin.Altphone = obj._providerEdit.altPhone;
                admin.Createdby = null;
                admin.Createddate = _user.Createddate;
                admin.Status = 1;
                admin.Roleid = obj._providerEdit.Roleid;

                _context.Admins.Add(admin);
                _context.SaveChanges();

                foreach (var item in regions)
                {
                    var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                    _context.Adminregions.Add(new Adminregion
                    {
                        Adminid = admin.Adminid,
                        Regionid = region.Regionid,
                    });
                }
                _context.SaveChanges();

                Aspnetuserrole _userRole = new Aspnetuserrole();
                _userRole.Userid = _user.Id;
                _userRole.Roleid = 1;

                _context.Aspnetuserroles.Add(_userRole);
                _context.SaveChanges();
            }

        }


        //*******************************************************Provider Location***********************************************

        public List<Physicianlocation> GetPhysicianlocations()
        {
            //var address = "75 Ninth Avenue 2nd and 4th Floors New York, NY 10011";
            //var locationService = new GoogleLocationService();
            //var point = locationService.GetLatLongFromAddress(address);
            //var latitude = point.Latitude;
            //var longitude = point.Longitude; 

            var phyLocation = _context.Physicianlocations.ToList();
            return phyLocation;
        }




        //*****************************************************Partners********************************************************

        public List<Healthprofessionaltype> GetProfession()
        {

            var professions = _context.Healthprofessionaltypes.Where(r => r.Isdeleted == null).Select(r => r).ToList();
            return professions;
        }

        public List<Partnersdata> GetPartnersdata(int professionid)
        {
            var vendor = _context.Healthprofessionals.Where(r => r.Isdeleted == null).ToList();
            if (professionid != 0)
            {
                vendor = vendor.Where(i => i.Profession == professionid).ToList();
            }
            var Partnersdata = vendor.Select(r => new Partnersdata()
            {
                VendorId = r.Vendorid,
                VendorName = r.Vendorname,
                ProfessionName = _context.Healthprofessionaltypes.FirstOrDefault(i => i.Healthprofessionalid == r.Profession).Professionname,
                VendorEmail = r.Email,
                FaxNo = r.Faxnumber,
                PhoneNo = r.Phonenumber,
                Businesscontact = r.Businesscontact,
            }).ToList();
            return Partnersdata;
        }

        public bool CreateNewBusiness(partnerModel partnerModel, string sessionEmail)
        {
            if (!_context.Healthprofessionals.Any(x => x.Email == partnerModel.Email))
            {
                var healthprof = new Healthprofessional()
                {
                    Vendorname = partnerModel.BusinessName,
                    Profession = partnerModel.SelectedhealthprofID,
                    Faxnumber = partnerModel.FAXNumber,
                    Phonenumber = partnerModel.Phonenumber,
                    Email = partnerModel.Email,
                    Businesscontact = partnerModel.BusinessContact,
                    Address = partnerModel.Street,
                    City = partnerModel.City,
                    Regionid = partnerModel.RegionId,
                    Zip = partnerModel.Zip,
                };
                _context.Healthprofessionals.Add(healthprof);
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public partnerModel GetEditBusinessData(int vendorID)
        {
            var vendorDetails = _context.Healthprofessionals.FirstOrDefault(i => i.Vendorid == vendorID);
            var partnerModel = new partnerModel()
            {
                BusinessName = vendorDetails.Vendorname,
                SelectedhealthprofID = vendorDetails.Profession,
                FAXNumber = vendorDetails.Faxnumber,
                Phonenumber = vendorDetails.Phonenumber,
                Email = vendorDetails.Email,
                BusinessContact = vendorDetails.Businesscontact,
                Street = vendorDetails.Address,
                City = vendorDetails.City,
                RegionId = vendorDetails.Regionid,
                Zip = vendorDetails.Zip,
            };
            return partnerModel;
        }

        public bool UpdateBusiness(partnerModel partnerModel)
        {
            var vendorDetails = _context.Healthprofessionals.FirstOrDefault(i => i.Vendorid == partnerModel.vendorID);
            if (partnerModel.BusinessName != vendorDetails.Vendorname || partnerModel.SelectedhealthprofID != vendorDetails.Profession || partnerModel.FAXNumber != vendorDetails.Faxnumber
            || partnerModel.Phonenumber != vendorDetails.Phonenumber || partnerModel.Email != vendorDetails.Email || partnerModel.BusinessContact != vendorDetails.Businesscontact
            || partnerModel.Street != vendorDetails.Address || partnerModel.City != vendorDetails.City || partnerModel.RegionId != vendorDetails.Regionid || partnerModel.Zip != vendorDetails.Zip)
            {
                vendorDetails.Vendorname = partnerModel.BusinessName;
                vendorDetails.Profession = partnerModel.SelectedhealthprofID;
                vendorDetails.Faxnumber = partnerModel.FAXNumber;
                vendorDetails.Phonenumber = partnerModel.Phonenumber;
                vendorDetails.Email = partnerModel.Email;
                vendorDetails.Businesscontact = partnerModel.BusinessContact;
                vendorDetails.Address = partnerModel.Street;
                vendorDetails.City = partnerModel.City;
                vendorDetails.Regionid = partnerModel.RegionId;
                vendorDetails.Zip = partnerModel.Zip;
                _context.SaveChanges();
                return true;
            }
            return false;
        }

        public void DltBusiness(int vendorID)
        {
            var vendor = _context.Healthprofessionals.Where(r => r.Vendorid == vendorID).Select(r => r).First();

            if(vendor.Isdeleted == null)
            {
                vendor.Isdeleted = new BitArray(1, true);
                _context.SaveChanges();
            }
        }


        //*****************************************************Scheduling****************************************

        public void createShift(ScheduleModel scheduleModel, int Aspid)
        {


            Shift shift = new Shift();
            shift.Physicianid = scheduleModel.Physicianid;
            shift.Repeatupto = scheduleModel.Repeatupto;
            shift.Startdate = scheduleModel.Startdate;
            shift.Createdby = Aspid;
            shift.Createddate = DateTime.Now;
            shift.Isrepeat = scheduleModel.Isrepeat == false ? new BitArray(1, false) : new BitArray(1, true);
            shift.Repeatupto = scheduleModel.Repeatupto;
            _context.Shifts.Add(shift);
            _context.SaveChanges();

            Shiftdetail sd = new Shiftdetail();
            sd.Shiftid = shift.Shiftid;
            sd.Shiftdate = new DateTime(scheduleModel.Startdate.Year, scheduleModel.Startdate.Month, scheduleModel.Startdate.Day);
            sd.Starttime = scheduleModel.Starttime;
            sd.Endtime = scheduleModel.Endtime;
            sd.Regionid = scheduleModel.Regionid;
            sd.Status = 1;
            // sd.Isdeleted =new BitArray(1, false);
            _context.Shiftdetails.Add(sd);
            _context.SaveChanges();

            Shiftdetailregion sr = new Shiftdetailregion();
            sr.Shiftdetailid = sd.Shiftdetailid;
            sr.Regionid = scheduleModel.Regionid;
            //sr.Isdeleted = false;
            _context.Shiftdetailregions.Add(sr);
            _context.SaveChanges();


            if (scheduleModel.Isrepeat != false)
            {

                List<int> day = scheduleModel.checkWeekday.Split(',').Select(int.Parse).ToList();

                foreach (int d in day)
                {
                    DayOfWeek desiredDayOfWeek = (DayOfWeek)d;
                    DateTime today = DateTime.Today;
                    DateTime nextOccurrence = new DateTime(scheduleModel.Startdate.Year, scheduleModel.Startdate.Month, scheduleModel.Startdate.Day);
                    int occurrencesFound = 0;
                    while (occurrencesFound < scheduleModel.Repeatupto)
                    {
                        if (nextOccurrence.DayOfWeek == desiredDayOfWeek)
                        {

                            Shiftdetail sdd = new Shiftdetail();
                            sdd.Shiftid = shift.Shiftid;
                            sdd.Shiftdate = nextOccurrence;
                            sdd.Starttime = scheduleModel.Starttime;
                            sdd.Endtime = scheduleModel.Endtime;
                            sdd.Regionid = scheduleModel.Regionid;
                            sdd.Status = 1;
                            //sdd.Isdeleted = false;
                            _context.Shiftdetails.Add(sdd);
                            _context.SaveChanges();

                            Shiftdetailregion srr = new Shiftdetailregion();
                            srr.Shiftdetailid = sdd.Shiftdetailid;
                            srr.Regionid = scheduleModel.Regionid;
                            //srr.Isdeleted = false;
                            _context.Shiftdetailregions.Add(srr);
                            _context.SaveChanges();
                            occurrencesFound++;
                        }
                        nextOccurrence = nextOccurrence.AddDays(1);
                    }
                }
            }

        }


        public List<ShiftDetailsmodal> ShiftDetailsmodal(DateTime date, DateTime sunday, DateTime saturday, string type)
        {
            var shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year);

            switch (type)
            {
                case "month":
                    shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && u.Isdeleted != null);
                    break;

                case "week":
                    shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate >= sunday && u.Shiftdate <= saturday && u.Isdeleted != null);
                    break;

                case "day":
                    shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && u.Shiftdate.Day == date.Day && u.Isdeleted != null);
                    break;
            }


            var list = shiftdetails.Select(s => new ShiftDetailsmodal
            {
                Shiftid = s.Shiftid,
                Shiftdetailid = s.Shiftdetailid,
                Shiftdate = s.Shiftdate,
                Startdate = s.Shift.Startdate,
                Starttime = s.Starttime,
                Endtime = s.Endtime,
                Physicianid = s.Shift.Physicianid,
                PhysicianName = s.Shift.Physician.Firstname,
                Status = s.Status,
                regionname = _context.Regions.FirstOrDefault(i => i.Regionid == s.Regionid).Name,
                Abberaviation = _context.Regions.FirstOrDefault(i => i.Regionid == s.Regionid).Abbreviation,
            });

            return list.ToList();
        }

        public ShiftDetailsmodal GetShift(int shiftdetailsid)
        {
            var shiftdetails = _context.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftdetailsid);
            var physicianlist = _context.Physicianregions.Where(p => p.Regionid == shiftdetails.Regionid).Select(s => s.Physicianid).ToList();

            ShiftDetailsmodal shift = new ShiftDetailsmodal
            {
                Shiftdetailid = shiftdetailsid,
                Shiftdate = shiftdetails.Shiftdate,
                Shiftid = shiftdetails.Shiftid,
                Starttime = shiftdetails.Starttime,
                Endtime = shiftdetails.Endtime,
                Regionid = shiftdetails.Regionid,
                Abberaviation = _context.Regions.FirstOrDefault(i => i.Regionid == shiftdetails.Regionid).Abbreviation,
                Status = shiftdetails.Status,
                regions = _context.Regions.ToList(),
                Physicians = _context.Physicians.Where(p => physicianlist.Contains(p.Physicianid)).ToList(),
            };

            return shift;
        }


        //Physician List
        public List<Physician> GetPhysicians(int regionid)
        {
            if (regionid == 0)
            {
                var physicians = _context.Physicians.ToList();
                return physicians;
            }
            else
            {
                var physicians1 = _context.Physicians.Where(i => i.Regionid == regionid).ToList();
                return physicians1;
            }

        }


        //****************************************************************Records********************************************************

        public List<requestsRecordModel> searchRecords(recordsModel recordsModel)
        {
            //List<requestsRecordModel> listdata = new List<requestsRecordModel>();
            //requestsRecordModel requestsRecordModel = new requestsRecordModel();

          var requestList =  _context.Requests.Where(r => r.Isdeleted == null).Select(x => new requestsRecordModel()
            {
                requestid = x.Requestid,
                requesttypeid = x.Requesttypeid,
                patientname = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Firstname).First(),
                requestor = x.Firstname,
                email = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Email).First(),
                contact = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Phonenumber).First(),
                address = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Street).First() + " " + x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.City).First() + " " + x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.State).First(),
                zip = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Zipcode).First(),
                statusId = x.Status,
                physician = _context.Physicians.Where(r => r.Physicianid == x.Physicianid).Select(r => r.Firstname).First(),
                physicianNote = x.Requestnotes.Where(r => r.Requestid == x.Requestid).Select(r => r.Physiciannotes).First(),
                AdminNote = x.Requestnotes.Where(r => r.Requestid == x.Requestid).Select(r => r.Adminnotes).First(),
                pateintNote = x.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Notes).First(),
            }).ToList();

            if(recordsModel.requestListMain != null)
            {           
                if (recordsModel.requestListMain[0].searchRecordOne != null)
                {
                    requestList = requestList.Where(r => r.statusId == recordsModel.requestListMain[0].searchRecordOne).Select(r => r).ToList();                    
                }
            
                if (recordsModel.requestListMain[0].searchRecordTwo != null)
                {
                    requestList = requestList.Where(r => r.patientname.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();                    
                } 
            
                if (recordsModel.requestListMain[0].searchRecordThree != null)
                {
                    requestList = requestList.Where(r =>  r.requesttypeid == recordsModel.requestListMain[0].searchRecordThree).Select(r => r).ToList();                   
                }

                if (recordsModel.requestListMain[0].searchRecordSix != null)
                {
                    requestList = requestList.Where(r => r.requestor.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordSix.Trim().ToLower())).Select(r => r).ToList();                   
                }

                if (recordsModel.requestListMain[0].searchRecordSeven != null)
                {
                    requestList = requestList.Where(r => r.email.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordSeven.Trim().ToLower())).Select(r => r).ToList();                    
                }

                if (recordsModel.requestListMain[0].searchRecordEight != null)
                {
                    requestList = requestList.Where(r => r.contact.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordEight.Trim().ToLower())).Select(r => r).ToList();                    
                }
            }

            return requestList;
        }

        public void DeleteRecords(int reqId)
        {
            var reqClient  = _context.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();

            if (reqClient.Isdeleted == null)
            {
                reqClient.Isdeleted = new BitArray(1, true);
                _context.SaveChanges();
            }
        }

        public byte[] GenerateExcelFile(List<requestsRecordModel> recordsModel)
        {
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial; using (var excelPackage = new ExcelPackage())
            {
                var worksheet = excelPackage.Workbook.Worksheets.Add("Requests");

                // Add headers
                worksheet.Cells[1, 1].Value = "Patient Name";
                worksheet.Cells[1, 2].Value = "Requestor";
                worksheet.Cells[1, 3].Value = "Date Of Service";
                worksheet.Cells[1, 4].Value = "Close Case Date";
                worksheet.Cells[1, 5].Value = "Email";
                worksheet.Cells[1, 6].Value = "Phone Number";
                worksheet.Cells[1, 7].Value = "Address";
                worksheet.Cells[1, 8].Value = "Zip";
                worksheet.Cells[1, 9].Value = "Physician";
                worksheet.Cells[1, 10].Value = "Physician Notes";
                worksheet.Cells[1, 11].Value = "Admin Note";
                worksheet.Cells[1, 12].Value = "Patient Notes";

                // Populate data
                for (int i = 0; i < recordsModel.Count; i++)
                {
                    var rowData = recordsModel[i];
                    worksheet.Cells[i + 2, 1].Value = rowData.patientname;
                    worksheet.Cells[i + 2, 2].Value = rowData.requestor;
                    worksheet.Cells[i + 2, 3].Value = rowData.dateOfService;
                    worksheet.Cells[i + 2, 4].Value = rowData.closeCaseDate;
                    worksheet.Cells[i + 2, 5].Value = rowData.email;
                    worksheet.Cells[i + 2, 6].Value = rowData.contact;
                    worksheet.Cells[i + 2, 7].Value = rowData.address;
                    worksheet.Cells[i + 2, 8].Value = rowData.zip;
                    worksheet.Cells[i + 2, 9].Value = rowData.physician;
                    worksheet.Cells[i + 2, 10].Value = rowData.physicianNote;
                    worksheet.Cells[i + 2, 11].Value = rowData.AdminNote;
                    worksheet.Cells[i + 2, 12].Value = rowData.pateintNote;
                }

                // Convert package to bytes for download
                return excelPackage.GetAsByteArray();
            }
        }

        public List<User> patientRecords(GetRecordsModel GetRecordsModel)
        {
            GetRecordsModel data = new GetRecordsModel();

            data.users = _context.Users.ToList();

            if(GetRecordsModel != null)
            {
                if(GetRecordsModel.searchRecordOne != null)
                {
                    data.users = data.users.Where(r => r.Firstname.Trim().ToLower().Contains(GetRecordsModel.searchRecordOne.Trim().ToLower())).Select(r => r).ToList();
                }
                if(GetRecordsModel.searchRecordTwo != null)
                {
                    data.users = data.users.Where(r => r.Lastname.Trim().ToLower().Contains(GetRecordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                }
                if(GetRecordsModel.searchRecordThree != null)
                {
                    data.users = data.users.Where(r => r.Email.Trim().ToLower().Contains(GetRecordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                }
                if(GetRecordsModel.searchRecordFour != null)
                {
                    data.users = data.users.Where(r => r.Mobile.Trim().ToLower().Contains(GetRecordsModel.searchRecordFour.Trim().ToLower())).Select(r => r).ToList();
                }
            }

            return data.users;
        }

        public List<GetRecordExplore> GetPatientRecordExplore(int userId)
        {

            var dataMain = _context.Requests.Where(r => r.Userid == userId).Select(r => new GetRecordExplore()
            {
                requestid = r.Requestid,
                createddate = r.Createddate.ToString("yyyy-MM-dd"),
                confirmationnumber = r.Confirmationnumber,
                providername = _context.Physicians.Where(x => x.Physicianid == r.Physicianid).Select(x => x.Firstname).First(),
                status = r.Status,
                fullname = r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Firstname).First() + " " + r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Firstname).First(),
                concludedate = r.Status == 6 ? Convert.ToDateTime(r.Modifieddate).ToString("yyyy-MM-dd") : null, // Add this condition to check if the status is equal to 6,
            }).ToList();

            return dataMain;
        }
    }
}
