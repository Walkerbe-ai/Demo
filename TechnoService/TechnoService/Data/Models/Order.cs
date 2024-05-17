using System;
using System.Collections.Generic;

namespace TechnoService.Data.Models;

public partial class Order
{
    public int IdApplication { get; set; }

    public DateTime? DateAddition { get; set; }

    public string? NameEquipment { get; set; }

    public int? IdTypeProblem { get; set; }

    public string? CommentApplication { get; set; }

    public string? Status { get; set; }

    public string? NameClient { get; set; }

    public decimal? Cost { get; set; }

    public DateTime? DateEnd { get; set; }

    public TimeOnly? TimeWork { get; set; }

    public string? WorkStatus { get; set; }

    /// <summary>
    /// предварительная дата завершения
    /// </summary>
    public DateTime? PeriodExecution { get; set; }

    public int? IdTypeEquipment { get; set; }

    /// <summary>
    /// серийный номер
    /// </summary>
    public int? SerialNumber { get; set; }

    public string? DescriptionApplication { get; set; }

    public virtual ICollection<CatalogOrder> CatalogOrders { get; set; } = new List<CatalogOrder>();

    public virtual TypeEquipment? IdTypeEquipmentNavigation { get; set; }

    public virtual TypeProblem? IdTypeProblemNavigation { get; set; }
}
