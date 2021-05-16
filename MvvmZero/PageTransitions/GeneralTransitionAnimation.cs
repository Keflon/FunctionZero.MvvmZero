using System.Threading.Tasks;
using Xamarin.Forms;

namespace FunctionZero.MvvmZero.PageTransitions
{
    public class GeneralTransitionAnimation : TransitionAnimation
    {
        private const int _delay = 4000;

        public override async Task DoAnimation(Page currentPage, Page nextPage, bool isPush)
        {
            if (isPush)
            {
                _ = currentPage.TranslateTo(-currentPage.Width, 0, _delay, Easing.BounceIn);

                nextPage.TranslationX = nextPage.Width;
                await nextPage.TranslateTo(0, 0, _delay, Easing.BounceIn);
            }
            else
            {
                _ = currentPage.TranslateTo(currentPage.Width, 0, _delay, Easing.BounceOut);

                nextPage.TranslationX = -nextPage.Width;
                await nextPage.TranslateTo(0, 0, _delay, Easing.BounceOut);
            }
        }
    }
}