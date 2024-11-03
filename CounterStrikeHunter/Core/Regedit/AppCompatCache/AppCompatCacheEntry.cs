using System;
using System.IO;

namespace CounterStrikeHunter.Core.Regedit.AppCompatCache
{
    public class AppCompatCacheEntry : IRegistryEntry
    {
        public DateTimeOffset? LastModifiedTimeUTC { get; set; }
        public long LastModifiedFILETIMEUTC => LastModifiedTimeUTC.HasValue ? LastModifiedTimeUTC.Value.ToFileTime() : 0;
        public string LastModifyDate
        {
            get => "last modify at " + LastModifiedTimeUTC.Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        public string Path { get; set; }
        public int PathSize { get; set; }
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
