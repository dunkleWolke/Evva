using Evva.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Evva.Commands
{
    public class UpdateViewCommand : ICommand
    {
        private static LogInViewModel logInViewModel;


        public UpdateViewCommand(LogInViewModel viewModel)
        {
            logInViewModel = viewModel;
        }

        public UpdateViewCommand(LogInPageViewModel viewModel)
        {
            
        }
        public UpdateViewCommand(RegisterPageViewModel viewModel)
        {

        }


        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter.ToString() == "LogInPage")
            {
                logInViewModel.SelectedViewModel = new LogInPageViewModel();
            }
            else if (parameter.ToString() == "RegisterPage")
            {
                logInViewModel.SelectedViewModel = new RegisterPageViewModel();
            }
        }
    }
}
