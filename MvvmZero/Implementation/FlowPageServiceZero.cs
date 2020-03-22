using FunctionZero.MvvmZero.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero
{
    public class FlowPageServiceZero : IFlowPageServiceZero
    {
        public Page CurrentPage => _application.MainPage;

        private readonly Application _application;
        private readonly Action<Page> _pageCreateAction;
        private Func<Type, object> _typeFactory;

        public NavigationPage CurrentNavigationPage => CurrentPage as NavigationPage;

        public FlowPageServiceZero(Application application, Action<Page> pageCreateAction)
        {
            _application = application;
            _pageCreateAction = pageCreateAction;
        }

        public TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
        {
            TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
            TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
            setState.Invoke(vm);
            page.BindingContext = vm;

            _pageCreateAction?.Invoke(page);

            if (vm is IHasOwnerPage tHasOwnerPage)
            {
                page.Appearing += (s, e) => tHasOwnerPage.OwnerPageAppearing(null, this.CurrentNavigationPage?.StackDepth);
                page.Disappearing += (s, e) => tHasOwnerPage.OwnerPageDisappearing();
            }
            return page;
        }

        public async Task PopAsync(bool isModal, bool animated = true)
        {
            if (isModal)
                await CurrentNavigationPage.PopAsync(animated);
            else
                await CurrentNavigationPage.Navigation.PopModalAsync(animated);
        }

        public async Task<Page> PushPageAsync(Page page, bool isModal)
        {
            if (isModal == false)
            {
                if (CurrentNavigationPage == null /* || killExistingNavigationPage */)
                {
                    var navPage = new NavigationPage(page);
                    // When a Page is popped, tell it to release any bindings to the view model.
                    navPage.Popped += (s, e) => e.Page.BindingContext = null;
                    this.SetPage(navPage);
                }
                else
                {
                    await CurrentNavigationPage.Navigation.PushAsync(page, true);
                }
            }
            else
            {
                if (CurrentNavigationPage == null)
                    this.SetPage(new NavigationPage());

                await CurrentNavigationPage.Navigation.PushModalAsync(page, true);
            }

            return page;
        }

        public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool isModal = false) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
            return await PushPageAsync(newPage, isModal);
        }

        public void RegisterTypeFactory(Func<Type, object> typeFactory)
        {
            _typeFactory = typeFactory;
        }

        public void SetPage(Page page)
        {
            _application.MainPage = page;
        }

        public TPage SetPage<TPage, TViewModel>(Action<TViewModel> setStateAction) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setStateAction);
            SetPage(newPage);
            return newPage;
        }
    }

    //public class FlowPageServiceZeroy : IFlowPageServiceZero
    //{
    //    private Func<Type, object> _typeFactory;

    //    private readonly Application _application;
    //    private readonly Action<Page> _pageCreateAction;

    //    public Page CurrentPage => _application.MainPage;
    //    public NavigationPage CurrentNavigationPage => CurrentPage as NavigationPage;

    //    public FlowPageServiceZero(Application application, Action<Page> pageCreateAction)
    //    {
    //        _application = application;
    //        _pageCreateAction = pageCreateAction;
    //    }

    //    public void SetPage(Page page)
    //    {
    //        _application.MainPage = page;
    //    }

    //    public Page SetPage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
    //    {
    //        var newPage = MakePage<TPage, TViewModel>(setState);
    //        SetPage(newPage);

    //        return newPage;

    //    }


    //    public TPage MakePage<TPage, TViewModel>(Action<TViewModel> setState) where TPage : Page
    //    {
    //        TPage page = (TPage)_typeFactory.Invoke(typeof(TPage));
    //        TViewModel vm = (TViewModel)_typeFactory.Invoke(typeof(TViewModel));
    //        setState.Invoke(vm);
    //        page.BindingContext = vm;

    //        _pageCreateAction?.Invoke(page);

    //        if (vm is IHasOwnerPage tHasOwnerPage)
    //        {
    //            page.Appearing += (s, e) => tHasOwnerPage.OwnerPageAppearing(null, this.CurrentNavigationPage?.StackDepth);
    //            page.Disappearing += (s, e) => tHasOwnerPage.OwnerPageDisappearing();
    //        }
    //        return page;
    //    }


    //    public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setStateAction, bool killExistingNavigationPage = false) where TPage : Page
    //    {
    //        TPage newPage = MakePage<TPage, TViewModel>(setStateAction);

    //        if (CurrentNavigationPage == null || killExistingNavigationPage)
    //        {
    //            var navPage = new NavigationPage(newPage);
    //            this.SetPage(navPage);
    //        }
    //        else
    //        {
    //            await CurrentNavigationPage.Navigation.PushAsync(newPage, true);
    //        }

    //        return newPage;
    //    }



    //    public void RegisterTypeFactory(Func<Type, object> typeFactory)
    //    {
    //        _typeFactory = typeFactory;
    //    }

    //    public async Task Test()
    //    {
    //        await this.PushPageAsync<Page, TestVm>((vm) => vm.SetState(4));
    //        await this.PushPageAsync<Page, TestVm>((vm) => vm.SetState(4));
    //    }



    //}
}
