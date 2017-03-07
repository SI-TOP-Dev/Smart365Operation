using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Prism;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataAnaysisViewModel : BindableBase, IRegionManagerAware
    {
        private readonly ICustomerService _customerService;
        private readonly ICustomerEquipmentService _customerEquipmentService;

        public DataAnaysisViewModel(ICustomerService customerService, ICustomerEquipmentService customerEquipmentService)
        {
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
            var parameters = new NavigationParameters();
            parameters.Add("Equipment", obj);
            RegionManager.RequestNavigate("DataAnaysisRegion", "DataCurveChart", parameters);
        }

        private bool CanInitialize()
        {
            return true;
        }

        private void Initialize()
        {
            
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

        private void GetEquipmentTableListTaskAsync(string s) => Task.Run(() => EquipmentTableList = new ObservableCollection<CustomerEquipmentTableDTO>(GetEquipmentTableList(s)));

    }
}
