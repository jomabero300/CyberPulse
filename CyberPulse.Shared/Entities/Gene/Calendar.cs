using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Gene;

[Table("Calendars", Schema = "Gene")]
public class Calendar
{
    public int Id { get; set; }

    [Column(TypeName = "date")]
    [Display(Name = "Holiday", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime Holiday { get; set; }
}