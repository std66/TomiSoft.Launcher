using System;
using System.Windows.Input;
using TomiSoft.Launcher.Core;

namespace TomiSoft.Launcher.UI.Commands
{
    class StopCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly IService service;

        public StopCommand(IService viewModel)
        {
            this.service = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.service.Stop();
        }
    }
}
