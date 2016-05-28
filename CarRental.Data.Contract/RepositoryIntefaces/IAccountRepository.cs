using CarRental.Business.Entities;
using Core.Common.Contracts;

namespace CarRental.Data.Contract.RepositoryIntefaces
{
    public interface IAccountRepository:IDataRepository<Account>
    {
        Account GetByLogin(string login);
    }
}
