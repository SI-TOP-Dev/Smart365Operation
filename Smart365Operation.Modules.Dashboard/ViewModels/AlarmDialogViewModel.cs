using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Smart365Operation.Modules.Dashboard.Events;
using Smart365Operation.Modules.Dashboard.Views;
using Smart365Operations.Common.Infrastructure.Models;
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
        private readonly IEventAggregator _eventAggregator;
        public AlarmDialogViewModel(IEventAggregator eventAggregator, List<AlarmInfo> alarmList, AlarmInfo currentAlarm)
        {
            _eventAggregator = eventAggregator;
            AlarmList.AddRange(alarmList);
            CurrentAlarmInfo = currentAlarm;

            _eventAggregator.GetEvent<AlarmUpdatedEvent>().Subscribe(arg => AlarmUpdate(arg));
        }

        private void AlarmUpdate(AlarmUpdatedEventArg arg)
        {
            CurrentAlarmInfo = arg.Alarm;
            AlarmList.Add(arg.Alarm);
        }

        private AlarmInfo _currentAlarmInfo;
        public AlarmInfo CurrentAlarmInfo
        {
            get { return _currentAlarmInfo; }
            set { SetProperty(ref _currentAlarmInfo, value); }
        }

        private ObservableCollection<AlarmInfo> _alarmList = new ObservableCollection<AlarmInfo>();
        public ObservableCollection<AlarmInfo> AlarmList
        {
            get { return _alarmList; }
            set { SetProperty(ref _alarmList, value); }
        }

        public DelegateCommand<object> MinWindowCommand => new DelegateCommand<object>(MinWindow, CanMinWindow);

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
    }
}
