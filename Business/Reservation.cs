using System;
using Core.Common.Contracts;
using Core.Common.Core;
using System.Runtime.Serialization;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Reservation : EntityBase, IIdentifiableEntity, IAccountOwnerEntity
    {
        [DataMember]
        public int AccountId { get; set; }
        [DataMember]
        public int ReservationId { get; set; }
        [DataMember]
        public int CarId { get; set; }
        [DataMember]
        public DateTime RentalDate { get; set; }
        [DataMember]
        public DateTime ReturnDate { get; set; }

        public int EntityId
        {
            get
            {
                return ReservationId;
            }

            set
            {
                ReservationId = value;
            }
        }

        public int OwnerAccountId
        {
            get
            {
                return AccountId;
            }
        }
    }
}
