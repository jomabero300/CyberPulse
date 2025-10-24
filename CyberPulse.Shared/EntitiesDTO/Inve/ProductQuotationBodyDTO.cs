namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProductQuotationBodyDTO
{
    public int Id { get; set; }
    public int BudgetCourseId { get; set; }
    public int ProductCurrentValueId { get; set; }
    public int RequestedQuantity { get; set; }
    public int AcceptedQuantity { get; set; }
    public int QuotedValue { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? UnitMeasurementName { get; set; }
    public int ProductId { get; set; }
}