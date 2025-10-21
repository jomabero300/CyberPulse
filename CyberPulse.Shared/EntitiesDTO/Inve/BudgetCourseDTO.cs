using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Resources;
using CyberPulse.Shared.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class BudgetCourseDTO
{
    public int Id { get; set; }

    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string InstructorId { get; set; } = null!;

    [Display(Name = "BudgetLot", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetLotId { get; set; }

    [Display(Name = "Validity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ValidityId { get; set; }

    [Display(Name = "Course", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CourseProgramLotId { get; set; }

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime? StartDate { get; set; }

    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [DateRangeValidation("StartDate", ErrorMessageResourceName = "DateNoEndStart", ErrorMessageResourceType = typeof(Literals))]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public BudgetProgram2DTO? BudgetProgram { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public BudgetLot1DTO? BudgetLot { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public CourseProgramLot1DTO? CourseProgramLot { get; set; }
    public ChipUserDTO? Instructor { get; set; }
}