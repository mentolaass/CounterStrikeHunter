using System.Globalization;
using System.Windows.Media;

namespace CounterStrikeHunter.Model.Language
{
    public class Language
    {
        public string Name { get; set; }
        public ImageSource Image { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
