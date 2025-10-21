using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("ProductCurrentValues", Schema = "Inve")]
public class ProductCurrentValue
{
    public int Id { get; set; }
    public int ValidityId { get; set; }
    public int ProductId { get; set; }
    public int IvaId { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Column(TypeName = "decimal(3,1)")]
    [Display(Name = "Percentage", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Percentage { get; set; }

    public Validity? Validity { get; set; }
    public Product? Product { get; set; }
    public Iva? Iva { get; set; }
}
