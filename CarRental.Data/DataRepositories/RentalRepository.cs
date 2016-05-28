using CarRental.Business.Entities;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Data;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Core.Common.Extensions;
using CarRental.Data.Contract.DTOs;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalRepository : DataRepositoryBase<Rental, CarRentalContext>, IRentalRepository
    {
        
        protected override Rental AddEntity(CarRentalContext entityContex, Rental entity)
        {
            return entityContex.RentalSet.Add(entity);
        }

        protected override IEnumerable<Rental> GetEntities(CarRentalContext entityContex)
        {
            
            return from e in entityContex.RentalSet select e;
        }

        protected override Rental GetEntities(CarRentalContext entityContex, int id)
        {
            return (from e in entityContex.RentalSet where e.RentalId == id select e).FirstOrDefault();
        }

        protected override Rental UpdateEntity(CarRentalContext entityContex, Rental entity)
        {
            return (from e in entityContex.RentalSet where e.RentalId == entity.RentalId select e).FirstOrDefault();
        }
        public IEnumerable<Rental> GetRentalHistoryByCar(int carId)
        {
            using(CarRentalContext context = new CarRentalContext())
            {
                var query = from e in context.RentalSet
                            where e.CarId == carId
                            select e;
                return query.ToFullyLoaded();
            }
        }
        public Rental GetRentalByCar(int carId)
        {
            using(CarRentalContext context = new CarRentalContext())
            {
                var query = from e in context.RentalSet
                            where e.CarId == carId && e.DateReturned == null
                            select e;
                return query.FirstOrDefault();
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentedCars()
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                var query = from e in context.RentalSet
                            where e.DateReturned == null
                            select e;
                return query.ToFullyLoaded();
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (CarRentalContext contex = new CarRentalContext())
            {
                var query = from e in contex.RentalSet
                            where e.AccountId == accountId
                            select e;
                return query.ToFullyLoaded();
            }
        }
        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerRentalInfo()
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                var query = from r in context.RentalSet
                            where r.DateRented == null
                            join a in context.AccountSet on r.AccountId equals a.AccountId
                            join c in context.CarSet on r.CarId equals c.CarId
                            select new CustomerRentalInfo()
                            {
                                Car = c,
                                Customer = a,
                                Rental = r
                            };
                return query.ToFullyLoaded();
            }
        }


        
    }
    
}
