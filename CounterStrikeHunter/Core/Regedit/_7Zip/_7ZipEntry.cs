namespace CounterStrikeHunter.Core.Regedit._7Zip
{
    public class _7ZipEntry : IRegistryEntry
    {
        public string LastModifyDate => "undefined";
        public string Path { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
