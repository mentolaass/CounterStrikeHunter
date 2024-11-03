using CounterStrikeHunter.Core.Regedit.Radar;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit.UserAssist
{
    public class UserAssistConverter : IConverter<List<string>, UserAssistReadResult>
    {
        public UserAssistReadResult Convert(RegisrtyViewModel meta, List<string> value)
        {
            List<UserAssistEntry> entries = new List<UserAssistEntry>();

            foreach (var item in value)
            {
                entries.Add(new UserAssistEntry()
                {
                    Path = item
                });
            }

            return new UserAssistReadResult()
            {
                Description = meta.Description,
                Path = meta.Path,
                Name = meta.Name,
                Result = new ObservableCollection<UserAssistEntry>(entries)
            };
        }
    }
}
