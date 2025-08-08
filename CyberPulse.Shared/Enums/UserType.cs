using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.Enums;

public enum UserType
{
    [Display(Name = "Administrador", ResourceType = typeof(Literals))]
    Admi,
    [Display(Name = "Coordinador", ResourceType = typeof(Literals))]
    Coor,
    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    Inst,
    [Display(Name = "Usuario", ResourceType = typeof(Literals))]
    User
}
