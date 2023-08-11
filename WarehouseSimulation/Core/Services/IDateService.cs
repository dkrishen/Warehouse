using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseSimulation.Core.Services
{
    public interface IDateService
    {
        public DateTime CurrentDate { get; }
        public void NextDay();
    }
}
