using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TomiSoft.Launcher.Core.HealthMonitoring;

namespace TomiSoft.Launcher.Core
{
    public class ServiceProvider : IServiceProvider
    {
        private readonly string launcherConfigFileName;
        private readonly Regex variableRegex = new Regex(@"#\[(?<variable>[\w\.]+)\]");

        public ServiceProvider(string launcherConfigFileName)
        {
            this.launcherConfigFileName = launcherConfigFileName;
        }

        public IEnumerable<IService> GetServices()
        {
            string json = File.ReadAllText(launcherConfigFileName);
            LauncherConfiguration config = JsonConvert.DeserializeObject<LauncherConfiguration>(json);

            IReadOnlyDictionary<string, string> variables = GetVariables(config);

            foreach (LauncherConfiguration.ServiceConfiguration service in config.Services)
            {
                yield return GetService(service, variables);
            }
        }

        private IReadOnlyDictionary<string, string> GetVariables(LauncherConfiguration config)
        {
            Dictionary<string, string> result = new Dictionary<string, string>()
            {
                ["File.ContainingDirectory"] = Path.GetDirectoryName(this.launcherConfigFileName)
            };

            foreach (var item in config.Variables)
            {
                result.Add($"Variables.{item.Key}", item.Value);
            }

            return result;
        }

        private Service GetService(LauncherConfiguration.ServiceConfiguration config, IReadOnlyDictionary<string, string> variables)
        {
            return new Service(
                name: config.DisplayName,
                executablePath: ReplaceVariables(config.ExecutablePath, variables),
                arguments: config.CommandLineArguments.Select(x => ReplaceVariables(x, variables)).ToArray(),
                workingDirectory: ReplaceVariables(config.WorkingDirectory, variables),
                environmentVariables: config.EnvironmentVariables.Select(x => new {Key = x.Key, Value = ReplaceVariables(x.Value, variables)}).ToDictionary(x => x.Key, x => x.Value),
                healthChecker: GetHealthMonitor(config.HealthMonitor, variables)
            );
        }

        private IHealthMonitor GetHealthMonitor(LauncherConfiguration.ServiceConfiguration.HealthMonitorConfiguration config, IReadOnlyDictionary<string, string> variables)
        {
            if (config == null)
                return null;

            switch (config.Monitor)
            {
                case "RestApiHealthMonitor":
                    return new RestApiHealthMonitor(new Uri(ReplaceVariables(config.Configuration["Url"], variables)));

                default:
                    return null;
            }
        }

        private string ReplaceVariables(string input, IReadOnlyDictionary<string, string> variables)
        {
            HashSet<string> variablesFound = new HashSet<string>();

            MatchCollection m = variableRegex.Matches(input);
            if (m.Count == 0)
                return input;

            foreach (Match match in m)
            {
                variablesFound.Add(match.Groups["variable"].Value);
            }

            foreach (string variableToReplace in variablesFound)
            {
                string replacement = string.Empty;

                if (variables.ContainsKey(variableToReplace))
                    replacement = variables[variableToReplace];

                input = input.Replace($"#[{variableToReplace}]", replacement);
            }

            return input;
        }
    }
}
