using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("Chips", Schema = "Chip")]
public class Chip
{
    public int Id { get; set; }

    [Display(Name = "Apprentices", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Apprentices { get; set; }

    [Column(TypeName = "varchar(15)")]
    [Display(Name = "ChipNo", ResourceType = typeof(Literals))]
    [MaxLength(15, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string ChipNo { get; set; } = null!;
    
    [Display(Name = "ChipProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ChipProgramId { get; set; }
    
    [Column(TypeName = "varchar(100)")]
    [Display(Name = "Company", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Company { get; set; } = null!;
    
    [Column(TypeName = "nvarchar(450)")]
    [Display(Name = "Employee", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String EmployeeId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Display(Name = "Settlement", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NeighborhoodId { get; set; }

    [Display(Name = "TypeOfTraining", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypeOfTrainingId { get; set; }

    [Column(TypeName = "nvarchar(450)")]
    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String UserId { get; set; } = null!;
    
    public User Employee { get; set; } = null!;
    public User User { get; set; } = null!;
    public ChipProgram ChipProgram { get; set; } = null!;
    public Neighborhood Neighborhood { get; set; } = null!;
    public TypeOfTraining TypeOfTraining { get; set; } = null!;
}