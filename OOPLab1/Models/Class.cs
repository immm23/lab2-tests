using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace OOPLab1.Models;
#region Attribues
[ExcludeFromCodeCoverage]
#endregion
public partial class PillClass
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Display(Name = "Назва групи засобів")]
    [Required(ErrorMessage = "Назва не може бути порожньою")]
    public string Name { get; set; } = null!;
    public virtual ICollection<Pill> Pills { get; set; } = new List<Pill>();
}
