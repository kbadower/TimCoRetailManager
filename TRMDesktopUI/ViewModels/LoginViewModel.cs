using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using TRMDesktopUI.EventModels;
using TRMDesktopUI.Models;
using TRMDesktopUILibrary.Api;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username = "kbadower@gmail.com";
        private string _password = "Pwd12345.";
        private string _errorBox;
        private IAPIHelper _apiHelper;
        private readonly IEventAggregator _eventAggregator;

        public LoginViewModel(IAPIHelper apiHelper, IEventAggregator eventAggregator)
        {
            _apiHelper = apiHelper;
            _eventAggregator = eventAggregator;
        }

        public string UserName
        {
            get { return _username; }
            set 
            {
                _username = value;
                NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string Password
        {
            get { return _password; }
            set 
            { 
                _password = value;
                NotifyOfPropertyChange(() => Password);
                NotifyOfPropertyChange(() => CanLogIn);
            }
        }

        public string ErrorBox
        {
            get { return _errorBox; }
            set
            {
                _errorBox = value;
                NotifyOfPropertyChange(() => ErrorBox);
            }
        }

        public bool CanLogIn
        {
            get
            {
                bool output = false;

                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }
        }

        public async Task LogIn()
        {
            ErrorBox = string.Empty;

            try
            {
                AuthenticatedUserModel result = await _apiHelper.Authenticate(UserName, Password);
                await _apiHelper.GetLoggedInUserInfo(result.Access_Token);
                await _eventAggregator.PublishOnUIThreadAsync(new LogInEventModel());
            }
            catch (Exception ex)
            {
                ErrorBox = "Your credentials are wrong!";
                Password = string.Empty;
                Console.WriteLine(ex.Message);
            }
        }

    }
}
