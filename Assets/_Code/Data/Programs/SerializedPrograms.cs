using System;
using HASH;
using UnityEngine;

namespace HASH
{
    /// <summary>
    /// Holds the serialized data of a program.
    /// </summary>
    [Serializable]
    public struct SerializedProgram
    {
        public string Name;
        public string Description;
        public TextAsset HelpMessage;

        public string[] Commands;
        public SerializedProgramOption[] Options;

        public ProgramType ProgramType;
    }

    /// <summary>
    /// Holds the serialized data of a program option.
    /// </summary>
    [Serializable]
    public struct SerializedProgramOption
    {
        public string Option;
        public string Description;
    }

    /// <summary>
    /// Holds the program serialized data.
    /// </summary>
    [CreateAssetMenu(fileName = "SerializedPrograms", menuName = "HASH/Serialized programs")]
    public class SerializedPrograms : ScriptableObject
    {
        public SerializedProgram[] Programs;
    }
}