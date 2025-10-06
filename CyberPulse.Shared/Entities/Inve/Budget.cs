using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("Budgets", Schema = "Inve")]
public class Budget
{
    public int Id { get; set; }

    [Display(Name = "Validity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ValidityId { get; set; }

    [Display(Name = "BudgetType", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetTypeId { get; set; }

    [Column(TypeName = "varchar(60)")]
    [Display(Name = "Rubro", ResourceType = typeof(Literals))]
    [MaxLength(60, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Rubro { get; set; } = null!;

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }
    public Statu? Statu { get; set; }
    public Validity? Validity { get; set; }
    public BudgetType? BudgetType { get; set; }
}
