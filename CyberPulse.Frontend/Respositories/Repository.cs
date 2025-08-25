
using CyberPulse.Shared.EntitiesDTO.Chipp.Report;
using System.Text;
using System.Text.Json;

namespace CyberPulse.Frontend.Respositories;

public class Repository : IRepository
{
    private HttpClient _httpClient;
    private JsonSerializerOptions _jsonDefaultOptions => new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
    };

    public Repository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseWrapper<T>> GetAsync<T>(string url)
    {
        var responseHttp = await _httpClient.GetAsync(url);

        if (responseHttp.IsSuccessStatusCode)
        {
            var response = await UnserializeAnswer<T>(responseHttp);
            return new HttpResponseWrapper<T>(response, false, responseHttp);
        }

        return new HttpResponseWrapper<T>(default, true, responseHttp);
    }

    public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
    {
        var messageJSON = JsonSerializer.Serialize(model);
        var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");
        var responseHttp = await _httpClient.PostAsync(url, messageContet);
        return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
    }

    public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
    {
        var messageJSON = JsonSerializer.Serialize(model);
        var messageContet = new StringContent(messageJSON, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = messageContet;

        var responseHttp = await _httpClient.PostAsync(url, messageContet);
        if (responseHttp.IsSuccessStatusCode)
        {
            var response = await UnserializeAnswer<TActionResponse>(responseHttp);
            return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
        }

        return new HttpResponseWrapper<TActionResponse>(default, !responseHttp.IsSuccessStatusCode, responseHttp);

    }

    public async Task<HttpResponseWrapper<object>> DeleteAsync(string url)
    {
        var responseHttp = await _httpClient.DeleteAsync(url);
        return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);

    }

    public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
    {
        var messageJson = JsonSerializer.Serialize(model);
        var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
        var responseHttp = await _httpClient.PutAsync(url, messageContent);
        return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp);
    }

    public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
    {
        var messageJson = JsonSerializer.Serialize(model);
        var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
        var responseHttp = await _httpClient.PutAsync(url, messageContent);
        if (responseHttp.IsSuccessStatusCode)
        {
            var response = await UnserializeAnswer<TActionResponse>(responseHttp);
            return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
        }
        return new HttpResponseWrapper<TActionResponse>(default, true, responseHttp);

    }

    public async Task<HttpResponseWrapper<object>> GetAsync(string url)
    {
        var responseHTTP = await _httpClient.GetAsync(url);
        return new HttpResponseWrapper<object>(null, !responseHTTP.IsSuccessStatusCode, responseHTTP);
    }

    private async Task<T> UnserializeAnswer<T>(HttpResponseMessage responseHttp)
    {
        var response = await responseHttp.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(response, _jsonDefaultOptions)!;
    }

    public async Task<HttpResponseWrapper<byte[]>> GetBytesAsync(string url)
    {
        var responseHTTP = await _httpClient.GetAsync(url);

        if (responseHTTP.IsSuccessStatusCode)
        {
            var bytes = await responseHTTP.Content.ReadAsByteArrayAsync();
            return new HttpResponseWrapper<byte[]>(bytes, false, responseHTTP);
        }

        return new HttpResponseWrapper<byte[]>(null, true, responseHTTP);
    }

    public async Task<HttpResponseWrapper<byte[]>> GetBytesAsync(string url, ChipReport chipReport)
    {
        var queryParameters = new List<string>();

        // Aquí puedes agregar los parámetros de tu objeto ChipReport
        // Por ejemplo:
        string strChipNo = "ChipNo=''";
        string strCode = "Code=''";
        string strInstructorId = "InstructorId=''";
        string strIdentificacion = "Identificacion=''";
        string strInstructorName = "InstructorName=''";

        string strProgramName = "ChipProgramName=''";
        
        if (!string.IsNullOrWhiteSpace(chipReport.ChipNo)) strChipNo = $"ChipNo={chipReport.ChipNo}";
        if (!string.IsNullOrWhiteSpace(chipReport.Code)) strCode = $"Code={chipReport.Code}";
        if (!string.IsNullOrWhiteSpace(chipReport.InstructorId)) strInstructorId = $"InstructorId={chipReport.InstructorId}";
        if (!string.IsNullOrWhiteSpace(chipReport.Identificacion)) strIdentificacion = $"Identificacion={chipReport.Identificacion}";
        if (!string.IsNullOrWhiteSpace(chipReport.InstructorName)) strInstructorName = $"InstructorName={chipReport.InstructorName}";
        if (!string.IsNullOrWhiteSpace(chipReport.ChipProgramName)) strProgramName = $"ChipProgramName={chipReport.ChipProgramName}";

        queryParameters.Add(strChipNo);
        queryParameters.Add(strCode);
        queryParameters.Add(strInstructorId);
        queryParameters.Add(strIdentificacion);
        queryParameters.Add(strInstructorName);
        queryParameters.Add(strProgramName);

        if (chipReport.StartDate.HasValue)
        {
            queryParameters.Add($"StartDate={chipReport.StartDate.Value:yyyy-MM-dd}");
        }

        if (chipReport.EndDate.HasValue)
        {
            queryParameters.Add($"EndDate={chipReport.EndDate.Value:yyyy-MM-dd}");
        }

        // ... y otros parámetros del reporte

        var fullUrl = url;
        if (queryParameters.Any())
        {
            fullUrl += "?" + string.Join("&", queryParameters);
        }

        var responseHTTP = await _httpClient.GetAsync(fullUrl);

        if (responseHTTP.IsSuccessStatusCode)
        {
            var bytes = await responseHTTP.Content.ReadAsByteArrayAsync();
            return new HttpResponseWrapper<byte[]>(bytes, false, responseHTTP);
        }
        else
        {
            // 🎉 Lee el mensaje de error del servidor
            var errorContent = await responseHTTP.Content.ReadAsStringAsync();

            // Imprime o registra este mensaje para ver el detalle del error
            Console.WriteLine("Error del servidor: " + errorContent);

            // Si el error es un JSON, puedes deserializarlo para una mejor lectura
            // var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(errorContent);
            // Console.WriteLine(problemDetails.Detail);

            return new HttpResponseWrapper<byte[]>(null, true, responseHTTP);
        }
        //return new HttpResponseWrapper<byte[]>(null, true, responseHTTP);
    }
}
