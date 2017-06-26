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
using MvvmDialogs;
using Prism.Events;
using Smart365Operation.Modules.Dashboard.Events;
using System.Runtime.InteropServices;

namespace Smart365Operation.Modules.Dashboard.ViewModels
{
    public class AlarmTipsViewModel : BindableBase
    {
        private readonly IShellService _shellService;
        private readonly IRegionManager _regionManager;
        private readonly IMonitoringDataService _monitoringDataService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private static bool _isCreateAlarmDialog = true;
        private System.Media.SoundPlayer _soundPlayer;
        private readonly string Alarm_Sound_FilePath = @"C:\Users\Hardborn\Desktop\Warning.wav";

        //[System.Runtime.InteropServices.DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true, CharSet = CharSet.Unicode, ThrowOnUnmappableChar = true)]
        //private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);

        //[System.Flags]
        //public enum PlaySoundFlags : int
        //{
        //    SND_SYNC = 0x0000,
        //    SND_ASYNC = 0x0001,
        //    SND_NODEFAULT = 0x0002,
        //    SND_LOOP = 0x0008,
        //    SND_NOSTOP = 0x0010,
        //    SND_NOWAIT = 0x00002000,
        //    SND_FILENAME = 0x00020000,
        //    SND_RESOURCE = 0x00040004
        //}


        public AlarmTipsViewModel(IEventAggregator eventAggregator, IShellService shellService, IRegionManager regionManager, IDialogService dialogService, IMonitoringDataService monitoringDataService)
        {
            _shellService = shellService;
            _regionManager = regionManager;
            _dialogService = dialogService;
            _monitoringDataService = monitoringDataService;
            _monitoringDataService.AlarmDataUpdated += _monitoringDataService_AlarmDataUpdated;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<AlarmDialogClosedEvent>().Subscribe(arg => HandleAlarmDialogClosed(arg));
            AlarmList.CollectionChanged += AlarmList_CollectionChanged;
        }

        private void AlarmList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                var self = sender as ObservableCollection<AlarmInfo>;
                if (self == null)
                    return;
                if (self.Count == 0)
                {
                    CurrentAlarmInfo = null;
                    if (_soundPlayer != null)
                    {
                        _soundPlayer.Stop();
                    }
                }
                else
                {
                    CurrentAlarmInfo = self[0];
                }
            }
        }

        private void HandleAlarmDialogClosed(AlarmDialogClosedEventArg arg)
        {
            _isCreateAlarmDialog = true;
        }

        private void _monitoringDataService_AlarmDataUpdated(object sender, AlarmDataEventArgs e)
        {
            var alarmStr = e.Data as string;
            if (!string.IsNullOrEmpty(alarmStr))
            {
                var alarmInfo = JsonConvert.DeserializeObject<AlarmInfo>(alarmStr);
                if (alarmInfo != null)
                {
                    var action = new Action(() =>
                   {
                       _soundPlayer = new System.Media.SoundPlayer();
                       _soundPlayer.SoundLocation = Alarm_Sound_FilePath;
                       _soundPlayer.PlayLooping();
                   });
                    action.BeginInvoke(null, null);
                    
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        AlarmList.Add(alarmInfo);
                        CurrentAlarmInfo = alarmInfo;
                        IsShow = true;
                        NotifiyAlarm();
                    }));
                }
            }
        }

        private void NotifiyAlarm()
        {
            if (_isCreateAlarmDialog)
            {
                var alarmDialogViewModel = new AlarmDialogViewModel(_eventAggregator,_shellService, AlarmList, CurrentAlarmInfo);
                _dialogService.Show(this, alarmDialogViewModel);
                _isCreateAlarmDialog = false;
            }
            else
            {
                _eventAggregator.GetEvent<AlarmUpdatedEvent>().Publish(new AlarmUpdatedEventArg(CurrentAlarmInfo));
            }
        }

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        public DelegateCommand<object> DeleteAlarmCommand => new DelegateCommand<object>(DeleteAlarm, CanDeleteAlarm);

        public DelegateCommand<object> LocateAlarmCommand => new DelegateCommand<object>(LocateAlarm, CanLocateAlarm);

        public DelegateCommand OpenAlarmDialogCommand => new DelegateCommand(OpenAlarmDialog, CanOpenAlarmDialog);

        private bool CanOpenAlarmDialog()
        {
            return true;
        }

        private void OpenAlarmDialog()
        {
            if (_isCreateAlarmDialog)
            {
                var alarmDialogViewModel = new AlarmDialogViewModel(_eventAggregator,_shellService, AlarmList, CurrentAlarmInfo);
                _dialogService.Show(this, alarmDialogViewModel);
                _isCreateAlarmDialog = false;
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
            AlarmList.Remove(info);
        }

        private bool _isShow;
        public bool IsShow
        {
            get { return _isShow; }
            set { SetProperty(ref _isShow, value); }
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
