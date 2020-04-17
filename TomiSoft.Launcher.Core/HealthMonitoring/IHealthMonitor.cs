using System.Threading.Tasks;

namespace TomiSoft.Launcher.Core.HealthMonitoring
{
    public interface IHealthMonitor
    {
        Task<bool> IsRunningAsync();
    }
}