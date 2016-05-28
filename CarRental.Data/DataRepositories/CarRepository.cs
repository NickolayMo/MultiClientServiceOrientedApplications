using CarRental.Business.Entities;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Data;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(ICarRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CarRepository : DataRepositoryBase<Car, CarRentalContext>, ICarRepository
    {
        
        protected override Car AddEntity(CarRentalContext entityContex, Car entity)
        {
            return entityContex.CarSet.Add(entity);
        }

        protected override IEnumerable<Car> GetEntities(CarRentalContext entityContex)
        {
            
            return from e in entityContex.CarSet select e;
        }

        protected override Car GetEntities(CarRentalContext entityContex, int id)
        {
            return (from e in entityContex.CarSet where e.CarId == id select e).FirstOrDefault();
        }

        protected override Car UpdateEntity(CarRentalContext entityContex, Car entity)
        {
            return (from e in entityContex.CarSet where e.CarId == entity.CarId select e).FirstOrDefault();
        }
    }
    
}
