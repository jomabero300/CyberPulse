using CyberPulse.Shared.Resources;
using CyberPulse.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class ChipDTO
{
    public int Id { get; set; }

    [Display(Name = "Apprentices", ResourceType = typeof(Literals))]
    [Range(1,int.MaxValue,ErrorMessageResourceName = "ApprenticeRange", ErrorMessageResourceType =typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int Apprentices { get; set; }

    [Display(Name = "ChipNo", ResourceType = typeof(Literals))]
    [MaxLength(15, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string ChipNo { get; set; } = null!;

    [Display(Name = "ChipProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ChipProgramId { get; set; }

    [Display(Name = "Company", ResourceType = typeof(Literals))]
    [MaxLength(100, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    public string Company { get; set; } = null!;

    [Display(Name = "Employee", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String InstructorId { get; set; } = null!;

    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime StartDate { get; set; }

    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime EndDate { get; set; }

    [Display(Name = "Neighborhood", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NeighborhoodId { get; set; }


    [Display(Name = "TrainingProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TrainingProgramId { get; set; }


    [Display(Name = "TypeOfTraining", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypeOfTrainingId { get; set; }

    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String UserId { get; set; } = null!;

    public int Duration { get; set; }

    [Display(Name = "Justification", ResourceType = typeof(Literals))]
    [MaxLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Justification { get; set; } = null!;

    public bool WingMeasure { get; set; }





    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayMorningStar { get; set; }

    [ValidareEndTime(nameof(MondayMorningStar), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayAfternoonStar { get; set; }

    [ValidareEndTime(nameof(MondayMorningStar), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayAfternoonEnd { get; set; }
    public TimeSpan MondayTotalHoras =>
        (MondayMorningStar > TimeSpan.Zero && MondayMorningEnd > MondayMorningStar ? MondayMorningEnd - MondayMorningStar : TimeSpan.Zero) +
        (MondayAfternoonStar > TimeSpan.Zero && MondayAfternoonEnd > MondayAfternoonStar ? MondayAfternoonEnd - MondayAfternoonStar : TimeSpan.Zero);


    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayMorningStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayMorningEnd { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayAfternoonStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayAfternoonEnd { get; set; }
    public TimeSpan TuesdayTotalHoras =>
        (TuesdayMorningStar > TimeSpan.Zero && TuesdayMorningEnd > TuesdayMorningStar ? TuesdayMorningEnd - TuesdayMorningStar : TimeSpan.Zero) +
        (TuesdayAfternoonStar > TimeSpan.Zero && TuesdayAfternoonEnd > TuesdayAfternoonStar ? TuesdayAfternoonEnd - TuesdayAfternoonStar : TimeSpan.Zero);


    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayMorningStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayMorningEnd { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayAfternoonStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayAfternoonEnd { get; set; }
    public TimeSpan WednesdayTotalHoras =>
        (WednesdayMorningStar > TimeSpan.Zero && WednesdayMorningEnd > WednesdayMorningStar ? WednesdayMorningEnd - WednesdayMorningStar : TimeSpan.Zero) +
        (WednesdayAfternoonStar > TimeSpan.Zero && WednesdayAfternoonEnd > WednesdayAfternoonStar ? WednesdayAfternoonEnd - WednesdayAfternoonStar : TimeSpan.Zero);


    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TursdayMorningStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TursdayMorningEnd { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TursdayAfternoonStar { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TursdayAfternoonEnd { get; set; }
    public TimeSpan TursdayTotalHoras =>
        (TursdayMorningStar > TimeSpan.Zero && TursdayMorningEnd > TursdayMorningStar ? TursdayMorningEnd - TursdayMorningStar : TimeSpan.Zero) +
        (TursdayAfternoonStar > TimeSpan.Zero && TursdayAfternoonEnd > TursdayAfternoonStar ? TursdayAfternoonEnd - TursdayAfternoonStar : TimeSpan.Zero);



    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayMorningStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayMorningEnd { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayAfternoonStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayAfternoonEnd { get; set; }
    public TimeSpan FridayTotalVertas { get; set; }
    public TimeSpan FridayTotalHoras =>
        (FridayMorningStar > TimeSpan.Zero && FridayMorningEnd > FridayMorningStar ? FridayMorningEnd - FridayMorningStar : TimeSpan.Zero) +
        (FridayAfternoonStar > TimeSpan.Zero && FridayAfternoonEnd > FridayAfternoonStar ? FridayAfternoonEnd - FridayAfternoonStar : TimeSpan.Zero);

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayMorningStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayMorningEnd { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayAfternoonStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayAfternoonEnd { get; set; }
    public TimeSpan SaturdayTotalHoras =>
        (SaturdayMorningStar > TimeSpan.Zero && SaturdayMorningEnd > SaturdayMorningStar ? SaturdayMorningEnd - SaturdayMorningStar : TimeSpan.Zero) +
        (SaturdayAfternoonStar > TimeSpan.Zero && SaturdayAfternoonEnd > SaturdayAfternoonStar ? SaturdayAfternoonEnd - SaturdayAfternoonStar : TimeSpan.Zero);



    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayMorningStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayMorningEnd { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayAfternoonStar { get; set; }
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayAfternoonEnd { get; set; }
    public TimeSpan SundayTotalHoras => 
        (SundayMorningStar>TimeSpan.Zero &&  SundayMorningEnd> SundayMorningStar? SundayMorningEnd-SundayMorningStar: TimeSpan.Zero)+
        (SundayAfternoonStar>TimeSpan.Zero &&  SundayAfternoonEnd> SundayAfternoonStar? SundayAfternoonEnd-SundayAfternoonStar:TimeSpan.Zero);


}