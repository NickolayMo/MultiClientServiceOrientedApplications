using System.Runtime.Serialization;
using Core.Common.ServiceModel;
using System;

namespace CarRental.Business.DataContracts
{
    [DataContract]
    public class CustomerReservationData: DataContractBase
    {
        [DataMember]
        public int ReservationId { get; set; }
        
        [DataMember]
        public string CustomerName { get; set; }
        
        [DataMember]
        public string Car {get;set;}
        
        [DataMember]
        public DateTime DateRented{get;set;}
        
        [DataMember]
        public DateTime ReturnDate{get;set;}
    }
}