using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;

namespace BlutoothLowEnergy.ViewModel
{
    public class Page1ViewModel: ViewModelBase
    {
        private readonly INavigationService navigationService;

        public Page1ViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

            NavigateCommand = new RelayCommand(() => this.navigationService.NavigateTo(ViewModelLocator.Page2_Name));
        }

        public ICommand NavigateCommand { get; set; }
    }
}
