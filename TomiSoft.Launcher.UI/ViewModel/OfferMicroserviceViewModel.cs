using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using TomiSoft.Launcher.Core;
using TomiSoft.Launcher.UI.Commands;

namespace TomiSoft.Launcher.UI.ViewModel
{
    class OfferMicroserviceViewModel : IDisposable, INotifyPropertyChanged
    {
        private readonly IService service;
        private readonly CancellationTokenSource cts = new CancellationTokenSource();

        private bool isRunning;

        public event PropertyChangedEventHandler PropertyChanged;

        public OfferMicroserviceViewModel(IService service)
        {
            Name = service.Name;
            Start = new StartCommand(service);
            Stop = new StopCommand(service);
            
            this.service = service;
            StartRefreshTask(cts.Token);
        }

        public string Name { get; set; }
        public bool IsRunning
        {
            get
            {
                return isRunning;
            }
            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRunning)));
                }
            }
        }

        public ICommand Start { get; }
        public ICommand Stop { get; }

        private void StartRefreshTask(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!disposedValue)
                {
                    IsRunning = await service.HealthMonitor.IsRunningAsync();
                    await Task.Delay(500, cancellationToken).ContinueWith(x => { });
                }
                Console.WriteLine($"Task cancelled for '{Name}'");
            }).ConfigureAwait(false);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls
        
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    cts.Cancel();
                }
                
                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OfferMicroserviceViewModel() {
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
