using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class BudgetProgramDTO
{
    public int Id { get; set; }

    [Display(Name = "Program", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ProgramId { get; set; }

    [Display(Name = "BudgetType", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetTypeId { get; set; }

    [Display(Name = "Validity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ValidityId { get; set; }

    [Display(Name = "Budget", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetId { get; set; }

    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]

    [Range(1, int.MaxValue, ErrorMessageResourceName = "RequiredWorth", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }

    public bool Estado { get; set; }
    public BudgetTypeDTO? BudgetType { get; set; }
}
