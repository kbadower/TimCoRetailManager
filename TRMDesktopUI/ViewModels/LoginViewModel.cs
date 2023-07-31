using Caliburn.Micro;
using System;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUI.Models;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
        private string _username;
        private string _password;
        private string _errorBox;
        private IAPIHelper _apiHelper;


        public LoginViewModel(IAPIHelper apiHelper)
        {
            _apiHelper = apiHelper;
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
