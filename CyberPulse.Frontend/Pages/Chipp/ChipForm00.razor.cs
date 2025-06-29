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
using MudBlazor;
using System.Xml.Linq;

namespace CyberPulse.Frontend.Pages.Chipp;

public partial class ChipForm00
{
    private EditContext editContext = null!;
    private bool desabledCompany = false;

    private ChipProgram selectedChipProgram = new();
    private List<ChipProgram>? chipPrograms;

    private City selectedCity = new();
    private List<City>? cities;

    private Neighborhood selectedNeighborhood = new();
    private List<Neighborhood>? neighborhoods;

    private User selectedInstructor = new();
    private List<User>? instructors;

    private List<TypeOfPoblationDTO>? typeOfPoblationDTOs=new List<TypeOfPoblationDTO>();

    int spacing;

    public PatternMask mask1 = new PatternMask("##:##")
    {
        MaskChars = new[] { new MaskChar('#', @"[0-9]") }
    };

    protected override void OnInitialized()
    {
        editContext = new(chipDTO);
    }
    [EditorRequired, Parameter] public ChipDTO chipDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    public bool FormPostedSuccessfully { get; set; } = false;

    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    protected override async Task OnInitializedAsync()
    {
        await LoadChipProgramAsync();
        await LoadCityAsync();
        await LoadInstructorsync();
        //await LoadChipPoblationAsync();
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


    private async Task LoadChipPoblationAsync()
    {
        var responseHttp = await repository.GetAsync<List<TypeOfPoblation>>("/api/typeofpoblations");

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            await SweetAlertService.FireAsync("Error", message, SweetAlertIcon.Error);
            return;
        }

        foreach (var item in responseHttp.Response!)
        {
            var typeofPoblacionDto=new TypeOfPoblationDTO { Id = item.Id, Name = item.Name,Value=0 };

            typeOfPoblationDTOs!.Add(typeofPoblacionDto);
        }
    }


//    private void OnValueChanged()
//    {
//        var x = chipDTO.MondayTotalHoras +
//                chipDTO.TuesdayTotalHoras +
//                chipDTO.WednesdayTotalHoras +
//                chipDTO.FridayTotalHoras +
//                chipDTO.SaturdayTotalHoras +
//                chipDTO.SundayTotalHoras;


//        if (x == TimeSpan.Zero) return;

//        //Fecha de ejecuacion
//        var dateStart = chipDTO.StartDate;
//        //Horas programadas
//        int horasProgramadas = chipDTO.Duration;
//        double sumaTotal = 0;



//        var monday = chipDTO.MondayTotalHoras;
//        var tuesday = chipDTO.TuesdayTotalHoras;
//        var wednesday = chipDTO.WednesdayTotalHoras;
//        var friday = chipDTO.FridayTotalHoras;
//        var saturday = chipDTO.SaturdayTotalHoras;
//        var sunday = chipDTO.SundayTotalHoras;

//        List<double> horas = new List<double>();
//        List<int> numeroDia = new List<int>();


//        if (monday != TimeSpan.Zero)
//        {
//            var Valor = monday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Monday);
//        }
//        if (tuesday != TimeSpan.Zero)
//        {
//            var Valor = tuesday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Tuesday);
//        }

//        if (wednesday != TimeSpan.Zero)
//        {
//            var Valor = wednesday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Wednesday);
//        }
//        if (friday != TimeSpan.Zero)
//        {
//            var Valor = friday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Friday);
//        }
//        if (saturday != TimeSpan.Zero)
//        {
//            var Valor = saturday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Saturday);
//        }
//        if (sunday != TimeSpan.Zero)
//        {
//            var Valor = sunday.ToString()[..5].Replace(':', ',');
//            horas.Add(double.Parse(Valor));
//            numeroDia.Add((int)DayOfWeek.Sunday);
//        }


//        while (sumaTotal < horasProgramadas)
//        {
//            for (int i = 0; i < numeroDia.Count; i++)
//            {
//                if ((int)dateStart.DayOfWeek == numeroDia[i])
//                {
//                    sumaTotal += horas[i];
//                    break;
//                }
//            }

//            if (sumaTotal < horasProgramadas)
//            {
//                dateStart = dateStart.AddDays(1);
//            }
//        }

//        chipDTO.EndDate = dateStart;
////        StateHasChanged();
//    }

    private void OnValueChanged(TypeOfPoblationDTO changeditem, int? newValue)
    {
        //actualiza el valor
        changeditem.Value = newValue!.Value;

        var noneItem = typeOfPoblationDTOs!.FirstOrDefault(i => i.Name == "Ninguno");

        if (changeditem.Name == "Ninguno")
        {
            if(newValue>0)
            {
                //Resetea otras opciones
                foreach (var item in typeOfPoblationDTOs!.Where(x=>x.Name != "Ninguno"))
                {
                    item.Value = 0;
                }
            }
        }
        else
        {
            if(newValue>0)
            {
                //Resetea ninguno si se activa otras opciones
                if (noneItem != null) noneItem.Value = 0;
            }
        }
        StateHasChanged();
    }

    private bool GetDisableStatus(TypeOfPoblationDTO item)
    {
        var noneItem = typeOfPoblationDTOs!.FirstOrDefault(i => i.Name == "Ninguno");

        bool HasActivateOption = typeOfPoblationDTOs!.Any(i => i.Name != "Ninguno" && i.Value > 0);

        if (item.Name=="Ninguno")
        {
            //Deshabilitar ninguno si hay opciones activo
            return HasActivateOption;
        }
        else
        {
            return noneItem?.Value > 0;
        }

    }

    private string ValidtePositiveInteger(int?value)
    {
        if (value == null) return "Debes ingresar un valor";
        if (value < 0) return "Debe ser mayor que cero";
        return null;
    }
}