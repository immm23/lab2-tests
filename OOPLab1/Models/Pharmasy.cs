using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.Models;
#region Attribues
[ExcludeFromCodeCoverage]
#endregion
public partial class Pharmasy
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Назва не може бути порожньою")]
    public string Name { get; set; } = null!;
    [Display(Name = "Адреса")]
    [Required(ErrorMessage = "Адреса не може бути порожньою")]
    public string Adress { get; set; } = null!;

    [Display(Name = "Номер телефону")]
    [Required(ErrorMessage = "Номер телефону не може бути порожнім")]
    [Phone]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Ім'я власника")]
    [Required(ErrorMessage = "Ім'я власника не може бути порожнім")]
    public string OwnerName { get; set; } = null!;

    [Display(Name = "Наявні ліки")]
    public virtual ICollection<Pill> Pills { get; set; } = new List<Pill>();

    [NotMapped]
    [Display(Name = "Наявні ліки")]
    public int[]? SelectedPills { get; set; }
}
