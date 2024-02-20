﻿using Microsoft.AspNetCore.Http;
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

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? ff_firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? ff_lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? ff_phone { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? ff_email { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? ff_relation { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? street { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? city { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? state { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? zipcode { get; set; }

        public string? symptoms { get; set; }


        [Required(ErrorMessage = "Please Enter Your Name")]
        public string? firstname { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? lastname { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? dateofbirth { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? email { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        [StringLength(10, ErrorMessage = "Mobile Number should less than 10 char")]
        public string? phone { get; set; }

        public string? room { get; set; }

        [Required(ErrorMessage = "Please Enter Your First Name")]
        public string? relation { get; set; }

        public IFormFile? upload { get; set; }
    }
}
