using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Animation;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.Transitions;
using FunctionZero.MvvmZero.PageTransitions;
using MvvmZeroTestApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.Platform.Android.AppCompat;
using AView = Android.Views.View;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(CustomTransitionsRenderer))]
namespace MvvmZeroTestApp.Droid.CustomRenderers
{
    public class CustomTransitionsRenderer : NavigationPageRenderer, Animator.IAnimatorListener
    {
        private Page _topPage;
        private TaskCompletionSource<bool> _tsc;

        public CustomTransitionsRenderer(Context c) : base(c)
        {

        }

        protected override async Task<bool> OnPushAsync(Page view, bool animated)
        {
            _topPage = view;

            TransitionAnimation transitionFunc = view.GetValue(CustomNavigationPage.TransitionAnimationProperty) as TransitionAnimation;


            if (transitionFunc != null)
            {
                var result = await base.OnPushAsync(view, false);

                INavigationPageController orderedNavigationStack = Element;

                // Lazy ...
                var things = new List<Page>(orderedNavigationStack.Pages);
                var prevPage = things[orderedNavigationStack.StackDepth - 2];

                if (prevPage != null)
                {
                    var containerToHide = GetContainerFromPage(prevPage);
                    var containerToAdd = GetContainerFromPage(view);

                    //Reverse situation back to before base.OnPushAsync:
                    containerToAdd.Visibility = ViewStates.Invisible;
                    containerToHide.Visibility = ViewStates.Visible;

                    await Task.Yield(); //Magic! ;)

                    containerToAdd.Visibility = ViewStates.Visible;

                    await transitionFunc.DoAnimation(prevPage, view, true);

                    //await StartAnimation(containerToHide, containerToAdd, true);

                    //Set status back:
                    containerToHide.Visibility = ViewStates.Gone;
                }

                return result;
            }
            else
                return await base.OnPushAsync(view, animated);
        }

        protected override async Task<bool> OnPopViewAsync(Page page, bool animated)
        {
            //var orderedNavigationStack = ((INavigationPageController)Element).StackCopy;
            //var prevPage = orderedNavigationStack.Skip(1).FirstOrDefault();

            TransitionAnimation transitionFunc = page.GetValue(CustomNavigationPage.TransitionAnimationProperty) as TransitionAnimation;

            if (transitionFunc != null)
            {

                INavigationPageController orderedNavigationStack = Element;

                // Lazy ...
                var things = new List<Page>(orderedNavigationStack.Pages);

                var prevPage = things[orderedNavigationStack.StackDepth - 2];

                _topPage = prevPage;

                if (prevPage != null)
                {
                    var container = SetupPop(page, prevPage);

                    await transitionFunc.DoAnimation(page, prevPage, false);

                    //await StartAnimation(GetContainerFromPage(page), GetContainerFromPage(prevPage), false);
                }

                return await base.OnPopViewAsync(page, false);
            }
            else
                return await base.OnPopViewAsync(page, animated);
        }

        protected override async Task<bool> OnPopToRootAsync(Page page, bool animated)
        {
            if (_topPage != null)
            {
                var container = SetupPop(_topPage, page);
                _topPage = null;
                //await StartAnimation(container, false);
            }

            return await base.OnPopToRootAsync(page, false);
        }

        private AView SetupPop(Page currentPage, Page prevPage)
        {
            var containerToRemove = GetContainerFromPage(currentPage);
            var containerToShow = GetContainerFromPage(prevPage);

            containerToShow.Visibility = ViewStates.Visible;

            return containerToRemove;
        }

        private Task StartAnimation(AView containerCurrent, AView containerNext, bool isPush)
        {
            _tsc = new TaskCompletionSource<bool>();

            ValueAnimator animatorCurrent;
            ValueAnimator animatorNext;

            if (isPush)
            {
                animatorCurrent = ValueAnimator.OfFloat(0, -containerCurrent.Width);
                animatorNext = ValueAnimator.OfFloat(containerNext.Width, 0);
            }
            else
            {
                animatorCurrent = ValueAnimator.OfFloat(0, containerCurrent.Width);
                animatorNext = ValueAnimator.OfFloat(-containerNext.Width, 0);
            }

            animatorCurrent.SetDuration(5000);
            animatorNext.SetDuration(5000);

            animatorCurrent.Update += (sender, e) =>
            {
                int newValue = (int)e.Animation.AnimatedValue;
                // Apply this new value to the object being animated.
                containerCurrent.TranslationX = newValue;
            };

            animatorNext.Update += (sender, e) =>
            {
                int newValue = (int)e.Animation.AnimatedValue;
                // Apply this new value to the object being animated.
                containerNext.TranslationX = newValue;
            };



            animatorNext.AddListener(this);

            animatorCurrent.Start();
            animatorNext.Start();

            return _tsc.Task;
        }


        private AView GetContainerFromPage(Page page)
        {
            var renderer = Platform.GetRenderer(page);
            //var container = (AView)renderer.ViewGroup.Parent;
            var container = (AView)renderer.View.Parent;
            return container;
        }

        public void OnAnimationCancel(Animator animation)
        {
            _tsc.SetResult(false);
        }

        public void OnAnimationEnd(Animator animation)
        {
            _tsc.SetResult(true);
        }

        public void OnAnimationRepeat(Animator animation)
        {
            //
        }

        public void OnAnimationStart(Animator animation)
        {
            //
        }
    }
}