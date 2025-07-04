using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.Entities.Chipp;
using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.EntitiesDTO.Chipp;
using CyberPulse.Shared.Enums;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using MudBlazor;
using static MudBlazor.Colors;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipForm
{
    private EditContext editContext = null!;
    [EditorRequired, Parameter] public ChipDTO chipDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;
    private bool DisabledTypeOfTraining = true;
    private bool loading;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;
    private bool desabledCompany = false;

    public PatternMask mask1 = new PatternMask("##:##")
    {
        MaskChars = new[] { new MaskChar('#', @"[0-9]") }
    };

    private ChipProgram selectedChipProgram = new();
    private List<ChipProgram>? chipPrograms;

    private User selectedInstructor = new();
    private List<User>? instructors;

    private City selectedCity = new();
    private List<City>? cities;

    private Neighborhood selectedNeighborhood = new();
    private List<Neighborhood>? neighborhoods;

    private TrainingProgram selectedTrainingProgram = new();
    private List<TrainingProgram>? trainingPrograms;

    private TypeOfTraining selectedTypeOfTraining = new();
    private List<TypeOfTraining>? typeOfTrainings;
    private List<TypeOfTraining>? typeOfTrainingsGen;

    protected override void OnInitialized()
    {
        editContext = new(chipDTO);

    }

    protected override async Task OnInitializedAsync()
    {
        loading = true;
        await LoadChipProgramAsync();
        await LoadInstructorsync();
        await LoadCityAsync();
        await LoadTrainingProgramAsync();
        await LoadTypeOfTrainingAsync();

        if (chipDTO.Id != 0)
        {
            selectedChipProgram = chipPrograms!.SingleOrDefault(x => x.Id == chipDTO.ChipProgramId)!;
            desabledCompany = chipDTO.WingMeasure;

            selectedInstructor = instructors!.SingleOrDefault(x => x.Id == chipDTO.InstructorId)!;

            var cityId = chipDTO.NeighborhoodId.ToString();
            selectedCity = cities!.FirstOrDefault(x => x.Id == int.Parse(cityId.Substring(0, 5)))!;
            await LoadNeighborhoodAsync(int.Parse(cityId.Substring(0, 5)));

            if (cityId.Substring(5) != "000")
            {
                selectedNeighborhood = neighborhoods!.FirstOrDefault(x => x.Id == chipDTO.NeighborhoodId)!;
            }

            selectedTrainingProgram = trainingPrograms!.FirstOrDefault(x => x.Id == chipDTO.TrainingProgramId)!;
            selectedTypeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Id == chipDTO.TypeOfTrainingId)!;
        }
        else
        {
            await LoadTypeOfPoblationAsync();
            chipDTO.UserId = "XXYY";
        }

        loading = false;
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

    private async Task LoadChipProgramAsync()
    {
        var responseHttp = await repository.GetAsync<List<ChipProgram>>("/api/chipprograms/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        chipPrograms = responseHttp.Response;
    }
    private async Task<IEnumerable<ChipProgram>> SearchChipProgram(string searchText, CancellationToken cancellationToken)
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
    private void ChipProgramChanged(ChipProgram entity)
    {
        selectedChipProgram = entity;
        chipDTO.ChipProgramId = entity.Id;
        chipDTO.WingMeasure = entity.WingMeasure;
        desabledCompany = entity.WingMeasure;
        chipDTO.Company = "";
        chipDTO.StartDate = entity.StartDate;
        chipDTO.Duration = entity.Duration;
    }


    private async Task LoadInstructorsync()
    {
        var userType = UserType.inst;

        var responseHttp = await repository.GetAsync<List<User>>($"/api/accounts/LoadUsers/{userType}");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        instructors = responseHttp.Response;
    }
    private async Task<IEnumerable<User>> SearchInstructor(string searchText, CancellationToken cancellationToken)
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
    private void InstructorChanged(User entity)
    {
        selectedInstructor = entity;
        chipDTO.InstructorId = entity.Id;
    }

    private async Task LoadCityAsync()
    {
        var responseHttp = await repository.GetAsync<List<City>>("/api/cities/combo/81");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        cities = responseHttp.Response;
    }
    private async Task<IEnumerable<City>> SearchCity(string searchText, CancellationToken cancellationToken)
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
    private async Task CityChanged(City entity)
    {
        selectedCity = entity;
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
        var responseHttp = await repository.GetAsync<List<TrainingProgram>>("/api/trainingPrograms/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        trainingPrograms = responseHttp.Response;
    }
    private async Task<IEnumerable<TrainingProgram>> SearchTrainingProgram(string searchText, CancellationToken cancellationToken)
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
    private void TrainingProgramChanged(TrainingProgram entity)
    {
        selectedTrainingProgram = entity;
        chipDTO.TrainingProgramId = entity.Id;

        if (entity.Name.Contains("Titulada"))
        {
            typeOfTrainings = typeOfTrainingsGen!.Where(x => !x.Name.Contains("Ninguno")).ToList();
            chipDTO.TypeOfTrainingId = 0;
            selectedTypeOfTraining = typeOfTrainings.FirstOrDefault(x => x.Id == 0)!;
            DisabledTypeOfTraining = false;
        }
        else
        {
            typeOfTrainings = typeOfTrainingsGen;
            var typeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Name.Contains("Ninguno"));
            if (typeOfTraining != null)
            {
                selectedTypeOfTraining = typeOfTrainings!.FirstOrDefault(x => x.Id == typeOfTraining.Id)!;
                chipDTO.TypeOfTrainingId = typeOfTraining.Id;
            }
            DisabledTypeOfTraining = true;
        }
    }


    private async Task LoadTypeOfTrainingAsync()
    {
        var responseHttp = await repository.GetAsync<List<TypeOfTraining>>("/api/typeOfTrainings/combo");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        typeOfTrainingsGen = responseHttp.Response;
        typeOfTrainings = typeOfTrainingsGen!.Where(x => x.Name.Contains("Ninguno")).ToList();
    }
    private async Task<IEnumerable<TypeOfTraining>> SearchTypeOfTraining(string searchText, CancellationToken cancellationToken)
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
    private void TypeOfTrainingChanged(TypeOfTraining entity)
    {
        selectedTypeOfTraining = entity;
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

        chipDTO.TypeOfPoblationDTO= responseHttp.Response!;
    }

    private void ValidateOnBlur()
    {
        if (chipDTO.Duration == 0) return;

        var monday = chipDTO.MondayTotalHoras;
        var tuesday = chipDTO.TuesdayTotalHoras;
        var wednesday = chipDTO.WednesdayTotalHoras;
        var tursday = chipDTO.TursdayTotalHoras;
        var friday = chipDTO.FridayTotalVertas;
        var saturday = chipDTO.SaturdayTotalHoras;
        var sunday = chipDTO.SundayTotalHoras;

        if (monday == TimeSpan.Zero &&
            tuesday == TimeSpan.Zero &&
            wednesday == TimeSpan.Zero &&
            tursday == TimeSpan.Zero &&
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
            numeroDia.Add((int)DayOfWeek.Monday);
        }

        if (wednesday != TimeSpan.Zero)
        {
            var Valor = wednesday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Wednesday);
        }

        if (tursday != TimeSpan.Zero)
        {
            var Valor = tursday.ToString()[..5].Replace(':', ',');
            horas.Add(double.Parse(Valor));
            numeroDia.Add((int)DayOfWeek.Friday);
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
            for (int i = 0; i < numeroDia.Count; i++)
            {
                if ((int)dateStart.DayOfWeek == numeroDia[i])
                {
                    sumaTotal += horas[i];
                    break;
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

    private void Reset()
    {
        StateHasChanged();
    }

}