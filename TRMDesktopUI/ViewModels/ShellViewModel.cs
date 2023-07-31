using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Caliburn.Micro;
using TRMDesktopUILibrary.Enums;

namespace TRMDesktopUI.ViewModels
{
    public class ShellViewModel : Conductor<object>, IHandle<LogInEventModel>
    {
        SalesViewModel _salesViewModel;
        SimpleContainer _container;
        private readonly IEventAggregator _eventAggregator;

        public event EventHandler<LoginViewModel> Login;

        public ShellViewModel(SalesViewModel salesViewModel, IEventAggregator eventAggregator, SimpleContainer container)
        {
            _salesViewModel = salesViewModel;
            _eventAggregator = eventAggregator;
            _container = container;
            _eventAggregator.SubscribeOnUIThread(this);
            ActivateItemAsync(_container.GetInstance<LoginViewModel>());
        }

        public async Task HandleAsync(LogInEventModel message, CancellationToken cancellationToken)
        {
            await ActivateItemAsync(_salesViewModel);
        }
    }
}
