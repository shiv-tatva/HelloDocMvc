using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class FamilyFriendData
    {
        public string? ff_firstname { get; set; }

        public string? ff_lastname { get; set; }

        public string? ff_phone { get; set; }

        public string? ff_email { get; set; }

        public string? ff_relation { get; set; }

        public string? street { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }

        public string? zipcode { get; set; }

        public string? symptoms { get; set; }


        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? firstname { get; set; }

        public string? lastname { get; set; }

        public string? dateofbirth { get; set; }

        public string? email { get; set; }

        public string? phone { get; set; }

        public string? room { get; set; }

        public string? relation { get; set; }

        public IFormFile? upload { get; set; }
    }
}
