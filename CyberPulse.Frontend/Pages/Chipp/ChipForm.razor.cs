using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using System.Security.Claims;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipForm
{
    private EditContext editContext = null!;
    [EditorRequired, Parameter] public ChipDTO chipDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;
    //private bool DisabledTypeOfTraining = true;
    private bool loading;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;

    private bool desabledCompany = false;

    public PatternMask mask1 = new PatternMask("##:##")
    {
        MaskChars = new[] { new MaskChar('#', @"[0-9]") }
    };

    [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
    private ClaimsPrincipal? user;
    private string? userId;
    private string? userRollId;
    private int indEsta = 0;


    private ChipProgramDTO selectedChipProgram = new();
    private List<ChipProgramDTO>? chipPrograms;

    private ChipUserDTO selectedInstructor = new();
    private List<ChipUserDTO>? instructors;

    private CityDTO selectedCity = new();
    private List<CityDTO>? cities;

    private Neighborhood selectedNeighborhood = new();
    private List<Neighborhood>? neighborhoods;

    private TrainingProgramDTO selectedTrainingProgram = new();
    private List<TrainingProgramDTO>? trainingPrograms;

    private TypeOfTrainingDTO selectedTypeOfTraining = new();
    private List<TypeOfTrainingDTO>? typeOfTrainings;
    private List<TypeOfTrainingDTO>? typeOfTrainingsGen;

    private HashSet<DateTime>? Holidays;

    protected override void OnInitialized()
    {
        editContext = new(chipDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        user = authState.User;
        userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        userRollId = user.FindFirst(ClaimTypes.Role)?.Value;
        
        if (userRollId != null)
        {
            if (userRollId == "Coor") indEsta = 1;
            if (userRollId == "Inst") indEsta = 2;
            if (userRollId == "Admi") indEsta = 3;
        }

        if (userRollId == "Inst")
        {
            await LoadChipProgramAsync(chipDTO.ChipProgramId);
            await LoadInstructorsync(chipDTO.InstructorId);
            desabledCompany = true;
        }
        else
        {
            await LoadChipProgramAsync();
            await LoadInstructorsync();
        }

        await loadHolidaysAsync();
        await LoadCityAsync();
        await LoadTrainingProgramAsync();
        await LoadTypeOfTrainingAsync();

        if (chipDTO.Id != 0)
        {
            selectedChipProgram = chipPrograms!.SingleOrDefault(x => x.Id == chipDTO.ChipProgramId)!;
            chipDTO.ChipProgram = chipPrograms!.SingleOrDefault(x => x.Id == chipDTO.ChipProgramId)!;
            chipDTO.Apprentices = chipDTO.Apprentices == 1 ? 0 : chipDTO.Apprentices;
            selectedInstructor = instructors!.SingleOrDefault(x => x.Id == chipDTO.InstructorId)!;
            chipDTO.Instructor = instructors!.SingleOrDefault(x => x.Id == chipDTO.InstructorId)!;

            if (chipDTO.StatuId > 6 && chipDTO.idEsta)
            {
                selectedTrainingProgram = trainingPrograms!.FirstOrDefault(x => x.Id == chipDTO.TrainingProgramId)!;
                chipDTO.TrainingProgram = trainingPrograms!.FirstOrDefault(x => x.Id == chipDTO.TrainingProgramId)!;
                selectedTypeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Id == chipDTO.TypeOfTrainingId)!;
                chipDTO.TypeOfTraining = selectedTypeOfTraining;
                var cityId = chipDTO.NeighborhoodId.ToString();
                selectedCity = cities!.FirstOrDefault(x => x.Id == int.Parse(cityId.Substring(0, 5)))!;
                chipDTO.City = selectedCity;
                await LoadNeighborhoodAsync(int.Parse(cityId.Substring(0, 5)));

                if (cityId.Substring(5) != "000")
                {
                    selectedNeighborhood = neighborhoods!.FirstOrDefault(x => x.Id == chipDTO.NeighborhoodId)!;
                }

            }
            else
            {
                selectedCity = new CityDTO();
            }
        }
        else
        {
            await LoadTypeOfPoblationAsync();
            chipDTO.UserId = "XXYY";
            chipDTO.StatuId = await SearchIndEstaAsync("Creada", 1);
        }
        chipDTO.Language= System.Globalization.CultureInfo.CurrentCulture.Name.Substring(0, 2); 
        loading = false;
    }

    private async Task loadHolidaysAsync()
    {
        var holidays = await repository.GetAsync<HashSet<DateTime>>("/api/calendars");
        if (holidays == null) return;
        Holidays = holidays.Response;
    }

    private async Task OnBeforeInternalNavigation(LocationChangingContext context)
    {
        var formwasEditad = editContext.IsModified();

        if (!formwasEditad || FormPostedSuccessfully)
        {
            return;
        }

        var result = await SweetAlertService.FireAsync(new SweetAlertOptions
        {
            Title = Localizer["Confirmation"],
            Text = Localizer["LeaveAndLoseChanges"],
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
        });

        var confirm = !string.IsNullOrEmpty(result.Value);

        if (confirm)
        {
            return;
        }

        context.PreventNavigation();
    }

    private async Task LoadChipProgramAsync(int id = 0)
    {
        var responseHttp = await repository.GetAsync<List<ChipProgramDTO>>($"/api/chipprograms/combo?id={id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        chipPrograms = responseHttp.Response;
    }
    private async Task<IEnumerable<ChipProgramDTO>> SearchChipProgram(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return chipPrograms!;
        }

        return chipPrograms!
            .Where(x => x.Code.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) || x.Designation.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void ChipProgramChanged(ChipProgramDTO entity)
    {
        if (!desabledCompany)
        {
            selectedChipProgram = entity;
            chipDTO.ChipProgramId = entity.Id;
            chipDTO.WingMeasure = entity.WingMeasure;
            chipDTO.Company = "";
            chipDTO.StartDate = entity.StartDate;
            chipDTO.Duration = entity.Duration;
            chipDTO.ChipProgram = entity;
        }
    }


    private async Task LoadInstructorsync(string id = "")
    {
        var userType = UserType.Inst;

        var responseHttp = string.IsNullOrWhiteSpace(id) ?
            await repository.GetAsync<List<ChipUserDTO>>($"/api/accounts/LoadUsers/{userType}") :
            await repository.GetAsync<List<ChipUserDTO>>($"/api/accounts/LoadUser/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        instructors = responseHttp.Response;
    }
    private async Task<IEnumerable<ChipUserDTO>> SearchInstructor(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return instructors!;
        }

        return instructors!
            .Where(x => x.FirstName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.LastName.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) ||
                        x.DocumentId.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void InstructorChanged(ChipUserDTO entity)
    {
        selectedInstructor = entity;
        chipDTO.InstructorId = entity.Id;
        chipDTO.Instructor = entity;
    }

    private async Task LoadCityAsync()
    {
        var responseHttp = await repository.GetAsync<List<CityDTO>>("/api/cities/combo/81");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        cities = responseHttp.Response;
    }
    private async Task<IEnumerable<CityDTO>> SearchCity(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return cities!;
        }

        return cities!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private async Task CityChanged(CityDTO entity)
    {
        selectedCity = entity;
        chipDTO.City = entity;
        await LoadNeighborhoodAsync(entity.Id);
        var neighborhood = neighborhoods!.FirstOrDefault(x => x.Name.Contains(entity.Name));
        chipDTO.NeighborhoodId = neighborhood!.Id;
        selectedNeighborhood.Name = string.Empty;
    }


    private async Task LoadNeighborhoodAsync(int id)
    {
        var responseHttp = await repository.GetAsync<List<Neighborhood>>($"/api/neighborhoods/combo/{id}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        neighborhoods = responseHttp.Response;
    }
    private async Task<IEnumerable<Neighborhood>> SearchNeighborhood(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return neighborhoods!;
        }

        return neighborhoods!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void NeighborhoodChanged(Neighborhood entity)
    {
        selectedNeighborhood = entity;
        chipDTO.NeighborhoodId = entity.Id;
    }


    private async Task LoadTrainingProgramAsync()
    {
        var responseHttp = await repository.GetAsync<List<TrainingProgramDTO>>("/api/trainingPrograms/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        trainingPrograms = responseHttp.Response;
    }
    private async Task<IEnumerable<TrainingProgramDTO>> SearchTrainingProgram(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return trainingPrograms!;
        }

        return trainingPrograms!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void TrainingProgramChanged(TrainingProgramDTO entity)
    {
        selectedTrainingProgram = entity;
        chipDTO.TrainingProgram = entity;
        chipDTO.TrainingProgramId = entity.Id;

        //if (entity.Name.Contains("Titulada"))
        //{
        //    typeOfTrainings = typeOfTrainingsGen!.Where(x => !x.Name.Contains("Ninguno")).ToList();
        //    chipDTO.TypeOfTrainingId = 0;
        //    selectedTypeOfTraining = typeOfTrainings.FirstOrDefault(x => x.Id == 0)!;
        //    chipDTO.TypeOfTraining = typeOfTrainings.FirstOrDefault(x => x.Id == 0)!;
        //    DisabledTypeOfTraining = false;
        //}
        //else
        //{
        //    typeOfTrainings = typeOfTrainingsGen;
        //    var typeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Name.Contains("Ninguno"));
        //    if (typeOfTraining != null)
        //    {
        //        selectedTypeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Id == typeOfTraining.Id)!;
        //        chipDTO.TypeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Id == typeOfTraining.Id)!;
        //        chipDTO.TypeOfTrainingId = typeOfTraining.Id;
        //    }
        //    DisabledTypeOfTraining = true;
        //}

    }


    private async Task LoadTypeOfTrainingAsync()
    {
        var responseHttp = await repository.GetAsync<List<TypeOfTrainingDTO>>("/api/typeOfTrainings/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }


        typeOfTrainings= responseHttp.Response;

        //typeOfTrainingsGen = responseHttp.Response;
        //typeOfTrainings = typeOfTrainingsGen!.Where(x => x.Name.Contains("Ninguno")).ToList();
    }
    private async Task<IEnumerable<TypeOfTrainingDTO>> SearchTypeOfTraining(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return typeOfTrainings!;
        }

        return typeOfTrainings!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void TypeOfTrainingChanged(TypeOfTrainingDTO entity)
    {
        selectedTypeOfTraining = entity;
        chipDTO.TypeOfTraining = entity;
        chipDTO.TypeOfTrainingId = entity.Id;

    }

    private async Task LoadTypeOfPoblationAsync()
    {
        var responseHttp = await repository.GetAsync<List<TypeOfPoblationDTO>>("/api/TypeOfPoblations/full/'Ninguno'");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        chipDTO.TypeOfPoblationDTO = responseHttp.Response!;
    }
    private async Task<int> SearchIndEstaAsync(string name, int nivel)
    {
        var responseHttp = await repository.GetAsync<Statu>($"/api/status/full?name={name}&nivel={nivel}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return 0;
        }

        return responseHttp.Response!.Id;
    }

    private void ValidateOnBlur()
    {
        if (chipDTO.Duration == 0) return;

        var monday = chipDTO.MondayTotalHoras;
        var tuesday = chipDTO.TuesdayTotalHoras;
        var wednesday = chipDTO.WednesdayTotalHoras;
        var thursday = chipDTO.ThursdayTotalHoras;
        var friday = chipDTO.FridayTotalVertas;
        var saturday = chipDTO.SaturdayTotalHoras;
        var sunday = chipDTO.SundayTotalHoras;

        if (monday == TimeSpan.Zero &&
            tuesday == TimeSpan.Zero &&
            wednesday == TimeSpan.Zero &&
            thursday == TimeSpan.Zero &&
            friday == TimeSpan.Zero &&
            saturday == TimeSpan.Zero &&
            sunday == TimeSpan.Zero) return;

        //Fecha de ejecuacion
        var dateStart = chipDTO.StartDate;
        //Horas programadas
        int horasProgramadas = chipDTO.Duration;

        double sumaTotal = 0;

        List<double> horas = new List<double>();
        List<int> numeroDia = new List<int>();

        if (monday != TimeSpan.Zero)
        {
            var Valor = monday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Monday);
        }
        if (tuesday != TimeSpan.Zero)
        {
            var Valor = tuesday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Tuesday);
        }

        if (wednesday != TimeSpan.Zero)
        {
            var Valor = wednesday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Wednesday);
        }

        if (thursday != TimeSpan.Zero)
        {
            var Valor = thursday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Thursday);
        }

        if (friday != TimeSpan.Zero)
        {
            var Valor = friday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Friday);
        }
        if (saturday != TimeSpan.Zero)
        {
            var Valor = saturday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Saturday);
        }
        if (sunday != TimeSpan.Zero)
        {
            var Valor = sunday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Sunday);
        }


        if (numeroDia.Count == 0) return;

        while (sumaTotal < horasProgramadas)
        {
            if (!(chipDTO.Holiday && Holidays!.Contains(dateStart)))
            {
                for (int i = 0; i < numeroDia.Count; i++)
                {
                    if ((int)dateStart.DayOfWeek == numeroDia[i])
                    {
                        sumaTotal += horas[i];
                        break;
                    }
                }
            }
            if (sumaTotal < horasProgramadas)
            {
                dateStart = dateStart.AddDays(1);
            }
        }

        chipDTO.EndDate = dateStart;
        chipDTO.AlertDate = dateStart.AddDays(5);

        StateHasChanged();
    }

    private string? timeValue;

    private IEnumerable<string> ValidateTime(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            yield return "Time is required";
            yield break;
        }

        if (TimeSpan.TryParse(value, out var time))
        {
            if (time.Hours < 12 || time.Hours >= 24)
            {
                yield return "Time must be between 12:00 and 23:59";
            }
        }
        else
        {
            yield return "Invalid time format (HH:mm)";
        }
    }
}