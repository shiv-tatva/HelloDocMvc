using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("requesttype")]
public partial class Requesttype
{
    [Key]
    [Column("requesttypeid")]
    public int Requesttypeid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;
}
