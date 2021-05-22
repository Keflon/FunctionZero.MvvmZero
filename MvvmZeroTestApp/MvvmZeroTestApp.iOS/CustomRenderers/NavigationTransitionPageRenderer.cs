using CoreAnimation;
using Foundation;
using FunctionZero.MvvmZero.PageTransitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationTransitionPageRenderer))]

class NavigationTransitionPageRenderer : NavigationRenderer
{
    public override async void PushViewController(UIViewController viewController, bool animated)
    {
        INavigationPageController orderedNavigationStack = Element as INavigationPageController;

        if (orderedNavigationStack.StackDepth > 1)
        {
            Stack<Page> pageStack = new Stack<Page>(orderedNavigationStack.Pages);

            var topPage = pageStack.Pop();
            var prevPage = pageStack.Pop();

            TransitionAnimation transitionFunc = topPage.GetValue(CustomNavigationPage.TransitionAnimationProperty) as TransitionAnimation;
            if (transitionFunc != null)
            {


                //UIView.Animate(3.75, () =>
                //{
                //    UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                //    base.PushViewController(viewController, false);
                //    UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.View, false);
                //});
                //var transition2 = CATransition.CreateAnimation();
                //transition2.Duration = 5.75;
                ////transition2.Type = CAAnimation.TransitionPush;
                //View.Layer.AddAnimation(transition2, null);
                base.PushViewController(viewController, false);

                //await prevPage.RotateTo(360, 1000);

                await transitionFunc.DoAnimation(prevPage, topPage, true);
                //_= topPage.RotateTo(360, 1000);
                // await Task.Yield(); //Magic! ;)
                return;
            }
        }
        //await transitionFunc.DoAnimation(prevPage, (Page)View, true);

        // Alternative way with different set of trannsition
        /*
        UIView.Animate(0.75, () =>
        {
            UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
            base.PushViewController(viewController, false);
            UIView.SetAnimationTransition(UIViewAnimationTransition.CurlUp, this.View, false);
        });
         */

        var transition = CATransition.CreateAnimation();
        
        transition.Duration = 0.75;
        transition.Type = CAAnimation.TransitionPush;
        View.Layer.AddAnimation(transition, null);
        base.PushViewController(viewController, false);
    }

    public override UIViewController PopViewController(bool animated)
    {
        if (animated)
        {
            // Alternative way with different set of trannsition
            /*                UIView.Animate(0.75, () =>
            {
                UIView.SetAnimationCurve(UIViewAnimationCurve.EaseInOut);
                UIView.SetAnimationTransition(UIViewAnimationTransition.CurlDown, this.View, false);
            });
            */

            var transition = CATransition.CreateAnimation();
            transition.Duration = 0.75;
            transition.Type = CAAnimation.TransitionFromTop;

            View.Layer.AddAnimation(transition, null);

            return base.PopViewController(false);
        }
        else
        {
            return base.PopViewController(false);
        }
    }
}