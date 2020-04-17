using System;
using System.ComponentModel;
using System.Windows;
using TomiSoft.Launcher.UI.ViewModel;

namespace TomiSoft.Launcher.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(TomiSoft.Launcher.Core.IServiceProvider serviceProvider) : this()
        {
            this.DataContext = new MainWindowViewModel(serviceProvider);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (this.DataContext is IDisposable disposable)
                disposable.Dispose();

            base.OnClosing(e);
        }
    }
}
