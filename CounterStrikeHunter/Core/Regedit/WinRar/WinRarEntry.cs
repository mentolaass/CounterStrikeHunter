namespace CounterStrikeHunter.Core.Regedit.WinRar
{
    public class WinRarEntry : IRegistryEntry
    {
        public string LastModifyDate => "undefined";
        public string Path { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
