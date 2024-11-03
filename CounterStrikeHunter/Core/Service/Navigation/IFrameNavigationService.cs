using GalaSoft.MvvmLight.Views;

namespace CounterStrikeHunter.Core.Service.Navigation
{
    public interface IFrameNavigationService : INavigationService
    {
        object Parameter { get; }
    }
}
