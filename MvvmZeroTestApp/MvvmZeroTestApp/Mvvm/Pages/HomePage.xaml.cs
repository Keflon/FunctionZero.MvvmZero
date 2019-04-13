using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public static readonly BindableProperty HomePageVmProperty = BindableProperty.Create("HomePageVm", typeof(HomePageVm), typeof(HomePage), null);

        public HomePageVm HomePageVm
        {
            get { return (HomePageVm)GetValue(HomePageVmProperty); }
            set { SetValue(HomePageVmProperty, value); }
        }
        public HomePage(HomePageVm viewModel)
        {
            this.HomePageVm = viewModel;
            InitializeComponent();
        }
    }
}