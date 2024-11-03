using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CounterStrikeHunter.Core.Regedit
{
    public class RegistryReader
    {
        private readonly Dictionary<string, object> _registryReaders = new Dictionary<string, object>();

        public ObservableCollection<RegisrtyViewModel> GetRegistryMetas()
        {
            var registryMeta = _registryReaders.Values
                .Select(m =>
                {
                    var entry = m as dynamic;
                    var meta = entry?.Meta as RegisrtyViewModel;
                    return meta;
                })
                .Where(info => info != null)
                .ToList();

            return new ObservableCollection<RegisrtyViewModel>(registryMeta);
        }

        public void RegisterPath<TInput, TOutput>(string name, string description, string path, IConverter<TInput, TOutput> converter, IRegistryReader<TInput> reader)
        {
            var meta = new ConvertableRegistryMeta<TInput, TOutput>
            {
                Name = name,
                Path = path,
                Description = description,
                Converter = converter,
                Result = default
            };

            _registryReaders[name] = new { Meta = meta, Reader = reader };
        }

        public TOutput GetValue<TInput, TOutput>(string name)
        {
            if (_registryReaders.TryGetValue(name, out var entry))
            {
                var meta = (ConvertableRegistryMeta<TInput, TOutput>)((dynamic)entry).Meta;
                var reader = (IRegistryReader<TInput>)((dynamic)entry).Reader;

                TInput value = reader.ReadValue(meta.Path, meta.Name);

                if (meta.Converter != null)
                {
                    meta.Result = meta.Converter.Convert(meta, value);
                    return meta.Result;
                }
                else
                {
                   
                }
            }

            throw new InvalidOperationException("Value does not exist.");
        }
    }
}
