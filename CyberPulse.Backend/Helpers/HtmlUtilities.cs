using System.Globalization;
using System.Text.RegularExpressions;

namespace CyberPulse.Backend.Helpers;

public static class HtmlUtilities
{
    public static string StripTags(string html)
    {
        if (string.IsNullOrEmpty(html))
            return html;

        // Reemplaza saltos de línea HTML por espacios
        html = html.Replace("<br>", "\n").Replace("<br/>", "\n").Replace("<br />", "\n");

        // Elimina todas las etiquetas HTML
        html = Regex.Replace(html, "<[^>]*(>|$)", string.Empty);

        // Decodifica entidades HTML
        html = System.Net.WebUtility.HtmlDecode(html);

        return html;
    }
    public static string ToTitleCase(string html)
    {
        CultureInfo culture=CultureInfo.InvariantCulture;
        TextInfo textinfo=culture.TextInfo;
        return textinfo.ToTitleCase(html);
    }

}
