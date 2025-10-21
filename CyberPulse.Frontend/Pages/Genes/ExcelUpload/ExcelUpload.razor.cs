using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace CyberPulse.Frontend.Pages.Genes.ExcelUpload;

[Authorize(Roles = "Admi")]
public partial class ExcelUpload
{
    private UploadModel uploadModel = new();
    private bool isLoading = false;
    private string resultMessage = string.Empty;
    private string errorMessage = string.Empty;

    [Inject] private HttpClient Http { get; set; } = null!;
    [Inject] private ILogger<ExcelUpload> Logger { get; set; } = null!;


    private class UploadModel
    {
        public IBrowserFile? File { get; set; }
    }

    private void OnInputFileChange(InputFileChangeEventArgs e)
    {
        uploadModel.File = e.File;
    }

    private async Task HandleValidSubmit()
    {
        if (uploadModel.File == null) return;

        isLoading = true;
        resultMessage = string.Empty;
        errorMessage = string.Empty;

        try
        {
            using var content = new MultipartFormDataContent();
            using var fileStream = uploadModel.File.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024); // 10MB max
            content.Add(new StreamContent(fileStream), "file", uploadModel.File.Name);

            //            var response = await Http.PostAsync("api/excelUpload/upload", content);

            var response = await Http.PostAsync("api/ExcelExports/upload", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<UploadResult>();
                resultMessage = $"{result?.Message}. Hojas procesadas: {result?.SheetsProcessed}, Filas procesadas: {result?.RowsProcessed}";
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                errorMessage = $"Error: {error}";
            }
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error al subir archivo");
            errorMessage = $"Error al procesar el archivo: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    private class UploadResult
    {
        public string? Message { get; set; }
        public int SheetsProcessed { get; set; }
        public int RowsProcessed { get; set; }
    }
}