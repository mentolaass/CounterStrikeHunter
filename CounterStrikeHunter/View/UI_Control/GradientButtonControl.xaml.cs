using System.Windows;
using System.Windows.Controls;

namespace CounterStrikeHunter.View.UI_Control
{
    public partial class GradientButtonControl : UserControl
    {
        public GradientButtonControl()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(GradientButtonControl));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static new readonly DependencyProperty FontSizeProperty =
            DependencyProperty.Register("FontSize", typeof(int), typeof(GradientButtonControl));

        public new int FontSize
        {
            get => (int)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly DependencyProperty BtnCornerRadiusProperty =
            DependencyProperty.Register("BtnCornerRadius", typeof(CornerRadius), typeof(GradientButtonControl));

        public CornerRadius BtnCornerRadius
        {
            get => (CornerRadius)GetValue(BtnCornerRadiusProperty);
            set => SetValue(BtnCornerRadiusProperty, value);
        }
    }
}
