using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Smart365Operations.Common.Infrastructure.Interfaces;

namespace Smart365Operation.Modules.Monitoring.ViewModels
{
    public class MonitoringViewModel:BindableBase
    {
        private readonly ICustomerService _customerService;

        public MonitoringViewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public DelegateCommand InitializeCommand => new DelegateCommand(Initialize, CanInitialize);

        private void Initialize()
        {
            
        }

        private bool CanInitialize()
        {
            return true;
        }
    }
}
