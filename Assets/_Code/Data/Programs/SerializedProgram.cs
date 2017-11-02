using System;
using HASH.Story;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Holds the serialized data of a program option.
    /// </summary>
    [Serializable]
    public struct SerializedProgramOption
    {
        public string Option;
        public string Description;
    }
    
    [CreateAssetMenu(fileName = "SerializedProgram", menuName = "HASH/Serialized program")]
    public class SerializedProgram : ScriptableObject
    {
        public string Name;
        public string Description;
        public TextAsset HelpMessage;

        public string[] Commands;
        public SerializedProgramOption[] Options;

        public ProgramType ProgramType;

        public HashStory.Condition Condition;
    }
}