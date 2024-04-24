using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("aspnetroles")]
public partial class Aspnetrole
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    [InverseProperty("Role")]
    public virtual ICollection<Aspnetuserrole> Aspnetuserroles { get; set; } = new List<Aspnetuserrole>();
}
