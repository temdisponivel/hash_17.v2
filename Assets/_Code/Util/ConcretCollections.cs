using System;
using SimpleCollections.Lists;

namespace HASH17.Util
{
    /// <summary>
    /// Create a concret (not generic) type of a simple list of strings.
    /// This is done in order to serialize stuff on the editor.
    /// </summary>
    [Serializable]
    public class StringList : SimpleList<string> { }
}