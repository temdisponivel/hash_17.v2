using System;
using System.Collections.Generic;
using HASH17.Terminal.TextEntry;
using HASH17.Util;
using SimpleCollections.Lists;

namespace HASH17.Terminal
{
    /// <summary>
    /// Holds references to stuff needed by the terminal.
    /// </summary>
    [Serializable]
    public class TerminalData
    {
        public SingleTextEntry SingleText;
        public DualTextEntry DualText;

        public UITable TextTable;
        public UIScrollView ScrollView;

        public UIInput Input;

        public bool Batching;
        public Stack<TextBatchEntry> BatchEntries;

        public SimpleList<string> CommandCache;
        public int CurrentCommandBufferIndex;

        public SimpleList<string> AvailableCommands;
        
        public SimpleList<string> CurrentCommandBuffer;
    }
}