using CounterStrikeHunter.Core.Service.Browser.Pair;
using System.Collections.ObjectModel;
using System.Data.SQLite;
using System.IO;

namespace CounterStrikeHunter.Core.Service.Browser.Reader
{
    public class ChromiumBrowserDataReader : IBrowserDataReader
    {
        private readonly BrowserMeta _meta;
        private static readonly string TempHistoryDir = Path.Combine(Path.GetTempPath(), "browser_temp_history");
        private static readonly string TempDipsDir = Path.Combine(Path.GetTempPath(), "browser_temp_dips");

        public ChromiumBrowserDataReader(BrowserMeta meta)
        {
            _meta = meta;
        }

        public bool CanRead()
        {
            return Directory.Exists(_meta.PathData) &&
                   File.Exists(Path.Combine(_meta.PathData, "History")) &&
                   File.Exists(Path.Combine(_meta.PathData, "DIPS"));
        }

        public BrowserData Read()
        {
            if (!CanRead()) return null;

            File.Copy(Path.Combine(_meta.PathData, "History"), TempHistoryDir, overwrite: true);
            File.Copy(Path.Combine(_meta.PathData, "DIPS"), TempDipsDir, overwrite: true);

            var pairs = new ObservableCollection<BrowserViewModel>();
            ReadDownloadHistory(pairs);
            ReadVisitHistory(pairs);
            ReadBounceData(pairs);

            return new BrowserData { Pairs = pairs };
        }

        private void ReadDownloadHistory(ObservableCollection<BrowserViewModel> pairs)
        {
            using (var connection = new SQLiteConnection($"Data Source={TempHistoryDir}"))
            {
                connection.Open();

                const string query = "SELECT * FROM downloads";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pairs.Add(new BrowserViewModel
                            {
                                IsDownloadHistory = true,
                                Url = reader.GetString(15),
                                File = Path.GetFileName(reader.GetString(2)),
                                Date = Util.GetDateTimeFromWhat(reader.GetInt64(11)).ToString("yyyy-MM-dd HH:mm:ss"),
                                Type = "DOWNLOAD",
                                Size = $"{((reader.GetDouble(6)/1024)/1000).ToString("0.00")} mb",
                                HasExpand = true
                            });
                        }
                    }
                }
            }
        }

        private void ReadVisitHistory(ObservableCollection<BrowserViewModel> pairs)
        {
            using (var connection = new SQLiteConnection($"Data Source={TempHistoryDir}"))
            {
                connection.Open();

                const string query = "SELECT * FROM urls";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pairs.Add(new BrowserViewModel
                            {
                                IsVisitHistory = true,
                                Url = reader.GetString(1),
                                Title = reader.GetString(2),
                                Date = Util.GetDateTimeFromWhat(reader.GetInt64(5)).ToString("yyyy-MM-dd HH:mm:ss"),
                                Hidden = reader.GetInt16(6) == 1,
                                Type = "HISTORY",
                                HasExpand = true
                            });
                        }
                    }
                }
            }
        }

        private void ReadBounceData(ObservableCollection<BrowserViewModel> pairs)
        {
            using (var connection = new SQLiteConnection($"Data Source={TempDipsDir}"))
            {
                connection.Open();

                const string query = "SELECT * FROM bounces";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pairs.Add(new BrowserViewModel
                            {
                                Type = "VISIT SITE",
                                HasExpand = false,
                                IsVisitHistory = true,
                                Url = reader.GetString(0)
                            });
                        }
                    }
                }
            }
        }
    }
}
