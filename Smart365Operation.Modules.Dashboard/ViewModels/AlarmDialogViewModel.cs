using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using RestSharp;
using Smart365Operation.Modules.Dashboard.Events;
using Smart365Operation.Modules.Dashboard.Views;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operation.Modules.Dashboard.ViewModels
{
    public class AlarmDialogViewModel:BindableBase
    {
        private readonly IShellService _shellService;
        private readonly IEventAggregator _eventAggregator;
        public AlarmDialogViewModel(IEventAggregator eventAggregator, IShellService shellService, ObservableCollection<AlarmInfo> alarmList, AlarmInfo currentAlarm)
        {
            _eventAggregator = eventAggregator;
            _shellService = shellService;
            AlarmList = alarmList;
            CurrentAlarmInfo = currentAlarm;

            _eventAggregator.GetEvent<AlarmUpdatedEvent>().Subscribe(arg => AlarmUpdate(arg));
        }

        private void AlarmUpdate(AlarmUpdatedEventArg arg)
        {
            CurrentAlarmInfo = arg.Alarm;
            //AlarmList.Add(arg.Alarm);
        }

        private AlarmInfo _currentAlarmInfo;
        public AlarmInfo CurrentAlarmInfo
        {
            get { return _currentAlarmInfo; }
            set { SetProperty(ref _currentAlarmInfo, value); }
        }

        private ObservableCollection<AlarmInfo> _alarmList;
        public ObservableCollection<AlarmInfo> AlarmList
        {
            get { return _alarmList; }
            set { SetProperty(ref _alarmList, value); }
        }

        public DelegateCommand<object> MinWindowCommand => new DelegateCommand<object>(MinWindow, CanMinWindow);

        public DelegateCommand<object> DeleteAlarmCommand => new DelegateCommand<object>(DeleteAlarm, CanDeleteAlarm);

        public DelegateCommand<object> LocateAlarmCommand => new DelegateCommand<object>(LocateAlarm, CanLocateAlarm);

        private bool CanMinWindow(object arg)
        {
            return true;
        }

        private void MinWindow(object obj)
        {
            var window = obj as AlarmDialog;
            if (window != null)
            {
                _eventAggregator.GetEvent<AlarmDialogClosedEvent>().Publish(new AlarmDialogClosedEventArg());
                window.Close();
                _eventAggregator.GetEvent<AlarmUpdatedEvent>().Unsubscribe(AlarmUpdate);
            }
        }

        private void LocateAlarm(object obj)
        {
            var info = obj as AlarmInfo;
            if (info == null)
            {
                return;
            }
            var parameters = new NavigationParameters();
            parameters.Add("CustomerId", info.CustomerId.ToString());
            _shellService.ShowShell("Monitoring", parameters);
            //_regionManager.RequestNavigate(KnownRegionNames.MainRegion, "Monitoring", parameters);
        }

        private bool CanLocateAlarm(object obj)
        {
            return true;
        }

        private bool CanDeleteAlarm(object obj)
        {
            return true;
        }

        private void DeleteAlarm(object obj)
        {
            var info = obj as AlarmInfo;
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"data/eliminate/alarm.json", Method.GET);
            request.AddParameter("id", info.AlarmId);
            bool result = httpServiceApi.Execute(request);
            if (result)
            {
                AlarmList.Remove(info);
            }
        }
    }
}
