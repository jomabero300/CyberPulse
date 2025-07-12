using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.Enums;

public enum UserType
{
    [Display(Name = "Administrador")]
    Admi,
    [Display(Name = "Coordinador")]
    Coor,
    [Display(Name = "Instructor")]
    Inst,
    [Display(Name = "Usuario")]
    User
}
