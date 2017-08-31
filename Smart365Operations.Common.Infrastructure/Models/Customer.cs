using System;

namespace Smart365Operations.Common.Infrastructure.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Latitude { get; set; }
        public string Name { get; set; }
        public string Longitude { get; set; }
        public string CompanyProfile { get; set; }
        public string Contacts { get; set; }
        public string ContactsPhone { get; set; }
        public string CompanyAddress { get; set; }
        public string IndustryType { get; set; }
        public double OperatingCapacity { get; set; }
        public int InstalledCapacity { get; set; }
        public int MeteringPoint { get; set; }
        public DateTime InitiationDate { get; set; }
        public DateTime ContractExpiresDate { get; set; }
        public int TransformerNumber { get; set; }
        public string CustomerPic { get; set; }

        public string Province { get; set; }
        public string City { get; set; }
        public string Agency { get; set; }
    }
}