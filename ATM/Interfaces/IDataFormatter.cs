using System;
using System.Collections.Generic;

namespace ATM.System
{
    public class TransponderArgs : EventArgs
    {
        public List<TransponderData> transponderData { get; set; }
    }

    public interface IDataFormatter
    {
        event EventHandler<TransponderArgs> transponderChanged;

        TransponderData StringToTransponderData(string s);
    }
}