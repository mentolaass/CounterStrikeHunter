using CounterStrikeHunter.Core.Service.Navigation;
using GalaSoft.MvvmLight;

namespace CounterStrikeHunter.ViewModel.Page
{
    public class QuickInspectionViewModel : ViewModelBase
    {
        private IFrameNavigationService _navigationService;

        public QuickInspectionViewModel(IFrameNavigationService navigationService)
        {
            _navigationService = navigationService;
        }
    }
}
