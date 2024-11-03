using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Navigation
{
    public class NavigationIntegratedTabViewModel : ListBoxItem
    {
        public static readonly DependencyProperty ModelTabIndexProperty =
            DependencyProperty.Register("ModelTabIndex", typeof(int), typeof(NavigationIntegratedTabViewModel));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavigationIntegratedTabViewModel));

        public NavigationIntegratedTabViewModel()
        {

        }

        public int ModelTabIndex
        {
            get => (int)GetValue(ModelTabIndexProperty);
            set => SetValue(ModelTabIndexProperty, value);
        }

        public string Icon
        {
            get => (string)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
