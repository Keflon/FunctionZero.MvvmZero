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
        private Func<Type, object> _typeFactory;

        private readonly Application _application;
        private readonly Action<Page> _pageCreateAction;

        public Page CurrentPage => _application.MainPage;
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

        public void SetPage(Page page)
        {
            _application.MainPage = page;
        }

        public async Task<Page> PushPageAsync<TPage, TViewModel>(Action<TViewModel> setState, bool killExistingNavigationPage = false) where TPage : Page
        {
            TPage newPage = MakePage<TPage, TViewModel>(setState);

            if (CurrentNavigationPage == null || killExistingNavigationPage)
            {
                var navPage = new NavigationPage(newPage);
                this.SetPage(navPage);
            }
            else
            {
                await CurrentNavigationPage.Navigation.PushAsync(newPage, true);
            }

            return newPage;
        }



        public void RegisterTypeFactory(Func<Type, object> typeFactory)
        {
            _typeFactory = typeFactory;
        }

        public async Task Test()
        {
            await this.PushPageAsync<Page, TestVm>((vm) => vm.SetState(4));
            await this.PushPageAsync<Page, TestVm>((vm) => vm.SetState(4));
        }


        public class TestVm : IFlowPageZero<int, string>
        {
            public string GetResult()
            {
                throw new NotImplementedException();
            }

            public int GetState()
            {
                throw new NotImplementedException();
            }

            public void SetState(int state)
            {
                throw new NotImplementedException();
            }
        }
    }
}
