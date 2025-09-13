using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class BudgetCourseDTO
{
    public int Id { get; set; }

    [Display(Name = "Validity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ValidityId { get; set; }
    [Display(Name = "Course", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CourseId { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }
}