using BLL_Business_Logic_Layer_.Interface;
using DAL_Data_Access_Layer_.CustomeModel;
using DAL_Data_Access_Layer_.DataContext;
using DAL_Data_Access_Layer_.DataModels;
using LinqKit;
using Microsoft.AspNetCore.Http;
using System.Collections;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;

namespace BLL_Business_Logic_Layer_.Services
{
    public class ProviderDash : IProviderDash
    {
        private readonly ApplicationDbContext _context;
        private readonly IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet> _weeklyTimeSheetRepo;
        private readonly IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail> _weeklyTimeSheetDetailRepo;
        private readonly IGenericRepository<PayRate> _payRateRepo;
        private readonly IGenericRepository<Shiftdetail> _shiftDetailrepo;

        public ProviderDash(ApplicationDbContext context, IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet> weeklyTimeSheetRepo, IGenericRepository<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail> weeklyTimeSheetDetailRepo,IGenericRepository<PayRate> payRateRepo, IGenericRepository<Shiftdetail> shiftDetailrepo)
        {
            _context = context;
            _weeklyTimeSheetRepo = weeklyTimeSheetRepo;
            _weeklyTimeSheetDetailRepo = weeklyTimeSheetDetailRepo;
            _payRateRepo = payRateRepo;
            _shiftDetailrepo = shiftDetailrepo;
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

        public bool FinalizeEncounter(int reqId)
        {
            if (_context.EncounterForms.Any(r => r.Requestid == reqId))
            {
                var encounter = _context.EncounterForms.Where(r => r.Requestid == reqId).Select(r => r).First();

                encounter.IsFinalized = new BitArray(1, true);

                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }

        }


        public adminDashData ProviderEncounterFormDownload(int reqId)
        {
            adminDashData data = new adminDashData();

            ProviderEncounterPopUp _ProviderEncounterPopUp = new ProviderEncounterPopUp();
            _ProviderEncounterPopUp.reqId = reqId;

            data._ProviderEncounterPopUp = _ProviderEncounterPopUp;

            return data;

        }


        public void ProviderConcludeCarePost(adminDashData adminDashData, string sessionEmail)
        {
            var reqData = _context.Requests.Where(r => r.Requestid == adminDashData._viewUpload[0].reqid).Select(r => r).First();
            var providerId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

            Requeststatuslog requeststatuslog = new Requeststatuslog();


            reqData.Status = 7;
            reqData.Modifieddate = DateTime.Now;
            _context.SaveChanges();

            requeststatuslog.Requestid = adminDashData._viewUpload[0].reqid;
            requeststatuslog.Status = 6;
            requeststatuslog.Createddate = DateTime.Now;
            requeststatuslog.Physicianid = providerId.Physicianid;
            requeststatuslog.Notes = adminDashData._viewUpload[0].notes;

            _context.Requeststatuslogs.Add(requeststatuslog);
            _context.SaveChanges();
        }

        public SchedulingCm PhysicainRegionTable(string sessionEmail)
        {
            SchedulingCm model = new SchedulingCm();

            var providerId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r).First();

            if (providerId.Physicianid != null)
            {
                var regions = _context.Physicianregions
                                       .Where(pr => pr.Physicianid == providerId.Physicianid)
                                       .Select(pr => pr.Regionid)
                                       .ToList();
                model.regions = _context.Regions
                                        .Where(r => regions.Contains(r.Regionid))
                                        .ToList();

                model.phyId = providerId.Physicianid;

            }

            return model;

        }

        public void RequestAdmin(ProviderTransferTab _ProviderTransferTab, string sessionEmail)
        {
            var email = _context.Admins.ToList();

            foreach (var item in email)
            {
                try
                {
                    SendRegistrationEmailCreateRequest(item.Email, _ProviderTransferTab.Note, sessionEmail);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void SendRegistrationEmailCreateRequest(string email, string note, string sessionEmail)
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
                Subject = "Request Note To Admin",
                IsBodyHtml = true,
                Body = $"Note: '{note}'"
            };

            Emaillog emailLog = new Emaillog()
            {
                Subjectname = mailMessage.Subject,
                Emailtemplate = "Sender : " + senderEmail + "Reciver :" + email + "Subject : " + mailMessage.Subject + "Message : " + "FileSent",
                Emailid = email,
                Roleid = 3,
                Physicianid = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First(),
                Createdate = DateTime.Now,
                Sentdate = DateTime.Now,
                Isemailsent = new BitArray(1, true),
                Confirmationnumber = sessionEmail.Substring(0, 2) + DateTime.Now.ToString().Substring(0, 19).Replace(" ", ""),
                Senttries = 1,
            };


            _context.Emaillogs.Add(emailLog);
            _context.SaveChanges();


            mailMessage.To.Add(email);

            client.Send(mailMessage);
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

        public int GetPhyID(string sessionEmail)
        {
            var phyId = _context.Physicians.Where(r => r.Email == sessionEmail).Select(r => r.Physicianid).First();

            return phyId;
        }

        public InvoicingViewModel GetInvoicingDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID)
        {
            InvoicingViewModel model = new InvoicingViewModel();
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && (u.StartDate == startDate && u.EndDate == endDate));
            if (weeklyTimeSheet != null)
            {
                var TimehseetsData = _weeklyTimeSheetDetailRepo.SelectWhereOrderBy(x => new Timesheet
                {
                    Date = x.Date,
                    NumberofShift = x.NumberOfShifts ?? 0,
                    NightShiftWeekend = x.IsWeekendHoliday == true ? 1 : 0,
                    NumberOfHouseCall = (x.IsWeekendHoliday == false ? x.HouseCall : 0) ?? 0,
                    HousecallNightsWeekend = (x.IsWeekendHoliday == true ? x.HouseCall : 0) ?? 0,
                    NumberOfPhoneConsults = (x.IsWeekendHoliday == false ? x.PhoneConsult : 0) ?? 0,
                    phoneConsultNightsWeekend = (x.IsWeekendHoliday == true ? x.PhoneConsult : 0) ?? 0,
                    BatchTesting = x.BatchTesting ?? 0
                }, x => x.TimeSheetId == weeklyTimeSheet.TimeSheetId, x => x.Date);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in TimehseetsData)
                {
                    list.Add(item);
                }
                model.timesheets = list;
                model.PhysicianId = PhysicianId ?? 0;
                model.IsFinalized = weeklyTimeSheet.IsFinalized == true ? true : false;
                model.startDate = startDate;
                model.endDate = endDate;
                model.Status = weeklyTimeSheet.Status == 1 ? "Pending" : "Aprooved";
            }
            else
            {
                model.timesheets = new List<Timesheet>();
            }
            model.isAdminSide = AdminID == null ? false : true;
            return model;
        }


        public InvoicingViewModel GetUploadedDataonChangeOfDate(DateOnly startDate, DateOnly endDate, int? PhysicianId, int pageNumber, int pagesize)
        {
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && (u.StartDate == startDate && u.EndDate == endDate));
            InvoicingViewModel model = new InvoicingViewModel();
            Expression<Func<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail, bool>> whereClauseSyntax = PredicateBuilder.New<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail>();
            whereClauseSyntax = x => x.Bill != null && x.TimeSheetId == weeklyTimeSheet.TimeSheetId;
            if (weeklyTimeSheet != null)
            {
                var UploadedItems = _weeklyTimeSheetDetailRepo.GetAllWithPagination(x => new Timesheet
                {
                    Date = x.Date,
                    Items = x.Item ?? "-",
                    Amount = x.Amount ?? 0,
                    BillName = x.Bill ?? "-",
                }, whereClauseSyntax, pageNumber, pagesize, x => x.Date, true);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in UploadedItems)
                {
                    list.Add(item);
                }
                model.timesheets = list;

                model.pager = new DAL_Data_Access_Layer_.CustomeModel.Pager
                {
                    TotalItems = _weeklyTimeSheetDetailRepo.GetTotalcount(whereClauseSyntax),
                    CurrentPage = pageNumber,
                    ItemsPerPage = pagesize
                };
                model.SkipCount = (pageNumber - 1) * pagesize;
                model.CurrentPage = pageNumber;
                model.TotalPages = (int)Math.Ceiling((decimal)model.pager.TotalItems / pagesize);
                model.IsFinalized = weeklyTimeSheet.IsFinalized == true ? true : false;
            }
            model.PhysicianId = PhysicianId ?? 0;
            return model;
        }

        public InvoicingViewModel getDataOfTimesheet(DateOnly startDate, DateOnly endDate, int? PhysicianId, int? AdminID)
        {
            InvoicingViewModel model = new InvoicingViewModel();
            model.startDate = startDate;
            model.endDate = endDate;
            model.differentDays = endDate.Day - startDate.Day;
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == startDate && u.EndDate == endDate);
            Expression<Func<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail, bool>> whereClauseSyntax1 = PredicateBuilder.New<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail>();

            if (weeklyTimeSheet != null)
            {
                PayRate payRate = _payRateRepo.GetFirstOrDefault(u => u.PhysicianId == weeklyTimeSheet.ProviderId);
                whereClauseSyntax1 = x => x.TimeSheet!.ProviderId == PhysicianId && x.TimeSheet.StartDate == startDate && x.TimeSheet.EndDate == endDate;
                model.TimeSheetId = weeklyTimeSheet.TimeSheetId;
                var ExistingTimeshet = _weeklyTimeSheetDetailRepo.SelectWhereOrderBy(x => new Timesheet
                {
                    Date = x.Date,
                    NumberOfHouseCall = x.HouseCall ?? 0,
                    NumberOfPhoneConsults = x.PhoneConsult ?? 0,
                    Weekend = x.IsWeekendHoliday ?? false,
                    TotalHours = x.TotalHours ?? 0,
                    OnCallhours = x.OnCallHours ?? 0,
                    Amount = x.Amount ?? 0,
                    Items = x.Item,
                    BillName = x.Bill,
                    WeeklyTimesheetDeatilsId = x.TimeSheetDetailId,
                }, whereClauseSyntax1, x => x.Date);
                List<Timesheet> list = new List<Timesheet>();
                foreach (Timesheet item in ExistingTimeshet)
                {
                    model.shiftTotal += (item.TotalHours * payRate.Shift) ?? 0;
                    model.weekendTotal += item.Weekend == true ? (1 * payRate.NightShiftWeekend) ?? 0 : 0;
                    model.HouseCallTotal += (item.NumberOfHouseCall * payRate.HouseCall) ?? 0;
                    model.phoneconsultTotal += (item.NumberOfPhoneConsults * payRate.PhoneConsult) ?? 0;
                    list.Add(item);
                }
                model.timesheets = list;
                model.shiftPayrate = payRate.Shift ?? 0;
                model.weekendPayrate = payRate.NightShiftWeekend ?? 0;
                model.HouseCallPayrate = payRate.HouseCall ?? 0;
                model.phoneConsultPayrate = payRate.PhoneConsult ?? 0;
                model.GrandTotal = model.shiftTotal + model.weekendTotal + model.HouseCallTotal + model.phoneconsultTotal;

            }
            else
            {
                DateOnly currentDate = startDate;
                while (currentDate <= endDate)
                {
                    model.timesheets.Add(new Timesheet
                    {
                        Date = currentDate,

                    });

                    currentDate = currentDate.AddDays(1);
                }
            }
            model.startDate = startDate;
            model.endDate = endDate;
            model.PhysicianId = PhysicianId ?? 0;
            model.IsFinalized = weeklyTimeSheet == null ? false : true;
            model.isAdminSide = AdminID == null ? false : true;
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

        public void SubmitTimeSheet(InvoicingViewModel model, int? PhysicianId)
        {
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet existingWeekltTimesheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.ProviderId == PhysicianId && u.StartDate == model.startDate && u.EndDate == model.endDate);
            if (existingWeekltTimesheet == null)
            {
                DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = new DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet();
                weeklyTimeSheet.StartDate = model.startDate;
                weeklyTimeSheet.EndDate = model.endDate;
                weeklyTimeSheet.Status = 1;
                weeklyTimeSheet.CreatedDate = DateTime.Now;
                weeklyTimeSheet.ProviderId = PhysicianId ?? 0;
                _weeklyTimeSheetRepo.Add(weeklyTimeSheet);

                foreach (var item in model.timesheets)
                {
                    BitArray deletedBit = new BitArray(new[] { false });

                    DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail detail = new DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail();
                    detail.Date = item.Date;
                    detail.NumberOfShifts = _shiftDetailrepo.Count(u => u.Shift.Physicianid == PhysicianId && DateOnly.FromDateTime(u.Shiftdate) == item.Date && u.Isdeleted == deletedBit);
                    detail.TotalHours = item.TotalHours;
                    detail.IsWeekendHoliday = item.Weekend;
                    detail.HouseCall = item.NumberOfHouseCall;
                    detail.PhoneConsult = item.NumberOfPhoneConsults;
                    detail.OnCallHours = item.OnCallhours;
                    detail.TimeSheetId = weeklyTimeSheet.TimeSheetId;
                    if (item.Bill != null)
                    {
                        IFormFile newFile = item.Bill;
                        detail.Bill = newFile.FileName;
                        var filePath = Path.Combine("wwwroot", "Uploaded_files", "ProviderBills", PhysicianId + "-" + item.Date + "-" + Path.GetFileName(newFile.FileName));
                        using (FileStream stream = System.IO.File.Create(filePath))
                        {
                            newFile.CopyTo(stream);
                        }
                    }
                    detail.Item = item.Items;
                    detail.Amount = item.Amount;
                    _weeklyTimeSheetDetailRepo.Add(detail);
                }
            }
            else
            {
                var exsitingTimeSheetDetail = _weeklyTimeSheetDetailRepo.GetList(u => u.TimeSheetId == existingWeekltTimesheet.TimeSheetId && u.Date >= model.startDate && u.Date <= model.endDate);
                List<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail> list = new List<DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail>();

                for (int i = 0; i < model.timesheets.Count; i++)
                {
                    var currentDate = model.timesheets[i].Date;
                    DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail weeklyTimeSheetDetail = exsitingTimeSheetDetail.FirstOrDefault(detail => detail.Date == currentDate)!;
                    if (weeklyTimeSheetDetail != null)
                    {
                        weeklyTimeSheetDetail.Date = model.timesheets[i].Date;
                        weeklyTimeSheetDetail.HouseCall = model.timesheets[i].NumberOfHouseCall;
                        weeklyTimeSheetDetail.PhoneConsult = model.timesheets[i].NumberOfPhoneConsults;
                        weeklyTimeSheetDetail.Item = model.timesheets[i].Items ?? null;
                        weeklyTimeSheetDetail.Amount = model.timesheets[i].Amount;
                        weeklyTimeSheetDetail.OnCallHours = model.timesheets[i].OnCallhours;
                        weeklyTimeSheetDetail.TotalHours = model.timesheets[i].TotalHours;
                        weeklyTimeSheetDetail.IsWeekendHoliday = model.timesheets[i].Weekend;
                        if (model.timesheets[i].Bill != null && model.timesheets[i].Bill!.Length > 0)
                        {
                            IFormFile newFile = model.timesheets[i].Bill!;
                            weeklyTimeSheetDetail.Bill = newFile.FileName;
                            var filePath = Path.Combine("wwwroot", "Uploaded_files", "ProviderBills", PhysicianId + "-" + model.timesheets[i].Date + "-" + Path.GetFileName(newFile.FileName));
                            FileStream stream = null;
                            try
                            {
                                stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                                newFile.CopyToAsync(stream)
;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"An error occurred: {ex.Message}");
                            }
                        }
                        list.Add(weeklyTimeSheetDetail);
                    }
                }
                foreach (DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail item in list)
                {
                    _weeklyTimeSheetDetailRepo.Update(item);
                }
            }

        }

        public void DeleteBill(int id)
        {
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheetDetail weeklyTimeSheetDetail = _weeklyTimeSheetDetailRepo.GetFirstOrDefault(u => u.TimeSheetDetailId == id);
            weeklyTimeSheetDetail.Bill = null;
            _weeklyTimeSheetDetailRepo.Update(weeklyTimeSheetDetail);

        }
        public void FinalizeTimeSheet(int id)
        {
            DAL_Data_Access_Layer_.DataModels.WeeklyTimeSheet weeklyTimeSheet = _weeklyTimeSheetRepo.GetFirstOrDefault(u => u.TimeSheetId == id);
            if (weeklyTimeSheet != null)
            {
                weeklyTimeSheet.IsFinalized = true;
                _weeklyTimeSheetRepo.Update(weeklyTimeSheet);
            }
        }
    }
}
