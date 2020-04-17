# TomiSoft.Launcher
A simple service launcher

How to configure
----------------
Create a JSON file with the following structure:
```
{
  "Variables": {
    "MyVariable": "MyValue"
  },

  "Services": [
    {
      "DisplayName": "The diplay name of the service on the UI",
      "ExecutablePath": "The full path of the executable",
      "CommandLineArguments": ["define", "your", "arguments", "here"],
      "WorkingDirectory": "The working directory used during the application's lifetime",
      "EnvironmentVariables": {
        "SomeVariable": "SomeValue",
        "SomeOtherVariable": "SomeOtherValue"
      },
      "HealthMonitor": {
        "Monitor": "Monitor name",
        "Configuration": {
          "MonitorConfiguration1": "SomeValue",
          "MonitorConfiguration2": "SomeValue"
        }
      }
    }
  ]
}
```

### Using variables

You can use variables anywhere in the "Services" section using #[VariableName], but you need to define them in the "Variables" section as well. Every variable you define in "Variables" section will be prefixed with "Variables." For example:
```
{
  "Variables": {
    "ServiceDirectory": "C:\\SomeDirectory"
  },
  "Services": {
    ...
    "ExecutablePath": "#[Variables.ServiceDirectory]\\MyApplication.exe",
    ...
  }
```

### Predefined variables

There are predefined variables as well.

| Name                     | Description                                                      |
|--------------------------|------------------------------------------------------------------|
| File.ContainingDirectory | The full path of the directory containing the configuration file |

Command-Line arguments
----------------------
The application has one mandatory command-line argument, and it is the full path of the configuration file. So, you need to start the software like this:
```
TomiSoft.Launcher.exe C:\MyLauncherConfig.json
```
