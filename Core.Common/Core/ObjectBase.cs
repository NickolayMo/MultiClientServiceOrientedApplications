using Core.Common.Extensions;
using Core.Common.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Core.Common.Contracts;
using System.Runtime.Serialization;
using System.Text;
using FluentValidation;
using FluentValidation.Results;
using System.ComponentModel.Composition.Hosting;

namespace Core.Common.Core
{
    public class ObjectBase : INotifyPropertyChanged, IDirtyCapable, IExtensibleDataObject, IDataErrorInfo
    {
        
        protected bool _isDirty = false;
        List<PropertyChangedEventHandler> _PropertyChangedSubscribers = new List<PropertyChangedEventHandler>();
        protected IValidator _validator = null;
        protected IEnumerable<ValidationFailure> _validatorErrors = null;
        public static CompositionContainer Container { get; set; }
        
        public event PropertyChangedEventHandler _PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                if (!_PropertyChangedSubscribers.Contains(value)){
                    _PropertyChanged += value;
                    _PropertyChangedSubscribers.Add(value);
                }
            }
            remove
            {
                _PropertyChanged -= value;
                _PropertyChangedSubscribers.Remove(value);
            }
        }



        public ObjectBase()
        {
            _validator = GetValidator();
            Validate();
        }

        public void Validate()
        {
            if (_validator != null)
            {
                ValidationResult results = _validator.Validate(this);
                _validatorErrors = results.Errors;
            }
        }

        protected virtual IValidator GetValidator()
        {
            return null;
        }
        [NotNavigable]
        public virtual bool IsValid
        {
            get
            {
                if (_validatorErrors != null && _validatorErrors.Count() > 0)
                    return false;
                else
                    return true;
            }
        }


        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(propertyName, true);

        }
        protected void WalkObjectGraph(Func<ObjectBase, bool> snippetForObject,
                                       Action<IList> snippetForCollection,
                                       params string[] exemptProperties)
        {
            List<ObjectBase> visited = new List<ObjectBase>();
            Action<ObjectBase> walk = null;
            List<string> exemption = new List<string>();
            if (exemptProperties != null)
            {
                exemption = exemptProperties.ToList();
            }
            walk = (o) =>
            {
                if (o != null && !visited.Contains(o))
                {
                    visited.Add(o);

                    bool exitWalk = snippetForObject.Invoke(o);
                    if (!exitWalk)
                    {
                        PropertyInfo[] properties = o.GetBrowsableProperties();
                        foreach (PropertyInfo property in properties)
                        {
                            if (property.PropertyType.IsSubclassOf(typeof(ObjectBase)))
                            {
                                ObjectBase obj = (ObjectBase)(property.GetValue(o, null));
                                walk(obj);
                            }
                            else
                            {
                                IList coll = property.GetValue(o, null) as IList;
                                if (coll != null)
                                {
                                    snippetForCollection.Invoke(coll);
                                    foreach (object item in coll)
                                    {
                                        if (item is ObjectBase)
                                        {
                                            walk((ObjectBase)item);
                                        }
                                    }
                                }

                            }
                        }
                    }
                }
            };
            walk(this);
        }
        public List<IDirtyCapable> GetDirtyObjects()
        {
            List<IDirtyCapable> dirtyObjects = new List<IDirtyCapable>();
            WalkObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    dirtyObjects.Add(o);
                }
                return false;
            }, coll => { }
            );
            return dirtyObjects;
        }

        public void CleanAll()
        {
            
            WalkObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    o.IsDirty =  false;
                }
                return false;
            }, coll => { }
            );
           
        }

        public virtual bool IsAnythingDirty()
        {
            bool isDirty = false;
            WalkObjectGraph(o =>
            {
                if (o.IsDirty)
                {
                    isDirty = true;
                   return true;
                }
                else
                {
                    return false;
                }
                
            }, coll => { }
            );
            return isDirty;
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, true);
        }
        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression, bool makeDirty) 
        {
            string propertyName = PropertySupport.ExtractPropertyName(propertyExpression);
            OnPropertyChanged(propertyName, makeDirty);
        }

        protected virtual void OnPropertyChanged(string propertyName, bool makeDirty)
        {
            if (_PropertyChanged != null)
            {
                _PropertyChanged(this, new PropertyChangedEventArgs(propertyName));

            }
            if (makeDirty)
            {
                _isDirty = true;
            }
            Validate();
        }
        public virtual bool IsDirty
        {
            get { return _isDirty; }
            set { _isDirty = value; }
        }

        public string Error
        {
            get
            {
                return string.Empty;
            }
        }

        public ExtensionDataObject ExtensionData { get; set; }

        public string this[string columnName]
        {
            get
            {
                StringBuilder errors = new StringBuilder();
                if (_validatorErrors != null && _validatorErrors.Count() > 0)
                {
                    foreach (ValidationFailure validationError in _validatorErrors)
                    {
                        if (validationError.PropertyName == columnName)
                        {
                            errors.Append(validationError.ErrorMessage);
                        }
                    }
                }
                return errors.ToString();
            }
        }
       
        public IEnumerable<ValidationFailure> ValidationErrors
        {
            get
            {
                return _validatorErrors;
            }
            set { }
        }
    }
}
