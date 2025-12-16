using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Shared;

public partial class InputImg
{
    private string? fileName;

    private string? _localImageData; // Para mostrar la imagen localmente
    
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Parameter] public string? Label { get; set; }
    [Parameter] public string? ImageURL { get; set; }
    [Parameter] public EventCallback<string> ImageSelected { get; set; }

    protected override void OnParametersSet()
    {
        if (string.IsNullOrWhiteSpace(Label))
        {
            Label = Localizer["Image"];
        }
    }
    // InputImg.razor.cs
    private async Task OnChange(InputFileChangeEventArgs e)
    {
        try
        {
            var file = e.File;

            if (file == null)
                return;

            // Validar tamaño máximo (512KB)
            if (file.Size > 512 * 1024)
            {
                Console.WriteLine("Archivo demasiado grande");
                return;
            }

            // Validar extensión
            var validExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.Name).ToLowerInvariant();

            if (!validExtensions.Contains(extension))
            {
                Console.WriteLine("Extensión no válida");
                return;
            }

            fileName = file.Name;

            // Leer archivo a bytes
            using var memoryStream = new MemoryStream();
            await file.OpenReadStream(512 * 1024).CopyToAsync(memoryStream);
            var arrBytes = memoryStream.ToArray();

            // Convertir a Base64
            var base64String = Convert.ToBase64String(arrBytes);

            // Determinar MIME type
            var mimeType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => "image/jpeg"
            };

            // Crear Data URL
            var dataUrl = $"data:{mimeType};base64,{base64String}";

            // Mostrar localmente
            _localImageData = dataUrl;
            ImageURL = null;

            // Enviar Data URL al padre
            await ImageSelected.InvokeAsync(dataUrl);

            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

}