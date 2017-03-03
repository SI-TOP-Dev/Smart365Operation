using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Smart365Operations.Common.Infrastructure.Interfaces;
using Smart365Operations.Common.Infrastructure.Models.TO;
using Smart365Operations.Common.Infrastructure.Utility;

namespace Smart365Operation.Modules.Monitoring.Services
{
    public class WiringDiagramService:IWiringDiagramService
    {
        public IList<WiringDiagramConfigDTO> GetWiringDiagramConfig(string customerId)
        {
            DataServiceApi httpServiceApi = new DataServiceApi();
            var request = new RestRequest($"customer/circuit/pic.json?customerId={customerId}", Method.GET);
            var wiringDiagramConfig = httpServiceApi.Execute<List<WiringDiagramConfigDTO>>(request);
            return wiringDiagramConfig;
        }
    }
}
