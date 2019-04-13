using System;

namespace FunctionZero.MvvmZero.Interfaces
{
    //public interface IHasOwnerPage
    //{
    //    void OwnerPageAppearing();
    //    void OwnerPageDisappearing();
    //    int OwnerPageKey { get; set; }
    //    int? PageDepth { get; set; }
    //}

    public interface IHasOwnerPage<TPageKey> where TPageKey : Enum
    {
        void OwnerPageAppearing();
        void OwnerPageDisappearing();
        TPageKey OwnerPageKey { get; set; }
        int? PageDepth { get; set; }
    }
}
