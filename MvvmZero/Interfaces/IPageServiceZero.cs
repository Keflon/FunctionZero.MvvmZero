using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.Interfaces
{
    public interface IPageServiceZero<TEnum> where TEnum  : Enum
    {
        void SetPage(Page page);
        Page SetPage(TEnum pageKey, object parameter);
        Page CurrentPage { get; }
        Task<Page> PushPageAsync(TEnum pageKey, object parameter, bool killExistingNavigationPage = false);
        Task<Page> PushModalPageAsync(TEnum pageKey, object parameter);
        void Register(TEnum pageKey, Func<object, Page> pageMaker);
        Task PopAsync(bool animated = true);
        Task PopModalAsync(bool animated = true);
        Task PopToDepthAsync(int desiredDepth, bool animated = true);

    }
}
