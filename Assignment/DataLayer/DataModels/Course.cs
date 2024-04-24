using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataModels;

[Table("course")]
public partial class Course
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [StringLength(255)]
    public string Name { get; set; } = null!;

    [InverseProperty("Course")]
    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
