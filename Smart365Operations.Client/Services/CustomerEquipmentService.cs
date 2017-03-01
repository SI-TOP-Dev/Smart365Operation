using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;
using RestSharp;

namespace Smart365Operations.Client.Services
{
    public class CustomerEquipmentService: ICustomerEquipmentService
    {
        public CustomerEquipmentTableDTO GetCustomerEquipmentTable(string customerId)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/equipmentlist.json?customerId={customerId}", Method.GET);
            var infoTable = httpServiceApi.Execute<CustomerEquipmentTableDTO>(request);
            return infoTable;
        }
    }
}
