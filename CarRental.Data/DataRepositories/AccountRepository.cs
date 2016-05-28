using CarRental.Business.Entities;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Data;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace CarRental.Data.DataRepositories
{
    [Export(typeof(IAccountRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountRepository : DataRepositoryBase<Account, CarRentalContext>, IAccountRepository
    {
        public Account GetByLogin(string login)
        {
            using (CarRentalContext context = new CarRentalContext())
            {
                return (from e
                        in context.AccountSet
                        where e.LoginEmail == login
                        select e)
                        .FirstOrDefault();
            }
        }

        protected override Account AddEntity(CarRentalContext entityContex, Account entity)
        {
            return entityContex.AccountSet.Add(entity);
        }

        protected override IEnumerable<Account> GetEntities(CarRentalContext entityContex)
        {
            
            return from e in entityContex.AccountSet select e;
        }

        protected override Account GetEntities(CarRentalContext entityContex, int id)
        {
            return (from e in entityContex.AccountSet where e.AccountId == id select e).FirstOrDefault();
        }

        protected override Account UpdateEntity(CarRentalContext entityContex, Account entity)
        {
            return (from e in entityContex.AccountSet where e.AccountId == entity.AccountId select e).FirstOrDefault();
        }
    }
    
}
