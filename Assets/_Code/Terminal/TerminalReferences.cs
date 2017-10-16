using System;
using System.Collections.Generic;
using HASH;
using SimpleCollections.Lists;

namespace HASH
{
    /// <summary>
    /// Holds references to stuff needed by the terminal.
    /// </summary>
    [Serializable]
    public class TerminalReferences
    {
        public SingleTextEntry SingleText;
        public DualTextEntry DualText;

        public UITable TextTable;
        public UIScrollView ScrollView;

        public UIInput Input;

        [NonSerialized]
        public bool Batching;
        public SimpleList<TextBatchEntry> BatchEntries;

        public SimpleList<string> CommandCache;

        [NonSerialized]
        public int CurrentCommandBufferIndex;

        public SimpleList<string> AvailableCommands;
        
        public SimpleList<string> CurrentCommandBuffer;

        [NonSerialized]
        public int MaxLineWidthInChars;
    }
}