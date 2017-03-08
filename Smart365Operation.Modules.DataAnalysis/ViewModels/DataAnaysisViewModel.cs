using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operation.Modules.DataAnalysis.Events;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataAnaysisViewModel : BindableBase, IRegionManagerAware
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ICustomerService _customerService;
        private readonly ICustomerEquipmentService _customerEquipmentService;

        public DataAnaysisViewModel(IEventAggregator eventAggregator, ICustomerService customerService, ICustomerEquipmentService customerEquipmentService)
        {
            _eventAggregator = eventAggregator;
            _customerService = customerService;
            _customerEquipmentService = customerEquipmentService;

            var principal = Thread.CurrentPrincipal as SystemPrincipal;
            var agentId = principal.Identity.Id;
            GetEquipmentTableListTaskAsync(agentId);
        }
        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);
        public DelegateCommand<object> SelectEquipmentCommand => new DelegateCommand<object>(SelectEquipment, CanSelectEquipment);

        private bool CanSelectEquipment(object arg)
        {
            return true;
        }

        private void SelectEquipment(object obj)
        {
            SelectedEquipment = obj as EquipmentDTO;
            var parameters = new NavigationParameters();
            parameters.Add("Equipment", SelectedEquipment);
            RegionManager.RequestNavigate("AnalysisSettingRegion", "AnalysisParameterSetting", parameters);
        }

        private bool CanInitialize()
        {
            return true;
        }

        private void Initialize()
        {

            RegionManager.RequestNavigate("DataChartRegion", "DataCurveChart");
        }

        public IRegionManager RegionManager { get; set; }

        private List<CustomerEquipmentTableDTO> GetEquipmentTableList(string agentId)
        {
            var equipmentTableList = new List<CustomerEquipmentTableDTO>();
            var customerList = _customerService.GetCustomersBy(agentId);
            foreach (var customer in customerList)
            {
                var equipmentTable = _customerEquipmentService.GetCustomerEquipmentTable(customer.Id.ToString());
                equipmentTableList.Add(equipmentTable);
            }
            return equipmentTableList;
        }

        private ObservableCollection<CustomerEquipmentTableDTO> _equipmentTableList = new ObservableCollection<CustomerEquipmentTableDTO>();
        public ObservableCollection<CustomerEquipmentTableDTO> EquipmentTableList
        {
            get { return _equipmentTableList; }
            set { SetProperty(ref _equipmentTableList, value); }
        }

        private EquipmentDTO _selectedEquipment;
        public EquipmentDTO SelectedEquipment
        {
            get { return _selectedEquipment; }
            set
            {
                if (value != _selectedEquipment)
                {
                    _eventAggregator.GetEvent<SelectedEquipmentChangedEvent>()
                        .Publish(new SelectedEquipmentChangedEventArg());
                }
                SetProperty(ref _selectedEquipment, value);
            }
        }

        private void GetEquipmentTableListTaskAsync(string s) => Task.Run(() => EquipmentTableList = new ObservableCollection<CustomerEquipmentTableDTO>(GetEquipmentTableList(s)));

    }
}
