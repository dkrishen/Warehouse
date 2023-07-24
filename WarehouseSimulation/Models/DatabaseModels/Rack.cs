using System;
using System.Collections.Generic;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class Rack
{
    public Guid Id { get; set; }

    public Guid TypeId { get; set; }

    public int Size { get; set; }

    public int Number { get; set; }

    public virtual ICollection<RacksProduct> RacksProducts { get; set; } = new List<RacksProduct>();

    public virtual ProductType Type { get; set; } = null!;
}
