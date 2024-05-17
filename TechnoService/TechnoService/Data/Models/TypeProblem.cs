using System;
using System.Collections.Generic;

namespace TechnoService.Data.Models;

public partial class TypeProblem
{
    /// <summary>
    /// тип неисправностей
    /// </summary>
    public int IdTypeProblem { get; set; }

    public string? NameTypeProblem { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
