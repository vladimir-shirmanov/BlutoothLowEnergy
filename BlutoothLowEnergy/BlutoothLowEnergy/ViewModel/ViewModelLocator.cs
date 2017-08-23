using BlutoothLowEnergy.Services;
using BlutoothLowEnergy.Views;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;

namespace BlutoothLowEnergy.ViewModel
{
    public class ViewModelLocator
    {
        public const string Page1_Name = nameof(Page1);
        public const string Page2_Name = nameof(Page2);

        public ViewModelLocator()
        {
            var nav = new NavigationService();
            nav.Configure(Page1_Name, typeof(Page1));
            nav.Configure(Page2_Name, typeof(Page2));
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<Page1ViewModel>();
            SimpleIoc.Default.Register<Page2ViewModel>();
            SimpleIoc.Default.Register<INavigationService>(() => nav);
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register(() => CrossBluetoothLE.Current);
            SimpleIoc.Default.Register(() => CrossBluetoothLE.Current.Adapter);
        }

        public INavigationService NavigationService
        {
            get => ServiceLocator.Current.GetInstance<INavigationService>();
        }

        public Page1ViewModel Page1
        {
            get => ServiceLocator.Current.GetInstance<Page1ViewModel>();
        }
        
        public Page2ViewModel Page2
        {
            get => ServiceLocator.Current.GetInstance<Page2ViewModel>(); 
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}