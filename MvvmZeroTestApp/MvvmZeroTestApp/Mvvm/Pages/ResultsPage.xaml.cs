using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp.Mvvm.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResultsPage : ContentPage
    {
        public static readonly BindableProperty ResultsPageVmProperty = BindableProperty.Create("ResultsPageVm", typeof(ResultsPageVm), typeof(ResultsPage), null);

        public ResultsPageVm ResultsPageVm
        {
            get { return (ResultsPageVm)GetValue(ResultsPageVmProperty); }
            set { SetValue(ResultsPageVmProperty, value); }
        }
        public ResultsPage(ResultsPageVm viewModel)
        {
            this.ResultsPageVm = viewModel;
            InitializeComponent();
        }
    }
}