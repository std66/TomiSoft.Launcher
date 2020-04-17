using System.Threading.Tasks;
using TomiSoft.Launcher.Core.HealthMonitoring;

namespace TomiSoft.Launcher.Core
{
    public interface IService
    {
        IHealthMonitor HealthMonitor { get; }
        string Name { get; }
        bool ProcessIsRunning { get; }

        Task Start();
        void Stop();
    }
}