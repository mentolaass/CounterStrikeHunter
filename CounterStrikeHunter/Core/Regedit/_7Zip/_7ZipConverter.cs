using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace CounterStrikeHunter.Core.Regedit._7Zip
{
    public class _7ZipConverter : IConverter<List<string>, _7ZipReadResult>
    {
        public _7ZipReadResult Convert(RegisrtyViewModel meta, List<string> value)
        {
            List<_7ZipEntry> entries = new List<_7ZipEntry>();

            foreach (var item in value)
            {
                entries.Add(new _7ZipEntry()
                {
                    Path = item
                });
            }

            return new _7ZipReadResult()
            {
                Description = meta.Description,
                Path = meta.Path,
                Name = meta.Name,
                Result = new ObservableCollection<_7ZipEntry>(entries)
            };
        }
    }
}
