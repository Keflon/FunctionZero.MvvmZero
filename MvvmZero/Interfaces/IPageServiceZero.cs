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
        Task<Page> PushPage(TEnum pageKey, object parameter, bool killExistingNavigationPage = false);
        Task<Page> PushModalPage(TEnum pageKey, object parameter);
        void Register(TEnum pageKey, Func<object, Page> pageMaker);
        Task Pop(bool animated = true);
        Task PopModal(bool animated = true);
    }
}
