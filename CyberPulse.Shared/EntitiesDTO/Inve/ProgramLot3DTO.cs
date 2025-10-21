namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProgramLot3DTO
{
    public int Id { get; set; }
    public int ProgramId { get; set; }
    public int LotId { get; set; }
    public LotDTO? Lot { get; set; }
}
