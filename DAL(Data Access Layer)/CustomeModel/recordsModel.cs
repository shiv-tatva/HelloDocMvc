using DAL_Data_Access_Layer_.DataModels;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class recordsModel
    {
        public List<requestsRecordModel>? requestListMain { get; set; }
        public List<GetRecordExplore>? getRecordExplore { get; set; }
        public List<blockHistory>? blockHistoryMain { get; set; }
        public List<emailSmsRecords>? emailRecords { get; set; }
        public int? tempid { get; set; }

        public string? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public DateTime? searchRecordFour { get; set; }
        public DateTime? searchRecordFive { get; set; }


    }
    public class requestsRecordModel
    {

        [RegularExpression(@"^\s*[A-Za-z]+\s*$", ErrorMessage = "Patient Name must contain only letters")]
        public string? patientname { get; set; }
        public string? requestor { get; set; }
        public DateTime? dateOfService { get; set; }
        public DateTime? closeCaseDate { get; set; }
        public string? email { get; set; }
        public string? contact { get; set; }
        public string? address { get; set; }
        public string? zip { get; set; }
        public string? status { get; set; }
        public int? statusId { get; set; }
        public string? physician { get; set; }
        public string? physicianNote { get; set; }
        public string? providerNote { get; set; }
        public string? AdminNote { get; set; }
        public string? pateintNote { get; set; }
        public int? requestid { get; set; }
        public int? requesttypeid { get; set; }
        public int? userid { get; set; }
        public int? flag { get; set; }
        public int? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public int? searchRecordThree { get; set; }
        public DateOnly? searchRecordFour { get; set; }
        public DateOnly? searchRecordFive { get; set; }
        public string? searchRecordSix { get; set; }
        public string? searchRecordSeven { get; set; }
        public string? searchRecordEight { get; set; }
    }

    public class GetRecordsModel
    {
        public List<User>? users { get; set; }
        public List<Request>? requestList { get; set; }
        public List<Requestclient>? requestClient { get; set; }
        public string? searchRecordOne { get; set; }
        public string? searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public string? searchRecordFour { get; set; }
        public int? flag { get; set; }

    }

    public class GetRecordExplore
    {
        public string? fullname { get; set; }
        public string? confirmationnumber { get; set; }
        public string? providername { get; set; }
        public string? concludedate { get; set; }
        public string? createddate { get; set; }
        public int? status { get; set; }
        public int? requestid { get; set; }
    }


    public class blockHistory
    {
        public string? patientname { get; set; }
        public string? phonenumber { get; set; }
        public string? email { get; set; }
        public string? createddate { get; set; }
        public BitArray? isActive { get; set; }
        public string? notes { get; set; }

        public int? blockId { get; set; }
        public int? flag { get; set; }
        public string? searchRecordOne { get; set; }
        public DateTime searchRecordTwo { get; set; }
        public string? searchRecordThree { get; set; }
        public string? searchRecordFour { get; set; }

        public bool indicate { get; set; }

    }

    public class emailSmsRecords
    {

        public int? requestid { get; set; }
        public int? smsLogId { get; set; }
        public int? emailLogId { get; set; }
        public string? recipient { get; set; }
        public string? action { get; set; }
        public string? rolename { get; set; }
        public string? email { get; set; }
        public DateTime? createddate { get; set; }
        public DateTime? sentdate { get; set; }
        public string? sent { get; set; }
        public int? senttries { get; set; }
        public string? confirmationNumber { get; set; }
        public string? contact { get; set; }
    }


}
