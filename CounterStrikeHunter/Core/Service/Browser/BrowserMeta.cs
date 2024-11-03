namespace CounterStrikeHunter.Core.Service.Browser
{
    public class BrowserMeta
    {
        public string Name { get; }
        public string PathData { get; }

        public BrowserMeta(string name, string pathData)
        {
            Name = name;
            PathData = pathData;
        }
    }
}
