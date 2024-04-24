﻿using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("physiciannotification")]
public partial class Physiciannotification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("physicianid")]
    public int Physicianid { get; set; }

    [Column("isnotificationstopped", TypeName = "bit(1)")]
    public BitArray Isnotificationstopped { get; set; } = null!;

    [ForeignKey("Physicianid")]
    [InverseProperty("Physiciannotifications")]
    public virtual Physician Physician { get; set; } = null!;
}
