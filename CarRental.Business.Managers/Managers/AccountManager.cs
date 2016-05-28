using CarRental.Business.Contracts.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using System.ServiceModel;
using Core.Common.Contracts;
using System.ComponentModel.Composition;
using Core.Common.Core;
using System.Security.Permissions;
using CarRental.Common;
using CarRental.Data.Contract.RepositoryIntefaces;
using Core.Common.Exceptions;

namespace CarRental.Business.Managers.Managers
{
    [ServiceBehavior(
        ConcurrencyMode = ConcurrencyMode.Multiple, 
        InstanceContextMode = InstanceContextMode.PerCall, 
        ReleaseServiceInstanceOnTransactionComplete = false
        )]
    public class AccountManager : ManagerBase, IAccountService
    {
        [Import]
        private IDataRepositoryFactory _DataRepositoryFactory;

        public AccountManager()
        {
            ObjectBase.Container.SatisfyImportsOnce(this);
        }

        public AccountManager(IDataRepositoryFactory repositoryFactory)
        {
            _DataRepositoryFactory = repositoryFactory;
        }

        protected override Account LoadAutarizationValidationAccount(string loginName)
        {
            IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
            Account account = accountRepository.GetByLogin(loginName);
            if (account == null)
            {
                NotFoundException ex = new NotFoundException($"Can not find account {loginName} for security");
                throw new FaultException<NotFoundException>(ex, ex.Message);
            }
            ValidateAutorization(account);
            return account;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public Account GetCustomerAccountInfo(string loginEmail)
        {
            return ExecuteFaultHandledOperation(()=> {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                Account account = accountRepository.GetByLogin(loginEmail);
                if (account == null)
                {
                    NotFoundException ex = new NotFoundException($"Account no found for {loginEmail}");
                    throw new FaultException<NotFoundException>(ex, ex.Message);
                }
                ValidateAutorization(account);
                return account;
            });
        }
        [OperationBehavior(TransactionScopeRequired = true)]
        [PrincipalPermission(SecurityAction.Demand, Role = Security.CAR_RENTAL_ADMIN)]
        [PrincipalPermission(SecurityAction.Demand, Name = Security.CAR_RENTAL_USER)]
        public void UpdateCustomerAccountInfo(Account account)
        {
            ExecuteFaultHandledOperation(()=> {
                IAccountRepository accountRepository = _DataRepositoryFactory.GetDataRepository<IAccountRepository>();
                ValidateAutorization(account);
                accountRepository.Update(account);
            });
        }
    }
}
