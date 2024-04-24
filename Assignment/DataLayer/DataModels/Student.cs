using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.DataModels;

[Table("student")]
public partial class Student
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("firstname")]
    [StringLength(255)]
    public string Firstname { get; set; } = null!;

    [Column("lastname")]
    [StringLength(255)]
    public string Lastname { get; set; } = null!;

    [Column("courseid")]
    public int? Courseid { get; set; }

    [Column("age")]
    public int Age { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("gender")]
    [StringLength(10)]
    public string Gender { get; set; } = null!;

    [Column("coursename")]
    [StringLength(255)]
    public string? Coursename { get; set; }

    [Column("grade")]
    [StringLength(255)]
    public string Grade { get; set; } = null!;

    [Column("strmonth")]
    [StringLength(20)]
    public string? Strmonth { get; set; }

    [Column("intyear")]
    public int? Intyear { get; set; }

    [Column("intdate")]
    public int? Intdate { get; set; }

    [ForeignKey("Courseid")]
    [InverseProperty("Students")]
    public virtual Course? Course { get; set; }
}
