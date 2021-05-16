using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific.AppCompat;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace FunctionZero.MvvmZero.PageTransitions
{
    // https://xamgirl.com/transparent-navigation-bar-in-xamarin-forms/

    public class CustomNavigationPage : NavigationPage
    {
        public static readonly BindableProperty TransitionAnimationProperty = BindableProperty.CreateAttached("TransitionAnimation", typeof(TransitionAnimation), typeof(CustomNavigationPage), null);

        public static TransitionAnimation GetTransitionAnimation(BindableObject view)
        {
            return (TransitionAnimation)view.GetValue(TransitionAnimationProperty);
        }

        public static void SetTransitionAnimation(BindableObject view, TransitionAnimation value)
        {
            view.SetValue(TransitionAnimationProperty, value);
        }
    }
}
