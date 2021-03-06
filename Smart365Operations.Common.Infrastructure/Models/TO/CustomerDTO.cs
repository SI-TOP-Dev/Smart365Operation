﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart365Operations.Common.Infrastructure.Models.TO
{
    //public class CustomerDTO
    //{
    //    public AgentDTO agent { get; set; }
    //    public DateTime contractTime { get; set; }
    //    public string customerAddress { get; set; }
    //    public int customerId { get; set; }
    //    public string customerPic { get; set; }
    //    public string customerIntroduce { get; set; }
    //    public string customerLinkman { get; set; }
    //    public string customerName { get; set; }
    //    public string customerPhone { get; set; }
    //    public CustomertypeDTO customerType { get; set; }
    //    public DateTime inTime { get; set; }
    //    public int installedCapacity { get; set; }
    //    public string latitude { get; set; }
    //    public string longitude { get; set; }
    //    public int meteringPoint { get; set; }
    //    public int operatingCapacity { get; set; }
    //    public RegionDTO region { get; set; }
    //    public int transformerNumber { get; set; }
    //}

    //public class AgentDTO
    //{
    //    public int agentId { get; set; }
    //    public string agentLinkman { get; set; }
    //    public string agentName { get; set; }
    //    public string agentPhone { get; set; }
    //}

    //public class CustomertypeDTO
    //{
    //    public int typeId { get; set; }
    //    public string typeName { get; set; }
    //}

    //public class RegionDTO
    //{
    //    public int id { get; set; }
    //    public int parentId { get; set; }
    //    public string regionName { get; set; }
    //    public int regionTypeId { get; set; }
    //}

    public class CustomerInfoWrapper
    {
        public CustomerInfoDTO root { get; set; }
    }

    public class CustomerInfoDTO
    {
        public int id { get; set; }
        public string regionName { get; set; }
        public RegionDTO[] subRegionList { get; set; }
    }

    public class RegionDTO
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string regionName { get; set; }
        public int regionTypeId { get; set; }
        public SubregionDTO[] subRegionList { get; set; }
    }

    public class SubregionDTO
    {
        public AgentDTO[] agentList { get; set; }
        public int id { get; set; }
        public int parentId { get; set; }
        public string regionName { get; set; }
        public int regionTypeId { get; set; }
    }

    public class AgentDTO
    {
        public string agentAddress { get; set; }
        public int agentId { get; set; }
        public string agentLinkman { get; set; }
        public string agentName { get; set; }
        public string agentPhone { get; set; }
        public CustomerDTO[] customerList { get; set; }
        public string inTime { get; set; }
    }

    public class CustomerDTO
    {
        public int completeInspectionCount { get; set; }
        public string contractTime { get; set; }
        public string customerAddress { get; set; }
        public int customerId { get; set; }
        public string customerIntroduce { get; set; }
        public string customerLinkman { get; set; }
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string inTime { get; set; }
        public int installedCapacity { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public int meteringPoint { get; set; }
        public int operatingCapacity { get; set; }
        public int transformerNumber { get; set; }
        public string customerPic { get; set; }
    }


}
