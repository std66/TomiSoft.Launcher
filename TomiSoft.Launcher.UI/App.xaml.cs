using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace TomiSoft.Launcher.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            if (e.Args.Length == 0)
            {
                MessageBox.Show(
                    messageBoxText: "No configuration file was provided in the command-line arguments.",
                    caption: "Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error
                );

                Environment.Exit(1);
            }

            if (!File.Exists(e.Args[0]))
            {
                MessageBox.Show(
                    messageBoxText: $"The configuration file does not exist: {e.Args[0]}",
                    caption: "Error",
                    button: MessageBoxButton.OK,
                    icon: MessageBoxImage.Error
                );

                Environment.Exit(1);
            }

            ServiceProvider serviceProvider = ConfigureServices(e.Args[0]).BuildServiceProvider();
            this.MainWindow = serviceProvider.GetRequiredService<MainWindow>();
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            this.MainWindow.Show();
        }

        private IServiceCollection ConfigureServices(string configFileName)
        {
            return new ServiceCollection()
                .AddSingleton(provider => new MainWindow(provider.GetRequiredService<Core.IServiceProvider>()))
                .AddSingleton<Core.IServiceProvider>(provider => new TomiSoft.Launcher.Core.ServiceProvider(configFileName));
        }
    }
}
