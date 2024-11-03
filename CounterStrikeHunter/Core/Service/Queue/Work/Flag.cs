using System.Windows.Media;

namespace CounterStrikeHunter.Core.Service.Queue.Work
{
    public class Flag
    {
        public static readonly Flag APP_CORE = new Flag("app core", Colors.Orange);

        public static readonly Flag NETWORK = new Flag("network", Colors.Blue);

        public static readonly Flag SYSTEM = new Flag("system", Colors.Green);

        public static readonly Flag INSPECTION = new Flag("inspection", Colors.Red);

        public static readonly Flag LOCAL = new Flag("local", Colors.Gray);

        private string _name;

        private Color _color;

        public Flag(string name, Color color)
        {
            this._name = name;
            this._color = color;
        }

        public string Name
        {
            get => _name;
        }

        public SolidColorBrush Color
        {
            get => new SolidColorBrush(_color);
        }
    }
}
