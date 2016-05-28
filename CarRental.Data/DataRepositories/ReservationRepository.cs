using CarRental.Business.Entities;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Data;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using CarRental.Data.Contract.DTOs;
using System;
using Core.Common.Extensions;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(IReservationRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class ReservationRepository : DataRepositoryBase<Reservation, CarRentalContext>, IReservationRepository
    {
        public IEnumerable<CustomerReservationInfo> GetCurrentCustomerReservationInfo()
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                var query = from r in context.ReservationSet
                            join a in context.AccountSet on r.AccountId equals a.AccountId
                            join c in context.CarSet on r.CarId equals c.CarId
                            select new CustomerReservationInfo()
                            {
                                Customer = a,
                                Car = c,
                                Reservation = r
                            };
                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerReservationInfo> GetCustomerOpenReservationInfo(int accountId)
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                var query = from r in context.ReservationSet
                            join a in context.AccountSet on r.AccountId equals a.AccountId
                            join c in context.CarSet on r.CarId equals c.CarId
                            where r.AccountId == accountId
                            select new CustomerReservationInfo()
                            {
                                Customer = a,
                                Car = c,
                                Reservation = r
                            };
                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<Reservation> GetReservationByPickUpDate(DateTime pickUpDate)
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                var query = from r in context.ReservationSet
                            where r.RentalDate < pickUpDate
                            select r;
                return query.ToFullyLoaded();
            }
        }

        protected override Reservation AddEntity(CarRentalContext entityContex, Reservation entity)
        {
            return entityContex.ReservationSet.Add(entity);
        }

        protected override IEnumerable<Reservation> GetEntities(CarRentalContext entityContex)
        {
            
            return from e in entityContex.ReservationSet select e;
        }

        protected override Reservation GetEntities(CarRentalContext entityContex, int id)
        {
            return (from e in entityContex.ReservationSet where e.ReservationId == id select e).FirstOrDefault();
        }

        protected override Reservation UpdateEntity(CarRentalContext entityContex, Reservation entity)
        {
            return (from e in entityContex.ReservationSet where e.ReservationId == entity.ReservationId select e).FirstOrDefault();
        }
    }
    
}
