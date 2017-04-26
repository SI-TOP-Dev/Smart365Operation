using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operation.Modules.VideoMonitoring.Services;
using Smart365Operation.Modules.VideoMonitoring.ViewModels;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Prism;
using Prism.Events;

namespace Smart365Operation.Modules.VideoMonitoring
{
    public class VideoMonitoringViewModel : BindableBase, IRegionManagerAware
    {
        private readonly IRegionManager _regionManager;
        private readonly IUnityContainer _container;
        private readonly ICustomerService _customerService;
        private readonly ICameraService _cameraService;
        private readonly IEventAggregator _eventAggregator;

        public VideoMonitoringViewModel(IRegionManager regionManager, IUnityContainer container, IEventAggregator eventAggregator, ICustomerService customerService, ICameraService cameraService)
        {
            _regionManager = regionManager;
            _container = container;
            _customerService = customerService;
            _cameraService = cameraService;
            _eventAggregator = eventAggregator;
           // VideoSurveillance = container.Resolve<VideoSurveillanceViewModel>();
        }

        private ObservableCollection<CustomerViewModel> _customerList = new ObservableCollection<CustomerViewModel>();
        public ObservableCollection<CustomerViewModel> CustomerList
        {
            get { return _customerList; }
            set { SetProperty(ref _customerList, value); }
        }

     

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        private DelegateCommand<object> _playVideoCommand;

        public DelegateCommand<object> PlayVideoCommand
        {
            get
            {
                if (_playVideoCommand == null)
                {
                    _playVideoCommand = new DelegateCommand<object>(PlayVideo, CanPlayVideo);
                }
                return _playVideoCommand;
            }
        }

        private void PlayVideo(object obj)
        {
            var cameraViewModel = obj as CameraViewModel;
            if (cameraViewModel != null)
            {
                _eventAggregator.GetEvent<PubSubEvent<string>>().Publish(cameraViewModel.CameraId);
            }
                
        }

        private bool CanPlayVideo(object obj)
        {
            return true;
        }

        private bool CanInitialize()
        {
            return true;
        }

        private void Initialize()
        {
            RegionManager.RequestNavigate("VideoSurveillanceRegion", "VideoSurveillanceView");
            //InitializeDataTaskAsync();
        }

        private Task InitializeDataTaskAsync() => Task.Run(() => InitializeData());

        private void InitializeData()
        {
        

            var principal = Thread.CurrentPrincipal as SystemPrincipal;
            var agentId = principal.Identity.Id;
            var customerList = _customerService.GetCustomersBy(agentId);

            foreach (var customer in customerList)
            {
                var cameraList = _cameraService.GetCamerasBy(customer.Id);
                Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    CustomerViewModel customerViewModel = new CustomerViewModel(customer, cameraList);
                    CustomerList.Add(customerViewModel);
                }));
            }


        }

        public IRegionManager RegionManager { get; set; }
    }
}
