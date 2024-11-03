using GalaSoft.MvvmLight;
using System;

namespace CounterStrikeHunter.Core.Regedit
{
    public interface IRegistryEntry
    {
        string LastModifyDate { get; }

        string Path { get; set; }

        bool IsDeleted { get; set; }
    }
}
