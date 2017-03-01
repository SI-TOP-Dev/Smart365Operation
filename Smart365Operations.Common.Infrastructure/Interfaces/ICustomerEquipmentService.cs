using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smart365Operations.Common.Infrastructure.Models.TO;

namespace Smart365Operations.Common.Infrastructure.Interfaces
{
   public interface ICustomerEquipmentService
   {
       CustomerEquipmentTableDTO GetCustomerEquipmentTable(string customerId);
   }
}
