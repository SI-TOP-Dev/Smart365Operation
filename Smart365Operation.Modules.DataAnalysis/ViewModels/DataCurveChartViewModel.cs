using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Regions;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operation.Modules.DataAnalysis.ViewModels
{
    public class DataCurveChartViewModel : BindableBase, INavigationAware
    {
        private readonly IDeviceParameterInfoService _deviceParameterInfoService;

        public DataCurveChartViewModel(IDeviceParameterInfoService deviceParameterInfoService)
        {
            _deviceParameterInfoService = deviceParameterInfoService;
        }

        private ObservableCollection<DeviceParameterInfoDTO> _parameterTypes = new ObservableCollection<DeviceParameterInfoDTO>();
        public ObservableCollection<DeviceParameterInfoDTO> ParameterTypes
        {
            get { return _parameterTypes; }
            set { SetProperty(ref _parameterTypes, value); }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            var equipment = navigationContext.Parameters["Equipment"] as EquipmentDTO;
            if (equipment != null)
            {
                var parameterList = _deviceParameterInfoService.GetDeviceParameterList(equipment.equipmentId.ToString());
                ParameterTypes.AddRange(parameterList);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
            //throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            //throw new NotImplementedException();
        }
    }
}
