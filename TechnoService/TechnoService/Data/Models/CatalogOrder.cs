using System;
using System.Collections.Generic;

namespace TechnoService.Data.Models;

public partial class CatalogOrder
{
    public int IdCatalogOrders { get; set; }

    public int? IdOrders { get; set; }

    public int? IdEmployee { get; set; }

    public virtual Human? IdEmployeeNavigation { get; set; }

    public virtual Order? IdOrdersNavigation { get; set; }
}
