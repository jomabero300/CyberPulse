using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Gene;

[Table("Ivas", Schema = "Gene")]
public class Iva
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(3)")]
    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [MaxLength(3, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    [Column(TypeName = "decimal(3,1)")]
    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }
    public int StatuId { get; set; }

    public Statu? Statu { get; set; }
}
