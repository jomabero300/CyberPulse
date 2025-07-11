using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Resources;
using CyberPulse.Shared.Validations;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class ChipDTO
{
    public int Id { get; set; }

    [Display(Name = "Apprentices", ResourceType = typeof(Literals))]
    [Range(1, int.MaxValue, ErrorMessageResourceName = "ApprenticeRange", ErrorMessageResourceType = typeof(Literals))]
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

    [Display(Name = "AlertDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime AlertDate { get; set; }

    [Display(Name = "Neighborhood", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int NeighborhoodId { get; set; }

    [Display(Name = "TrainingProgram", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TrainingProgramId { get; set; }


    [Display(Name = "TypeOfTraining", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int TypeOfTrainingId { get; set; }

    [Display(Name = "User", ResourceType = typeof(Literals))]
    [MaxLength(450, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public String UserId { get; set; } = null!;

    public int Duration { get; set; }

    [Display(Name = "Justification", ResourceType = typeof(Literals))]
    [MaxLength(500, ErrorMessageResourceName = "MaxLength", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public string Justification { get; set; } = null!;

    public bool WingMeasure { get; set; }





    //[DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayMorningStart { get; set; }

    [ValidareEndTime(nameof(MondayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayMorningEnd { get; set; }

    //    [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(MondayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan MondayAfternoonEnd { get; set; }
    public TimeSpan MondayTotalHoras =>
        (MondayMorningStart > TimeSpan.Zero && MondayMorningEnd > MondayMorningStart ? MondayMorningEnd - MondayMorningStart : TimeSpan.Zero) +
        (MondayAfternoonStart > TimeSpan.Zero && MondayAfternoonEnd > MondayAfternoonStart ? MondayAfternoonEnd - MondayAfternoonStart : TimeSpan.Zero);


    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayMorningStart { get; set; }

    [ValidareEndTime(nameof(TuesdayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(TuesdayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan TuesdayAfternoonEnd { get; set; }
    public TimeSpan TuesdayTotalHoras =>
        (TuesdayMorningStart > TimeSpan.Zero && TuesdayMorningEnd > TuesdayMorningStart ? TuesdayMorningEnd - TuesdayMorningStart : TimeSpan.Zero) +
        (TuesdayAfternoonStart > TimeSpan.Zero && TuesdayAfternoonEnd > TuesdayAfternoonStart ? TuesdayAfternoonEnd - TuesdayAfternoonStart : TimeSpan.Zero);


    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayMorningStart { get; set; }

    [ValidareEndTime(nameof(WednesdayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(WednesdayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan WednesdayAfternoonEnd { get; set; }
    public TimeSpan WednesdayTotalHoras =>
        (WednesdayMorningStart > TimeSpan.Zero && WednesdayMorningEnd > WednesdayMorningStart ? WednesdayMorningEnd - WednesdayMorningStart : TimeSpan.Zero) +
        (WednesdayAfternoonStart > TimeSpan.Zero && WednesdayAfternoonEnd > WednesdayAfternoonStart ? WednesdayAfternoonEnd - WednesdayAfternoonStart : TimeSpan.Zero);


    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan ThursdayMorningStart { get; set; }

    [ValidareEndTime(nameof(ThursdayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan ThursdayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan ThursdayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(ThursdayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan ThursdayAfternoonEnd { get; set; }
    public TimeSpan ThursdayTotalHoras =>
        (ThursdayMorningStart > TimeSpan.Zero && ThursdayMorningEnd > ThursdayMorningStart ? ThursdayMorningEnd - ThursdayMorningStart : TimeSpan.Zero) +
        (ThursdayAfternoonStart > TimeSpan.Zero && ThursdayAfternoonEnd > ThursdayAfternoonStart ? ThursdayAfternoonEnd - ThursdayAfternoonStart : TimeSpan.Zero);



    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayMorningStart { get; set; }

    [ValidareEndTime(nameof(FridayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(FridayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan FridayAfternoonEnd { get; set; }
    public TimeSpan FridayTotalVertas { get; set; }
    public TimeSpan FridayTotalHoras =>
        (FridayMorningStart > TimeSpan.Zero && FridayMorningEnd > FridayMorningStart ? FridayMorningEnd - FridayMorningStart : TimeSpan.Zero) +
        (FridayAfternoonStart > TimeSpan.Zero && FridayAfternoonEnd > FridayAfternoonStart ? FridayAfternoonEnd - FridayAfternoonStart : TimeSpan.Zero);


    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayMorningStart { get; set; }

    [ValidareEndTime(nameof(SaturdayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(SaturdayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SaturdayAfternoonEnd { get; set; }
    public TimeSpan SaturdayTotalHoras =>
        (SaturdayMorningStart > TimeSpan.Zero && SaturdayMorningEnd > SaturdayMorningStart ? SaturdayMorningEnd - SaturdayMorningStart : TimeSpan.Zero) +
        (SaturdayAfternoonStart > TimeSpan.Zero && SaturdayAfternoonEnd > SaturdayAfternoonStart ? SaturdayAfternoonEnd - SaturdayAfternoonStart : TimeSpan.Zero);



    [ValidarRangoHora(true, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayMorningStart { get; set; }

    [ValidareEndTime(nameof(SundayMorningStart), true, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayMorningEnd { get; set; }

    [ValidarRangoHora(false, ErrorMessageResourceName = "ValidarRangoHora", ErrorMessageResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayAfternoonStart { get; set; }

    [ValidareEndTime(nameof(SundayAfternoonStart), false, ErrorMessage = "")]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public TimeSpan SundayAfternoonEnd { get; set; }
    public TimeSpan SundayTotalHoras =>
        (SundayMorningStart > TimeSpan.Zero && SundayMorningEnd > SundayMorningStart ? SundayMorningEnd - SundayMorningStart : TimeSpan.Zero) +
        (SundayAfternoonStart > TimeSpan.Zero && SundayAfternoonEnd > SundayAfternoonStart ? SundayAfternoonEnd - SundayAfternoonStart : TimeSpan.Zero);

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }

    public bool idEsta { get; set; }

    public List<TypeOfPoblationDTO> TypeOfPoblationDTO { get; set; } = null!;

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "ChipProgram", ResourceType = typeof(Literals))]
    public ChipProgramDTO? ChipProgram { get; set; }


    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    public ChipUserDTO? Instructor { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "City", ResourceType = typeof(Literals))]
    public CityDTO? City { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "TrainingProgram", ResourceType = typeof(Literals))]
    public TrainingProgramDTO? TrainingProgram { get; set; }

    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    [Display(Name = "TrainingProgram", ResourceType = typeof(Literals))]
    public TypeOfTrainingDTO? TypeOfTraining { get; set; }
    public bool Holiday { get; set; }

    public string language { get; set; }
}