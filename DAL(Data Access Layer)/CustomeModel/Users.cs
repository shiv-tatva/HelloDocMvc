using DAL_Data_Access_Layer_.DataModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_Data_Access_Layer_.CustomeModel
{
    public class Users
    {
      
            [Key]
            [Column("id")]
            public int? Id { get; set; }

            [Column("username")]
            [StringLength(256)]
            public string? Username { get; set; }

            [Required(ErrorMessage = "Please Enter Your Password")]
            [Column("passwordhash", TypeName = "character varying")]
            public string? Passwordhash { get; set; }

            [Column("email")]
            [StringLength(256)]
            [Required(ErrorMessage = "Please Enter Your Email")]
             public string? Email { get; set; } = null!;

            [Column("phonenumber", TypeName = "character varying")]
            public string? Phonenumber { get; set; }

            [Column("ip")]
            [StringLength(20)]
            public string? Ip { get; set; }

            [Column("createddate", TypeName = "timestamp without time zone")]
            public DateTime? Createddate { get; set; }

            [Column("modifieddate", TypeName = "timestamp without time zone")]
            public DateTime? Modifieddate { get; set; }

            public string? RoleMain { get; set; }

            public string? passwordcheck {  get; set; } 
            public string? emailcheck {  get; set; } 

            public int roleId { get; set; }
            public int? flagId { get; set; }
    }

 
}
