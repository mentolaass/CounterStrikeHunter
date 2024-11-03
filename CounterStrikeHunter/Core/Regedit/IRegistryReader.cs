namespace CounterStrikeHunter.Core.Regedit
{
    public interface IRegistryReader<T>
    {
        T ReadValue(string path, string name);
    }
}
