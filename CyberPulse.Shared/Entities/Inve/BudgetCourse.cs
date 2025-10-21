﻿using CyberPulse.Shared.Entities.Gene;
using CyberPulse.Shared.Resources;
using CyberPulse.Shared.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyberPulse.Shared.Entities.Inve;

[Table("BudgetCourses", Schema = "Inve")]
public class BudgetCourse
{
    public int Id { get; set; }

    [Display(Name = "BudgetLot", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int BudgetLotId { get; set; }

    [Display(Name = "Validity", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int ValidityId { get; set; }

    [Display(Name = "Course", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int CourseProgramLotId { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "StartDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime? StartDate { get; set; }

    [Column(TypeName = "datetime")]
    [Display(Name = "EndDate", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public DateTime? EndDate { get; set; }

    [Column(TypeName = "decimal(14,2)")]
    [Display(Name = "Worth", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public double Worth { get; set; }

    [Display(Name = "Statu", ResourceType = typeof(Literals))]
    [Required(ErrorMessageResourceName = "RequiredField", ErrorMessageResourceType = typeof(Literals))]
    public int StatuId { get; set; }


    public BudgetLot? BudgetLot { get; set; }
    public Validity? Validity { get; set; }
    public CourseProgramLot? CourseProgramLot { get; set; }
    public Statu? Statu { get; set; }

}
