using System.Windows.Media;

namespace CounterStrikeHunter.Core.Service.Browser.Pair
{
    public class DataPairType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get => App.GetLocalizedString(Name); }
        public Geometry Icon { get; set; }
    }
}
