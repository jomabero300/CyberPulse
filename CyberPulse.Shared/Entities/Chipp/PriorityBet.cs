using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

[Table("PriorityBets", Schema = "Chip")]
public class PriorityBet
{
    public int Id { get; set; }

    [Column(TypeName = "varchar(60)")]
    [Display(Name = "Name", ResourceType = typeof(Literals))]
    [MaxLength(60, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Name { get; set; } = null!;

    public ICollection<ChipProgram>? ChipProgram { get; set; }
}
