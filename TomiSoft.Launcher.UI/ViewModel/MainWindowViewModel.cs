using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TomiSoft.Launcher.UI.Commands;

namespace TomiSoft.Launcher.UI.ViewModel
{
    class MainWindowViewModel : IDisposable
    {
        private readonly Core.IServiceProvider serviceProvider;

        public MainWindowViewModel()
        {
            this.Services = new List<OfferMicroserviceViewModel>();
        }

        public MainWindowViewModel(Core.IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            List<Core.IService> services = serviceProvider.GetServices().ToList();
            this.Services = new List<OfferMicroserviceViewModel>(services.Select(x => new OfferMicroserviceViewModel(x)));

            StartAllCommand = new StartAllCommand(services);
            StopAllCommand = new StopAllCommand(services);
        }

        public IReadOnlyList<OfferMicroserviceViewModel> Services { get; }
        public ICommand StartAllCommand { get; }
        public ICommand StopAllCommand { get; }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var service in Services)
                        service.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MainWindowViewModel() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
