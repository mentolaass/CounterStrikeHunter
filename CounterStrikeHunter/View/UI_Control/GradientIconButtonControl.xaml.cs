using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CounterStrikeHunter.View.UI_Control
{
    public partial class GradientIconButtonControl : UserControl
    {
        public GradientIconButtonControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(Geometry), typeof(GradientIconButtonControl));

        public Geometry Icon
        {
            get => (Geometry)GetValue(IconProperty);
            set => SetValue(IconProperty, value);
        }
    }
}
