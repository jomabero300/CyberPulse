using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CyberPulse.Frontend.Helpers;

public static class EnumHelper
{
    public static string GetDisplayName(this Enum enumValue)
    {
        var member = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();

        if (member == null)
        {
            return enumValue.ToString(); // Retorna el nombre del miembro si no hay DisplayAttribute
        }

        // Busca el DisplayAttribute en el miembro
        var displayAttribute = member.GetCustomAttribute<DisplayAttribute>();

        if (displayAttribute != null)
        {
            // Si el ResourceType está definido, usa GetName() para obtener el valor localizado
            if (displayAttribute.ResourceType != null)
            {
                // Lógica para obtener el valor localizado del recurso
                return displayAttribute.GetName() ?? enumValue.ToString();
            }

            // Si no hay ResourceType, usa el valor de Name
            return displayAttribute.Name ?? enumValue.ToString();
        }

        // Retorna el nombre del miembro si no se encuentra el DisplayAttribute
        return enumValue.ToString();
    }
}