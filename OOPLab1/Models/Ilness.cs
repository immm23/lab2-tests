using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.Models;
#region Attribues
[ExcludeFromCodeCoverage]
#endregion
public partial class Ilness
{
    public int Id { get; set; }

    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Назва не може бути порожньою")]
    public string Name { get; set; } = null!;

    [Display(Name = "Симптоми")]
    [Required(ErrorMessage = "Симптоми не можуть бути порожніми")]
    public string Symptoms { get; set; } = null!;

    [Display(Name = "Опис")]
    [Required(ErrorMessage = "Опис не може бути порожнім")]
    public string Description { get; set; } = null!;

    [Display(Name = "Лікарські засоби")]
    public virtual ICollection<Pill> Pills { get; } = new List<Pill>();

    [NotMapped]
    [Display(Name = "Лікарські засоби")]
    public int[]? PillsSelected { get; set; }
}
