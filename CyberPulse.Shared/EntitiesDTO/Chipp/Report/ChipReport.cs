using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp.Report;

public class ChipReport
{
    [Display(Name = "ChipNo", ResourceType = typeof(Literals))]
    [MaxLength(15, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string ChipNo { get; set; } = null!;

    [Display(Name = "ChipProgram", ResourceType = typeof(Literals))]
    [MaxLength(10, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Code { get; set; } = null!;

    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(15, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Identificacion { get; set; } = null!;

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    public DateTime? StartDate { get; set; }

    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    public DateTime? EndDate { get; set; }

    [Display(Name = "AlertDate", ResourceType = typeof(Literals))]
    public DateTime? AlertDate { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    public StatuDTO? Statu { get; set; }


    public int StatuId { get; set; }
    public string InstructorName { get; set; } = null!;
    public string InstructorId { get; set; } = null!;

    public int ChipProgramId { get; set; }

    public string ChipProgramName { get; set; } = null!;

}
