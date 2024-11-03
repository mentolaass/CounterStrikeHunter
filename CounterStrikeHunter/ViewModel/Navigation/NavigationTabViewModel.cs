using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Navigation
{
    public class NavigationTabViewModel : ListBoxItem
    {
        public static readonly DependencyProperty ModelTabIndexProperty =
            DependencyProperty.Register("ModelTabIndex", typeof(int), typeof(NavigationTabViewModel));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavigationTabViewModel));

        public static readonly DependencyProperty ModelNameProperty =
            DependencyProperty.Register("ModelName", typeof(string), typeof(NavigationTabViewModel));

        public static readonly DependencyProperty ExpandedProperty =
            DependencyProperty.Register("Expanded", typeof(bool), typeof(NavigationTabViewModel));

        public NavigationTabViewModel() 
        { 

        }

        public bool Expanded
        {
            get => (bool)GetValue(ExpandedProperty);
            set => SetValue(ExpandedProperty, value);
        }

        public int ModelTabIndex
        {
            get => (int)GetValue(ModelTabIndexProperty);
            set => SetValue(ModelTabIndexProperty, value);
        }

        public string Icon
        {
            get => (string) GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }

        public string ModelName
        {
            get => (string) GetValue(NameProperty); 
            set => SetValue(NameProperty, value);
        }
    }
}
