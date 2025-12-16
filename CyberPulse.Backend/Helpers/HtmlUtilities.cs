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
    public static async Task<string> UploadImageAsync(string image, string directoryFolder, string imageUrl = "")
    {

        var imageBase64 = Convert.FromBase64String(image!);

        string pathImage = $"{Guid.NewGuid()}.jpg";

        if (!Directory.Exists(directoryFolder))
        {
            Directory.CreateDirectory(directoryFolder);
        }

        var path = $"{directoryFolder}{pathImage}";

        await System.IO.File.WriteAllBytesAsync(path, imageBase64);

        if (!string.IsNullOrWhiteSpace(imageUrl))
        {
            System.IO.File.Delete(imageUrl);
        }

        return pathImage;
    }
}
