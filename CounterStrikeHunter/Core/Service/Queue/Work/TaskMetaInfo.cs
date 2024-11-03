namespace CounterStrikeHunter.Core.Service.Queue.Work
{
    public class TaskMetaInfo
    {
        private string _name, _description;

        private Flag[] _flags;

        public TaskMetaInfo(string name, string description, Flag[] flags)
        {
            _name = name;

            _flags = flags;

            _description = description;
        }

        public string Name
        {
            get => _name;
        }

        public Flag[] Flags
        {
            get => _flags;
        }

        public string Description
        {
            get => _description;
        }
    }
}
