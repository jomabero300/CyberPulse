using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Chipp;

public class ChipHour
{
    public int Id { get; set; }
    public int ChipId { get; set; }

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Monday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Monday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Tuesday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Tuesday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Wednesday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Wednesday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Thursday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Thursday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Friday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Friday { get; set; } = null!;

    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Saturday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Saturday { get; set; } = null!;


    [Column(TypeName = "varchar(23)")]
    [Display(Name = "Sunday", ResourceType = typeof(Literals))]
    [MaxLength(23, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Sunday { get; set; } = null!;

    //mañana Morning Start Finish
    //tarde Afternoon
    //Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    public Chip Chip { get; set; } = null!;
}
