using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("ProgramLots", Schema = "Inve")]
public class ProgramLot
{
    public int Id { get; set; }

    [Display(Name = "Program", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ProgramId { get; set; }

    [Display(Name = "Lot", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int LotId { get; set; }

    public InvProgram? Program { get; set; }
    public Lot? Lot { get; set; }
}
