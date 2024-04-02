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
    public class scheduling
    {
        public ScheduleModel ScheduleModel { get; set; }

        public List<Region> regions { get; set; }

        public DayShiftModal DayShiftModal { get; set; }

        public MonthShiftModal MonthShiftModal { get; set; }

        public PartialViewModal PartialViewModal { get; set; }

        public ShiftDetailsmodal ShiftDetailsmodal { get; set; }

        public WeekShiftModal WeekShiftModal { get; set; }
    }

    public class ScheduleModel
    {
        public int? Shiftid { get; set; }

        public int Physicianid { get; set; }
        public string? PhysicianName { get; set; }
        public string? PhysicianPhoto { get; set; }
        public int Regionid { get; set; }
        public string? RegionName { get; set; }

        public DateOnly Startdate { get; set; }
        public DateTime Shiftdate { get; set; }
        public TimeOnly Starttime { get; set; }
        public TimeOnly Endtime { get; set; }

        public bool Isrepeat { get; set; }

        public string? checkWeekday { get; set; }

        public int? Repeatupto { get; set; }
        public short Status { get; set; }
        public List<ScheduleModel> DayList { get; set; }

    }

    public class DayShiftModal
    {
        public DateTime currentDate { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int daysInMonth { get; set; }
        public DateTime firstDayOfMonth { get; set; }
        public int startDayIndex { get; set; }
        public string[] dayNames { get; set; }
        public List<ShiftDetailsmodal> shiftDetailsmodals { get; set; }
        public List<Physician> Physicians { get; set; }
    }

    public class MonthShiftModal
    {
        public DateTime currentDate { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int daysInMonth { get; set; }
        public int daysLoop { get; set; }
        public DateTime firstDayOfMonth { get; set; }
        public int startDayIndex { get; set; }
        public string[] dayNames { get; set; }
        public List<ShiftDetailsmodal> shiftDetailsmodals { get; set; }
    }

    public class PartialViewModal
    {
        public string actionType { get; set; }
        public int requestid { get; set; }
        public int requestwisefileid { get; set; }
        public string bcolor { get; set; }
        public string btext { get; set; }
        public string patientName { get; set; }
        public string email { get; set; }
        public long? phonenumber { get; set; }
        public int physicianid { get; set; }
        public int shiftdetailsid { get; set; }
        public string datestring { get; set; }
    }

    public class ShiftDetailsmodal
    {
        public int Shiftid { get; set; }
        public int Physicianid { get; set; }
        public string PhysicianName { get; set; }
        public DateOnly Startdate { get; set; }
        public BitArray Isrepeat { get; set; } = null!;
        public string? Weekdays { get; set; }
        public int? Repeatupto { get; set; }
        public int Shiftdetailid { get; set; }
        public DateTime Shiftdate { get; set; }
        public int? Regionid { get; set; }
        public string region { get; set; }
        public TimeOnly Starttime { get; set; }
        public TimeOnly Endtime { get; set; }
        public short Status { get; set; }
        public BitArray Isdeleted { get; set; }
        public string? Eventid { get; set; }

        public string? Abberaviation { get; set; }

        public string regionname { get; set; }


        public List<Region> regions { get; set; }
        public List<Physician> Physicians { get; set; }
    }

    public class WeekShiftModal
    {
        public int startdate { get; set; }
        public int enddate { get; set; }

        public string[] dayNames { get; set; }
        public List<int> datelist { get; set; }
        public List<ShiftDetailsmodal> shiftDetailsmodals { get; set; }
        public List<Physician> Physicians { get; set; }
    }

}




    
