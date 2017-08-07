using System;

namespace HASH.OS.Shell
{
    /// <summary>
    /// Defines a set of configurations for validating command line arguments.
    /// </summary>
    public struct CommandLineArgValidationOption<T>
    {
        public string ArgumentName;
        public ArgRequirement Requirements;

        public T AditionalData;

        public ArgValidationResult ValidationResult;
    }

    /// <summary>
    /// Enumerates all possible argument validations.
    /// </summary>
    [Flags]
    public enum ArgRequirement
    {
        None = 1 << 0,
        Unique = 1 << 1,
        Required = 1 << 2,
        ValueRequired = 1 << 3,
    }

    /// <summary>
    /// Enumerates the possible result of the command line validation.
    /// </summary>
    [Flags]
    public enum ArgValidationResult
    {
        EverythingOk = 1 << 0,
        NotFound = 1 << 1,
        EmptyValue = 1 << 2,
        Duplicated = 1 << 3,
    }
}