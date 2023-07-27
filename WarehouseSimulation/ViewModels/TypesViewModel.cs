using System.Collections.Generic;
using System.Linq;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.Core;
using WarehouseSimulation.Data;

namespace WarehouseSimulation.ViewModels
{
    public class TypesViewModel : ViewModelBase
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

        private List<string> _AllTypes = TypeDataWorker.GetTypeNames().ToList();
        public List<string> AllTypes
        {
            get { return _AllTypes; }
            set { _AllTypes = value; OnPropertyChanged("AllTypes"); }
        }

        public string NewType { get; set; }
        public string SelectedType { get; set; }

        public RelayCommand NavigateToPreviousViewCommand { get; set; }
        public RelayCommand AddTypeCommand { get; set; }
        public RelayCommand RemoveTypeCommand { get; set; }

        public TypesViewModel(INavigationServices navService)
        {
            Navigation = navService;

            NavigateToPreviousViewCommand = new RelayCommand(o =>
            {
                Navigation.ToBack();
            }, canExecute: o => true);
            AddTypeCommand = new RelayCommand(o =>
            {
                if (NewType != null 
                    && NewType.Replace(" ", "").Length != 0
                    && TypeDataWorker.AddType(NewType.Trim()))
                {
                    AllTypes = TypeDataWorker.GetTypeNames().ToList();
                }

            }, canExecute: o => true);
            RemoveTypeCommand = new RelayCommand(o =>
            {
                if(SelectedType != null
                    && TypeDataWorker.RemoveType(SelectedType.Trim()))
                {
                    AllTypes = TypeDataWorker.GetTypeNames().ToList();
                }

            }, canExecute: o => true);
        }
    }
}
