using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveCharts;
using LiveCharts.Wpf;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operation.Modules.DataAnalysis.Events;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class AnalysisParameterSettingViewModel : BindableBase, INavigationAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IDeviceParameterInfoService _deviceParameterInfoService;
        private readonly IHistoryDataService _historyDataService;

        public AnalysisParameterSettingViewModel(IEventAggregator eventAggregator, IDeviceParameterInfoService deviceParameterInfoService, IHistoryDataService historyDataService)
        {
            _eventAggregator = eventAggregator;
            _deviceParameterInfoService = deviceParameterInfoService;
            _historyDataService = historyDataService;

        }

        private ObservableCollection<DeviceParameterInfoDTO> _parameterTypes = new ObservableCollection<DeviceParameterInfoDTO>();
        public ObservableCollection<DeviceParameterInfoDTO> ParameterTypes
        {
            get { return _parameterTypes; }
            set { SetProperty(ref _parameterTypes, value); }
        }


        private EquipmentDTO _currentEquipment;
        public EquipmentDTO CurrentEquipment
        {
            get { return _currentEquipment; }
            set
            {
                SetProperty(ref _currentEquipment, value);
                QueryDataCommand.RaiseCanExecuteChanged();
            }
        }

        private DeviceParameterInfoDTO _selectedParameterType = null;
        public DeviceParameterInfoDTO SelectedParameterType
        {
            get { return _selectedParameterType; }
            set
            {
                SetProperty(ref _selectedParameterType, value);
                QueryDataCommand.RaiseCanExecuteChanged();
            }
        }

        private TimeType _selectedTimeType = TimeType.Day;
        public TimeType SelectedTimeType
        {
            get { return _selectedTimeType; }
            set
            {
                SetProperty(ref _selectedTimeType, value);
                QueryDataCommand.RaiseCanExecuteChanged();
            }
        }

        private DateTime _selectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                SetProperty(ref _selectedDate, value);
                QueryDataCommand.RaiseCanExecuteChanged();
            }
        }


        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        private bool CanInitialize()
        {
            return true;
        }

        private void Initialize()
        {

        }

        private DelegateCommand _queryDataCommand;
        public DelegateCommand QueryDataCommand
        {
            get
            {
                if (_queryDataCommand == null)
                {
                    _queryDataCommand = new DelegateCommand(QueryData, CanQueryData);
                }
                return _queryDataCommand;
            }
        }

        private void QueryData()
        {
            string selectedDateFormat = string.Empty;

            if (SelectedParameterType == null || SelectedDate == null || CurrentEquipment == null)
            {
                return;
            }

            switch (SelectedTimeType)
            {
                case TimeType.Day:
                    selectedDateFormat = "yyyy-MM-dd";
                    break;
                case TimeType.Month:
                    selectedDateFormat = "yyyy-MM";
                    break;
                case TimeType.Year:
                    selectedDateFormat = "yyyy";
                    break;
                default:
                    break;
            }

            var action = new Action(() =>
            {
                var dataList = _historyDataService.GetHistoryDataList(CurrentEquipment.equipmentId.ToString(),
                _selectedParameterType.typeId.ToString(), SelectedTimeType, SelectedDate.ToString(selectedDateFormat));
                _eventAggregator.GetEvent<HistoryDataUpdatedEvent>().Publish(new HistoryDataUpdatedEventArg(dataList, SelectedTimeType, SelectedParameterType.typeName));
            });
            action.BeginInvoke(null, null);
            
        }

        private bool CanQueryData()
        {
            var type = SelectedTimeType;
            if (SelectedParameterType == null)
            {
                return false;
            }
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var equipment = navigationContext.Parameters["Equipment"] as EquipmentDTO;
            if (equipment != null)
            {
                CurrentEquipment = equipment;
                var parameterList = _deviceParameterInfoService.GetDeviceParameterList(equipment.equipmentId.ToString());
                ParameterTypes.Clear();
                ParameterTypes.AddRange(parameterList);
                SelectedParameterType = ParameterTypes.Count == 0 ? null : ParameterTypes[0];
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }
    }
}
