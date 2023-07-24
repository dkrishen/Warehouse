using System;
using System.Collections.Generic;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class DeliveriesProduct
{
    public Guid DeliveryId { get; set; }

    public Guid ProductId { get; set; }

    public int ProductCount { get; set; }

    public virtual Delivery Delivery { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
