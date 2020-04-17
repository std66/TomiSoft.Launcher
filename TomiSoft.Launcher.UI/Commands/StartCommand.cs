using System;
using System.Windows.Input;
using TomiSoft.Launcher.Core;

namespace TomiSoft.Launcher.UI.Commands
{
    class StartCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private readonly IService service;

        public StartCommand(IService viewModel)
        {
            this.service = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            this.service.Start();
        }
    }
}
