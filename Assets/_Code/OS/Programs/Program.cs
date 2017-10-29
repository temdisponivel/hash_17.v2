﻿using SimpleCollections.Lists;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Represents a program on our virtual OS.
    /// </summary>
    public class Program
    {
        public string Name;
        public string Description;
        public string HelpText;

        public string[] Commands;
        public ProgramOption[] Options;

        public ProgramType ProgramType;
    }
}