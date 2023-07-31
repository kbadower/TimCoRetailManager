using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>
    {
        LoginViewModel _loginViewModel;

        public event EventHandler<LoginViewModel> Login;

        public ShellViewModel(LoginViewModel loginViewModel, IEventAggregator eventAggregator)
        {
            _loginViewModel = loginViewModel;
            ActivateItemAsync(_loginViewModel);
        }
    }
}
