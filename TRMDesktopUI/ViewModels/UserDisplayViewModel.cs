using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using TRMDesktopUILibrary.Api;
using TRMDesktopUILibrary.Models;

namespace TRMDesktopUI.ViewModels
{
    public class UserDisplayViewModel : Screen
    {

        private BindingList<UserModel> _users;
        private IUserEndpoint _userEndpoint;
        private StatusInfoViewModel _status;
        private IWindowManager _window;

        public UserDisplayViewModel(IUserEndpoint userEndpoint, StatusInfoViewModel status, IWindowManager window)
        {
            _userEndpoint = userEndpoint;
            _status = status;
            _window = window;
        }

        public BindingList<UserModel> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                NotifyOfPropertyChange(() => Users);
            }
        }


        protected override async void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            try
            {
                await LoadUsers();
            }
            catch (Exception ex)
            {
                dynamic settings = new ExpandoObject();
                settings.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                settings.ResizeMode = ResizeMode.NoResize;
                settings.Title = "System Error";

                if (ex.Message == "Unauthorized")
                {
                    _status.UpdateMessage("Permission denied.", "You do not have permission to view this page.");
                    await _window.ShowDialogAsync(_status, null, settings);
                }
                else
                {
                    _status.UpdateMessage("Fatal exception.", "Something went wrong loading the page.");
                    await _window.ShowDialogAsync(_status, null, settings);
                }

                TryCloseAsync();
            }
        }

        public async Task LoadUsers()
        {
            var users = await _userEndpoint.GetAllUsers();

            Users = new BindingList<UserModel>(users);
        }
    }
}
