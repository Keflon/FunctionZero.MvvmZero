using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RedPillPage : ContentPage
    {
        public static readonly BindableProperty RedPillPageVmProperty = BindableProperty.Create("RedPillPageVm", typeof(RedPillPageVm), typeof(RedPillPage), null);

        public RedPillPageVm RedPillPageVm
        {
            get { return (RedPillPageVm)GetValue(RedPillPageVmProperty); }
            set { SetValue(RedPillPageVmProperty, value); }
        }
        public RedPillPage(RedPillPageVm viewModel)
        {
            this.RedPillPageVm = viewModel;
            InitializeComponent();
        }
    }
}