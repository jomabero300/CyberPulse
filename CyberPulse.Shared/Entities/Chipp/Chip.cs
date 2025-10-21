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
    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string InstructorId { get; set; } = null!;

    [Column(TypeName = "datetime")]
    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "AlertDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime AlertDate { get; set; }

    [Display(Name = "Neighborhood", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NeighborhoodId { get; set; }

    [Display(Name = "TrainingProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TrainingProgramId { get; set; }

    [Display(Name = "TypeOfTraining", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypeOfTrainingId { get; set; }

    [Column(TypeName = "nvarchar(450)")]
    [Display(Name = "User", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string UserId { get; set; } = null!;


    [Column(TypeName = "varchar(500)")]
    [Display(Name = "Justification", ResourceType = typeof(Literals))]
    [MaxLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Justification { get; set; } = null!;



    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Monday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Monday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Tuesday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Tuesday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Wednesday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Wednesday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Tursday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Tursday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Friday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Friday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Saturday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Saturday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Sunday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Sunday { get; set; } = null!;


    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Range(0, int.MaxValue, ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }
    public bool idEsta { get; set; }
    public bool Holiday { get; set; }
    public bool SentStatus { get; set; }

    public User Instructor { get; set; } = null!;
    public User User { get; set; } = null!;
    public ChipProgram ChipProgram { get; set; } = null!;
    public Neighborhood Neighborhood { get; set; } = null!;
    public TypeOfTraining TypeOfTraining { get; set; } = null!;
    public TrainingProgram? TrainingProgram { get; set; }
    public Statu Statu { get; set; } = null!;
    public ICollection<ChipPoblation>? ChipPoblations { get; set; }
}