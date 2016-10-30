using CarRental.Admin.ViewModels;
using Core.Common.Core;
using MahApps.Metro.Controls;

namespace CarRental.Admin
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
            
        public MainWindow()
        {
            InitializeComponent();
            main.DataContext = ObjectBase.Container.GetExportedValue<MainViewModel>();
        }
    }
}
