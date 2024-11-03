using System.Windows.Media;

namespace CounterStrikeHunter.Model.Sorting
{
    public class SortingType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LocalizedName { get => App.GetLocalizedString(Name); }
        public Geometry Icon { get; set; }
    }
}
