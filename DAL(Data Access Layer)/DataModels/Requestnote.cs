﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("requestnotes")]
public partial class Requestnote
{
    [Key]
    [Column("requestnotesid")]
    public int Requestnotesid { get; set; }

    [Column("requestid")]
    public int Requestid { get; set; }

    [Column("strmonth")]
    [StringLength(20)]
    public string? Strmonth { get; set; }

    [Column("intyear")]
    public int? Intyear { get; set; }

    [Column("intdate")]
    public int? Intdate { get; set; }

    [Column("physiciannotes")]
    [StringLength(500)]
    public string? Physiciannotes { get; set; }

    [Column("adminnotes")]
    [StringLength(500)]
    public string? Adminnotes { get; set; }

    [Column("createdby")]
    public int Createdby { get; set; }

    [Column("createddate", TypeName = "timestamp without time zone")]
    public DateTime Createddate { get; set; }

    [Column("modifiedby")]
    public int? Modifiedby { get; set; }

    [Column("modifieddate", TypeName = "timestamp without time zone")]
    public DateTime? Modifieddate { get; set; }

    [Column("ip")]
    [StringLength(20)]
    public string? Ip { get; set; }

    [Column("administrativenotes")]
    [StringLength(500)]
    public string? Administrativenotes { get; set; }

    [ForeignKey("Createdby")]
    [InverseProperty("RequestnoteCreatedbyNavigations")]
    public virtual Aspnetuser CreatedbyNavigation { get; set; } = null!;

    [ForeignKey("Modifiedby")]
    [InverseProperty("RequestnoteModifiedbyNavigations")]
    public virtual Aspnetuser? ModifiedbyNavigation { get; set; }

    [ForeignKey("Requestid")]
    [InverseProperty("Requestnotes")]
    public virtual Request Request { get; set; } = null!;
}
