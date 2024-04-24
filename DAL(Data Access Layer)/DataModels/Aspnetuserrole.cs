﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("aspnetuserroles")]
public partial class Aspnetuserrole
{
    [Column("userid")]
    public int Userid { get; set; }

    [Column("roleid")]
    public int Roleid { get; set; }

    [Key]
    [Column("aspnetuserrolesid")]
    public int Aspnetuserrolesid { get; set; }

    [ForeignKey("Roleid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetrole Role { get; set; } = null!;

    [ForeignKey("Userid")]
    [InverseProperty("Aspnetuserroles")]
    public virtual Aspnetuser User { get; set; } = null!;
}
