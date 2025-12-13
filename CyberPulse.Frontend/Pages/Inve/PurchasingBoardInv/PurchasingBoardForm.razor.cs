using CurrieTechnologies.Razor.SweetAlert2;
using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Gene;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.PurchasingBoardInv;

public partial class PurchasingBoardForm
{
    private EditContext editContext = null!;

    private bool loading;
    private bool _disable = true;

    private bool HasRangeError = false;
    private string RangeErrorMessage = string.Empty;

    [EditorRequired, Parameter] public ProductQuotationPurcDTO ProductQuotationPurcDTO { get; set; } = null!;
    [EditorRequired, Parameter] public EventCallback OnValidSubmit { get; set; }
    [EditorRequired, Parameter] public EventCallback ReturnAction { get; set; }

    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private SweetAlertService SweetAlertService { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;


    public bool FormPostedSuccessfully { get; set; } = false;
    protected override void OnInitialized()
    {
        editContext = new(ProductQuotationPurcDTO);
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

    private void CalculateQuotedValue()
    {
        var values = new List<double>();

        if (ProductQuotationPurcDTO.Quoted01 != 0)
            values.Add(ProductQuotationPurcDTO.Quoted01);

        if (ProductQuotationPurcDTO.Quoted02 != 0)
            values.Add(ProductQuotationPurcDTO.Quoted02);

        if (ProductQuotationPurcDTO.Quoted03 != 0)
            values.Add(ProductQuotationPurcDTO.Quoted03);

        if (values.Count > 0)
        {
            var avg = values.Average();
            ProductQuotationPurcDTO.QuotedValue = avg;
            ValidateQuotedRange(avg);
        }
        else
        {
            ProductQuotationPurcDTO.QuotedValue = 0;
            HasRangeError = false;
            RangeErrorMessage = string.Empty;
        }
    }

    private void ValidateQuotedRange(double avg)
    {
        if (avg < ProductQuotationPurcDTO.PriceLow)
        {
            HasRangeError = true;
            RangeErrorMessage = Localizer["PriceLowMs"];
        }
        else if (avg > ProductQuotationPurcDTO.PriceHigh)
        {
            HasRangeError = true;
            RangeErrorMessage = Localizer["PriceHighMs"];
        }
        else
        {
            HasRangeError = false;
            RangeErrorMessage = string.Empty;
        }

        ProductQuotationPurcDTO.Estado = HasRangeError;
    }
}