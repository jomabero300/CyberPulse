using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("Validities", Schema = "Inve")]
public class Validity
{
    public int Id { get; set; }

    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Value { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }
    public Statu? Statu { get; set; }
}
