using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Data;
using WarehouseSimulation.Models.ViewModels;

namespace WarehouseSimulation.ViewModels
{
    public class RacksViewModel : ViewModelBase
    {
        private INavigationServices _Navigation;
        public INavigationServices Navigation
        {
            get => _Navigation;
            set
            {
                _Navigation = value;
                OnPropertyChanged();
            }
        }

        private List<RackViewDto> _AllRacks = RackDataWorker.GetRacks().ToList();
        public List<RackViewDto> AllRacks
        {
            get { return _AllRacks; }
            set { _AllRacks = value; OnPropertyChanged("AllRacks"); }
        }

        private List<string> _AllTypes = TypeDataWorker.GetTypeNames().ToList();
        public List<string> AllTypes
        {
            get { return _AllTypes; }
            set { _AllTypes = value; OnPropertyChanged("AllTypes"); }
        }

        public string SelectedType { get; set; }
        public RackViewDto SelectedRack { get; set; }
        public string NewRackSize { get; set; }
        public string NewRackNumber { get; set; }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public RelayCommand NavigateToTypesViewCommand { get; set; }
        public RelayCommand AddRackCommand { get; set; }
        public RelayCommand RemoveRackCommand { get; set; }

        public RacksViewModel(INavigationServices navService)
        {
            AllTypes = TypeDataWorker.GetTypeNames().ToList();

            Navigation = navService;
            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            NavigateToTypesViewCommand = new RelayCommand(o =>
            {
                Navigation.NavigateTo<TypesViewModel>();
            }, canExecute: o => true);
            AddRackCommand = new RelayCommand(o =>
            {
                try
                {
                    var newNumber = int.Parse(NewRackNumber);
                    var newSize = int.Parse(NewRackSize);

                    if(SelectedType != null 
                        && newSize > 0
                        && RackDataWorker.AddRack(new RackViewDto
                        {
                            Number = newNumber,
                            Size = newSize,
                            Type = SelectedType
                        }))
                    {
                        AllRacks = RackDataWorker.GetRacks().ToList();
                    }
                } catch { }
            }, canExecute: o => true);
            RemoveRackCommand = new RelayCommand(o =>
            {
                if (SelectedRack != null
                    && RackDataWorker.RemoveRack(SelectedRack.Number))
                {
                    AllRacks = RackDataWorker.GetRacks().ToList();
                }
            }, canExecute: o => true);
        }

        public void UpdateData()
        {
            AllRacks = RackDataWorker.GetRacks().ToList();
            AllTypes = TypeDataWorker.GetTypeNames().ToList();
        }
    }
}
