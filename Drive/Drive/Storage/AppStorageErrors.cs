using System;
using Dwares.Dwarf;


namespace Drive.Storage
{
    public class AppStorageError : DwarfException
    {
        public AppStorageError(string message) : base(message) { }
    }
}
