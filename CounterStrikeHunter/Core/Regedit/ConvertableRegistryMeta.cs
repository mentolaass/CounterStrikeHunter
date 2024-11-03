namespace CounterStrikeHunter.Core.Regedit
{
    public class ConvertableRegistryMeta<TInput, TOutput> : RegisrtyViewModel
    {
        public IConverter<TInput, TOutput> Converter { get; set; }
        public TOutput Result { get; set; }
    }
}
