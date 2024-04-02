using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using DAL_Data_Access_Layer_.DataModels;
using Microsoft.AspNetCore.Http;
using System.Collections;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class recordsModel
    {
        public List<requestsRecordModel>? requestListMain { get; set; }
    }
    public class requestsRecordModel
    {
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

    }
}
