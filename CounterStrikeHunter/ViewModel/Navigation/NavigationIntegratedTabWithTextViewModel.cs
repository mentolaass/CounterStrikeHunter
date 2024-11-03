using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Navigation
{
    public class NavigationIntegratedTabWithTextViewModel : ListBoxItem
    {
        public static readonly DependencyProperty ModelTabIndexProperty =
            DependencyProperty.Register("ModelTabIndex", typeof(int), typeof(NavigationIntegratedTabWithTextViewModel));

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(NavigationIntegratedTabWithTextViewModel));

        public static readonly DependencyProperty TextProperty = 
            DependencyProperty.Register("Text", typeof(string), typeof(NavigationIntegratedTabWithTextViewModel), new PropertyMetadata(null));


        public NavigationIntegratedTabWithTextViewModel()
        {
            
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public int ModelTabIndex
        {
            get => (int)GetValue(ModelTabIndexProperty);
            set => SetValue(ModelTabIndexProperty, value);
        }

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
