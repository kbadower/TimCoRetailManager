﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
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

        private UserModel _selectedUser;

        public UserModel SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                _selectedUser = value;
                SelectedUserName = value.Email;
                UserRoles = new BindingList<string>(value.Roles.Select(x => x.Value).ToList());
                LoadAvailableRoles(); // TODO - fix not awaiting async method, this can cause issues
                NotifyOfPropertyChange(() => SelectedUser);
            }
        }

        private string _selectedUserName;

        public string SelectedUserName
        {
            get { return _selectedUserName; }
            set
            {
                _selectedUserName = value;
                NotifyOfPropertyChange(() => SelectedUserName);
            }
        }

        private BindingList<string> _userRoles = new BindingList<string>();

        public BindingList<string> UserRoles
        {
            get { return _userRoles; }
            set
            {
                _userRoles = value;
                NotifyOfPropertyChange(() => UserRoles);
            }
        }

        private BindingList<string> _availableRoles = new BindingList<string>();

        public BindingList<string> AvailableRoles
        {
            get { return _availableRoles; }
            set
            { 
                _availableRoles = value;
                NotifyOfPropertyChange(() => AvailableRoles);
            }
        }

        private string _selectedAvailableRole;

        public string SelectedAvailableRole
        {
            get { return _selectedAvailableRole; }
            set
            {
                _selectedAvailableRole = value;
                NotifyOfPropertyChange(() => SelectedAvailableRole);
            }
        }

        private string _selectedUserRole;

        public string SelectedUserRole
        {
            get { return _selectedUserRole; }
            set
            {
                _selectedUserRole = value;
                NotifyOfPropertyChange(() => SelectedUserRole);
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

        public async Task LoadAvailableRoles()
        {
            var roles = await _userEndpoint.GetAllRoles();
            foreach (var role in roles)
            {
                if (UserRoles.IndexOf(role.Value) < 0)
                {
                    AvailableRoles.Add(role.Value);
                }
            }
        }

        public async void AddSelectedRole()
        {
            await _userEndpoint.AddUserToRole(SelectedUser.Id, SelectedAvailableRole);

            UserRoles.Add(SelectedAvailableRole);
            AvailableRoles.Remove(SelectedAvailableRole);
        }

        public async void RemoveSelectedRole()
        {
            await _userEndpoint.RemoveUserFromRole(SelectedUser.Id, SelectedUserRole);

            AvailableRoles.Add(SelectedUserRole);
            UserRoles.Remove(SelectedUserRole);
        }
    }
}
