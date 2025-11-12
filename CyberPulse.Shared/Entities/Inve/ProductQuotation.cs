using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("ProductQuotations", Schema = "Inve")]
public class ProductQuotation
{
    public int Id { get; set; }

    [Display(Name = "BudgetCourse", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetCourseId { get; set; }

    [Display(Name = "ProductCurrentValue", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ProductCurrentValueId { get; set; }

    [Display(Name = "RequestedQuantity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int RequestedQuantity { get; set; }

    [Display(Name = "AcceptedQuantity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int AcceptedQuantity { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "QuotedValue", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double QuotedValue { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Quoted01", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Quoted01 { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Quoted02", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Quoted02 { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Quoted03", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Quoted03 { get; set; }


    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }
    public Statu? Statu { get; set; }

    public BudgetCourse? BudgetCourse { get; set; }
    public ProductCurrentValue? ProductCurrentValue { get; set; }
}