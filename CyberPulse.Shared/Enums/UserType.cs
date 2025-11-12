using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;

namespace CyberPulse.Shared.Enums;

public enum UserType
{
    [Display(Name = "Admi", ResourceType = typeof(Literals))]
    Admi,
    [Display(Name = "Coor", ResourceType = typeof(Literals))]
    Coor,
    [Display(Name = "Purc", ResourceType = typeof(Literals))]
    Purc,
    [Display(Name = "Instructor", ResourceType = typeof(Literals))]
    Inst,
    [Display(Name = "User", ResourceType = typeof(Literals))]
    User
}
