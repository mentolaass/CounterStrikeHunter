using System.Diagnostics;
using System.Windows.Input;

namespace CounterStrikeHunter.Model.FileData
{
    public class UsnFileData
    {
        public string Path { get; set; }
        public bool IsDeleted { get; set; }

        public ICommand Open
        {
            get => new RelayCommand((obj) =>
            {
                try
                {
                    Process.Start(Path);
                } catch
                {

                }
            });
        }
    }
}
