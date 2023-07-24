using System;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class RacksProduct
{
    public Guid RackId { get; set; }

    public Guid ProductId { get; set; }

    public int ProductCount { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Rack Rack { get; set; } = null!;
}
