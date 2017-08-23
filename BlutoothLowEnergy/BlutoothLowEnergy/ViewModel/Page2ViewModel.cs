using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Plugin.BLE.Abstractions.Contracts;

namespace BlutoothLowEnergy.ViewModel
{
    public class Page2ViewModel: ViewModelBase
    {
        private readonly IDialogService dialogService;
        private readonly IAdapter bluetoothAdapter;
        private readonly IBluetoothLE bluetoothService;

        public Page2ViewModel(IBluetoothLE bluetoothService, IAdapter bluetoothAdapter, IDialogService dialogService)
        {
            this.dialogService = dialogService;
            this.bluetoothAdapter = bluetoothAdapter ?? throw new ArgumentNullException(nameof(bluetoothAdapter));
            this.bluetoothService = bluetoothService ?? throw new ArgumentNullException(nameof(bluetoothService));

            NavigationCommand = new RelayCommand(CheckBluetoth);

            this.Devices = new ObservableCollection<IDevice>();
        }

        public ICommand NavigationCommand { get; set; }

        public ObservableCollection<IDevice> Devices { get; }

        private bool isScaning;

        public bool IsScaning
        {
            get => isScaning;
            set => this.Set(nameof(IsScaning), ref isScaning, value);
        }


        private async void CheckBluetoth()
        {
            this.Devices.Clear();

            var state = this.bluetoothService.State;

            if (state == BluetoothState.Off)
            {
                await this.dialogService.ShowMessage("Please turn on bluetooth on your device", "Bluetooth is off");
                return;
            }

            this.IsScaning = true;
            this.bluetoothAdapter.ScanMode = ScanMode.LowPower;
            this.bluetoothAdapter.DeviceDiscovered += (sender, args) =>
            {
                Devices.Add(args.Device);
            };

            this.bluetoothAdapter.ScanTimeoutElapsed += (sender, args) =>
            {
                this.IsScaning = false;
                this.RaisePropertyChanged(nameof(this.Devices));
            };

            await this.bluetoothAdapter.StartScanningForDevicesAsync();
        }
    }
}
