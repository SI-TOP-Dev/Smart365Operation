using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operations.Client.Services
{

    public class CustomerService : ICustomerService
    {
        Dictionary<string, object> localCacheStoreDic = new Dictionary<String, Object>();
        public IList<Customer> GetCustomersBy(string agentId)
        {
            IList<Customer> customerList = null;
            if (localCacheStoreDic.ContainsKey(agentId))
            {
                customerList = localCacheStoreDic[agentId] as IList<Customer>;
            }
            else
            {
                var httpServiceApi = new DataServiceApi();
                var request = new RestRequest($"customer/list_by_region.json", Method.GET);
                var customerInfoWrapper = httpServiceApi.Execute<CustomerInfoWrapper>(request);
                
                customerList = new List<Customer>();
                foreach (var regionDto in customerInfoWrapper.root.subRegionList)
                {
                    foreach (var subregionDto in regionDto.subRegionList)
                    {
                        foreach (var agentDto in subregionDto.agentList)
                        {
                            foreach (var customerDto in agentDto.customerList)
                            {
                                Customer customer = new Customer()
                                {
                                    Id = customerDto.customerId,
                                    Name = customerDto.customerName,
                                    Latitude = customerDto.latitude,
                                    Longitude = customerDto.longitude,
                                    CompanyProfile = customerDto.customerIntroduce,
                                    Contacts = customerDto.customerLinkman,
                                    ContactsPhone = customerDto.customerPhone,
                                    CompanyAddress = customerDto.customerAddress,
                                    IndustryType = string.Empty,
                                    ContractExpiresDate = DateTime.Parse(customerDto.contractTime),
                                    InitiationDate = DateTime.Parse(customerDto.inTime),
                                    MeteringPoint = customerDto.meteringPoint,
                                    InstalledCapacity = customerDto.installedCapacity,
                                    OperatingCapacity = customerDto.operatingCapacity,
                                    TransformerNumber = customerDto.transformerNumber,
                                    Province = regionDto.regionName,
                                    City = subregionDto.regionName,
                                    Agency = agentDto.agentName
                                };
                                if (string.IsNullOrEmpty(customerDto.customerPic))
                                {
                                    customer.CustomerPic = "pack://application:,,,/Smart365Operation.Modules.Monitoring;component/Resources/Images/image_default.png";
                                }
                                else
                                {
                                    customer.CustomerPic = DataServiceApi.BaseUrl + "/" + customerDto.customerPic;
                                }
                                customerList.Add(customer);
                            }
                        }
                    }
                }
                localCacheStoreDic.Add(agentId, customerList);
            }
            return customerList;
        }
    }
}
