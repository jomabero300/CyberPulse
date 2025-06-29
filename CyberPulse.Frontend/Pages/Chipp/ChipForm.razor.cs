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
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipForm
{
    private EditContext editContext = null!;
    [EditorRequired, Parameter] public ChipDTO chipDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;

    private bool desabledCompany = false;

    private ChipProgram selectedChipProgram = new();
    private List<ChipProgram>? chipPrograms;

    private User selectedInstructor = new();
    private List<User>? instructors;

    private City selectedCity = new();
    private List<City>? cities;

    private Neighborhood selectedNeighborhood = new();
    private List<Neighborhood>? neighborhoods;

    private TrainingProgram selectedTrainingProgram = new();
    private List<TrainingProgram>? trainingProgram;

    private TypeOfTraining selectedTypeOfTraining=new();
    private List<TypeOfTraining>? typeOfTraining;

    protected override void OnInitialized()
    {
        editContext = new(chipDTO);
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadChipProgramAsync();
        await LoadInstructorsync();
        await LoadCityAsync();
        await LoadTrainingProgramAsync();
        await LoadTypeOfTrainingAsync();
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
        selectedNeighborhood.Id = 0;
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

        trainingProgram = responseHttp.Response;
    }
    private async Task<IEnumerable<TrainingProgram>> SearchTrainingProgram(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return trainingProgram!;
        }

        return trainingProgram!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase) )
            .ToList();
    }
    private void TrainingProgramChanged(TrainingProgram entity)
    {
        selectedTrainingProgram = entity;
        chipDTO.TrainingProgramId = entity.Id;
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

        typeOfTraining = responseHttp.Response;
    }
    private async Task<IEnumerable<TypeOfTraining>> SearchTypeOfTraining(string searchText, CancellationToken cancellationToken)
    {
        await Task.Delay(5);
        if (string.IsNullOrWhiteSpace(searchText))
        {
            return typeOfTraining!;
        }

        return typeOfTraining!
            .Where(x => x.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase))
            .ToList();
    }
    private void TypeOfTrainingChanged(TypeOfTraining entity)
    {
        selectedTypeOfTraining = entity;
        chipDTO.TypeOfTrainingId = entity.Id;
    }

}