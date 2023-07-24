using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WarehouseSimulation.Core;

namespace WarehouseSimulation.Services
{
    public interface INavigationServices
    {
        public ViewModelBase CurrentView { get; }
        public void NavigateTo<T>() where T : ViewModelBase;
    }
}
