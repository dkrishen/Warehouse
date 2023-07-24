using System;
using System.Collections.Generic;

namespace WarehouseSimulation.Models.DatabaseModels;

public partial class Dispatch
{
    public Guid Id { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public DateTime CreationDate { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<DispatchesProduct> DispatchesProducts { get; set; } = new List<DispatchesProduct>();
}
