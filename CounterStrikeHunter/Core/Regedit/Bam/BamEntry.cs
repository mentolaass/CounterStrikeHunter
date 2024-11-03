using System.IO;

namespace CounterStrikeHunter.Core.Regedit.Bam
{
    public class BamEntry : IRegistryEntry
    {
        public string LastModifyDate => "undefined";
        public string Path { get; set; }
        public bool IsDeleted
        {
            get
            {
                return (!File.Exists(Path) && !Directory.Exists(Path));
            }
            set { }
        }
    }
}
