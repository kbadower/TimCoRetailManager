using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using TRMDesktopUI.EventModels;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogInEventModel>
    {
        SalesViewModel _salesViewModel;
        private readonly IEventAggregator _eventAggregator;
        private ILoggedInUserModel _loggedInUser;
        private IAPIHelper _apiHelper;

        public event EventHandler<LoginViewModel> Login;



        public ShellViewModel(SalesViewModel salesViewModel, IEventAggregator eventAggregator, ILoggedInUserModel loggedInUser, IAPIHelper apiHelper)
        {
            _salesViewModel = salesViewModel;
            _eventAggregator = eventAggregator;
            _loggedInUser = loggedInUser;
            _apiHelper = apiHelper;
            _eventAggregator.SubscribeOnUIThread(this);
            ActivateItemAsync(IoC.Get<LoginViewModel>());
        }

        public bool IsAccountVisible
        {
            get
            {
                bool output = false;

                if(string.IsNullOrWhiteSpace(_loggedInUser.Token) == false)
                {
                    output = true;
                }

                return output;
            }
        }

        public void ExitApplication()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close TimCo Retail Manager?",
                                          "Exit application?",
                                          MessageBoxButton.YesNo,
                                          MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                TryCloseAsync();
            }
        }

        public void UserManagement()
        {
            ActivateItemAsync(IoC.Get<UserDisplayViewModel>());
        }

        public async Task LogOut()
        {
            _loggedInUser.ResetUserModel();
            _apiHelper.LogOffUser();
            await ActivateItemAsync(IoC.Get<LoginViewModel>());
            NotifyOfPropertyChange(() => IsAccountVisible);
        }

        public async Task HandleAsync(LogInEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
            NotifyOfPropertyChange(() => IsAccountVisible);
        }
    }
}
