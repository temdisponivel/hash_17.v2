using System;
using System.Collections.Generic;
using HASH17.Terminal.TextEntry;

namespace HASH17.Terminal
{
    [Serializable]
    public class TerminalData
    {
        public SingleTextEntry SingleText;
        public DualTextEntry DualText;

        public UITable TextTable;
        public UIScrollView ScrollView;

        public bool Batching;
        public Stack<TextBatchEntry> BatchEntries;
    }
}