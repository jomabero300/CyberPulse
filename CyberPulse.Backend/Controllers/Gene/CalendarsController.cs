using CyberPulse.Backend.UnitsOfWork.Interfaces;
using CyberPulse.Shared.Entities.Gene;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CyberPulse.Backend.Controllers.Gene;

[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[ApiController]
[Route("api/[controller]")]
public class CalendarsController : GenericController<Calendar>
{
    private const string ApiUrl = "https://date.nager.at/api/v3/PublicHolidays/{0}/CO";
    private static readonly HttpClient httpClient = new HttpClient();
    public CalendarsController(IGenericUnitOfWork<Calendar> unitOfWork) : base(unitOfWork)
    {

    }
    [HttpGet]
    public override async Task<IActionResult> GetAsync()
    {
        var festivos = await ObtenerFestivosColombiaAsync(DateTime.UtcNow.Year);

        if (festivos != null)
        {
            return Ok(festivos);
        }

        return BadRequest();
    }

    //[HttpGet("paginated")]
    //public override async Task<IActionResult> GetAsync([FromQuery] PaginationDTO pagination)
    //{
    //    var response = await _calendarUnitOfWork.GetAsync(pagination);

    //    if (response.WasSuccess)
    //    {
    //        return Ok(response.Result);
    //    }

    //    return BadRequest();
    //}

    //[HttpDelete("full/{id}")]
    //public override async Task<IActionResult> DeleteAsync(int id)
    //{
    //    var response = await _calendarUnitOfWork.DeleteAsync(id);

    //    if (response.WasSuccess)
    //    {
    //        return Ok(response.Result);
    //    }

    //    return BadRequest();
    //}

    //[HttpGet("Calendar")]
    //public async Task<IActionResult> GetAsync(string year)
    //{
    //    var festivos = await ObtenerFestivosColombiaAsync(int.Parse(year));

    //    if (festivos!=null)
    //    {
    //        return Ok(festivos);
    //    }

    //    return BadRequest();
    //}

    //[HttpGet("TotalRecordsPaginated")]
    //public async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
    //{
    //    var response = await _calendarUnitOfWork.GetTotalRecordsAsync(pagination);

    //    if (response.WasSuccess)
    //    {
    //        return Ok(response.Result);
    //    }

    //    return BadRequest();
    //}

    //[HttpPost("full")]
    //public async Task<IActionResult> PostAsync(CalendarDTO model)
    //{
    //    var action = await _calendarUnitOfWork.AddAsync(model);

    //    if (action.WasSuccess)
    //    {
    //        return Ok(action.Result);
    //    }

    //    return BadRequest(action.Message);
    //}

    //[HttpPut("full")]
    //public async Task<IActionResult> PustAsync(CalendarDTO model)
    //{
    //    var action = await _calendarUnitOfWork.UpdateAsync(model);

    //    if (action.WasSuccess)
    //    {
    //        return Ok(action.Result);
    //    }

    //    return BadRequest(action.Message);
    //}

    //[AllowAnonymous]
    //[HttpGet("Combo")]
    //public async Task<IActionResult> GetComboAsync()
    //{
    //    return Ok(await _calendarUnitOfWork.GetComboAsync());
    //}


    private static async Task<HashSet<DateTime>> ObtenerFestivosColombiaAsync(int año)
    {
        var festivos = new HashSet<DateTime>();

        try
        {
            var response = await httpClient.GetAsync(string.Format(ApiUrl, año));
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                // Configurar opciones para el deserializador
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters = { new DateTimeConverter() }
                };

                var festivosApi = JsonSerializer.Deserialize<List<FestivoApiResponse>>(content, options);

                foreach (var festivo in festivosApi!)
                {
                    festivos.Add(festivo.Date);
                }
            }
                //var content = await response.Content.ReadAsStringAsync();

                //var festivosApi = JsonSerializer.Deserialize<List<FestivoApiResponse>>(content);

                //foreach (var festivo in festivosApi)
                //{
                //    if (DateTime.TryParse(festivo.Date, out DateTime fechaFestivo))
                //    {
                //        festivos.Add(fechaFestivo);
                //    }
                //}
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error al obtener días festivos: {ex.Message}");
        }

        return festivos;
    }

    private class FestivoApiResponse
    {
        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("localName")]
        public string LocalName { get; set; } = null!;

        [JsonPropertyName("name")]
        public string Name { get; set; }= null!;
    }

    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
        }
    }
}
