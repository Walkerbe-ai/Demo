using System;
using System.Collections.Generic;

namespace TechnoService.Data.Models;

public partial class Human
{
    public int IdUser { get; set; }

    public string? Login { get; set; }

    public string? PasswordUser { get; set; }

    public string? FullnameUser { get; set; }

    public string? RoleUser { get; set; }

    public virtual ICollection<CatalogOrder> CatalogOrders { get; set; } = new List<CatalogOrder>();
}
