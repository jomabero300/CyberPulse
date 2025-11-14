using CyberPulse.Frontend.Respositories;
using CyberPulse.Shared.EntitiesDTO.Inve;
using CyberPulse.Shared.Resources;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Inve.ProductQuotationInv;

public partial class ProductQuotationCreate
{
    private ProductQuotationForm? productQuotationForm;
    private ProductQuotationHeadDTO productQuotationHeadDTO = new();
    [Inject] private IRepository Repository { get; set; } = null!;
    [Inject] private ISqlInjValRepository _sqlValidator { get; set; } = null!;
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; } = null!;
    [Inject] private IStringLocalizer<Literals> Localizer { get; set; } = null!;
    [Inject] private IRepository repository { get; set; } = null!;
    [Parameter] public int Id { get; set; }
    [Parameter] public int Code { get; set; }
    [Parameter] public string? CourseName { get; set; }
    [Parameter] public string? StartDate { get; set; }
    [Parameter] public string? EndDate { get; set; }
    [Parameter] public string? Worth { get; set; }
    [Parameter] public string? StatuName { get; set; }

    protected override void OnInitialized()
    {
        productQuotationHeadDTO.Id = Id;
        productQuotationHeadDTO.Code = Code;
        productQuotationHeadDTO.CourseName = CourseName;
        productQuotationHeadDTO.DateStart = StartDate;
        productQuotationHeadDTO.DateEnd = EndDate;
        productQuotationHeadDTO.Worth = Worth;
        productQuotationHeadDTO.StatuName = StatuName;
    }
    private async Task CreateAsync()
    {
        //if (_sqlValidator.HasSqlInjection(chipDTO!.ChipNo) ||
        //    _sqlValidator.HasSqlInjection(chipDTO!.Company) ||
        //    _sqlValidator.HasSqlInjection(chipDTO!.InstructorId) ||
        //    _sqlValidator.HasSqlInjection(chipDTO!.Justification))
        //{
        //    Snackbar.Add(Localizer["ERR010"], Severity.Error);
        //    return;
        //}
        
        if(double.Parse(productQuotationHeadDTO.Worth!)< productQuotationHeadDTO.ProductQuotationBody!.Sum(x=>x.Total))
        {
            Snackbar.Add(Localizer["ERR018"], Severity.Error);
            return;
        }
        if(productQuotationHeadDTO.ProductQuotationBody!.Sum(x => x.Total)<=0)
        {
            Snackbar.Add(Localizer["ERR019"], Severity.Error);
            return;
        }

        var responseHttp = await Repository.PostAsync("/api/productquotations/full", productQuotationHeadDTO);

        if (responseHttp.Error)
        {
            var message = await responseHttp.GetErrorMessageAsync();
            Snackbar.Add(Localizer[message!], Severity.Error);
            return;
        }

        Return();
        Snackbar.Add(Localizer["RecordCreateOk"], Severity.Success);
    }
    private void Return()
    {
        productQuotationForm!.FormPostedSuccessfully = true;
        NavigationManager.NavigateTo("/Stocktaking");
    }
}