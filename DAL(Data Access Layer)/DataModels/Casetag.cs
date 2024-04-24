using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL_Data_Access_Layer_.DataModels;

[Table("casetag")]
public partial class Casetag
{
    [Key]
    [Column("casetagid")]
    public int Casetagid { get; set; }

    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;
}
