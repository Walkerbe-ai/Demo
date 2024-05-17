using System;
using System.Collections.Generic;

namespace TechnoService.Data.Models;

public partial class TypeEquipment
{
    public int IdTypeEquipment { get; set; }

    public string? NameTypeEquipment { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
