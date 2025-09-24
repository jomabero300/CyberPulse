using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("ChipPrograms", Schema = "Chip")]
public class ChipProgram
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(10)")]
    [Display(Name = "Unspsc", ResourceType = typeof(Literals))]
    [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "Version", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Version { get; set; }

    [Column(TypeName = "varchar(300)")]
    [Display(Name = "Unspsc", ResourceType = typeof(Literals))]
    [MaxLength(300, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Designation { get; set; }= null!;

    [Display(Name = "Duration", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Duration { get; set; }

    [Display(Name = "PriorityBet", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int PriorityBetId { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Display(Name = "SupportFic", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public bool SupportFic { get; set; }

    [Display(Name = "TriningLevel", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TriningLevelId { get; set; }

    [Column(TypeName = "varchar(20)")]
    [Display(Name = "TypeOfTraining", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [MaxLength(20, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string TypeOfTraining { get; set; }=null!;

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public bool WingMeasure { get; set; }

    public PriorityBet PriorityBet { get; set; } = null!;
    public TriningLevel TriningLevel { get; set; } = null!;
    public ICollection<Chip> Chips { get; set; } = null!;
    public int ChipNunber => Chips == null ? 0 : Chips.Count;


}