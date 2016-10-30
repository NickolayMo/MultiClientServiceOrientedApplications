using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Core.Common.UI.Core
{
    public class UserControlViewBase : UserControl 
    {
        public static readonly DependencyProperty ViewLoadedProperty = 
            DependencyProperty.Register("ViewLoaded", typeof(object),typeof(UserControlViewBase),new PropertyMetadata(null));

        public UserControlViewBase()
        {
            BindingOperations.SetBinding(this, ViewLoadedProperty, new Binding("ViewLoaded"));
        }

        protected void OnWireViewModelEvents(ViewModelBase viewModelBase) { }
        protected void OnUnWireViewModelEvents(ViewModelBase viewModelBase) { }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
            {
                if (e.OldValue != null)
                {
                    OnUnWireViewModelEvents(e.OldValue as ViewModelBase);
                }
                else
                {
                    OnWireViewModelEvents(e.NewValue as ViewModelBase);
                }
            }
        }
    }
}
