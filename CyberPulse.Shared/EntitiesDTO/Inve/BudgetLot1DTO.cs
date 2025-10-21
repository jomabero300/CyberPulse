namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class BudgetLot1DTO
{
    public int Id { get; set; }
    public int BudgetProgramId { get; set; }
    public int ProgramLotId { get; set; }
    public int ValidityId { get; set; }
    public double Worth { get; set; }
    public int StatuId { get; set; }
    //public string Name { get; set; } = null!;

    public ProgramLot3DTO? ProgramLot { get; set; }
}
