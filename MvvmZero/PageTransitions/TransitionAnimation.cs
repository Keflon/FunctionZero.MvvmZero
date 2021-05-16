using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.PageTransitions
{
    public abstract class TransitionAnimation
    {
        public abstract Task DoAnimation(Page currentPage, Page nextPage, bool isPush);
    }
}