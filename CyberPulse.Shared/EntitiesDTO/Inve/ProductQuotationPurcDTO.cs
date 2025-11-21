namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProductQuotationPurcDTO
{
    public string? Id { get; set; } 
    public int Code { get; set; }
    public string? Name { get; set; }
    public int RequestedQuantity { get; set; }
    public double Quoted01 { get; set; }
    public double Quoted02 { get; set; }
    public double Quoted03 { get; set; }
    public double QuotedValue { get; set; }
    public string? Statu { get; set; }
    public int ValidityId { get; set; }
}