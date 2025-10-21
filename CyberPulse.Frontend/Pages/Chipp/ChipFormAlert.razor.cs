using CyberPulse.Shared.Entities.Chipp;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace CyberPulse.Frontend.Pages.Chipp
{
    public partial class ChipFormAlert
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = null!;

        [Parameter] public List<Chip> chips { get; set; } = new();
    }
}