using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using System;

namespace CarRental.Business.DataContracts
{
    [DataContract]
    public class CustomerRentalData: DataContractBase
    {
        [DataMember]
        public int CarRentalId { get; set; }
        
        [DataMember]
        public string CustomerName { get; set; }
        
        [DataMember]
        public string Car {get;set;}
        
        [DataMember]
        public DateTime DateRented{get;set;}
        
        [DataMember]
        public DateTime? ExpectedReturn{get;set;}
    }
}