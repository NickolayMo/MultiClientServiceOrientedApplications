using CarRental.Business.Entities;
using CarRental.Common;
using CarRental.Common.Exceptions;
using Core.Common.Contracts;
using Core.Common.Core;
using Core.Common.Exceptions;
using System;
using System.ComponentModel.Composition;
using System.ServiceModel;
using System.Threading;

namespace CarRental.Business.Managers
{
    public class ManagerBase
    {
        protected string _LoginName;
        protected Account _AutarizationAccount = null;
        protected virtual Account LoadAutarizationValidationAccount(string loginName)
        {
            return null;
        }

        public ManagerBase()
        {
            OperationContext context = OperationContext.Current;
            try
            {
                _LoginName = OperationContext.Current.IncomingMessageHeaders.GetHeader<string>("String", "System");
                if (_LoginName.IndexOf(@"\") > -1)
                {
                    _LoginName = string.Empty;
                }
            }
            catch
            {
                _LoginName = string.Empty;
            }
            if (ObjectBase.Container != null)
            {
                ObjectBase.Container.SatisfyImportsOnce(this);

            }
            if (!string.IsNullOrEmpty(_LoginName))
            {
                _AutarizationAccount = LoadAutarizationValidationAccount(_LoginName);
            }
        }

        protected T ExecuteFaultHandledOperation<T>(Func<T> codeToExecute)
        {
            try
            {
                return codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }

        }
        protected void ExecuteFaultHandledOperation(Action codeToExecute) {
            try
            {
                codeToExecute.Invoke();
            }
            catch (FaultException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new FaultException(ex.Message);
            }
        }

        protected void ValidateAutorization(IAccountOwnerEntity entity)
        {
            if (Thread.CurrentPrincipal.IsInRole(Security.CAR_RENTAL_ADMIN))
            {
                if(_AutarizationAccount != null)
                {
                    if (_LoginName != string.Empty && entity.OwnerAccountId != _AutarizationAccount.AccountId)
                    {
                        AuthorizationValidationException ex = new AuthorizationValidationException("Attempt to access a secure record with improper user authorization validation.");
                        throw new FaultException<AuthorizationValidationException>(ex, ex.Message);
                    }
                }

            }
        }


    }
}
