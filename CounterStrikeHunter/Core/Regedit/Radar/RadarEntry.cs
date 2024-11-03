namespace CounterStrikeHunter.Core.Regedit.Radar
{
    public class RadarEntry : IRegistryEntry
    {
        public string LastModifyDate => "undefined";
        public string Path { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
