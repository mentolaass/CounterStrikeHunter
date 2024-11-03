namespace CounterStrikeHunter.Core.Service.Browser.Reader
{
    public interface IBrowserDataReader
    {
        bool CanRead();

        BrowserData Read();
    }
}
