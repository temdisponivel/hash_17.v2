using System;
using System.Collections.Generic;
using HASH;
using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Holds data to stuff needed by the terminal.
    /// </summary>
    public class TerminalData
    {
        public bool Batching;
        public SimpleList<TextBatchEntry> BatchEntries;
        public SimpleList<string> CommandCache;
        public int CurrentCommandBufferIndex;
        public SimpleList<string> AvailableCommands;
        public SimpleList<string> CurrentCommandBuffer;
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