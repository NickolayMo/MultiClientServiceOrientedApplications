using System;
using System.Collections.Generic;
using System.Linq;
using Core.Common.Core;
using System.ComponentModel;
using FluentValidation.Results;

namespace Core.Common.UI.Core
{
    public class ViewModelBase : ObjectBase
    {
        private bool _ErrorVisible;

        public ViewModelBase()
        {
            ToggleErrorCommand = new DelegateCommand<object>(OnToggleErrorCommandExecute, OnToggleErrorsCommandCanExecute);
        }

        protected virtual bool OnToggleErrorsCommandCanExecute(object arg)
        {
            return !IsValid;
        }

        protected virtual void OnToggleErrorCommandExecute(object obj)
        {
            ErrorsVisible = !ErrorsVisible;
        }

        public DelegateCommand<object> ToggleErrorCommand { get; protected set; }


        public virtual bool ErrorsVisible
        {

            get { return _ErrorVisible; }
            set
            {
                if (_ErrorVisible == value)
                {
                    return;
                }
                _ErrorVisible = false;
                OnPropertyChanged(() => ErrorsVisible, false);
            }

        }
        public object ViewLoaded
        {
            get
            {
                OnViewLoaded();
                return null;
            }
        }


        protected virtual void OnViewLoaded()
        {

        }
        protected void WithClient<T>(T proxy, Action<T> codeToExecute)
        {
            codeToExecute.Invoke(proxy);

            IDisposable disposableClient = proxy as IDisposable;
            if (disposableClient != null)
            {
                disposableClient.Dispose();
            }
        }

        List<ObjectBase> _Models;
        protected virtual void AddModel(List<ObjectBase> models) { }

        protected void ValidateModel()
        {
            if (_Models == null)
            {
                _Models = new List<ObjectBase>();
                AddModel(_Models);
            }
            _validatorErrors = new List<ValidationFailure>();
            if (_Models.Count > 0)
            {
                foreach (ObjectBase modelObject in _Models)
                {
                    if (modelObject != null)
                    {
                        modelObject.Validate();
                    }
                    _validatorErrors = _validatorErrors.Union(modelObject.ValidationErrors).ToList();
                }
                OnPropertyChanged(() => ValidationErrors, false);
                OnPropertyChanged(() => ValidationHeaderText, false);
                OnPropertyChanged(() => ValidationHeaderVisible, false);
            }
        }
        public virtual string ValidationHeaderText {
            get
            {
                string ret = string.Empty;
                if (ValidationErrors != null)
                {
                    string verb = (ValidationErrors.Count() == 1 ? "is" : "are");
                    string suffix = (ValidationErrors.Count() == 1 ? "" : "s");

                    if (!IsValid)
                        ret = string.Format("There {0} {1} validation error{2}.", verb, ValidationErrors.Count(), suffix);
                }
                return ret;
            }
        }
        public virtual bool ValidationHeaderVisible { get { return ValidationErrors != null && ValidationErrors.Count() > 0;  } }

        public virtual string ViewTitle
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
