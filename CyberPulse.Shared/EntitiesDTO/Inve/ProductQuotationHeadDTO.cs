namespace CyberPulse.Shared.EntitiesDTO.Inve;

public class ProductQuotationHeadDTO
{
    public int Id { get; set; }
    public int Code { get; set; }
    public string? CourseName { get; set; }
    public string? DateStart { get; set; }
    public string? DateEnd { get; set; }
    public string? Worth { get; set; }
    public string? StatuName { get; set; }

    public List<ProductQuotationBodyDTO>? ProductQuotationBody { get; set; }
}
