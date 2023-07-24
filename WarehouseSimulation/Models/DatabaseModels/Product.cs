using System;
using System.Collections.Generic;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class Product
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }

    public string Sku { get; set; } = null!;

    public float Cost { get; set; }

    public virtual ICollection<DeliveriesProduct> DeliveriesProducts { get; set; } = new List<DeliveriesProduct>();

    public virtual ICollection<DispatchesProduct> DispatchesProducts { get; set; } = new List<DispatchesProduct>();

    public virtual ICollection<RacksProduct> RacksProducts { get; set; } = new List<RacksProduct>();

    public virtual ProductType Type { get; set; } = null!;
}
