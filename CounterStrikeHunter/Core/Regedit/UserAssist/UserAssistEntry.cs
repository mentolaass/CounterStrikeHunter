namespace CounterStrikeHunter.Core.Regedit.UserAssist
{
    public class UserAssistEntry : IRegistryEntry
    {
        public string LastModifyDate => "undefined";
        public string Path { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
