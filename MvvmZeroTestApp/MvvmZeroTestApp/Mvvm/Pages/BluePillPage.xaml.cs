using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BluePillPage : ContentPage
    {
        public static readonly BindableProperty BluePillPageVmProperty = BindableProperty.Create("BluePillPageVm", typeof(BluePillPageVm), typeof(BluePillPage), null);

        public BluePillPageVm BluePillPageVm
        {
            get { return (BluePillPageVm)GetValue(BluePillPageVmProperty); }
            set { SetValue(BluePillPageVmProperty, value); }
        }
        public BluePillPage(BluePillPageVm viewModel)
        {
            this.BluePillPageVm = viewModel;
            InitializeComponent();
        }
    }
}