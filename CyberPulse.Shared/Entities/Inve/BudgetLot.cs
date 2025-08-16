using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("BudgetLots", Schema = "Inve")]
public class BudgetLot
{
    public int Id { get; set; }
    public int BudgetProgramId { get; set; }
    public int LotId { get; set; }

    [Display(Name = "worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }

    public BudgetProgram? BudgetProgram { get; set; }
    public Lot? Lot { get; set; }
    public Statu? Statu { get; set; }

}
