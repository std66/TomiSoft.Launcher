using System.Collections.Generic;

namespace TomiSoft.Launcher.Core
{
    public interface IServiceProvider
    {
        IEnumerable<IService> GetServices();
    }
}
