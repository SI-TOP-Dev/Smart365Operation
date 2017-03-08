using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operation.Modules.Dashboard.ViewModels
{
    public class AlarmTipsViewModel : BindableBase
    {
        private readonly IShellService _shellService;
        private readonly IRegionManager _regionManager;
        private readonly IMonitoringDataService _monitoringDataService;

        public AlarmTipsViewModel(IShellService shellService,IRegionManager regionManager, IMonitoringDataService monitoringDataService)
        {
            _shellService = shellService;
            _regionManager = regionManager;
            _monitoringDataService = monitoringDataService;
            _monitoringDataService.AlarmDataUpdated += _monitoringDataService_AlarmDataUpdated;
            AlarmList = new ObservableCollection<AlarmInfo>()
            {
                new AlarmInfo()
                {
                    Content = "配电室环境湿度阈值越限告警",
                    CustomerId = 1,
                    EquipmentId = 1,
                    Level = 3,
                    Time = "2017-03-07 10=46=00",
                    Type = 12
                },
                 new AlarmInfo()
                {
                    Content = "配电室环境湿度阈值越限告警",
                    CustomerId = 1,
                    EquipmentId = 1,
                    Level = 3,
                    Time = "2017-03-07 10=46=00",
                    Type = 12
                },
                  new AlarmInfo()
                {
                    Content = "配电室环境湿度阈值越限告警",
                    CustomerId = 1,
                    EquipmentId = 1,
                    Level = 3,
                    Time = "2017-03-07 10=46=00",
                    Type = 12
                },
                   new AlarmInfo()
                {
                    Content = "配电室环境湿度阈值越限告警",
                    CustomerId = 1,
                    EquipmentId = 1,
                    Level = 3,
                    Time = "2017-03-07 10=46=00",
                    Type = 12
                },
            };

            CurrentAlarmInfo = new AlarmInfo()
            {
                Content = "配电室环境湿度阈值越限告警",
                CustomerId = 1,
                EquipmentId = 1,
                Level = 3,
                Time = "2017-03-07 10:46:00",
                Type = 12
            };
        }

        private void _monitoringDataService_AlarmDataUpdated(object sender, AlarmDataEventArgs e)
        {
            var alarmStr = e.Data as string;
            if (!string.IsNullOrEmpty(alarmStr))
            {
                var alarmInfo = JsonConvert.DeserializeObject<AlarmInfo>(alarmStr);
                AlarmList.Add(alarmInfo);
                CurrentAlarmInfo = alarmInfo;
            }
        }

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        public DelegateCommand<object> DeleteAlarmCommand => new DelegateCommand<object>(DeleteAlarm, CanDeleteAlarm);

        public DelegateCommand<object> LocateAlarmCommand => new DelegateCommand<object>(LocateAlarm, CanLocateAlarm);

        private void LocateAlarm(object obj)
        {
            var info = obj as AlarmInfo;
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
            AlarmList.Remove(info);
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

        private bool CanInitialize()
        {
            return true;
        }

        private void Initialize()
        {

        }

        public IRegionManager RegionManager { get; set; }
    }
}
