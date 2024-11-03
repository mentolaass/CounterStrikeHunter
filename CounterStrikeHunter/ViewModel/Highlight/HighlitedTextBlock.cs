using CounterStrikeHunter.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace CounterStrikeHunter.ViewModel.Highlight
{
    [TemplatePart(Name = PART_TextDisplay, Type = typeof(TextBlock))]
    public class HighlitedTextBlock : Control
    {
        private const string PART_TextDisplay = "PART_TextDisplay";

        private TextBlock _displayTextBlock;

        public static readonly DependencyProperty HighlightColorProperty
            = DependencyProperty.Register("HighlightColor", typeof(Brush), typeof(HighlitedTextBlock));

        public static readonly DependencyProperty HighlightTextProperty
            = DependencyProperty.Register("HighlightText", typeof(string), typeof(HighlitedTextBlock));

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(HighlitedTextBlock),
            new PropertyMetadata(string.Empty, OnHighlightPropertiesChanged));

        static HighlitedTextBlock()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HighlitedTextBlock), new FrameworkPropertyMetadata(typeof(HighlitedTextBlock)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            _displayTextBlock = GetTemplateChild(PART_TextDisplay) as TextBlock;
            UpdateHighlightDisplay();
        }

        private static void OnHighlightPropertiesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is HighlitedTextBlock highlightTextBlock)
            {
                highlightTextBlock.UpdateHighlightDisplay();
            }
        }

        private void UpdateHighlightDisplay()
        {
            if (_displayTextBlock == null)
                return;

            _displayTextBlock.Inlines.Clear();

            if (string.IsNullOrEmpty(HighlightText) || (string.IsNullOrEmpty(HighlightText) || string.IsNullOrEmpty(Text) || !Util.ContainsFilteredWords(Text, HighlightText)))
            {
                var whiteRun = new Run(Text) { Foreground = Brushes.White };
                _displayTextBlock.Inlines.Add(whiteRun);
                return;
            }

            int lastIndex = 0;
            int index = 0;

            var filterParts = HighlightText.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var filter in filterParts)
            {
                string trimmedFilter = filter.Trim();

                while ((index = Text.IndexOf(trimmedFilter, lastIndex, StringComparison.OrdinalIgnoreCase)) >= 0)
                {
                    if (index > lastIndex)
                    {
                        _displayTextBlock.Inlines.Add(new Run(Text.Substring(lastIndex, index - lastIndex)));
                    }

                    _displayTextBlock.Inlines.Add(CreateHighlightedRun(Text.Substring(index, trimmedFilter.Length)));

                    lastIndex = index + trimmedFilter.Length;
                }
            }

            if (lastIndex < Text.Length)
            {
                _displayTextBlock.Inlines.Add(new Run(Text.Substring(lastIndex)));
            }
        }
        private Run CreateHighlightedRun(string text)
        {
            return new Run(text) { Background = HighlightColor };
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public Brush HighlightColor
        {
            get => (Brush)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        public string HighlightText
        {
            get => (string)GetValue(HighlightTextProperty);
            set => SetValue(HighlightTextProperty, value);
        }
    }
}
