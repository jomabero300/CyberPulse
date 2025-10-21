using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class ChipCoordinator
{
    public int Id { get; set; }

    [Display(Name = "ChipNo", ResourceType = typeof(Literals))]
    [MaxLength(15, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string ChipNo { get; set; } = null!;

    [Display(Name = "ChipProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Identificacion { get; set; } = null!;

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime? StartDate { get; set; }

    public string InstructorName { get; set; } = null!;
    public string InstructorId { get; set; } = null!;
    public int ChipProgramId { get; set; }

    public string ChipProgramName { get; set; } = null!;
    public int StatuId { get; set; }
    public bool idEsta { get; set; }

    public string language { get; set; }
}
