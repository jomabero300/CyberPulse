namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProductQuotationBodyDTO
{
    public int Id { get; set; }
    public int BudgetCourseId { get; set; }
    public int ProductCurrentValueId { get; set; }
    public int RequestedQuantity 
    { 
        get=>_requestedQuantity;
        set
        {
            if(_requestedQuantity!=value)
            {
                _requestedQuantity = value;
            }
        }
    }
    public int AcceptedQuantity { get; set; }
    public double QuotedValue { get; set; }
    public double Work { get; set; }
    public int ProductId { get; set; }
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string? UnitMeasurementName { get; set; }
    public double Total => Work * RequestedQuantity;
    private int _requestedQuantity;

}