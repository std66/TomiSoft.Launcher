using System.Collections.Generic;

namespace TomiSoft.Launcher.Core
{
    internal class LauncherConfiguration
    {
        internal class ServiceConfiguration
        {
            internal class HealthMonitorConfiguration
            {
                public string Monitor { get; set; }
                public Dictionary<string, string> Configuration { get; set; }
            }

            public string DisplayName { get; set; }
            public string ExecutablePath { get; set; }
            public string WorkingDirectory { get; set; }
            public string[] CommandLineArguments { get; set; }
            public Dictionary<string, string> EnvironmentVariables { get; set; }
            public HealthMonitorConfiguration HealthMonitor { get; set; }
        }

        public Dictionary<string, string> Variables { get; set; }
        public List<ServiceConfiguration> Services { get; set; }
    }
}
