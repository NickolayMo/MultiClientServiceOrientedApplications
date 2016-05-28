using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Common.Tests.TestClasses
{
    public class TestChild : ObjectBase
    {
        private string _childName = string.Empty;

        public string ChildName
        {
            get { return _childName; }
            set {
                if (_childName == value)
                {
                    return;
                }
                _childName = value;
                OnPropertyChanged(()=>ChildName);
            }
        }

    }
}
