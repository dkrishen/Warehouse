using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using WarehouseSimulation.Core;
using WarehouseSimulation.Core.Services;
using WarehouseSimulation.ViewModels;
using WarehouseSimulation.Views;

namespace WarehouseSimulation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<WarehouseViewModel>();
            services.AddSingleton<DeliveryInfoViewModel>();
            services.AddSingleton<DeliveriesViewModel>();
            services.AddSingleton<DispatchInfoViewModel>();
            services.AddSingleton<DispatchesViewModel>();
            services.AddSingleton<AddDeliveryViewModel>();
            services.AddSingleton<AddDispatchViewModel>();
            services.AddSingleton<TypesViewModel>();
            services.AddSingleton<RacksViewModel>();
            services.AddSingleton<ProductsViewModel>();
            services.AddSingleton<ProductLocaionInfoViewModel>();
            services.AddSingleton<INavigationServices, NavigationServices>();
            services.AddSingleton<IDateService, DateService>();

            services.AddSingleton<Func<Type, ViewModelBase>>(serviceProvider => viewModelType => (ViewModelBase)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }
    }
}
