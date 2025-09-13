using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class CourseLotDTO
{
    public int Id { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CourseId { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ProgramLotId { get; set; }


    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public CourseDTO? Course { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public ProgramLotDTO? ProgramLot { get; set; }
}