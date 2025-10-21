namespace CyberPulse.Shared.EntitiesDTO.Chipp;

public class ChipProgramDTO
{
    public int Id { get; set; }

    public string Code { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public int Duration { get; set; }

    public DateTime StartDate { get; set; }

    public bool WingMeasure { get; set; }
}