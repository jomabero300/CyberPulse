using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class CourseProgramLot1DTO
{
    public int Id { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CourseId { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ProgramLotId { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public CourseDTO? Course { get; set; }

    public ProgramLot2DTO? ProgramLot { set; get; }
}