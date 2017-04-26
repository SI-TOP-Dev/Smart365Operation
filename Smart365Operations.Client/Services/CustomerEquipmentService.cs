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
        Dictionary<string, object> localCacheStoreDic = new Dictionary<String, Object>();
        private static object _lockObject = new object();
        public CustomerEquipmentTableDTO GetCustomerEquipmentTable(string customerId)
        {
            lock (_lockObject)
            {
                CustomerEquipmentTableDTO equipmentTable = null;
                if (localCacheStoreDic.ContainsKey(customerId))
                {
                    equipmentTable = localCacheStoreDic[customerId] as CustomerEquipmentTableDTO;
                }
                else
                {
                    DataServiceApi httpServiceApi = new DataServiceApi();
                    var request = new RestRequest($"customer/equipmentlist.json?customerId={customerId}", Method.GET);
                    equipmentTable = httpServiceApi.Execute<CustomerEquipmentTableDTO>(request);
                    if (equipmentTable != null)
                    {
                        localCacheStoreDic.Add(customerId, equipmentTable);
                    }
                }
                return equipmentTable;
            }      
                       
        }
    }
}
