using System;
using System.Collections.Generic;
using System.Windows.Input;
using TomiSoft.Launcher.Core;

namespace TomiSoft.Launcher.UI.Commands
{
    class StopAllCommand : ICommand
    {
        private readonly IReadOnlyList<IService> services;

        public event EventHandler CanExecuteChanged;

        public StopAllCommand(IReadOnlyList<IService> services)
        {
            this.services = services;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            foreach (var service in services)
            {
                service.Stop();
            }
        }
    }
}
