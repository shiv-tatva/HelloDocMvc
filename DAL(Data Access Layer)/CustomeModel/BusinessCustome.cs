﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class BusinessCustome
    {
        public string? business_firstname { get; set; }

        public string? business_lastname { get; set; }

        public string? business_phone { get; set; }

        public string? business_email { get; set; }

        [Required(ErrorMessage = "Please Enter Property/Business Name")]
        public string? business_property { get; set; }

        public string? business_casenumber { get; set; }

        public string? street { get; set; }

        public string? city { get; set; }

        public string? state { get; set; }

        public string? zipcode { get; set; }

        public string? symptoms { get; set; }

        public string? firstname { get; set; }

        public string? lastname { get; set; }

        public string? dateofbirth { get; set; }

        public string? email { get; set; }

        public string? phone { get; set; }

        public string? room { get; set; }
    }
}
