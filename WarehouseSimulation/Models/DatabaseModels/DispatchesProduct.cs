using System;
using System.Collections.Generic;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class DispatchesProduct
{
    public Guid DispatchId { get; set; }

    public Guid ProductId { get; set; }

    public int ProductCount { get; set; }

    public virtual Dispatch Dispatch { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
