using Core.Common.Core;

namespace CarRental.Client.Entities
{
    public class Account : ObjectBase
    {
        private int _accountId;
        private string _loginEmail;
        private string _firstName;
        private string _lastName;
        private string _address;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _creditCart;
        private string _expDate;

        public int AccountId
        {
            get
            {
                return _accountId;
            }
            set
            {
                _accountId = value;
                OnPropertyChanged(()=>AccountId);
            }
        }


        public string LoginEmail
        {
            get
            {
                return _loginEmail;
            }
            set
            {
                _loginEmail = value;
                OnPropertyChanged(() => LoginEmail);
            }
        }


        public string FirstName
        {
            get
            {
                return _firstName;
            }
            set
            {
                _firstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }


        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                _lastName = value;
                OnPropertyChanged(() => LastName);
            }
        }


        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                _address = value;
                OnPropertyChanged(() => Address);
            }
        }


        public string City
        {
            get
            {
                return _city;
            }
            set
            {
                _city = value;
                OnPropertyChanged(() => City);
            }
        }


        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnPropertyChanged(() => State);
            }
        }


        public string ZipCode
        {
            get
            {
                return _zipCode;
            }
            set
            {
                _zipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }


        public string CreditCart
        {
            get
            {
                return _creditCart;
            }
            set
            {
                _creditCart = value;
                OnPropertyChanged(() => CreditCart);
            }
        }


        public string ExpDate
        {
            get
            {
                return _expDate;
            }
            set
            {
                _expDate = value;
                OnPropertyChanged(() => ExpDate);
            }
        }
    }
}
