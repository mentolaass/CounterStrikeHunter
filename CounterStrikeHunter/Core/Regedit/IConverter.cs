namespace CounterStrikeHunter.Core.Regedit
{
    public interface IConverter<TInput, TOutput>
    {
        TOutput Convert(RegisrtyViewModel meta, TInput value);
    }
}
