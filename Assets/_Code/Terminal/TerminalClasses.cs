using System;
using System.Collections.Generic;
using HASH;
using SimpleCollections.Lists;
using UnityEngine;

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

        public ImageEntry Image;

        public UITable TextTable;
        public UIScrollView ScrollView;

        public UIInput Input;
        public UILabel CurrentPath;

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

        public SimpleList<TerminalEntry> AllEntries;
    }
    
    /// <summary>
    /// Enumerates all possible text entry types.
    /// </summary>
    public enum TerminalEntryType
    {
        SingleText,
        DualText,
        Image,
    }

    public class TerminalEntry
    {
        public GameObject SceneObject;
        public TerminalEntryType EntryType;
    }

    public enum TerminalEntryRemoveType
    {
        OlderEntries,
        NewerEntries,
    }   
}