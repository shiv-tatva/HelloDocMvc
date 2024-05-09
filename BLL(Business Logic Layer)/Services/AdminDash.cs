using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections;
using System.Net;
using System.Net.Mail;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BLL_Business_Logic_Layer_.Services
{
    public class AdminDash : IAdminDash
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<Physician> _physicianrepo;
        private readonly IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet> _weeklyTimeSheetRepo;
        private readonly IGenericRepository<Chat> _chatRepo;

        public AdminDash(ApplicationDbContext context, IGenericRepository<Physician> physicianrepo , IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet> weeklyTimeSheetRepo, IGenericRepository<Chat> chatRepo)
        {
            _context = context;
            _physicianrepo = physicianrepo;
            _weeklyTimeSheetRepo = weeklyTimeSheetRepo;
            _chatRepo = chatRepo;
        }


        #region adminData
        public List<adminDash> adminData(int[] status, int typeId, int regionId, string sessionName, int dataFlag, string sessionFilter)
        {
            var requestList = _context.Requests.Where(i => status.Contains(i.Status));

            if (_context.Physicians.Any(r => r.Email == sessionFilter))
            {
                var phyDetail = _context.Physicians.Where(r => r.Email == sessionFilter).Select(r => r).First();
                requestList = _context.Requests.Where(i => status.Contains(i.Status));
                requestList = requestList.Where(r => r.Physicianid == phyDetail.Physicianid).Select(r => r);
            }



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
                             phy_id = r.Physicianid,
                             zipcode = rc.Zipcode,
                             address = r.Requestclients.Select(x => x.Street).First() + "," + r.Requestclients.Select(x => x.City).First() + "," + r.Requestclients.Select(x => x.State).First(),
                             request_type_id = r.Requesttypeid,
                             status = r.Status,
                             call_type = (int)r.Calltype,
                             region_id = rc.Regionid,
                             //transfer_notes = r.Requeststatuslogs.OrderBy(i => i.Requeststatuslogid).LastOrDefault().Notes,
                             notes = r.Requeststatuslogs.Where(x => x.Requestid == r.Requestid && (x.Transtophysicianid != null || x.Transtoadmin != null)).OrderBy(x => x.Requeststatuslogid).Select(x => x).ToList(),
                             isFinalize = r.EncounterForms.Where(x => x.Requestid == r.Requestid).Select(x => x.IsFinalized).First(),
                             //phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                             //region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                             //region_table = _context.Regions.ToList(),
                             reqid = r.Requestid,
                             email = rc.Email,
                             fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
                         }).ToList();

            if (typeId > 0)
            {
                query = query.Where(x => x.request_type_id == typeId).Select(r => r).ToList();
            }

            if (regionId > 0)
            {
                query = query.Where(x => x.region_id == regionId).Select(r => r).ToList();
            }

            if (dataFlag == 11)
            {
                var phyDetailMain = _context.Physicians.Where(x => x.Email == sessionName).Select(x => x).First();
                query = query.Where(x => x.phy_id == phyDetailMain.Physicianid && x.status == 1).Select(r => r).ToList();
            }
            if (dataFlag == 12)
            {
                var phyDetailMain = _context.Physicians.Where(x => x.Email == sessionName).Select(x => x).First();
                query = query.Where(x => x.phy_id == phyDetailMain.Physicianid && x.status == 2).Select(r => r).ToList();
            }
            if (dataFlag == 13)
            {
                var phyDetailMain = _context.Physicians.Where(x => x.Email == sessionName).Select(x => x).First();
                query = query.Where(x => x.phy_id == phyDetailMain.Physicianid && (x.status == 4 || x.status == 5)).Select(r => r).ToList();
            }
            if (dataFlag == 14)
            {
                var phyDetailMain = _context.Physicians.Where(x => x.Email == sessionName).Select(x => x).First();
                query = query.Where(x => x.phy_id == phyDetailMain.Physicianid && x.status == 6).Select(r => r).ToList();
            }

            //if(query.Count() == 0)
            //{
            //    query[0].status = status[0];
            //}
            //var result = query.ToList();

            return query;
        }
        #endregion

        #region countService
        public countMain countService(string sessionName, int flagCount)
        {
            if (flagCount != 10)
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
            else
            {
                var phyDetail = _context.Physicians.Where(x => x.Email == sessionName).Select(x => x).First();

                var requestsWithClients = _context.Requests
                   .Join(_context.Requestclients,
                       r => r.Requestid,
                       rc => rc.Requestid,
                       (r, rc) => new { Request = r, RequestClient = rc })
                   .ToList();

                requestsWithClients = requestsWithClients.Where(r => r.Request.Physicianid == phyDetail.Physicianid).ToList();

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
        }
        #endregion

        #region GetListOfRoleMenu
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
        #endregion

        #region adminDataViewCase
        public List<adminDash> adminDataViewCase(int reqId, int flag = 1)
        {
            var query = from r in _context.Requests
                        join rc in _context.Requestclients on r.Requestid equals rc.Requestid
                        where r.Requestid == reqId
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
                            phy_id = r.Physicianid,
                            phy_name = _context.Physicians.FirstOrDefault(a => a.Physicianid == r.Physicianid).Firstname,
                            region = _context.Regions.FirstOrDefault(a => a.Regionid == rc.Regionid).Name,
                            reqid = r.Requestid,
                            email = rc.Email,
                            fulldateofbirth = new DateTime((int)r.Requestclients.Select(x => x.Intyear).First(), Convert.ToInt16(r.Requestclients.Select(x => x.Strmonth).First()), (int)r.Requestclients.Select(x => x.Intdate).First()).ToString("yyyy-MM-dd"),
                            cnf_number = r.Confirmationnumber,
                            flagId = flag,
                        };


            var result = query.ToList();


            return result;
        }
        #endregion

        #region adminDataViewNote
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
                viewNotes.requeststatuslogs = _context.Requeststatuslogs.Where(r => r.Requestid == reqId).OrderBy(r => r.Requeststatuslogid).ToList();
                if (b != null)
                {
                    if (_context.Requeststatuslogs.Any(r => r.Requestid == reqId && r.Status == 3))
                        viewNotes.AdminCancleNotes = _context.Requeststatuslogs.Where(r => r.Requestid == reqId && r.Status == 3).Select(x => x.Notes).First();
                    if (_context.Requeststatuslogs.Any(r => r.Requestid == reqId && r.Status == 7))
                        viewNotes.PatientCancleNotes = _context.Requeststatuslogs.Where(x => x.Requestid == reqId && x.Status == 7).Select(x => x.Notes).First();//status 7 is for Request Status Cancelled By Patient 
                }

            }
            else
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
        #endregion

        #region adminDataViewNote
        public void adminDataViewNote(adminDashData obj)
        {
            var reqNoteId = _context.Requestnotes.FirstOrDefault(r => r.Requestid == obj._viewNote.reqid);

            if (reqNoteId != null)
            {
                reqNoteId.Adminnotes = obj._viewNote.AdminNotes;
                reqNoteId.Modifiedby = (int)_context.Requests.Where(x => x.Requestid == obj._viewNote.reqid).Select(x => x.User.Aspnetuserid).First();
                reqNoteId.Modifieddate = DateTime.Now.Date;
                //_reqNote.Physiciannotes = obj._viewNote.PhysicianNotes;
                _context.SaveChanges();

            }

        }
        #endregion

        #region closeCaseNote
        public CloseCase closeCaseNote(int reqId)
        {

            CloseCase _closeCase = new CloseCase();

            _closeCase.first_name = _context.Requestclients.FirstOrDefault(r => r.Requestid == reqId).Firstname;
            _closeCase.reqid = reqId;
            _closeCase.status = _context.Requests.FirstOrDefault(r => r.Requestid == reqId).Status;

            return _closeCase;
        }
        #endregion

        #region casetag
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
        #endregion

        #region MyRegion
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
        #endregion

        #region adminDataAssignCase
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
        #endregion

        #region adminDataAssignCaseDocList
        public AssignCase adminDataAssignCaseDocList(int regionId)
        {
            AssignCase assignCase = new AssignCase();

            assignCase.phy_name = _context.Physicianregions.Where(x => x.Regionid == regionId).Select(x => x.Physician.Firstname).ToList();
            assignCase.phy_id = _context.Physicianregions.Where(x => x.Regionid == regionId).Select(x => x.Physician.Physicianid).ToList();

            return assignCase;
        }
        #endregion

        #region adminDataAssignCase
        public void adminDataAssignCase(adminDashData assignObj)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();
            Request _req = new Request();

            var requestedRowPatient = _context.Requests.Where(x => x.Requestid == assignObj.assignCase.reqid).Select(r => r).First();

            requeststatuslog.Requestid = assignObj.assignCase.reqid;
            requeststatuslog.Notes = assignObj.assignCase.description;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Status = 1;
            requeststatuslog.Adminid = 1;
            requeststatuslog.Transtophysicianid = assignObj.assignCase.phy_id_main;


            _context.Add(requeststatuslog);
            _context.SaveChanges();

            requestedRowPatient.Physicianid = assignObj.assignCase.phy_id_main;
            requestedRowPatient.Status = 1;

            _context.SaveChanges();


        }
        #endregion

        #region blockcase
        public blockCaseModel blockcase(int req)
        {
            blockCaseModel _block = new blockCaseModel();

            _block.reqid = req;
            _block.first_name = _context.Requestclients.Where(r => r.Requestid == req).Select(r => r.Firstname).First();

            return _block;
        }
        #endregion

        #region blockcase
        public void blockcase(adminDashData obj, string sessionEmail)
        {
            var request = _context.Requests.FirstOrDefault(r => r.Requestid == obj._blockCaseModel.reqid);

            if (request != null)
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
            blockrequest.Isactive = new BitArray(1);
            blockrequest.Isactive[0] = true;

            _context.Blockrequests.Add(blockrequest);
            _context.SaveChanges();
        }
        #endregion

        #region viewUploadMain
        public List<viewUploads> viewUploadMain(int reqId, int flag)
        {


            var reqIdDeleted = _context.Requestwisefiles.Where(r => r.Requestid == reqId).Select(r => r.Isdeleted[0]);

            var query = _context.Requests.Where(r => r.Requestid == reqId).AsNoTracking().Select(r => new viewUploads()
            {
                reqid = reqId,
                email = r.Requestclients.Where(r => r.Requestid == reqId).Select(r => r.Email).First(),
                fname = r.Requestclients.Where(r => r.Requestid == reqId).Select(r => r.Firstname).First(),
                lname = r.Requestclients.Where(r => r.Requestid == reqId).Select(r => r.Lastname).First(),
                cnf_number = r.Confirmationnumber,
                documentsname = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Filename).ToList(),
                created_date = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Createddate).ToList(),
                requestWiseFileId = r.Requestwisefiles.Where(r => r.Isdeleted == null).Select(r => r.Requestwisefileid).ToList(),
                flagId = flag,
                isFinalize = r.EncounterForms.Where(x => x.Requestid == r.Requestid).Select(x => x.IsFinalized).First(),
            }).ToList();

            return query;
        }
        #endregion

        #region viewUploadMain
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
        #endregion

        #region DeleteFile
        public void DeleteFile(bool data, int reqFileId)
        {
            var reqWiseFile = _context.Requestwisefiles.Where(r => r.Requestwisefileid == reqFileId).Select(r => r).First();

            Requestwisefile _req = new Requestwisefile();

            reqWiseFile.Isdeleted = new BitArray(1);
            reqWiseFile.Isdeleted[0] = data;

            _context.SaveChanges();
        }
        #endregion

        #region SendRegistrationEmail
        public void SendRegistrationEmail(string emailMain, string[] data, string sessionEmail)
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

            for (var i = 0; i < data.Length; i++)
            {
                string pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Documents", data[i]);
                Attachment attachment = new Attachment(pathname);
                mailMessage.Attachments.Add(attachment);
            }

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + emailMain + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = emailMain,
                Roleid = 1,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (_context.Admins.Any(r => r.Email == sessionEmail))
            {
                emailLog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            }

            if (_context.Physicians.Any(r => r.Email == sessionEmail))
            {
                emailLog.Physicianid = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First();
                emailLog.Roleid = 3;
            }

            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();

            mailMessage.To.Add(emailMain);

            client.Send(mailMessage);
        }
        #endregion

        #region sendMail
        public void sendMail(string emailMain, string[] data, string sessionEmail)
        {
            string emailConfirmationToken = Guid.NewGuid().ToString();

            try
            {
                SendRegistrationEmail(emailMain, data, sessionEmail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region viewOrder
        public activeOrder viewOrder(int reqId)
        {
            activeOrder _active = new activeOrder();

            _active.reqid = reqId;
            _active.profession = _context.Healthprofessionaltypes.ToList();

            return _active;
        }
        #endregion

        #region businessName
        public activeOrder businessName(int profession_id)
        {
            activeOrder _active = new activeOrder();

            _active.business_data = _context.Healthprofessionals.Where(r => r.Profession == profession_id).Select(r => r.Vendorname).ToList();
            _active.business_id = _context.Healthprofessionals.Where(r => r.Profession == profession_id).Select(r => r.Vendorid).ToList();

            return _active;
        }
        #endregion

        #region businessDetail
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
        #endregion

        #region viewOrder
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

            if (_health.Email != adminDashData._activeOrder.email || _health.Faxnumber != adminDashData._activeOrder.fax_num || _health.Businesscontact != adminDashData._activeOrder.business_contact)
            {
                _health.Email = adminDashData._activeOrder.email;
                _health.Faxnumber = adminDashData._activeOrder.fax_num;
                _health.Businesscontact = adminDashData._activeOrder.business_contact;

                _context.Healthprofessionals.Update(_health);
                _context.SaveChanges();
            }
        }
        #endregion

        #region transferReq
        public transferRequest transferReq(int req)
        {
            transferRequest transferRequest = new transferRequest();

            transferRequest.region_name = _context.Regions.Select(x => x.Name).ToList();
            transferRequest.region_id = _context.Regions.Select(y => y.Regionid).ToList();
            transferRequest.regions = _context.Regions.ToList();
            transferRequest.reqid = req;
            transferRequest.phy_id_main = (int)_context.Requests.Where(r => r.Requestid == req).Select(r => r.Physicianid).First();
            return transferRequest;
        }
        #endregion

        #region transferReq
        public void transferReq(adminDashData data, string sessionEmail)
        {
            Requeststatuslog requeststatuslog = new Requeststatuslog();

            var requestedRowPatient = _context.Requests.Where(x => x.Requestid == data.transferRequest.reqid).Select(r => r).First();


            requeststatuslog.Requestid = data.transferRequest.reqid;
            requeststatuslog.Notes = data.transferRequest.description;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Status = 1;
            requeststatuslog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            requeststatuslog.Transtophysicianid = data.transferRequest.phy_id_main;

            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();

            requestedRowPatient.Physicianid = data.transferRequest.phy_id_main;
            requestedRowPatient.Status = 1;
            requestedRowPatient.Modifieddate = DateTime.Now;

            _context.SaveChanges();
        }

        #endregion

        #region clearCase
        public blockCaseModel clearCase(int reqId)
        {
            blockCaseModel blockCaseModel = new blockCaseModel();
            blockCaseModel.reqid = reqId;
            return blockCaseModel;
        }

        #endregion

        #region clearCase
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

        #endregion

        #region sendAgree
        public sendAgreement sendAgree(int reqId)
        {
            sendAgreement sendAgreement = new sendAgreement();
            sendAgreement.reqid = reqId;
            sendAgreement.request_type_id = _context.Requests.Where(x => x.Requestid == reqId).Select(r => r.Requesttypeid).First();
            sendAgreement.email = _context.Requestclients.Where(x => x.Requestid == reqId).Select(r => r.Email).First();
            sendAgreement.mobile_num = _context.Requestclients.Where(x => x.Requestid == reqId).Select(r => r.Phonenumber).First();
            return sendAgreement;
        }

        #endregion

        #region sendAgree
        public void sendAgree(adminDashData dataMain, string sessionEmail)
        {
            string registrationLink = "http://localhost:5145/Home/pendingReviewAgreement?reqId=" + dataMain._sendAgreement.reqid;

            try
            {
                SendRegistrationEmailMain(dataMain._sendAgreement.email, registrationLink, sessionEmail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        #endregion

        #region SendRegistrationEmailMain
        public void SendRegistrationEmailMain(string toEmail, string registrationLink, string sessionEmail)
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

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + toEmail + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = toEmail,
                Roleid = 1,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (_context.Admins.Any(r => r.Email == sessionEmail))
            {
                emailLog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            }

            if (_context.Physicians.Any(r => r.Email == sessionEmail))
            {
                emailLog.Physicianid = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First();
                emailLog.Roleid = 3;
            }

            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();

            mailMessage.To.Add(toEmail);

            client.Send(mailMessage);
        }

        #endregion

        #region closeCaseMain
        public closeCaseMain closeCaseMain(int reqId)
        {
            var reqClient = _context.Requestclients.Where(r => r.Requestid == reqId).Select(r => new closeCaseMain()
            {
                mobile_num = r.Phonenumber,
                email = r.Email,
                fname = r.Firstname,
                lname = r.Lastname,
                fulldateofbirth = new DateTime((int)(r.Intyear), Convert.ToInt16(r.Strmonth), (int)(r.Intdate)).ToString("yyyy-MM-dd"),
                reqid = r.Requestid,
            }).ToList().FirstOrDefault();

            var cnf = _context.Requests.Where(r => r.Requestid == reqId).FirstOrDefault().Confirmationnumber;
            var requestWiseFile = _context.Requestwisefiles.Where(r => r.Requestid == reqId && r.Isdeleted == null).Select(r => r).ToList();

            reqClient.confirmation_no = cnf;
            reqClient._requestWiseFile = requestWiseFile;

            return reqClient;
        }

        #endregion

        #region closeCaseSaveMain
        public void closeCaseSaveMain(adminDashData obj)
        {

            var requestStatus = _context.Requestclients.Where(r => r.Requestid == obj._closeCaseMain.reqid).Select(r => r).First();
            requestStatus.Email = obj._closeCaseMain.email;
            requestStatus.Phonenumber = obj._closeCaseMain.mobile_num;

            _context.SaveChanges();

        }

        #endregion

        #region closeCaseCloseBtn
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

        #endregion

        #region myProfile
        public myProfile myProfile(string sessionEmail)
        {
            var myProfileMain = _context.Admins.Where(x => x.Email == sessionEmail).Select(x => new myProfile()
            {
                admin_id = x.Adminid,
                fname = x.Firstname,
                lname = x.Lastname,
                email = x.Email,
                confirm_email = x.Email,
                mobile_no = x.Mobile,
                addr1 = x.Address1,
                addr2 = x.Address2,
                city = x.City,
                zip = x.Zip,
                regionId = (int)x.Regionid,
                roles = _context.Aspnetroles.ToList(),
                altphone = x.Altphone,
            }).ToList().FirstOrDefault();

            var userName = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Username).First();
            var pass = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r.Passwordhash).First();

            myProfileMain.username = userName;
            myProfileMain.password = pass;

            return myProfileMain;
        }

        #endregion

        #region myProfileReset
        public bool myProfileReset(myProfile obj, string sessionEmail)
        {
            var aspUser = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if (aspUser.Passwordhash != obj.password)
            {
                var pass = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();
                pass.Passwordhash = obj.password;

                _context.SaveChanges();

                return true;
            }
            return false;

        }

        #endregion

        #region myProfileAdminInfo
        public myProfile myProfileAdminInfo(myProfile obj, string sessionEmail, List<int> adminRegions)
        {
            myProfile _myprofile = new myProfile();
            var aspUser = _context.Aspnetusers.Where(r => r.Email == sessionEmail).Select(r => r).First();

            var adminInfo = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

            var abc = _context.Adminregions.Where(x => x.Adminid == obj.admin_id).Select(r => r.Regionid).ToList();

            var changes = abc.Except(adminRegions);

            if (adminInfo.Firstname != obj.fname || adminInfo.Lastname != obj.lname || adminInfo.Email != obj.email || adminInfo.Mobile != obj.mobile_no || changes.Any() || abc.Count() != adminRegions.Count())
            {
                if (adminInfo.Firstname != obj.fname)
                {
                    adminInfo.Firstname = obj.fname;
                    aspUser.Username = obj.fname;
                }

                if (adminInfo.Lastname != obj.lname)
                {
                    adminInfo.Lastname = obj.lname;
                }

                if (adminInfo.Email != obj.email)
                {
                    adminInfo.Email = obj.email;
                    aspUser.Email = obj.email;
                }

                if (adminInfo.Mobile != obj.mobile_no)
                {
                    adminInfo.Mobile = obj.mobile_no;
                    aspUser.Phonenumber = obj.mobile_no;
                }

                if (_context.Adminregions.Any(x => x.Adminid == obj.admin_id))
                {
                    var adminRegion = _context.Adminregions.Where(x => x.Adminid == obj.admin_id).ToList();

                    _context.Adminregions.RemoveRange(adminRegion);
                    _context.SaveChanges();
                }

                //var phyRegion = _context.Physicianregions.ToList();

                foreach (var item in adminRegions)
                {
                    var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                    _context.Adminregions.Add(new Adminregion
                    {
                        Adminid = (int)obj.admin_id,
                        Regionid = region.Regionid,
                    });
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

        #endregion

        #region myProfileAdminBillingInfo
        public bool myProfileAdminBillingInfo(myProfile obj, string sessionEmail)
        {
            var adminInfo = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if (adminInfo.Address1 != obj.addr1 || adminInfo.Address2 != obj.addr2 || adminInfo.City != obj.city || adminInfo.Zip != obj.zip || adminInfo.Altphone != obj.altphone || adminInfo.Regionid != obj.regionId)
            {

                if (adminInfo.Address1 != obj.addr1)
                {
                    adminInfo.Address1 = obj.addr1;
                }

                if (adminInfo.Address2 != obj.addr2)
                {
                    adminInfo.Address2 = obj.addr2;
                }

                if (adminInfo.City != obj.city)
                {
                    adminInfo.City = obj.city;
                }

                if (adminInfo.Zip != obj.zip)
                {
                    adminInfo.Zip = obj.zip;
                }
                if (adminInfo.Altphone != obj.altphone)
                {
                    adminInfo.Altphone = obj.altphone;
                }
                if (adminInfo.Regionid != obj.regionId)
                {
                    adminInfo.Regionid = obj.regionId;
                }



                _context.SaveChanges();

                return true;
            }

            return false;
        }

        #endregion

        #region concludeEncounter
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
            if (obj3 != null)
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

        #endregion

        #region concludeEncounter
        public concludeEncounter concludeEncounter(concludeEncounter obj)
        {
            concludeEncounter _obj = new concludeEncounter();

            var obj1 = _context.EncounterForms.FirstOrDefault(r => r.Requestid == obj.reqid);

            if (obj1 == null)
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
                _obj.indicate = false;

                var obj2 = _context.EncounterForms.Where(r => r.Requestid == obj.reqid).Select(r => r).First();

                if (obj2.Requestid != obj.reqid || obj2.HistoryIllness != obj.HistoryIllness || obj2.MedicalHistory != obj.MedicalHistory || obj2.Date != obj.Date || obj2.Medications != obj.Medications || obj2.Allergies != obj.Allergies || obj2.Temp != obj.Temp || obj2.Hr != obj.Hr || obj2.Rr != obj.Rr || obj2.BpS != obj.BpS || obj2.BpD != obj.BpD || obj2.O2 != obj.O2 || obj2.Pain != obj.Pain || obj2.Heent != obj.Heent || obj2.Cv != obj.Cv || obj2.Chest != obj.Chest || obj2.Abd != obj.Abd || obj2.Extr != obj.Extr || obj2.Skin != obj.Skin || obj2.Neuro != obj.Neuro || obj2.Other != obj.Other || obj2.Diagnosis != obj.Diagnosis || obj2.TreatmentPlan != obj.TreatmentPlan || obj2.MedicationDispensed != obj.MedicationDispensed || obj2.Procedures != obj.Procedures || obj2.FollowUp != obj.FollowUp)
                {
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
                }


            };


            _context.SaveChanges();

            return _obj;
        }

        #endregion

        #region sendLink
        public sendLink sendLink(sendLink data, string sessionEmail)
        {
            sendLink _send = new sendLink();

            string registrationLink = "http://localhost:5145/Patient/SubmitRequest";

            try
            {
                SendRegistrationEmailSendLink(data.Email, registrationLink, sessionEmail);
                _send.indicate = true;
            }
            catch (Exception e)
            {
                _send.indicate = false;
            }

            return _send;
        }

        #endregion

        #region SendRegistrationEmailSendLink
        public void SendRegistrationEmailSendLink(string email, string registrationLink, string sessionEmail)
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
                Subject = "SubmitRequest",
                IsBodyHtml = true,
                Body = $"Click the following link to Create Request: <a href='{registrationLink}'>{registrationLink}</a>"
            };


            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + email + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = email,
                Roleid = 1,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (_context.Admins.Any(r => r.Email == sessionEmail))
            {
                emailLog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            }

            if (_context.Physicians.Any(r => r.Email == sessionEmail))
            {
                emailLog.Physicianid = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First();
                emailLog.Roleid = 3;
            }
            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();

            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }

        #endregion

        #region createRequest
        public createRequest createRequest(createRequest data, string sessionEmail, int flag)
        {
            if (flag != 15)
            {
                createRequest _create = new createRequest();

                var stateMain = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).FirstOrDefault();

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
                        _user.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                        _user.Createdby = _asp.Id;
                        _user.Createddate = DateTime.Now;
                        _user.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).Select(r => r.Regionid).FirstOrDefault();
                        _context.Users.Add(_user);
                        _context.SaveChanges();

                        _role.Userid = _asp.Id;
                        _role.Roleid = 2;

                        _context.Aspnetuserroles.Add(_role);
                        _context.SaveChanges();

                        string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _asp.Id;

                        try
                        {
                            SendRegistrationEmailCreateRequest(data.email, registrationLink, sessionEmail);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    _req.Requesttypeid = 1;
                    if (existUser == null)
                    {
                        _req.Userid = _context.Users.Where(r => r.Email == data.email).Select(r => r.Userid).First();
                    }
                    else
                    {
                        var a = _context.Users.Where(r => r.Email == data.email).Select(r => r.Userid).First();
                        _req.Userid = a;
                    }

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
                    _reqClient.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).Select(r => r.Regionid).FirstOrDefault();
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
            else
            {
                createRequest _create = new createRequest();

                var stateMain = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).FirstOrDefault();

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

                    var _phy = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

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
                        _user.Intyear = Convert.ToInt16(data.dateofbirth.Substring(0, 4));
                        _user.Createdby = _asp.Id;
                        _user.Createddate = DateTime.Now;
                        _user.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).Select(r => r.Regionid).FirstOrDefault();
                        _context.Users.Add(_user);
                        _context.SaveChanges();

                        _role.Userid = _asp.Id;
                        _role.Roleid = 2;

                        _context.Aspnetuserroles.Add(_role);
                        _context.SaveChanges();

                        string registrationLink = "http://localhost:5145/Home/CreateAccount?aspuserId=" + _asp.Id;

                        try
                        {
                            SendRegistrationEmailCreateRequest(data.email, registrationLink, sessionEmail);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    _req.Requesttypeid = 1;
                    _req.Userid = _context.Users.Where(r => r.Email == data.email).Select(r => r.Userid).First();
                    _req.Firstname = _phy.Firstname;
                    _req.Lastname = _phy.Lastname;
                    _req.Phonenumber = _phy.Mobile;
                    _req.Email = _phy.Email;
                    _req.Status = 2;
                    _req.Physicianid = _phy.Physicianid;
                    _req.Confirmationnumber = _phy.Firstname.Substring(0, 1) + DateTime.Now.ToString().Substring(0, 19);
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
                    _reqClient.Regionid = _context.Regions.Where(r => r.Name.ToLower() == data.state.Trim().ToLower()).Select(r => r.Regionid).FirstOrDefault();
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

        }

        #endregion

        #region SendRegistrationEmailCreateRequest
        public void SendRegistrationEmailCreateRequest(string email, string registrationLink, string sessionEmail)
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

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + email + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = email,
                Roleid = 1,
                //Physicianid = phyIdMain,
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            if (_context.Admins.Any(r => r.Email == sessionEmail))
            {
                emailLog.Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First();
            }

            if (_context.Physicians.Any(r => r.Email == sessionEmail))
            {
                emailLog.Physicianid = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First();
                emailLog.Roleid = 3;
            }
            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();


            mailMessage.To.Add(email);

            client.Send(mailMessage);
        }
        #endregion

        #region verifyState
        public createRequest verifyState(string state)
        {
            createRequest _create = new createRequest();

            var stateMain = _context.Regions.Where(r => r.Name.ToLower() == state.Trim().ToLower()).FirstOrDefault();

            if (stateMain == null)
            {
                _create.indicate = false;
            }
            else
            {
                _create.indicate = true;
            }

            return _create;
        }
        #endregion

        #region GenerateExcelFile
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
        #endregion

        #region providerMain
        public provider providerMain(int regionId = 0)
        {
            BitArray deletedBit = new BitArray(new[] { false });

            provider provider = new provider()
            {
                _physician = _context.Physicians.OrderByDescending(r => r.Physicianid).Select(r => r).ToList(),
                _notification = _context.Physiciannotifications.ToList(),
                _roles = _context.Roles.Where(r => r.Isdeleted.Equals(deletedBit)).Select(r => r).ToList(),
                _shift = _context.Shifts.ToList(),
                _shiftDetails = _context.Shiftdetails.ToList(),
                DateTime = DateTime.Now,
            };



            if (regionId != 0)
            {
                provider._physician = _context.Physicians.Where(r => r.Regionid == regionId).ToList();
            }

            return provider;
        }
        #endregion

        #region stopNotification
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

        #endregion

        #region providerContact
        public provider providerContact(int phyId)
        {
            provider _provider = new provider()
            {
                phyId = phyId,
            };

            return _provider;
        }
        #endregion

        #region providerContactEmail
        public provider providerContactEmail(int phyIdMain, string msg, string sessionEmail)
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
        #endregion

        #region providerContactSms
        public provider providerContactSms(int phyIdMain, string msg, string sessionEmail)
        {
            provider _provider = new provider();

            _provider.phyId = phyIdMain;

            var provider = _context.Physicians.Where(r => r.Physicianid == phyIdMain).Select(r => r).First();

            try
            {
                var accountSid = "ACaebecb91ffc32c15dd5694bbc3a27883";
                var authToken = "27e0529de8732f6e126c6fcde86a7b91";
                var twilionumber = "+16562192200";

                var messageBody = $"Hello {provider.Firstname} {provider.Lastname},\n {msg} \n\n\nRegards,\n(HelloDoc Admin)";

                TwilioClient.Init(accountSid, authToken);

                var messagee = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber(twilionumber),
                body: messageBody,
                to: new Twilio.Types.PhoneNumber("+91" + provider.Mobile)
                );

                Smslog smslog = new Smslog()
                {
                    Smstemplate = "Sender : " + twilionumber + "Reciver :" + provider.Mobile + "Message : " + msg,
                    Mobilenumber = provider.Mobile,
                    Roleid = 1,
                    Adminid = _context.Admins.Where(r => r.Email == sessionEmail).Select(r => r.Adminid).First(),
                    Createdate = DateTime.Now,
                    Sentdate = DateTime.Now,
                    Issmssent = new BitArray(1, true),
                    Confirmationnumber = provider.Firstname.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                    Senttries = 1,
                };

                _context.Smslogs.Add(smslog);
                _context.SaveChanges();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return _provider;
        }
        #endregion

        #region SendRegistrationproviderContactEmail
        private void SendRegistrationproviderContactEmail(string provider, string msg, string sessionEmail, int phyIdMain)
        {
            var phyName = _context.Physicians.Where(r => r.Physicianid == phyIdMain).Select(r => r.Firstname).First();
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
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = phyName.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };

            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();

            mailMessage.To.Add(provider);

            client.Send(mailMessage);
        }

        #endregion

        #region adminEditPhysicianProfile
        public AdminEditPhysicianProfile adminEditPhysicianProfile(int phyId, string sessionEmail, int flag, int statusId)
        {
            if (statusId != 2)
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
                    flagId = (int)flag,

                    username = user.Username,
                    password = user.Passwordhash,
                };

                return _profile;
            }
            else
            {
                var phy = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

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
                    PhyID = phy.Physicianid,
                    Roleid = phy.Roleid,
                    Regionid = phy.Regionid,
                    PhotoValue = phy.Photo,
                    SignatureValue = phy.Signature,
                    IsContractorAgreement = phy.Isagreementdoc == null ? false : true,
                    IsBackgroundCheck = phy.Isbackgrounddoc == null ? false : true,
                    IsHIPAA = phy.Istrainingdoc == null ? false : true,
                    IsNonDisclosure = phy.Isnondisclosuredoc == null ? false : true,
                    IsLicenseDocument = phy.Islicensedoc == null ? false : true,
                    flagId = (int)flag,

                    username = user.Username,
                    password = user.Passwordhash,
                };

                return _profile;
            }



        }

        #endregion

        #region RegionTable
        List<DAL_Data_Access_Layer_.DataModels.Region> IAdminDash.RegionTable()
        {
            var region = _context.Regions.ToList();
            return region;
        }
        #endregion

        #region PhyRegionTable
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
        #endregion

        #region physicainRole
        public List<Role> physicainRole()
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var role = _context.Roles.Where(i => i.Isdeleted.Equals(deletedBit) && i.Accounttype == 3).ToList();

            return role;
        }
        #endregion

        #region adminRole
        public List<Role> adminRole()
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var role = _context.Roles.Where(i => i.Isdeleted.Equals(deletedBit) && i.Accounttype == 1).ToList();

            return role;
        }
        #endregion

        #region providerResetPass
        public bool providerResetPass(string email, string password)
        {
            var resetPass = _context.Aspnetusers.Where(r => r.Email == email).Select(r => r).First();

            if (resetPass.Passwordhash != password)
            {
                resetPass.Passwordhash = password;
                _context.SaveChanges();

                return true;
            }
            return false;

        }
        #endregion

        #region editProviderForm1
        public bool editProviderForm1(int phyId, int roleId, int statusId)
        {
            var user = _context.Physicians.Where(r => r.Physicianid == phyId).Select(r => r).First();

            if (user.Status != (short)statusId || user.Roleid != roleId)
            {
                user.Status = (short)statusId;
                user.Roleid = roleId;

                _context.SaveChanges();
                return true;
            }
            return false;
        }
        #endregion

        #region editProviderForm2
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
                if (user.Email != email)
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

        #endregion

        #region editProviderForm3
        public AdminEditPhysicianProfile editProviderForm3(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var data = _context.Physicians.Where(r => r.Physicianid == dataMain._providerEdit.PhyID).Select(r => r).First();
            var location = _context.Physicianlocations.Where(r => r.Physicianid == dataMain._providerEdit.PhyID).Select(r => r).First();

            if (data.Address1 != dataMain._providerEdit.Address1 || data.Address2 != dataMain._providerEdit.Address2 || data.City != dataMain._providerEdit.city || data.Regionid != dataMain._providerEdit.Regionid || data.Zip != dataMain._providerEdit.zipcode || data.Altphone != dataMain._providerEdit.altPhone)
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
        #endregion

        #region PhysicianBusinessInfoUpdate
        public AdminEditPhysicianProfile PhysicianBusinessInfoUpdate(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var physician = _context.Physicians.FirstOrDefault(x => x.Physicianid == dataMain._providerEdit.PhyID);

            flag.indicate = false;

            if (physician != null)
            {

                if (physician.Businessname != dataMain._providerEdit.Businessname || physician.Businesswebsite != dataMain._providerEdit.BusinessWebsite || physician.Adminnotes != dataMain._providerEdit.Adminnotes)
                {
                    physician.Businessname = dataMain._providerEdit.Businessname;
                    physician.Businesswebsite = dataMain._providerEdit.BusinessWebsite;
                    physician.Adminnotes = dataMain._providerEdit.Adminnotes;
                    physician.Modifieddate = DateTime.Now;

                    _context.SaveChanges();

                    flag.indicate = true;

                }

                if (dataMain._providerEdit.Photo != null || dataMain._providerEdit.Signature != null)
                {
                    AddProviderBusinessPhotos(dataMain._providerEdit.Photo, dataMain._providerEdit.Signature, dataMain._providerEdit.PhyID);
                    flag.indicate = true;
                }


            }

            flag.PhyID = dataMain._providerEdit.PhyID;
            return flag;
        }
        #endregion

        #region MyRegion
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
        #endregion

        #region EditOnBoardingData
        public AdminEditPhysicianProfile EditOnBoardingData(adminDashData dataMain)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            flag.indicate = false;

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

                flag.indicate = true;
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

                flag.indicate = true;
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

                flag.indicate = true;
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

                flag.indicate = true;
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

                flag.indicate = true;
            }

            _context.SaveChanges();

            flag.PhyID = dataMain._providerEdit.PhyID;

            return flag;

        }

        #endregion

        #region createProviderAccount
        public AdminEditPhysicianProfile createProviderAccount(adminDashData obj, List<int> physicianRegions)
        {
            AdminEditPhysicianProfile flag = new AdminEditPhysicianProfile();

            var aspUser = _context.Aspnetusers.FirstOrDefault(r => r.Email == obj._providerEdit.Email);


            if (aspUser == null && obj._providerEdit.latitude != null)
            {


                Aspnetuser _user = new Aspnetuser();
                Physician phy = new Physician();
                _user.Username = "MD." + obj._providerEdit.Lastname + "." + obj._providerEdit.Firstname.Substring(0, 1);
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
                phy.Npinumber = obj._providerEdit.NPInumber;

                _context.Physicians.Add(phy);
                _context.SaveChanges();

                PayRate payRate = new PayRate();

                payRate.PhysicianId = phy.Physicianid;
                payRate.CreatedDate = DateTime.Now;

                _context.PayRates.Add(payRate);
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

                if (_context.Physicianlocations.Any(r => r.Latitude == decimal.Round(obj._providerEdit.latitude, 6)))
                {
                    _phyLoc.Latitude = obj._providerEdit.latitude + 0.100000m;
                }
                else
                {

                    _phyLoc.Latitude = obj._providerEdit.latitude;
                }
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
            else if (aspUser != null)
            {
                flag.indicateTwo = "email";
                return flag;
            }
            else
            {
                flag.indicateTwo = "zip";
                return flag;
            }

            return flag;
        }

        #endregion

        #region AddProviderDocuments
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

        #endregion

        #region editProviderDeleteAccount
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
        #endregion

        /// <summary>
        /// Access
        /// </summary>
        /// <returns></returns>

        #region GetAccountAccessData
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
        #endregion

        #region GetAccountType
        public List<Aspnetrole> GetAccountType()
        {
            var role = _context.Aspnetroles.ToList();
            return role;
        }
        #endregion

        #region GetMenu
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
        #endregion

        #region SetCreateAccessAccount
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
        #endregion

        #region GetAccountMenu
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
        #endregion

        #region GetEditAccessData
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
        #endregion

        #region SetEditAccessAccount
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
        #endregion

        #region DeleteAccountAccess
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
        #endregion

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

                    aspnetid = r.Adminid,
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
                    aspnetid = (int)r.Physicianid,
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

                aspnetid = r.Adminid,
                Accounttype = "Admin",
                accountname = r.Firstname + ", " + r.Lastname,
                phone = r.Mobile,
                status = (int)r.Status,
                openrequest = Adrequest,
                roleid = 1,
            }).ToList();


            var phdata = Providerdata.Select(r => new UserAccess()
            {
                aspnetid = (int)r.Physicianid,
                Accounttype = "Provider",
                accountname = r.Firstname + ", " + r.Lastname,
                phone = r.Mobile,
                status = (int)r.Status,
                openrequest = _context.Requests.Where(i => i.Physicianid == r.Physicianid && (i.Status == 1 || i.Status == 2 || i.Status == 4 || i.Status == 5 || i.Status == 6)).Count(),
                roleid = 2,
            }).ToList();


            var combineddata = Addata.Concat(phdata).OrderByDescending(x => x.aspnetid).ToList();

            return combineddata;
        }

        public void createAdminAccount(adminDashData obj, List<int> regions)
        {
            var aspUser = _context.Aspnetusers.FirstOrDefault(r => r.Email == obj._providerEdit.Email);

            if (aspUser == null)
            {
                Aspnetuser _user = new Aspnetuser();
                Admin admin = new Admin();

                _user.Username = "AD." + obj._providerEdit.Lastname + obj._providerEdit.Firstname.Substring(0, 1);
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

        public AdminEditPhysicianProfile adminEditPage(int adminId)
        {
            BitArray bitArray = new BitArray(1, true);

            var query = _context.Admins.Where(x => x.Adminid == adminId).Select(r => new AdminEditPhysicianProfile()
            {
                username = r.Aspnetuser.Username,
                Email = r.Email,
                Con_Email = r.Email,
                password = r.Aspnetuser.Passwordhash,
                adminId = adminId,
                aspnetUserId = r.Aspnetuserid,
                Firstname = r.Firstname,
                Lastname = r.Lastname,
                PhoneNumber = r.Mobile,

                Address1 = r.Address1,
                Address2 = r.Address2,
                city = r.City,
                Regionid = r.Regionid,
                zipcode = r.Zip,
                altPhone = r.Altphone,
                createdBy = r.Createdby,
                created_date = r.Createddate,
                modifiedBy = r.Modifiedby,
                modified_date = r.Modifieddate,
                Status = r.Status.ToString(),
                Roleid = r.Roleid,

                roles = _context.Roles.Where(x => x.Accounttype == 1).Select(x => x).ToList(),//1 for admin



            }).ToList().First();
            query.State = _context.Regions.Where(x => x.Regionid == query.Regionid).Select(x => x.Name).FirstOrDefault();

            return query;
        }

        public List<AdminregionTable> GetRegionsAdmin(int adminId)
        {
            var regions = _context.Regions.ToList();

            var AdminRegion = _context.Adminregions.ToList();

            var CheckdRegion = regions.Select(r1 => new AdminregionTable
            {
                Regionid = r1.Regionid,
                Name = r1.Name,
                ExistsInTable = AdminRegion.Any(r2 => r2.Adminid == adminId && r2.Regionid == r1.Regionid)
            }).ToList();

            return CheckdRegion;
        }


        public bool EditAdminDetailsDb(adminDashData adminDashData, string email, List<int> adminRegions)
        {
            if (_context.Admins.Where(x => x.Adminid == adminDashData._providerEdit.adminId).Any())
            {

                var query = _context.Admins.Where(x => x.Adminid == adminDashData._providerEdit.adminId).Select(r => r).First();

                var asp_row = _context.Aspnetusers.Where(x => x.Id == adminDashData._providerEdit.aspnetUserId).Select(r => r).First();

                if (asp_row.Email != adminDashData._providerEdit.Email || asp_row.Passwordhash != adminDashData._providerEdit.password)
                {
                    asp_row.Email = adminDashData._providerEdit.Email;
                    asp_row.Passwordhash = adminDashData._providerEdit.password;
                    _context.SaveChanges();
                }

                query.Email = adminDashData._providerEdit.Email;


                query.Aspnetuser.Passwordhash = adminDashData._providerEdit.password;

                query.Firstname = adminDashData._providerEdit.Firstname;

                query.Lastname = adminDashData._providerEdit.Lastname;

                query.Mobile = adminDashData._providerEdit.PhoneNumber;

                query.Address1 = adminDashData._providerEdit.Address1;

                query.Address2 = adminDashData._providerEdit.Address2;

                query.City = adminDashData._providerEdit.city;

                query.Regionid = adminDashData._providerEdit.Regionid;

                query.Zip = adminDashData._providerEdit.zipcode;

                query.Altphone = adminDashData._providerEdit.altPhone;

                query.Modifiedby = _context.Aspnetusers.Where(x => x.Email == email).Select(X => X.Id).First();

                query.Modifieddate = DateTime.Now;

                query.Status = 1;

                //if(query.Roleid != adminDashData._providerEdit.Roleid)
                //{
                //    var aspNetUserRole = _context.Aspnetuserroles.Where(r => r.Userid == asp_row.Id).Select(r => r).First();
                //    var Role = _context.Aspnetroles.Where(r => r.Id == query.Roleid).Select(r => r).First();
                //    aspNetUserRole.Roleid = Role.Id;
                //}
                query.Roleid = (int)adminDashData._providerEdit.Roleid;

                _context.SaveChanges();

                var abc = _context.Adminregions.Where(x => x.Adminid == adminDashData._providerEdit.adminId).Select(r => r.Regionid).ToList();

                var changes = abc.Except(adminRegions);


                if (changes.Any() || abc.Count() != adminRegions.Count())
                {
                    if (_context.Adminregions.Any(x => x.Adminid == adminDashData._providerEdit.adminId))
                    {
                        var adminRegion = _context.Adminregions.Where(x => x.Adminid == adminDashData._providerEdit.adminId).ToList();

                        _context.Adminregions.RemoveRange(adminRegion);
                        _context.SaveChanges();
                    }

                    //var phyRegion = _context.Physicianregions.ToList();

                    foreach (var item in adminRegions)
                    {
                        var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                        _context.Adminregions.Add(new Adminregion
                        {
                            Adminid = adminDashData._providerEdit.adminId,
                            Regionid = region.Regionid,
                        });
                    }
                    _context.SaveChanges();
                }



                //var abc = _context.Physicianregions.Where(x => x.Physicianid == phyId).Select(r => r.Regionid).ToList();

                //var changes = abc.Except(phyRegionArray);


                //if (changes.Any() || abc.Count() != phyRegionArray.Length)
                //{
                //    if (_context.Physicianregions.Any(x => x.Physicianid == phyId))
                //    {
                //        var physicianRegion = _context.Physicianregions.Where(x => x.Physicianid == phyId).ToList();

                //        _context.Physicianregions.RemoveRange(physicianRegion);
                //        _context.SaveChanges();
                //    }

                //    var phyRegion = _context.Physicianregions.ToList();

                //    foreach (var item in phyRegionArray)
                //    {
                //        var region = _context.Regions.FirstOrDefault(x => x.Regionid == item);

                //        _context.Physicianregions.Add(new Physicianregion
                //        {
                //            Physicianid = phyId,
                //            Regionid = region.Regionid,
                //        });
                //    }
                //    _context.SaveChanges();
                //    indicate = true;
                //}

                //return indicate;


                return true;

            }
            else
            {
                return false;
            }

        }


        public void editDeleteAdminAccount(int adminId)
        {
            var adminMain = _context.Admins.Where(r => r.Adminid == adminId).Select(r => r).First();

            if (adminMain.Isdeleted == null)
            {
                adminMain.Isdeleted = true;

                _context.SaveChanges();
            }

            var adminReg = _context.Adminregions.Where(r => r.Adminid == adminId).ToList();
            if (adminReg != null)
            {
                _context.Adminregions.RemoveRange(adminReg);
                _context.SaveChanges();
            }
        }



        //*******************************************************Provider Location***********************************************

        public List<Physicianlocation> GetPhysicianlocations()
        {
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

            if (vendor.Isdeleted == null)
            {
                vendor.Isdeleted = new BitArray(1, true);
                _context.SaveChanges();
            }
        }


        //*****************************************************Scheduling****************************************

        public List<Physician> GetPhysicians(int regionid)
        {
            if (regionid == 0)
            {
                var physicians = _context.Physicians.Where(r => r.Isdeleted == null).Select(r => r).ToList();
                return physicians;
            }
            else
            {
                var physicians1 = _context.Physicians.Where(i => i.Regionid == regionid && i.Isdeleted == null).ToList();
                return physicians1;
            }

        }

        public List<Physician> GetRegionvalue(int selectedregion)
        {
            var query = from r in _context.Physicianregions
                        join rw in _context.Physicians on r.Physicianid equals rw.Physicianid
                        where r.Regionid == selectedregion && rw.Isdeleted == null
                        select (new Physician()
                        {
                            Physicianid = rw.Physicianid,
                            Firstname = rw.Firstname,
                        });

            var data = query.ToList();

            return data;
        }


        public bool createShift(ScheduleModel scheduleModel, int Aspid)
        {
            if (_context.Shifts.Where(x => x.Physicianid == scheduleModel.Physicianid).Count() > 1)
            {
                var shiftData = _context.Shifts.Where(i => i.Physicianid == scheduleModel.Physicianid).ToList();
                var shiftDetailData = new List<Shiftdetail>();

                foreach (var obj in shiftData)
                {
                    var details = _context.Shiftdetails.Where(x => x.Shiftid == obj.Shiftid).ToList();
                    shiftDetailData.AddRange(details);
                }


                foreach (var obj in shiftDetailData)
                {
                    var shiftDate = new DateTime(scheduleModel.Startdate.Year, scheduleModel.Startdate.Month, scheduleModel.Startdate.Day);

                    if (obj.Shiftdate.Date == shiftDate.Date)
                    {
                        if ((obj.Starttime <= scheduleModel.Starttime && obj.Endtime >= scheduleModel.Starttime) || (obj.Starttime <= scheduleModel.Endtime && obj.Endtime >= scheduleModel.Endtime) || (obj.Starttime >= scheduleModel.Starttime && obj.Endtime <= scheduleModel.Endtime))
                        {
                            return false;
                        }
                    }
                }
            }

            Shift shift = new Shift();
            shift.Physicianid = scheduleModel.Physicianid;
            shift.Repeatupto = scheduleModel.Repeatupto;
            shift.Startdate = scheduleModel.Startdate;
            shift.Createdby = Aspid;
            shift.Createddate = DateTime.Now;
            shift.Isrepeat = scheduleModel.Isrepeat == false ? new BitArray(1, false) : new BitArray(1, true);
            shift.Repeatupto = scheduleModel.Repeatupto;
            shift.Weekdays = scheduleModel.checkWeekday;
            _context.Shifts.Add(shift);
            _context.SaveChanges();

            Shiftdetail sd = new Shiftdetail();
            sd.Shiftid = shift.Shiftid;
            sd.Shiftdate = new DateTime(scheduleModel.Startdate.Year, scheduleModel.Startdate.Month, scheduleModel.Startdate.Day);
            sd.Starttime = scheduleModel.Starttime;
            sd.Endtime = scheduleModel.Endtime;
            sd.Regionid = scheduleModel.Regionid;
            sd.Status = 1;
            sd.Isdeleted = new BitArray(1, false);
            _context.Shiftdetails.Add(sd);
            _context.SaveChanges();

            Shiftdetailregion sr = new Shiftdetailregion();
            sr.Shiftdetailid = sd.Shiftdetailid;
            sr.Regionid = scheduleModel.Regionid;
            sr.Isdeleted = new BitArray(1, false);
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
                        if (nextOccurrence.DayOfWeek == desiredDayOfWeek && (nextOccurrence.Day != scheduleModel.Startdate.Day))
                        {

                            Shiftdetail sdd = new Shiftdetail();
                            sdd.Shiftid = shift.Shiftid;
                            sdd.Shiftdate = nextOccurrence;
                            sdd.Starttime = scheduleModel.Starttime;
                            sdd.Endtime = scheduleModel.Endtime;
                            sdd.Regionid = scheduleModel.Regionid;
                            sdd.Status = 1;
                            sdd.Isdeleted = new BitArray(1, false);
                            _context.Shiftdetails.Add(sdd);
                            _context.SaveChanges();

                            Shiftdetailregion srr = new Shiftdetailregion();
                            srr.Shiftdetailid = sdd.Shiftdetailid;
                            srr.Regionid = scheduleModel.Regionid;
                            srr.Isdeleted = new BitArray(1, false);
                            _context.Shiftdetailregions.Add(srr);
                            _context.SaveChanges();
                            occurrencesFound++;
                        }
                        nextOccurrence = nextOccurrence.AddDays(1);
                    }
                }
            }

            return true;
        }


        public List<ShiftDetailsmodal> ShiftDetailsmodal(DateTime date, DateTime sunday, DateTime saturday, string type, int flag, string email)
        {


            if (flag != 15)
            {
                var shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year);

                BitArray deletedBit = new BitArray(new[] { true });

                switch (type)
                {
                    case "month":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && !u.Isdeleted.Equals(deletedBit));
                        break;

                    case "week":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate >= sunday.Date && u.Shiftdate <= saturday && !u.Isdeleted.Equals(deletedBit));
                        break;

                    case "day":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && u.Shiftdate.Day == date.Day && !u.Isdeleted.Equals(deletedBit));
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
                    Regionid = s.Regionid,
                }).ToList();

                return list;
            }
            else
            {
                var phyMain = _context.Physicians.Where(r => r.Email == email).Select(r => r).First();

                var shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year);

                BitArray deletedBit = new BitArray(new[] { true });

                switch (type)
                {
                    case "month":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && !u.Isdeleted.Equals(deletedBit));
                        break;

                    case "week":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate >= sunday.Date && u.Shiftdate <= saturday && !u.Isdeleted.Equals(deletedBit));
                        break;

                    case "day":
                        shiftdetails = _context.Shiftdetails.Where(u => u.Shiftdate.Month == date.Month && u.Shiftdate.Year == date.Year && u.Shiftdate.Day == date.Day && !u.Isdeleted.Equals(deletedBit));
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
                    Regionid = s.Regionid,
                }).ToList();

                list = list.Where(r => r.Physicianid == phyMain.Physicianid).Select(r => r).ToList();

                return list;
            }


        }

        public ShiftDetailsmodal GetShift(int shiftdetailsid)
        {
            var shiftdetails = _context.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftdetailsid);
            var shifttable = _context.Shifts.FirstOrDefault(s => s.Shiftid == shiftdetails.Shiftid);
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
                Physicianid = shifttable.Physicianid,
            };

            return shift;
        }


        public void SetReturnShift(int status, int shiftdetailid, int Aspid)
        {
            var shiftdetails = _context.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftdetailid);
            if (status == 1)
            {
                shiftdetails.Status = 2;
                shiftdetails.Modifieddate = DateTime.Now;
                shiftdetails.Modifiedby = Aspid;
            }
            else
            {
                shiftdetails.Status = 1;
                shiftdetails.Modifieddate = DateTime.Now;
                shiftdetails.Modifiedby = Aspid;
            }
            _context.SaveChanges();
        }


        public void SetDeleteShift(int shiftdetailid, int Aspid)
        {
            var shiftdetails = _context.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftdetailid);

            shiftdetails.Isdeleted = new BitArray(1, true);
            shiftdetails.Modifieddate = DateTime.Now;
            shiftdetails.Modifiedby = Aspid;

            _context.SaveChanges();
        }


        public bool SetEditShift(ShiftDetailsmodal shiftDetailsmodal, int Aspid)
        {
            var shiftdetails = _context.Shiftdetails.FirstOrDefault(s => s.Shiftdetailid == shiftDetailsmodal.Shiftdetailid);

            if (shiftdetails != null)
            {
                shiftdetails.Shiftdate = shiftDetailsmodal.Shiftdate;
                shiftdetails.Starttime = shiftDetailsmodal.Starttime;
                shiftdetails.Endtime = shiftDetailsmodal.Endtime;
                shiftdetails.Modifieddate = DateTime.Now;
                shiftdetails.Modifiedby = Aspid;
                _context.SaveChanges();
            }
            return true;
        }


        //MDs On call
        public OnCallModal GetOnCallDetails(int regionId)
        {
            var currentTime = new TimeOnly(DateTime.Now.Hour, DateTime.Now.Minute);
            BitArray deletedBit = new BitArray(new[] { false });

            var onDutyQuery = _context.Shiftdetails
                .Include(sd => sd.Shift.Physician)
                .Where(sd => (regionId == 0 || sd.Shift.Physician.Physicianregions.Any(pr => pr.Regionid == regionId)) &&
                             sd.Shiftdate.Date == DateTime.Today &&
                             currentTime >= sd.Starttime &&
                             currentTime <= sd.Endtime &&
                             sd.Isdeleted.Equals(deletedBit))
                .Select(sd => sd.Shift.Physician)
                .Distinct()
                .ToList();


            var offDutyQuery = _context.Physicians
                .Include(p => p.Physicianregions)
                .Where(p => (regionId == 0 || p.Physicianregions.Any(pr => pr.Regionid == regionId))
                    && !_context.Shiftdetails.Any(sd =>
                    sd.Shift.Physicianid == p.Physicianid &&
                    sd.Shiftdate.Date == DateTime.Today &&
                    currentTime >= sd.Starttime &&
                    currentTime <= sd.Endtime &&
                    sd.Isdeleted.Equals(deletedBit)) && p.Isdeleted == null).ToList();
            var onCallModal = new OnCallModal
            {
                OnCall = onDutyQuery,
                OffDuty = offDutyQuery,
                regions = GetRegions(),
            };

            return onCallModal;
        }


        public List<ShiftReview> GetShiftReview(int regionId, int callId)
        {
            BitArray deletedBit = new BitArray(new[] { false });

            var shiftDetail = _context.Shiftdetails.Where(i => i.Isdeleted.Equals(deletedBit) && i.Status != 2);

            DateTime currentDate = DateTime.Now;

            if (regionId != 0)
            {
                shiftDetail = shiftDetail.Where(i => i.Regionid == regionId);
            }

            if (callId == 1)
            {
                shiftDetail = shiftDetail.Where(i => i.Shiftdate.Month == currentDate.Month);
            }

            var reviewList = shiftDetail.Select(x => new ShiftReview
            {
                shiftDetailId = x.Shiftdetailid,
                PhysicianName = _context.Physicians.FirstOrDefault(y => y.Physicianid == _context.Shifts.FirstOrDefault(z => z.Shiftid == x.Shiftid).Physicianid).Firstname + ", " + _context.Physicians.FirstOrDefault(y => y.Physicianid == _context.Shifts.FirstOrDefault(z => z.Shiftid == x.Shiftid).Physicianid).Lastname,
                ShiftDate = x.Shiftdate.ToString("MMM dd, yyyy"),
                ShiftTime = x.Starttime.ToString("hh:mm tt") + " - " + x.Endtime.ToString("hh:mm tt"),
                ShiftRegion = _context.Regions.FirstOrDefault(y => y.Regionid == x.Regionid).Name,
            }).ToList();
            return reviewList;
        }
        public void ApproveSelectedShift(int[] shiftDetailsId, int Aspid)
        {
            foreach (var shiftId in shiftDetailsId)
            {
                var shift = _context.Shiftdetails.FirstOrDefault(i => i.Shiftdetailid == shiftId);
                shift.Status = 2;
                shift.Modifieddate = DateTime.Now;
                shift.Modifiedby = Aspid;
            }
            _context.SaveChanges();
        }

        public void DeleteShiftReview(int[] shiftDetailsId, int Aspid)
        {
            foreach (var shiftId in shiftDetailsId)
            {
                var shift = _context.Shiftdetails.FirstOrDefault(i => i.Shiftdetailid == shiftId);

                shift.Isdeleted = new BitArray(1, true);
                shift.Modifieddate = DateTime.Now;
                shift.Modifiedby = Aspid;

            }
            _context.SaveChanges();
        }


        public List<DAL_Data_Access_Layer_.DataModels.Region> GetRegions()
        {
            var regions = _context.Regions.ToList();
            return regions;
        }



        //****************************************************************Records********************************************************

        public List<requestsRecordModel> searchRecords(recordsModel recordsModel)
        {
            //List<requestsRecordModel> listdata = new List<requestsRecordModel>();
            //requestsRecordModel requestsRecordModel = new requestsRecordModel();

            var requestList = _context.Requests.Where(r => r.Isdeleted == null).Select(x => new requestsRecordModel()
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

            if (recordsModel.requestListMain != null)
            {
                if (recordsModel.requestListMain[0].searchRecordOne != null && recordsModel.requestListMain[0].searchRecordOne != 0)
                {
                    requestList = requestList.Where(r => r.statusId != null && r.statusId == recordsModel.requestListMain[0].searchRecordOne).Select(r => r).ToList();
                }

                if (recordsModel.requestListMain[0].searchRecordTwo != null)
                {
                    requestList = requestList.Where(r => r.patientname != null && r.patientname.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.requestListMain[0].searchRecordThree != null && recordsModel.requestListMain[0].searchRecordThree != 0)
                {
                    requestList = requestList.Where(r => r.requesttypeid != null && r.requesttypeid == recordsModel.requestListMain[0].searchRecordThree).Select(r => r).ToList();
                }

                if (recordsModel.requestListMain[0].searchRecordSix != null)
                {
                    requestList = requestList.Where(r => r.physician != null && r.physician.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordSix.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.requestListMain[0].searchRecordSeven != null)
                {
                    requestList = requestList.Where(r => r.email != null && r.email.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordSeven.Trim().ToLower())).Select(r => r).ToList();
                }

                if (recordsModel.requestListMain[0].searchRecordEight != null)
                {
                    requestList = requestList.Where(r => r.contact != null && r.contact.Trim().ToLower().Contains(recordsModel.requestListMain[0].searchRecordEight.Trim().ToLower())).Select(r => r).ToList();
                }
            }

            return requestList;
        }

        public void DeleteRecords(int reqId)
        {
            var reqClient = _context.Requests.Where(r => r.Requestid == reqId).Select(r => r).First();

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

            if (GetRecordsModel != null)
            {
                if (GetRecordsModel.searchRecordOne != null)
                {
                    data.users = data.users.Where(r => r.Firstname != null && r.Firstname.Trim().ToLower().Contains(GetRecordsModel.searchRecordOne.Trim().ToLower())).Select(r => r).ToList();
                }
                if (GetRecordsModel.searchRecordTwo != null)
                {
                    data.users = data.users.Where(r => r.Lastname != null && r.Lastname.Trim().ToLower().Contains(GetRecordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                }
                if (GetRecordsModel.searchRecordThree != null)
                {
                    data.users = data.users.Where(r => r.Email != null && r.Email.Trim().ToLower().Contains(GetRecordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                }
                if (GetRecordsModel.searchRecordFour != null)
                {
                    data.users = data.users.Where(r => r.Mobile != null && r.Mobile.Trim().ToLower().Contains(GetRecordsModel.searchRecordFour.Trim().ToLower())).Select(r => r).ToList();
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
                fullname = r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Firstname).First() + " " + r.Requestclients.Where(x => x.Requestid == r.Requestid).Select(r => r.Lastname).First(),
                concludedate = r.Status == 6 ? Convert.ToDateTime(r.Modifieddate).ToString("yyyy-MM-dd") : null, // Add this condition to check if the status is equal to 6,
            }).ToList();

            return dataMain;
        }


        public List<blockHistory> blockHistory(recordsModel recordsModel)
        {
            var requestData = _context.Blockrequests.Where(x => x.Isactive != null).Select(x => new blockHistory()
            {
                patientname = _context.Requestclients.Where(r => r.Requestid == x.Requestid).Select(r => r.Firstname).First(),
                phonenumber = x.Phonenumber,
                email = x.Email,
                createddate = Convert.ToDateTime(x.Createddate).ToString("yyyy-MM-dd"),
                notes = x.Reason,
                blockId = x.Blockrequestid,
                isActive = x.Isactive,
            }).ToList();

            if (recordsModel.blockHistoryMain != null)
            {
                if (recordsModel.blockHistoryMain[0].searchRecordOne != null)
                {
                    requestData = requestData.Where(r => r.patientname != null && r.patientname.Trim().ToLower().Contains(recordsModel.blockHistoryMain[0].searchRecordOne.Trim().ToLower())).Select(r => r).ToList();
                }
                if (recordsModel.blockHistoryMain[0].searchRecordTwo != null && recordsModel.blockHistoryMain[0].searchRecordTwo != DateTime.MinValue)
                {
                    requestData = requestData.Where(r => r.createddate != null && r.createddate == Convert.ToDateTime(recordsModel.blockHistoryMain[0].searchRecordTwo).ToString("yyyy-MM-dd")).Select(r => r).ToList();
                }
                if (recordsModel.blockHistoryMain[0].searchRecordThree != null)
                {
                    requestData = requestData.Where(r => r.email != null && r.email.Trim().ToLower().Contains(recordsModel.blockHistoryMain[0].searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                }
                if (recordsModel.blockHistoryMain[0].searchRecordFour != null)
                {
                    requestData = requestData.Where(r => r.phonenumber != null && r.phonenumber.Trim().ToLower().Contains(recordsModel.blockHistoryMain[0].searchRecordFour.Trim().ToLower())).Select(r => r).ToList();
                }
            }


            return requestData;
        }




        public void unblockBlockHistoryMain(int blockId)
        {
            var block = _context.Blockrequests.Where(r => r.Blockrequestid == blockId).Select(r => r).First();

            block.Isactive = null;
            _context.SaveChanges();

            var request = _context.Requests.Where(r => r.Requestid == block.Requestid).Select(r => r).First();

            request.Status = 1;
            request.Isdeleted = null;
            _context.SaveChanges();

            var requestStatus = _context.Requeststatuslogs.Where(r => r.Requestid == block.Requestid).Select(r => r).First();
            requestStatus.Status = 1;
            _context.SaveChanges();
        }

        public recordsModel emailLogsMain(int tempId, recordsModel recordsModel)
        {
            recordsModel model = new recordsModel();
            model.tempid = tempId;
            model.emailRecords = new List<emailSmsRecords>();
            if (tempId == 0)
            {
                var records = _context.Emaillogs.ToList();
                foreach (var item in records)
                {


                    var newRecord = new emailSmsRecords
                    {
                        emailLogId = item.Emaillogid,
                        email = item.Emailid,
                        createddate = item.Createdate,
                        sentdate = item.Sentdate,
                        sent = item.Isemailsent[0] ? "Yes" : "No",
                        recipient = _context.Aspnetusers.Any(x => x.Email == item.Emailid) ? _context.Aspnetusers.Where(x => x.Email == item.Emailid).Select(x => x.Username).First() : null,
                        rolename = _context.Aspnetroles.Where(i => i.Id == item.Roleid).Select(i => i.Name).First(),
                        senttries = item.Senttries,
                        confirmationNumber = item.Confirmationnumber,
                    };

                    model.emailRecords.Add(newRecord);

                }

                if (recordsModel != null)
                {
                    if (recordsModel.searchRecordOne != null && recordsModel.searchRecordOne != "All")
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.rolename != null && r.rolename.Contains(recordsModel.searchRecordOne)).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordTwo != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.recipient != null && r.recipient.Trim().ToLower().Contains(recordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordThree != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.email != null && r.email.Trim().ToLower().Contains(recordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFour != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate != null && item.createddate >= recordsModel.searchRecordFour).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFive != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate != null && item.createddate <= recordsModel.searchRecordFive).Select(r => r).ToList();
                    }
                }
            }

            else
            {
                var records = _context.Smslogs.ToList();
                foreach (var item in records)
                {

                    var newRecord = new emailSmsRecords
                    {
                        smsLogId = item.Smslogid,
                        contact = item.Mobilenumber,
                        createddate = item.Createdate,
                        sentdate = item.Sentdate,
                        sent = item.Issmssent[0] ? "Yes" : "No",
                        recipient = _context.Requestclients.Where(i => i.Requestid == item.Requestid).Select(i => i.Firstname).FirstOrDefault(),
                        rolename = _context.Aspnetroles.Where(i => i.Id == item.Roleid).Select(i => i.Name).First(),
                        senttries = item.Senttries,
                        confirmationNumber = item.Confirmationnumber,
                    };

                    model.emailRecords.Add(newRecord);
                }
                if (recordsModel != null)
                {
                    if (recordsModel.searchRecordOne != null && recordsModel.searchRecordOne != "All")
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.rolename != null && r.rolename.Contains(recordsModel.searchRecordOne)).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordTwo != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.recipient != null && r.recipient.Trim().ToLower().Contains(recordsModel.searchRecordTwo.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordThree != null)
                    {
                        model.emailRecords = model.emailRecords.Where(r => r.contact != null && r.contact.Trim().ToLower().Contains(recordsModel.searchRecordThree.Trim().ToLower())).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFour != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate != null && item.createddate >= recordsModel.searchRecordFour).Select(r => r).ToList();
                    }
                    if (recordsModel.searchRecordFive != null)
                    {
                        model.emailRecords = model.emailRecords.Where(item => item.createddate != null && item.createddate <= recordsModel.searchRecordFive).Select(r => r).ToList();
                    }
                }

            }
            return model;
        }

        public List<DateViewModel> GetDates()
        {
            List<DateViewModel> dates = new List<DateViewModel>();
            int startMonth = 0;
            int startYear = 0;
            int startDate = 1;
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);
            int nextDate = 1;
            if (today.Day > 15)
            {
                nextDate = 2;
            }
            if (today.Month - 6 < 0)
            {
                startMonth = 12 - (6 - today.Month) + 1;
                startYear = today.Year - 1;
            }
            else if (today.Month - 6 == 0)
            {
                startMonth = 1;
                startYear = today.Year;
            }
            else
            {
                startMonth = today.Month - 6;
                startYear = today.Year;
            }
            int count = 12;
            if (nextDate == 1)
            {
                count = 11;
            }
            for (int i = 1; i <= count; i++)
            {

                if (i % 2 == 0)
                {
                    startDate = 16;
                }
                else
                {
                    startDate = 1;

                }
                if (startMonth > 12)
                {
                    startMonth = 1;
                    startYear = today.Year;
                }
                DateViewModel date = new DateViewModel();
                date.StartDate = new DateOnly(startYear, startMonth, startDate);
                if (startDate != 1)
                    date.EndDate = date.StartDate.AddMonths(1).AddDays(-16);
                else
                    date.EndDate = new DateOnly(startYear, startMonth, 15);
                dates.Add(date);
                if (startDate == 16)
                    startMonth += 1;
            }
            dates.Reverse();
            return dates;
        }


        public List<PhysicianViewModel> GetPhysiciansForInvoicing()
        {
            var physicians = _physicianrepo.SelectWhere(x => new PhysicianViewModel
            {
                PhysicianId = x.Physicianid,
                PhysicianName = x.Firstname + " " + x.Lastname,
            }, x => x.Isdeleted == null);
            List<PhysicianViewModel> PhysicianList = new List<PhysicianViewModel>();
            foreach (PhysicianViewModel item in physicians)
            {
                PhysicianList.Add(item);
            }
            return PhysicianList;
        }


        public string CheckInvoicingAproove(string selectedValue, int PhysicianId)
        {
            string[] dateRange = selectedValue.Split('*');
            DateOnly startDate = DateOnly.Parse(dateRange[0]);
            DateOnly endDate = DateOnly.Parse(dateRange[1]);
            string result = "";
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == startDate && u.EndDate == endDate);
            if (weeklyTimeSheet != null)
            {
                if (weeklyTimeSheet.IsFinalized != true && weeklyTimeSheet.Status == 1)
                {
                    result = "NotFinalized-NotAprooved";
                }
                else if (weeklyTimeSheet.IsFinalized == true && weeklyTimeSheet.Status == 1)
                {
                    result = "Finalized-NotAprooved";
                }
                else if (weeklyTimeSheet.IsFinalized == true && weeklyTimeSheet.Status == 2)
                {
                    result = "Finalized-Aprooved";
                }
            }
            else
            {
                result = "False";
            }
            return result;
        }

        public InvoicingViewModel GetApprovedViewData(string selectedValue, int PhysicianId)
        {
            string[] dateRange = selectedValue.Split('*');
            DateOnly startDate = DateOnly.Parse(dateRange[0]);
            DateOnly endDate = DateOnly.Parse(dateRange[1]);
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == startDate && u.EndDate == endDate);

            InvoicingViewModel model = new InvoicingViewModel();
            if (weeklyTimeSheet != null)
            {
                model.startDate = weeklyTimeSheet.StartDate;
                model.endDate = weeklyTimeSheet.EndDate;
                model.Status = weeklyTimeSheet.Status == 1 ? "Pending" : "Aprooved";
                model.TimeSheetId = weeklyTimeSheet.TimeSheetId;
                model.IsFinalized = weeklyTimeSheet.IsFinalized == true ? true : false;
            }
            return model;

        }

        public void AprooveTimeSheet(InvoicingViewModel model, int? AdminID)
        {
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == model.PhysicianId && u.StartDate == model.startDate && u.EndDate == model.endDate);
            if (weeklyTimeSheet != null)
            {
                weeklyTimeSheet.AdminId = AdminID;
                weeklyTimeSheet.Status = 2;
                weeklyTimeSheet.BonusAmount = model.BonusAmount;
                weeklyTimeSheet.AdminNote = model.AdminNotes;
                _weeklyTimeSheetRepo.Update(weeklyTimeSheet);
            }
        }

        public GetPayRate GetPayRate(int physicianId, int callid)
        {
            var payrate = _context.PayRates.FirstOrDefault(i => i.PhysicianId == physicianId);
            var Aspid = _context.Physicians.FirstOrDefault(a => a.Physicianid == physicianId).Aspnetuserid;
            if (payrate == null)
            {
                var GetPayRate = new GetPayRate()
                {
                    PhysicianId = physicianId,
                    callid = callid,
                };
                return GetPayRate;
            }
            else
            {
                var GetPayRate = new GetPayRate()
                {
                    PhysicianId = physicianId,
                    NightShift_Weekend = payrate.NightShiftWeekend != 0 ? payrate.NightShiftWeekend : default,
                    Shift = payrate.Shift != 0 ? payrate.Shift : default,
                    HouseCalls_Nights_Weekend = payrate.HouseCallNightWeekend != 0 ? payrate.HouseCallNightWeekend : default,
                    PhoneConsult = payrate.PhoneConsult != 0 ? payrate.PhoneConsult : default,
                    PhoneConsults_Nights_Weekend = payrate.PhoneConsultNightWeekend != 0 ? payrate.PhoneConsultNightWeekend : default,
                    BatchTesting = payrate.BatchTesting != 0 ? payrate.BatchTesting : default,
                    HouseCalls = payrate.HouseCall != 0 ? payrate.HouseCall : default,
                    callid = callid,
                };
                return GetPayRate;
            }
        }

        public bool SetPayRate(GetPayRate getPayRate)
        {
            var payrate = _context.PayRates.FirstOrDefault(i => i.PhysicianId == getPayRate.PhysicianId);

            var phyDetails = _context.Physicians.Where(r => r.Physicianid == getPayRate.PhysicianId).Select(r => r).First();

            PayRate payRate = new PayRate();

            if (payrate == null)
            {
                payRate.PhysicianId = phyDetails.Physicianid;
                payRate.NightShiftWeekend = getPayRate.NightShift_Weekend;
                payRate.Shift = getPayRate.Shift;
                payRate.HouseCallNightWeekend = getPayRate.HouseCalls_Nights_Weekend;
                payRate.PhoneConsult = getPayRate.PhoneConsult;
                payRate.PhoneConsultNightWeekend = getPayRate.PhoneConsults_Nights_Weekend;
                payRate.BatchTesting = getPayRate.BatchTesting;
                payRate.HouseCall = getPayRate.HouseCalls;
                payRate.CreatedDate = DateTime.Now;

                _context.PayRates.Add(payRate);
                _context.SaveChanges();
                return true;
            }
            else
            {
                if (getPayRate.NightShift_Weekend != payrate.NightShiftWeekend || getPayRate.Shift != payrate.Shift || getPayRate.HouseCalls_Nights_Weekend != payrate.HouseCallNightWeekend ||
                getPayRate.PhoneConsult != payrate.PhoneConsult || getPayRate.PhoneConsults_Nights_Weekend != payrate.PhoneConsultNightWeekend || getPayRate.BatchTesting != payrate.BatchTesting ||
                getPayRate.HouseCalls != payrate.HouseCall)
                {
                    payrate.NightShiftWeekend = getPayRate.NightShift_Weekend;
                    payrate.Shift = getPayRate.Shift;
                    payrate.HouseCallNightWeekend = getPayRate.HouseCalls_Nights_Weekend;
                    payrate.PhoneConsult = getPayRate.PhoneConsult;
                    payrate.PhoneConsultNightWeekend = getPayRate.PhoneConsults_Nights_Weekend;
                    payrate.BatchTesting = getPayRate.BatchTesting;
                    payrate.HouseCall = getPayRate.HouseCalls;

                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public ChatViewModel GetChats(int RequestId, int AdminID, int ProviderId, int RoleId)
        {
            var requestClient = _context.Requestclients.FirstOrDefault(u => u.Requestid == RequestId);
            var physician = _context.Physicians.FirstOrDefault(u => u.Physicianid == ProviderId);
            var admin = _context.Admins.FirstOrDefault(u => u.Adminid == AdminID);
            ChatViewModel model = new ChatViewModel();
            var chats = _chatRepo.SelectWhereOrderBy(x => new ChatViewModel
            {
                ChatId = x.ChatId,
                Message = x.Message ?? "",
                ChatDate = x.SentDate!.Value.ToString("hh:mm tt"),
                SentBy = x.SentBy ?? 0,

            }, x => x.AdminId == AdminID && x.RequestId == RequestId && x.PhyscainId == ProviderId, x => x.SentDate!);
            List<ChatViewModel> list = new List<ChatViewModel>();
            foreach (ChatViewModel item in chats)
            {
                item.ChatBoxClass = (item.SentBy == Convert.ToInt32(RoleId) ? "Sender" : "Reciever");
                list.Add(item);
            }
            if (ProviderId == 0)
            {
                model.RecieverName = (RoleId == 1 ? requestClient.Firstname + " " + requestClient.Lastname : "Admins");
            }
            if (RequestId == 0)
            {
                model.RecieverName = (RoleId == 1 ? physician.Firstname + " " + physician.Lastname : "Admins");
            }
            if (AdminID == 0)
            {
                model.RecieverName = (RoleId == 2 ? physician.Firstname + " " + physician.Lastname : requestClient.Firstname + " " + requestClient.Lastname);
            }
            model.Chats = list;
            model.RequestId = RequestId;
            model.ProviderId = ProviderId;
            model.AdminId = AdminID;
            model.RoleId = RoleId;
            return model;
        }

        public void NewChat(ChatViewModel model, int RoleID)
        {
            if(model.AdminId == 1 && model.flag != "Admin")
            {
                var adminData = _context.Admins.ToList();

                foreach(var item in adminData)
                {
                    Chat chat = new Chat();
                    chat.Message = model.Message;
                    chat.SentBy = Convert.ToInt32(RoleID);
                    chat.AdminId = item.Adminid;
                    chat.RequestId = model.RequestId;
                    chat.PhyscainId = model.ProviderId;
                    chat.SentDate = DateTime.Now;
                    _chatRepo.Add(chat);
                }
            }
            else
            {
                Chat chat = new Chat();
                chat.Message = model.Message;
                chat.SentBy = Convert.ToInt32(RoleID);
                chat.AdminId = model.AdminId;
                chat.RequestId = model.RequestId;
                chat.PhyscainId = model.ProviderId;
                chat.SentDate = DateTime.Now;
                _chatRepo.Add(chat);
            }
            
        }
    }
}
