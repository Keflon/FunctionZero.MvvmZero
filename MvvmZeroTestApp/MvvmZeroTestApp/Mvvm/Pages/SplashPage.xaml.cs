using MvvmZeroTestApp.Mvvm.PageViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MvvmZeroTestApp.Mvvm.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SplashPage : ContentPage
	{
		public static readonly BindableProperty SplashPageVmProperty = BindableProperty.Create("SplashPageVm", typeof(SplashPageVm), typeof(SplashPage), null);

		public SplashPageVm SplashPageVm
		{
			get { return (SplashPageVm)GetValue(SplashPageVmProperty); }
			set { SetValue(SplashPageVmProperty, value); }
		}
		public SplashPage(SplashPageVm viewModel)
		{
			this.SplashPageVm = viewModel;
			InitializeComponent();
		}
	}
}