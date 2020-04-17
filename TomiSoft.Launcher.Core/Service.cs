using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using TomiSoft.Launcher.Core.HealthMonitoring;

namespace TomiSoft.Launcher.Core
{
    public class Service : IService
    {
        private readonly ProcessStartInfo startInfo;
        private Process process;

        public Service(string name, string executablePath, string[] arguments, string workingDirectory, IReadOnlyDictionary<string, string> environmentVariables, IHealthMonitor healthChecker)
        {
            Name = name;
            Arguments = arguments;
            EnvironmentVariables = environmentVariables;
            HealthMonitor = healthChecker;

            startInfo = new ProcessStartInfo(executablePath)
            {
                WorkingDirectory = workingDirectory,
                Arguments = string.Join(" ", arguments),
                UseShellExecute = false
            };

            foreach (var envVar in environmentVariables)
            {
                startInfo.Environment.Add(envVar);
            }
        }

        public string Name { get; }
        public IReadOnlyList<string> Arguments { get; }
        public IReadOnlyDictionary<string, string> EnvironmentVariables { get; }
        public IHealthMonitor HealthMonitor { get; }

        public bool ProcessIsRunning
        {
            get
            {
                if (process != null)
                {
                    try
                    {
                        int id = process.Id;
                        if (id != 0)
                            return true;
                    }
                    catch (InvalidOperationException)
                    {
                        //do nothing
                    }
                }

                return false;
            }
        }

        public async Task Start()
        {
            if (!ProcessIsRunning || !await HealthMonitor.IsRunningAsync())
            {
                process = new Process()
                {
                    StartInfo = startInfo
                };

                process.Start();
            }
        }

        public void Stop()
        {
            try
            {
                process.Kill();
            }
            catch
            {
                //do nothing
            }
        }
    }
}
