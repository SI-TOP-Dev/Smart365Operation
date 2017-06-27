using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
  
    public class CustomerEquipmentTableDTO
    {
        public List<AreaDTO> areaList { get; set; }
        public string customerAddress { get; set; }
        public int customerId { get; set; }
        public string customerLinkman { get; set; }
        public string customerName { get; set; }
        public string customerPhone { get; set; }
    }

    public class AreaDTO
    {
        public int areaId { get; set; }
        public string areaName { get; set; }
        public List<SwitchingRoomDTO> switchingRoomList { get; set; }
    }

    public class SwitchingRoomDTO
    {
        public List<EquipmentDTO> equipmentList { get; set; }
        public int roomId { get; set; }
        public string roomName { get; set; }
    }

    public class EquipmentDTO
    {
        public int equipmentId { get; set; }
        public string equipmentName { get; set; }
        public int equipmentType { get; set; }
        public int isCount { get; set; }
        public int isApparent { get; set; }
        public int isPowerFactor { get; set; }
    }

}
